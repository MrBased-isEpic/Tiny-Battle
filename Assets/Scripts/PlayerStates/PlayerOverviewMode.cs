using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerOverviewMode : PlayerBaseState
{
    private BuildingCell DraggedBuilding;
    private Transform DraggedBuildingTransform;
    private Player player;
    private bool SetDraggedBuildingThisFrame = false;

    public override void StartState(Player player)
    {
        this.player = player;
        GameInput.Instance.OnMouseWorldPosChanged += GameInput_OnMouseWorldPosChanged;
        GameInput.Instance.OnMouseClicked += GameInput_OnMouseClicked;
        //GameInput.Instance.OnMousePressed += GameInput_OnMousePressed;
        GameInput.Instance.OnMouseReleased += GameInput_OnMouseReleased; 

        GameManager.Instance.TurnOnOverviewVisuals();

        player.SetSelectedCell(GridManager.Instance.GetCellFromPosition(GameInput.Instance.GetMouseWorldPositionRounded()));

    }

    public override void EndState(Player player)
    {
        GameInput.Instance.OnMouseWorldPosChanged -= GameInput_OnMouseWorldPosChanged;
        GameInput.Instance.OnMouseClicked -= GameInput_OnMouseClicked;
        GameInput.Instance.OnMouseReleased -= GameInput_OnMouseReleased; 

        GameManager.Instance.TurnOffOverviewVisuals();
    }
    // private void GameInput_OnMousePressed(object sender, EventArgs e)
    // {

    // }

    private void GameInput_OnMouseReleased(object sender, EventArgs e)
    {

    }

    private void GameInput_OnMouseClicked(object sender, EventArgs e)
    {   
         // Spawn DraggedBuilding code
        if(DraggedBuilding != null)
        {
            if(!SetDraggedBuildingThisFrame)
            {
                if(GridManager.Instance.CanBaseCellBePlacedHere(DraggedBuildingTransform.position))
                {
                    if(DraggedBuilding.GetCost() <= GameManager.Instance.Money)
                    {
                        Debug.Log("Cost:" + DraggedBuilding.GetCost());
                        Debug.Log("Money Left:" + GameManager.Instance.Money);
                        GridManager.Instance.InsertBaseCellAtWorldPosition(DraggedBuilding, DraggedBuildingTransform.position);
                        GameManager.Instance.TurnOnOverviewVisuals();
                        GameManager.Instance.TakeFromMoney(DraggedBuilding.GetCost());
                        DraggedBuilding = null;
                    }
                    else
                    {
                        GameManager.Instance.TurnOnOverviewVisuals();
                        player.DestroyObject(DraggedBuilding.gameObject);
                    }
                }
            }
            else
            {
                SetDraggedBuildingThisFrame = false;
            }
        }
        else
        {
            // Interact with buildingCell code 
            BuildingCell SelectedBuildingCell = player.SelectedCell as BuildingCell;
            if(SelectedBuildingCell != null)
            {
                SelectedBuildingCell.Interact();
            }
            else
            {
                player.SelectedCell.Interact();
            }
             
        }
    }

    private void GameInput_OnMouseWorldPosChanged(object sender, EventArgs e)
    {
        player.SetSelectedCell(GridManager.Instance.GetCellFromPosition(GameInput.Instance.GetMouseWorldPositionRounded()));
        
        if(DraggedBuilding != null)
        {
            DraggedBuildingTransform.position = GameInput.Instance.GetMouseWorldPositionRounded();
            if(!GridManager.Instance.CanBaseCellBePlacedHere(GameInput.Instance.GetMouseWorldPositionRounded()))
            {          
                DraggedBuilding.TurnOnCantPlaceHereVisual();
            }
            else
            {
                DraggedBuilding.TurnOffCantPlaceHereVisual();
            }
        }
    }

    public override void UpdateState(Player player)
    {
        
    }

    public void SetDraggedBuilding(Transform BuildingTransform)
    {
        DraggedBuildingTransform = GameManager.Instance.player.InstantiateObject(BuildingTransform, GameInput.Instance.GetMouseWorldPositionRounded());
        DraggedBuilding = DraggedBuildingTransform.GetComponent<BuildingCell>();
        SetDraggedBuildingThisFrame = true;
    }
}
