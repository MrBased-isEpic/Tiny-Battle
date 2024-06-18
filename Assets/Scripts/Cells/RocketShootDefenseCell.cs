using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketShootDefenseCell : BuildingCell
{
    private Truck Target;
    [SerializeField] private LayerMask EnemyTruck;
    [SerializeField] private float intervalBetweenShoot;
    [SerializeField] private Transform Rocket;
    private float timer;

    private new void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if(GameManager.Instance.GetCurrentState() == GameManager.State.GamePlaying && !IsDestroyed())
        {
            timer -= Time.deltaTime;
            if(timer < 0)
            {
                EnemyTruck targetTruck = GetTargetTruck();
                if (targetTruck != null)
                {
                    ShootRocket(targetTruck);
                    timer = intervalBetweenShoot;
                }
            }
        }
    }

    private EnemyTruck GetTargetTruck()
    {
        if (Physics2D.CircleCast(transform.position, 3, Vector2.zero, 3, EnemyTruck))
        {
            RaycastHit2D result = Physics2D.CircleCast(transform.position, 3, Vector2.zero, 3, EnemyTruck);
            EnemyTruck truck = result.transform.gameObject.GetComponent<EnemyTruck>();
            return truck;
        }
        return null;
    }

    private void ShootRocket(EnemyTruck targetTruck)
    {
        Rocket rocket = Instantiate(Rocket, transform.position, Quaternion.identity).GetComponent<Rocket>();
        rocket.SetTruck(targetTruck);
    }
}