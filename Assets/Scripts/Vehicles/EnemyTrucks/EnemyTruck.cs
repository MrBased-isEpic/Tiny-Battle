using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTruck : Truck
{
    protected BuildingCell targetBuildingCell { get; private set; }
    [SerializeField] private float AttackRate;
    private float attackTimer;
    [SerializeField] private int Damage;

    private new void Start()
    {
        base.Start();
        base.baseHealth.HealthIsZero += BaseHealth_HealthIsZero;
    }

    private void BaseHealth_HealthIsZero(object sender, System.EventArgs e)
    {
        DestroyTruck();
    }

    protected override void SetDestination()
    {
        // Choose Target
        BuildingCell buildingCell = GameManager.Instance.mapBuildingCells[UnityEngine.Random.Range(0,GameManager.Instance.mapBuildingCells.Count - 1)];

        // Get RoadCell to Drive to
        List<RoadCell> neighbourRoadCells = new List<RoadCell>();
        neighbourRoadCells = buildingCell.GetNeighbourRoadCellList();
        targetBuildingCell = buildingCell;
        Destination = neighbourRoadCells[0];
    }

    protected override void Attack()
    {
        if(targetBuildingCell.IsDestroyed())
        {
            Debug.Log("Building is already destroyed");
            this.SetDestination();
            SwitchState(TruckState.PathFinding);
            Debug.Log("Moving to another building");
            return;
        }
        attackTimer -= Time.deltaTime;
        if (attackTimer < 0)
        {
            attackTimer = 1 / AttackRate;
            targetBuildingCell.GetComponent<BaseHealth>().GiveDamage(Damage);
        }
    }

    public override void SwitchState(TruckState truckState)
    {
        this.truckState = truckState;

        switch (truckState)
        {
            case TruckState.Moving:
                currentPos = path[0].transform.position;
                goingToCell = path[1];
                CheckDirection(GridManager.Instance.GetCellFromPosition(transform.position) as RoadCell, goingToCell);
                break;

            case TruckState.Waiting:
                break;

            case TruckState.Attacking:
                attackTimer = 1 / AttackRate;
                break;
        }
    }

    private void DestroyTruck()
    {
        GameManager.Instance.mapTrucks.Remove(this);
        Destroy(this.gameObject);
    }
}
