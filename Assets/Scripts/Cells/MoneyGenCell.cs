using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyGenCell : BuildingCell
{
    private float addRate = .5f;
    private float timer;
    private new void Awake()
    {
        base.Awake();
        timer = 1/addRate;
    }
    private void Update()
    {
        if(GameManager.Instance.GetCurrentState() == GameManager.State.GamePlaying && !IsDestroyed())
        {
            timer -= Time.deltaTime;
            if(timer < 0)
            {
                GameManager.Instance.AddToMoney(20);
                timer = 1/addRate;
            }
        }
    }
}
