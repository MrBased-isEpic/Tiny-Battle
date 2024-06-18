using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

public class BuildingCell : BaseCell
{
    [SerializeField] protected int Cost;
    [SerializeField] private int MaxHealth;
    [SerializeField] private Transform healthVisual;
    [SerializeField] private BaseHealth baseHealth;
    [SerializeField] protected Transform cantPlaceHereVisual;
    [SerializeField] private Transform spriteRendererObject;
    private SpriteRenderer spriteRenderer;
    [SerializeField] protected Sprite destroyedBuildingSprite;
    protected Sprite buildingSprite;

    private bool isDestroyed = false;

    public string ErrorMessage { get; protected set; } = "BuildingCell not connected to HQ!";

    protected void Awake()
    {
        GameManager.Instance.player.OnRoadBuilt += GridManager_OnRoadBuilt;
        baseHealth.HealthIsZero += BaseHealth_HealthIsZero;
        baseHealth.HealthIsNotZero += BaseHealth_HealthIsNotZero;

        spriteRenderer = spriteRendererObject.GetComponent<SpriteRenderer>();
        buildingSprite = spriteRenderer.sprite;
    }

    private void BaseHealth_HealthIsNotZero(object sender, EventArgs e)
    {
        OnHealthNotZero();
    }

    private void BaseHealth_HealthIsZero(object sender, EventArgs e)
    {
        OnHealthZero();
    }

    private void GridManager_OnRoadBuilt(object sender, System.EventArgs e)
    {
        CheckRoutes();
    }

    protected new void Start()
     {
          base.Start();
          TurnOffHealthVisual();
          baseHealth.SetMaxHealth(MaxHealth);
          GameManager.Instance.player.OnPlayerModeChanged += Player_OnModeChanged;

          if(GameManager.Instance.player.GetMode() == Player.Mode.OverviewMode)
          {
            TurnOnHealthVisual();
          }
          else
          {
            TurnOffHealthVisual();
          }
     }

    private void Player_OnModeChanged(object sender, EventArgs e)
    {
        if(GameManager.Instance.player.GetMode() == Player.Mode.OverviewMode)
        {
          TurnOnHealthVisual();
        }
        else
        {
          TurnOffHealthVisual();
        }
    }

    public void TurnOnHealthVisual()
    {
        healthVisual.gameObject.SetActive(true);
    }
    public void TurnOffHealthVisual()
    {
        healthVisual.gameObject.SetActive(false);
    }
    public new virtual void Interact()
    {
         Debug.Log("Interacted with BuildingCell");
    }
    protected virtual void OnHealthZero()
    {
        spriteRenderer.sprite = destroyedBuildingSprite;
        GameManager.Instance.RemoveBuildingCellFromList(this);
        isDestroyed = true;
    }
    protected virtual void OnHealthNotZero()
    {
        spriteRenderer.sprite = buildingSprite;
        GameManager.Instance.AddBuildingCellToList(this);
        isDestroyed = false;
    }

    public void TurnOnCantPlaceHereVisual()
    {
        cantPlaceHereVisual.gameObject.SetActive(true);
    }
    public void TurnOffCantPlaceHereVisual()
    {
         cantPlaceHereVisual.gameObject.SetActive(false);
    }
    public virtual bool IsReady()
    {
        if(cantPlaceHereVisual.gameObject.activeSelf == true)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public BaseCell FindNeighbourCell(GridManager.Direction direction) 
     {
        Vector2 findPosition = Vector2.zero;

        switch(direction)
        {
            case GridManager.Direction.Up:
                findPosition = transform.position + new Vector3(0, 1);
                break; 
            case GridManager.Direction.Down:
                findPosition = transform.position + new Vector3(0, -1);
                break;
            case GridManager.Direction.Left:
                findPosition = transform.position + new Vector3(-1, 0);
                break;
            case GridManager.Direction.Right:
                findPosition = transform.position + new Vector3(1, 0);
                break;
        }
        return GridManager.Instance.GetCellFromPosition(findPosition);
     }
    public List<RoadCell> GetNeighbourRoadCellList()
    {
          List<RoadCell> NeighbourRoadCells = new List<RoadCell>();

          RoadCell roadCell = FindNeighbourCell(GridManager.Direction.Down) as RoadCell;
          if(roadCell != null)
               NeighbourRoadCells.Add(roadCell);

          roadCell = FindNeighbourCell(GridManager.Direction.Right) as RoadCell;
          if(roadCell != null)
               NeighbourRoadCells.Add(roadCell);

          roadCell = FindNeighbourCell(GridManager.Direction.Up) as RoadCell;
          if(roadCell != null)
               NeighbourRoadCells.Add(roadCell);

          roadCell = FindNeighbourCell(GridManager.Direction.Left) as RoadCell;
          if(roadCell != null)
               NeighbourRoadCells.Add(roadCell);
        
          return NeighbourRoadCells;
    }
    public int GetCost()
    {
        return Cost;
    }
    public bool IsDestroyed()
    {
        return isDestroyed;
    }

    public virtual void CheckRoutes()
    {
        if (GetNeighbourRoadCellList().Count > 0 && GameManager.Instance.mapBuildingCells[0].GetNeighbourRoadCellList().Count > 0)
        {
            if (GridManager.Instance.FindPath(GetNeighbourRoadCellList()[0], GameManager.Instance.mapBuildingCells[0].GetNeighbourRoadCellList()[0]))
            {
                TurnOffCantPlaceHereVisual();
            }
            else
            {
                TurnOnCantPlaceHereVisual();
            }
        }
        else
        {
            TurnOnCantPlaceHereVisual();
        }
    }
}
