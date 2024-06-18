using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerBuildingMode : PlayerBaseState
{
    private BuildingCell DragedBuilding;
    private List<RoadBlueprintCell> onDragRoadBlueprintCells;
    private BuildingModeUI buildingModeUI;
    private Player player;

    private bool isMouseHeldDown = false;
    private bool isDestroying = false;
    private bool buildingRoad = false;
    public bool isFirstRoad = true;


    public enum BuildingMode
    {
        BaseBuildingMode,
        RoadBuildingMode
    }
    private BuildingMode mode;

    public override void StartState(Player player)
    {
        this.player = player;
        buildingModeUI = GameManager.Instance.GetBuildingModeUI();
        GameManager.Instance.TurnOnBuildingModeVisuals();
        onDragRoadBlueprintCells = new List<RoadBlueprintCell>();

        GameInput.Instance.OnMouseWorldPosChanged += GameInput_OnMouseWorldPosChanged;
        GameInput.Instance.OnMouseClicked += GameInput_OnMouseClicked;
        GameInput.Instance.OnMouseHeldDown += GameInput_OnMouseHeldDown;
        GameInput.Instance.OnMouseReleased += GameInput_OnMouseReleased;

        buildingModeUI.OnBaseBuildingModeButtonClicked += BuildingModeUI_OnBaseBuildingModeButtonClicked;
        buildingModeUI.OnRoadBuildingModeButtonClicked += BuildingModeUI_OnRoadBuildingModeButtonClicked;
        buildingModeUI.OnRoadDestroyingButtonClicked += BuildingModeUI_OnRoadDestoryButtonClicked;


        player.SetSelectedCell(GridManager.Instance.GetCellFromPosition(GameInput.Instance.GetMouseWorldPositionRounded()));

    }

    private void BuildingModeUI_OnRoadDestoryButtonClicked(object sender, EventArgs e)
    {
        isDestroying = true;
    }

    public override void EndState(Player player)
    {
        GameInput.Instance.OnMouseWorldPosChanged -= GameInput_OnMouseWorldPosChanged;
        GameInput.Instance.OnMouseClicked -= GameInput_OnMouseClicked;
        GameInput.Instance.OnMouseHeldDown -= GameInput_OnMouseHeldDown;
        GameInput.Instance.OnMouseReleased -= GameInput_OnMouseReleased;

        buildingModeUI.OnBaseBuildingModeButtonClicked -= BuildingModeUI_OnBaseBuildingModeButtonClicked;
        buildingModeUI.OnRoadBuildingModeButtonClicked -= BuildingModeUI_OnRoadBuildingModeButtonClicked;
        buildingModeUI.OnRoadDestroyingButtonClicked -= BuildingModeUI_OnRoadDestoryButtonClicked;
        GameManager.Instance.TurnOffBuildingModeVisuals();
    }

    private void BuildingModeUI_OnRoadBuildingModeButtonClicked(object sender, System.EventArgs e)
    {
        SwitchMode(BuildingMode.RoadBuildingMode);
        isDestroying = false;
    }
    private void BuildingModeUI_OnBaseBuildingModeButtonClicked(object sender, System.EventArgs e)
    {
        SwitchMode(BuildingMode.BaseBuildingMode);
    }

    private void GameInput_OnMouseReleased(object sender, System.EventArgs e)
    {
        switch(mode)
        {
            case BuildingMode.BaseBuildingMode:
                if (DragedBuilding != null)
                {
                    GridManager.Instance.SetBaseCellAtWorldPosition(DragedBuilding, GameInput.Instance.GetMouseWorldPositionRounded());
                }
                DragedBuilding = null;
                buildingModeUI.Show();
                isMouseHeldDown = false;
                break;
            case BuildingMode.RoadBuildingMode:
                if(isMouseHeldDown)
                {
                    if(isDestroying == true)
                    {
                        GridManager.Instance.DestroyRoad(onDragRoadBlueprintCells);
                        GameManager.Instance.AddToMoney(onDragRoadBlueprintCells.Count * GameManager.Instance.RoadCellCost);
                    }
                    else
                    {
                        GridManager.Instance.MakeRoad(onDragRoadBlueprintCells);
                        GameManager.Instance.TakeFromMoney(onDragRoadBlueprintCells.Count * GameManager.Instance.RoadCellCost);
                    }
                    foreach(RoadBlueprintCell roadBlueprintCell in onDragRoadBlueprintCells)
                    {
                        player.DestroyObject(roadBlueprintCell.gameObject);
                    }
                    player.RoadBuilt();
                    onDragRoadBlueprintCells.Clear();
                    buildingRoad = false;
                    isFirstRoad = false;
                    buildingModeUI.Show();
                }
                isMouseHeldDown = false;
                break;
        }
        GameManager.Instance.RefreshVisuals();
    }
    private void GameInput_OnMouseHeldDown(object sender, System.EventArgs e)
    {
        switch(mode)
        {
            case BuildingMode.BaseBuildingMode:
                if (!isMouseHeldDown)
                {
                    SetDraggedBuilding(GridManager.Instance.GetCellFromPosition(GameInput.Instance.GetMouseWorldPositionRounded()) as BuildingCell);
                    buildingModeUI.Hide();
                    isMouseHeldDown = true;
                }
                break;
            case BuildingMode.RoadBuildingMode:
                if(!isMouseHeldDown)
                {
                    RoadCell testIfRoadCell = GridManager.Instance.GetCellFromPosition(GameInput.Instance.GetMouseWorldPositionRounded()) as RoadCell;
                    if(testIfRoadCell != null)
                    {
                        PlaceRoadBlueprintCell(GameInput.Instance.GetMouseWorldPositionRounded());
                        buildingModeUI.Hide();
                        buildingRoad = true;
                    }
                    else if(isFirstRoad)
                    {
                        PlaceRoadBlueprintCell(GameInput.Instance.GetMouseWorldPositionRounded());
                        buildingModeUI.Hide();
                        buildingRoad = true;
                    }

                    isMouseHeldDown = true;
                }
                break;
        }     
    }
    private void GameInput_OnMouseClicked(object sender, System.EventArgs e)
    {
        BuildingCell SelectedBuildingCell = player.SelectedCell as BuildingCell;
        if(SelectedBuildingCell != null)
        {
            SelectedBuildingCell.Interact();
        }
        else if(player.SelectedCell != null)
        {
            player.SelectedCell.Interact();
        }
    }
    private void GameInput_OnMouseWorldPosChanged(object sender, System.EventArgs e)
    {
        player.SetSelectedCell(GridManager.Instance.GetCellFromPosition(GameInput.Instance.GetMouseWorldPositionRounded()));
        
        switch (mode)
        {
            case BuildingMode.BaseBuildingMode:           
                if (isMouseHeldDown)
                {
                    if (DragedBuilding != null)
                    {
                        DragedBuilding.transform.position = GameInput.Instance.GetMouseWorldPositionRounded();
                        if(!GridManager.Instance.CanBaseCellBePlacedHere(GameInput.Instance.GetMouseWorldPositionRounded()))
                        {          
                            DragedBuilding.TurnOnCantPlaceHereVisual();
                        }
                        else
                        {
                            DragedBuilding.TurnOffCantPlaceHereVisual();
                        }
                    }
                }
                break;
            case BuildingMode.RoadBuildingMode:  
            if(isMouseHeldDown)
            {
                if(buildingRoad)
                {             
                    if(onDragRoadBlueprintCells.Count >= 2)
                    {
                        if(GameInput.Instance.GetMouseWorldPositionRounded() == (Vector2) onDragRoadBlueprintCells[onDragRoadBlueprintCells.Count - 2].transform.position)
                        {
                            RemoveRoadBlueprintCell();
                        }
                        else
                        {
                            PlaceRoadBlueprintCell(GameInput.Instance.GetMouseWorldPositionRounded());
                        }
                    }
                    else
                    {
                        PlaceRoadBlueprintCell(GameInput.Instance.GetMouseWorldPositionRounded());
                    }
                }
            }
            break;
        }
    }

    public override void UpdateState(Player player)
    {

    }

    private void SetDraggedBuilding(BuildingCell cell)
    {
        if(cell != null)
        {
            DragedBuilding = cell;
        }
    }
    public void SwitchMode(BuildingMode mode) // Switches between two modes
    {
        this.mode = mode;

        switch(mode)
        {
            case BuildingMode.BaseBuildingMode:
                break;
            case BuildingMode.RoadBuildingMode: 
                break;
        }
    }
    public RoadBlueprintCell PlaceRoadBlueprintCell(Vector2 position)
    {
        RoadBlueprintCell roadBlueprintCell = player.InstantiateObject(GridManager.Instance.RoadBlueprintCell, position).GetComponent<RoadBlueprintCell>();

        if(!GridManager.Instance.CanBaseCellBePlacedHere(GameInput.Instance.GetMouseWorldPositionRounded()))
        {
            roadBlueprintCell.TurnOnCantPlaceHereVisual();
        }
        else if(onDragRoadBlueprintCells.Count * GameManager.Instance.RoadCellCost >= GameManager.Instance.Money)
        {
            roadBlueprintCell.TurnOnCantPlaceHereVisual();
        }
        else
        {
            roadBlueprintCell.TurnOffCantPlaceHereVisual();
        }

        onDragRoadBlueprintCells.Add(roadBlueprintCell);

        if(onDragRoadBlueprintCells.Count > 1)
        {
            onDragRoadBlueprintCells[onDragRoadBlueprintCells.Count-2].FindNextCellDirection(roadBlueprintCell);
            roadBlueprintCell.FindPrevCellDirection(onDragRoadBlueprintCells[onDragRoadBlueprintCells.Count-2].transform);
        }
        else
        {
            roadBlueprintCell.FindPrevCellDirection(GameManager.Instance.mapRoadCells[1].transform);
        }
        return roadBlueprintCell;
    }
    public void RemoveRoadBlueprintCell()
    {
        RoadBlueprintCell roadBlueprintCell = onDragRoadBlueprintCells[onDragRoadBlueprintCells.Count - 1];
        onDragRoadBlueprintCells.Remove(roadBlueprintCell);
        player.DestroyObject(roadBlueprintCell.gameObject);
    }
}
