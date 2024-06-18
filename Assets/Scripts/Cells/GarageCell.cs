using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageCell : BuildingCell
{
    [SerializeField] private Transform PlayerTruck;
    private float spawnRate = .2f;
    private float timer;
    Vector3 spawnPos;

    private new void Awake()
    {
        base.Awake();
        ErrorMessage = "GarageCell not connected to ExitRoad!";

        //Game State Events
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;

        PlayerTruck = GameManager.Instance.PlayerTruck;
        timer = 1 / spawnRate;
        
        //Debug.Log("VehicleMakerCell position: " + transform.position);
    }

    private void GameManager_OnStateChanged(object sender, EventArgs e)
    {
        if(GameManager.Instance.GetCurrentState() == GameManager.State.GamePlaying)
        {
            RoadCell neighbourRoadCell = GetNeighbourRoadCellList()[0];
            spawnPos = neighbourRoadCell.transform.position;
        }
    }

    private void Update()
    {
        if(GameManager.Instance.GetCurrentState() == GameManager.State.GamePlaying && !IsDestroyed())
        {
            timer -= Time.deltaTime;
            if(timer < 0)
            {     
                Truck truck = Instantiate(PlayerTruck, spawnPos, Quaternion.identity).GetComponent<PlayerTruck>();
                //truck.SetDestination();
                timer = 1/spawnRate;
            }
        }
    }

    // Inherited Override Functions
    public override void CheckRoutes()
    {
        if (GetNeighbourRoadCellList().Count > 0)
        {
            if (GridManager.Instance.FindPath(GetNeighbourRoadCellList()[0], GameManager.Instance.mapRoadCells[0]))
            {
                Debug.Log(GetNeighbourRoadCellList()[0].transform.position);
                TurnOffCantPlaceHereVisual();
                Debug.Log("GarageCell turns off Visual");
            }
            else
            {
                TurnOnCantPlaceHereVisual();
            }
        }
        else
        {
            TurnOnCantPlaceHereVisual();
        }
    }
    public override bool IsReady()
    {
        if (cantPlaceHereVisual.gameObject.activeSelf == true)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
