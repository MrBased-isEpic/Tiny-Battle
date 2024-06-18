using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlayingMode : PlayerBaseState
{

    private BaseCell SelectedCell;

    public override void StartState(Player player)
    {
        GameInput.Instance.OnMouseWorldPosChanged += GameInput_OnMouseWorldPosChanged;
        GameInput.Instance.OnMouseClicked += GameInput_OnMouseClicked;

        if(GameManager.Instance.GetCurrentState() == GameManager.State.Preparation || GameManager.Instance.GetCurrentState() == GameManager.State.GamePlaying)
        {
            player.SetSelectedCell(GridManager.Instance.GetCellFromPosition(GameInput.Instance.GetMouseWorldPositionRounded()));
        }
    }
    public override void UpdateState(Player player)
    {

    }
    public override void EndState(Player player)
    {
        GameInput.Instance.OnMouseWorldPosChanged -= GameInput_OnMouseWorldPosChanged;
        GameInput.Instance.OnMouseClicked -= GameInput_OnMouseClicked;
    }

    private void GameInput_OnMouseClicked(object sender, System.EventArgs e)
    {
        BuildingCell SelectedBuildingCell = SelectedCell as BuildingCell;
        if(SelectedBuildingCell != null)
        {
           SelectedBuildingCell.Interact();
        }
        else
        {
           SelectedCell.Interact();
        }
    }

    private void GameInput_OnMouseWorldPosChanged(object sender, System.EventArgs e)
    {
        //Debug.Log(GameInput.Instance.GetMouseWorldPositionRounded());
        if (GridManager.Instance.GetCellFromPosition(GameInput.Instance.GetMouseWorldPositionRounded()) != null)
        {
            SetSelectedCell(GridManager.Instance.GetCellFromPosition(GameInput.Instance.GetMouseWorldPositionRounded()));
        }
    }

    private void SetSelectedCell(BaseCell cell)
    {
        //Debug.Log("SelectedCell Ran");
        if (SelectedCell != null)
        {
            SelectedCell.HideSelectedVisual();
        }
        SelectedCell = cell;
        // SelectedCell = cell as BuildingCell;
        // if(SelectedCell == null)
        // {
        //     SelectedCell = cell;
        // }
        SelectedCell.ShowSelectedVisual();
    }
}
