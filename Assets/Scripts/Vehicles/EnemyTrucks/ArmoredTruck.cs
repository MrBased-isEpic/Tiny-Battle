using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmoredTruck : EnemyTruck
{
    public GridManager.Direction GetDirection()
    {
        return base.direction;
    }
}
