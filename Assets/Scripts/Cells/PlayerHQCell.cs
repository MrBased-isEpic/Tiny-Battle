using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHQCell : BuildingCell
{
    private new void Awake()
    {
        base.Awake();
        ErrorMessage = "HQ not connected to EnemyEntryRoad!";
    }

    // IInteractible functions
    public override void Interact()
    {
        //Debug.Log("Interacted with PlayerHQ");
        if(GameManager.Instance.player.GetMode() == Player.Mode.OverviewMode && !IsDestroyed())
        {
            GameManager.Instance.player.SwitchMode(Player.Mode.PlayingMode);
        }
        else
        {
            GameManager.Instance.player.SwitchMode(Player.Mode.OverviewMode);
        }
    }

    // Inherited Override Functions
    public override void CheckRoutes()
    {
        if (GetNeighbourRoadCellList().Count > 0)
        {
            if (GridManager.Instance.FindPath(GetNeighbourRoadCellList()[0], GameManager.Instance.mapRoadCells[1]))
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
