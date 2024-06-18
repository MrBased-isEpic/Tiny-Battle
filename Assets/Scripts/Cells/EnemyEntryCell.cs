using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEntryCell : RoadCell
{
    private float spawnRate = .2f;
    private float timer;
    private void Awake()
    {
        timer = 1 / spawnRate;
    }
    private void Update()
    {
        if(GameManager.Instance.GetCurrentState() == GameManager.State.GamePlaying)
        {
            timer -= Time.deltaTime;
            if(timer < 0)
            {
                Transform Truck = GameManager.Instance.EnemyTrucks[Random.Range(0, GameManager.Instance.EnemyTrucks.Length)];
                Truck truck = Instantiate(Truck, transform.position, Quaternion.identity).GetComponent<EnemyTruck>();
                timer = 1/spawnRate;
            }
        }
    }
}
