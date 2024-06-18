using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShootDefenseCell : BuildingCell
{
    [SerializeField] private LayerMask EnemyTruck;
    [SerializeField] private float intervalBetweenShoot;
    private GridManager.Direction direction;
    private float timer;

    [SerializeField] private Transform directionVisual;
    [SerializeField] private Transform directionUI;

    private new void Start()
    {
        base.Start();
        SetDirection(direction);
    }

    private void Update()
    {
        if(GameManager.Instance.GetCurrentState() == GameManager.State.GamePlaying)// && !IsDestroyed())
        {
            timer -= Time.deltaTime;
            if(timer < 0)
            {
                ShootLaser();
                timer = intervalBetweenShoot;
            }
        }
    }

    private List<EnemyTruck> GetTargetTrucks()
    {
        List<EnemyTruck> truckList = new List<EnemyTruck>();
        Vector2 direction = Vector2.zero;
        switch(this.direction)
        {
            case GridManager.Direction.Left:
                direction = Vector2.left;
                break;
            case GridManager.Direction.Right:
                direction = Vector2.right;
                break;
            case GridManager.Direction.Up:
                direction = Vector2.up;
                break;
            case GridManager.Direction.Down:
                direction = Vector2.down;
                break;
        }
        RaycastHit2D[] results = Physics2D.RaycastAll(transform.position, direction, 100, EnemyTruck);
        List<EnemyTruck> enemyTrucks = new List<EnemyTruck>();

        foreach (RaycastHit2D result in results) // Taking the truck object from the rayCast results
        {
            EnemyTruck truck = result.transform.gameObject.GetComponent<EnemyTruck>();
            enemyTrucks.Add(truck);
        }

        List<EnemyTruck> enemyTrucksInOrder = new List<EnemyTruck>();
        while (enemyTrucks.Count >= 1)
        {
            EnemyTruck closestTruck;
            closestTruck = enemyTrucks[0];
            for (int i = 0; i < enemyTrucks.Count; i++)
            {
                float closestTruckDistance = Vector2.Distance(this.transform.position , closestTruck.transform.position);
                float nextTruckDistance = Vector2.Distance(this.transform.position, enemyTrucks[i].transform.position);

                if(closestTruckDistance < nextTruckDistance || closestTruckDistance == nextTruckDistance)
                {
                    continue;
                }
                else
                {
                    closestTruck = enemyTrucks[i];
                }
            }
            enemyTrucksInOrder.Add(closestTruck);
            enemyTrucks.Remove(closestTruck);

            if(closestTruck as ArmoredTruck != null)
            {
                break;
            }
        }

        foreach(EnemyTruck truck in enemyTrucksInOrder)
        {
            truck.gameObject.GetComponent<BaseHealth>().GiveDamage(1);
        }

        
        return null;
    }

    private void ShootLaser()
    {
        List<EnemyTruck> truckList = GetTargetTrucks();
        if (truckList != null && truckList.Count != 0)
        {
            foreach (EnemyTruck truck in truckList)
            {
                truck.GetComponent<BaseHealth>().GiveDamage(1);
                Debug.Log("Truck Damaged");
            }
        }
    }
    
    public void SetDirection(GridManager.Direction direction)
    {
        this.direction = direction;
        switch(direction)
        {
            case GridManager.Direction.Left:
                directionVisual.transform.rotation = Quaternion.AngleAxis(180,Vector3.forward);
                break;
            case GridManager.Direction.Right:
                directionVisual.transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
                break;
            case GridManager.Direction.Up:
                directionVisual.transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
                break;
            case GridManager.Direction.Down:
                directionVisual.transform.rotation = Quaternion.AngleAxis(-90, Vector3.forward);
                break;
        }
    }

    public override void Interact()
    {
        if(!directionUI.gameObject.activeSelf)
        {
            directionUI.gameObject.SetActive(true);
        }
        else
        {
            directionUI.gameObject.SetActive(false);
        }
    }
}