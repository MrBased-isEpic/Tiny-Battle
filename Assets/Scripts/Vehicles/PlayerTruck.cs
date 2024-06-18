using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTruck : Truck
{
    protected override void SetDestination()
    {
        base.Destination = GameManager.Instance.mapRoadCells[0];
    }
}
