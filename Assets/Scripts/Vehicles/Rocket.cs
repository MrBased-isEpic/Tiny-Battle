using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private EnemyTruck targetTruck;
    [SerializeField] private float speed;

    public void SetTruck(EnemyTruck truck)
    {
        targetTruck = truck;
    }

    private void Update()
    {
        if(GameManager.Instance.GetCurrentState() == GameManager.State.GamePlaying)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetTruck.transform.position, speed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<EnemyTruck>() != null)
        {
            if(collision.GetComponent<EnemyTruck>() == targetTruck)
            {
                targetTruck.GetComponent<BaseHealth>().GiveDamage(1);
                Destroy(this.gameObject);
            }
        }
    }
}
