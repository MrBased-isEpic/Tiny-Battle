using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class Truck : PathFinder
{
    [SerializeField] private Transform healthVisual;
    [SerializeField] private int MaxHealth;
    protected BaseHealth baseHealth;
    public enum TruckState
    {
        PathFinding,
        Moving,
        Waiting,
        Attacking
    }

    public TruckState truckState;
    public RoadCell currentCell{get; private set;}
    
    [SerializeField] private float speed = 2;
    protected List<RoadCell> path = new List<RoadCell>();
    public RoadCell Destination{get; protected set;}

    List<RoadCell> ReversePathList = new List<RoadCell>();

    protected void Start()
    {
        baseHealth = GetComponent<BaseHealth>();
        baseHealth.SetMaxHealth(MaxHealth);
        GameManager.Instance.player.OnPlayerModeChanged += Player_OnModeChanged;
        GameManager.Instance.AddTruckToList(this);
        SetDestination();
        SwitchState(TruckState.PathFinding);
        currentPos = transform.position;

        CheckForOverviewMode();
    }

    private void Player_OnModeChanged(object sender, EventArgs e)
    {
        CheckForOverviewMode();
    }

    public void TurnOnHealthVisual()
    {
        healthVisual.gameObject.SetActive(true);
    }
    public void TurnOffHealthVisual()
    {
        healthVisual.gameObject.SetActive(false);
    }
    public void CheckForOverviewMode()
    {
        if (GameManager.Instance.player.GetMode() == Player.Mode.OverviewMode)
        {
            TurnOnHealthVisual();
        }
        else
        {
            TurnOffHealthVisual();
        }
    }

    protected void Update()
    {
        switch (truckState)
        {
            case TruckState.PathFinding:  
                RunPathFinder();     
                break;
            case TruckState.Moving:
                MoveTruck();
                break;
            case TruckState.Waiting:
                break;
            case TruckState.Attacking:
                Attack();
                break;
            default:
            break;
        }
    }

    protected Vector3 currentPos;
    protected RoadCell goingToCell;
    protected GridManager.Direction direction;
    protected void CheckDirection(RoadCell currentCell, RoadCell goingToCell)
    {
        Vector2 direction = currentCell.transform.position - goingToCell.transform.position;

        if(direction == Vector2.left)
        {
            this.direction = GridManager.Direction.Right;
        }
        else if(direction == Vector2.right)
        {
            this.direction = GridManager.Direction.Left;
        }
        else if(direction == Vector2.up)
        {
            this.direction = GridManager.Direction.Up;
        }
        else if (direction == Vector2.down)
        {
            this.direction = GridManager.Direction.Down;
        }
    }
    protected virtual void SetDestination()
    {
        
    }
    protected virtual void Attack()
    {

    }
    public virtual void SwitchState(TruckState truckState)
    {
        this.truckState = truckState;

        switch(truckState)
        {
            case TruckState.Moving:
                currentPos = path[0].transform.position;
                goingToCell = path[1];
                break;

            case TruckState.Waiting:
                break;

            case TruckState.Attacking:
                break;
        }
    }

    public void SetCurrentCell(RoadCell roadCell)
    {
        currentCell = roadCell;
    }

    protected virtual void RunPathFinder()
    {
        if(FindPath(GridManager.Instance.GetCellFromPosition(currentPos) as RoadCell, Destination))
        {
            currentCell = Destination;
            ReversePathList.Add(currentCell);
            GetPath(this, GridManager.Instance.GetCellFromPosition(currentPos) as RoadCell, ReversePathList, path);
            ReversePathList.Clear();
        }    
        SwitchState(TruckState.Moving);
    }
    private void MoveTruck()
    {
        if (GameManager.Instance.GetCurrentState() == GameManager.State.GamePlaying)
        {
            if (path != null)
            {
                if (currentPos != path[path.Count - 1].transform.position) // If truck has not reached the last Cell
                {
                    if (currentPos != goingToCell.transform.position) // If truck has not reached the next cell
                    {
                        currentPos = Vector3.MoveTowards(currentPos, goingToCell.transform.position, speed * Time.deltaTime);
                        transform.position = currentPos;
                    }
                    else // If the truch has reached the next cell
                    {
                        RoadCell roadCell = goingToCell;
                        goingToCell = path[path.IndexOf(goingToCell) + 1];
                        CheckDirection(roadCell, goingToCell);
                    }
                }
                else
                {
                    SwitchState(TruckState.Attacking);
                }
            }
        }
    }
}

