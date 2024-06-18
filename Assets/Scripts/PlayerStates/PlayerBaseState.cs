using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState
{
    public abstract void StartState(Player player);

    public abstract void UpdateState(Player player);

    public abstract void EndState(Player player);
}
