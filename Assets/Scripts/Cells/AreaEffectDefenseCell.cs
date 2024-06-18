using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEffectDefenseCell : BuildingCell
{
    [SerializeField] private LayerMask EnemyTruck;
    [SerializeField] private float intervalBetweenShoot;

    private float timer;

    private new void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if (GameManager.Instance.GetCurrentState() == GameManager.State.GamePlaying && !IsDestroyed())
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                Attack();
                timer = intervalBetweenShoot;
            }
        }
    }

    private List<EnemyTruck> GetTrucksInRange()
    { 
        if (Physics2D.CircleCast(transform.position, 3, Vector2.zero, 3, EnemyTruck))
        {
            RaycastHit2D[] results = Physics2D.CircleCastAll(transform.position, 3, Vector2.zero, 3, EnemyTruck);
            List<EnemyTruck> truckList = new List<EnemyTruck>();
            foreach(RaycastHit2D result in results)
            {
                EnemyTruck truck = result.transform.gameObject.GetComponent<EnemyTruck>();
                truckList.Add(truck);
            }
            return truckList;
        }
        else
            return null;
    }

    private void Attack()
    {   
        List<EnemyTruck> truckList = GetTrucksInRange();
        if (truckList!= null && truckList.Count != 0)
        {
            foreach (EnemyTruck truck in truckList)
            {
                truck.GetComponent<BaseHealth>().GiveDamage(1);
            }
        }
    }
}
