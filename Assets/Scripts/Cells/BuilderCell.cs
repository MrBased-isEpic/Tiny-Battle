using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderCell : BuildingCell
{
    // IInteractible functions
    public override void Interact()
    {
        //Debug.Log("Interacted with RoadBuilder");
        if(GameManager.Instance.player.GetMode() == Player.Mode.BuildingMode && !IsDestroyed())
        {
            GameManager.Instance.player.SwitchMode(Player.Mode.PlayingMode);
        }
        else
        {
            GameManager.Instance.player.SwitchMode(Player.Mode.BuildingMode);
        }
    }

}
