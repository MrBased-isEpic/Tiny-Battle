using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerExitCell : RoadCell
{
    public override void Interact()
    {
        Debug.Log("Interacted with PlayerExit");
    }
}
