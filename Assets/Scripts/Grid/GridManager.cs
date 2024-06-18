using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class GridManager : PathFinder
{
    public static GridManager Instance;

    [SerializeField] private int gridHeight;
    [SerializeField] private int gridWidth;

    [SerializeField] private Transform BaseCell;
    [SerializeField] private Transform GroundCell;
    [SerializeField] private Transform PlayerExitCell;
    [SerializeField] private Transform EnemyEntryCell;
    public Transform RoadBlueprintCell;
    [SerializeField] private Transform RoadCell;
    [SerializeField] private Transform[] BuildingCells;

    //Visuals
    public List<Sprite> RoadBlueprintCellSprites;
    public List<Sprite> RoadCellSprites;

    public enum BuildingCellEnum
    {
        playerHQCell = 0,
        BuilderBaseCell = 1,
        GarageCell = 2,
        MoneyGenCell = 3,
        RocketShootDefenseCell = 4,
        AreaEffectDefenseCell = 5,
        LaserShootDefenseCell = 6
    }
    public enum Direction
    {
        Up,
        Left,
        Down,
        Right,
        None
    }
    public static Direction[] Directions = 
    {
        Direction.Up,
        Direction.Down,
        Direction.Left,
        Direction.Right,
        Direction.None
    };

    private BaseCell[,] gameCells;
    private GroundCell[,] groundCells;
    private Vector2 GridToWorldOffset;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gameCells = new BaseCell[gridHeight, gridWidth];
        groundCells = new GroundCell[gridHeight, gridWidth];
        GridToWorldOffset = new Vector2(gridWidth / 2, gridHeight / 2);
    }
    public void GenerateGrid() // Generates BaseCells and GroundCells and stores them in an array
    {
        //Generating empty BaseCells
        for (int y = 0; y < gridHeight; y++)
        {
            for(int x = 0; x < gridWidth; x++)
            {
                BaseCell baseCell = Instantiate(BaseCell,new Vector3(x - GridToWorldOffset.x, y - GridToWorldOffset.y, 0),Quaternion.identity).GetComponent<BaseCell>();
                gameCells[y,x] = baseCell;
            }       
        }

        Transform playerHQCell = Instantiate(BuildingCells[GetIndexOfBuildingCell(BuildingCellEnum.playerHQCell)], new Vector3(-7,-4,0), Quaternion.identity);
        InsertBaseCellAtWorldPosition(playerHQCell.GetComponent<BuildingCell>(), new Vector2(-7,-4));
        Transform builderCell = Instantiate(BuildingCells[GetIndexOfBuildingCell(BuildingCellEnum.BuilderBaseCell)], new Vector3(7,-4,0), Quaternion.identity);
        InsertBaseCellAtWorldPosition(builderCell.GetComponent<BuildingCell>(), new Vector2(7,-4));

        Transform playerExitCell = Instantiate(PlayerExitCell, new Vector3(gridWidth/2, gridHeight/5, 0), Quaternion.identity);
        InsertBaseCellAtWorldPosition(playerExitCell.GetComponent<BaseCell>(), new Vector2(gridWidth/2, gridHeight/5));
        
        Transform enemyEntryCell = Instantiate(EnemyEntryCell, new Vector3(gridWidth/2, -gridHeight/5, 0), Quaternion.identity);
        InsertBaseCellAtWorldPosition(enemyEntryCell.GetComponent<BaseCell>(), new Vector2(gridWidth/2, -gridHeight/5));

        GameManager.Instance.mapRoadCells.Add(playerExitCell.GetComponent<RoadCell>());
        GameManager.Instance.mapRoadCells.Add(enemyEntryCell.GetComponent<RoadCell>());
        
        //Generating GroundCells
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                GroundCell groundCell = Instantiate(GroundCell, new Vector3(x - GridToWorldOffset.x, y - GridToWorldOffset.y, .5f), Quaternion.identity).GetComponent<GroundCell>();
                groundCells[y, x] = groundCell;
            }
        }
    }

    public BaseCell GetCellFromPosition(Vector2 position) // Returns the baseCell present at given world position
    {
        if(Mathf.Abs(position.x) > GridToWorldOffset.x || Mathf.Abs(position.y) > GridToWorldOffset.y) return null;

        return gameCells[(int)(position.y + GridToWorldOffset.y), (int)(position.x + GridToWorldOffset.x)];
    }
    public bool CanBaseCellBePlacedHere(Vector2 position) // Checks if there are any building cells at given world position
    {
        BaseCell cell = GetCellFromPosition(position) as BuildingCell;

        if (cell != null)
        {
            return false;
        }
        else return true;
    }
    public void SetBaseCellAtWorldPosition(BaseCell cell, Vector2 position) // Swaps the positions of given BaseCell with the BaseCell present at given world position
    {
        BaseCell baseCell = GetCellFromPosition(position);
        Vector2 index = GetIndexOfBaseCell(cell);

        baseCell.transform.position = new Vector2(index.y - GridToWorldOffset.x, index.x - GridToWorldOffset.y);
        cell.transform.position = position;

        gameCells[(int)(position.y + GridToWorldOffset.y), (int)(position.x + GridToWorldOffset.x)] = cell;
        gameCells[(int)index.x, (int)index.y] = baseCell;
    }
    private Vector2 GetIndexOfBaseCell(BaseCell cell) 
    {
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                BaseCell baseCell = gameCells[y, x];
                if(baseCell == cell)
                {
                    return new Vector2(y, x);
                }
            }
        }
        return new Vector2(0, 0);
    }
    public int GetIndexOfBuildingCell(BuildingCellEnum buildingCell)
    {
        return (int)buildingCell;
    }
    public void InsertBaseCellAtWorldPosition(BaseCell spawnCell, Vector2 position)
    {
        BaseCell baseCell = GetCellFromPosition(position);
        gameCells[(int)(position.y + GridToWorldOffset.y), (int)(position.x + GridToWorldOffset.x)] = spawnCell;
        Destroy(baseCell.gameObject);

        BuildingCell buildingCell= spawnCell as BuildingCell; 
        if(buildingCell != null)
        {
            GameManager.Instance.AddBuildingCellToList(buildingCell);
            buildingCell.CheckRoutes();
        }

        
    }
    public Transform GetBuildingCellFromArray(int Index)
    {
        return BuildingCells[Index];
    }
    public void MakeRoad(List<RoadBlueprintCell> roadBlueprintCells)
    {
        foreach(RoadBlueprintCell blueprintCell in roadBlueprintCells)
        {
            if(blueprintCell.canBePlaced)
            {
                Vector2 position = blueprintCell.transform.position;
                Transform roadCell = Instantiate(RoadCell, position, Quaternion.identity);
                BaseCell baseCell = GetCellFromPosition(position);
                RoadCell worldRoadCell = baseCell as RoadCell;
                if(worldRoadCell != null)
                {
                    GameManager.Instance.RemoveRoadCellFromList(worldRoadCell);
                }
                Destroy(baseCell.gameObject);
                RoadCell newRoadCell = roadCell.GetComponent<RoadCell>();
                gameCells[(int)(position.y + GridToWorldOffset.y), (int)(position.x + GridToWorldOffset.x)] = newRoadCell;
                newRoadCell.RefreshVisuals(true);
                if(newRoadCell != null)
                {
                    GameManager.Instance.AddRoadCellToList(newRoadCell);
                }
            }
        }
    }
    public void DestroyRoad(List<RoadBlueprintCell> roadBlueprintCells)
    {
        foreach(RoadBlueprintCell blueprintCell in roadBlueprintCells)
        {
            Vector2 position = blueprintCell.transform.position;
            Transform baseCell = Instantiate(BaseCell, position, Quaternion.identity);
            RoadCell roadCell = GetCellFromPosition(position) as RoadCell;
            if(roadCell != null)
            {
                GameManager.Instance.RemoveRoadCellFromList(roadCell);
                Destroy(roadCell.gameObject);
                gameCells[(int)(position.y + GridToWorldOffset.y), (int)(position.x + GridToWorldOffset.x)] = BaseCell.GetComponent<BaseCell>();
            }
        }
    }
}
