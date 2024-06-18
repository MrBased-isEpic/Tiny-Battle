using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCell : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private bool isWater;

    public void ToggleWaterState()
    {
        isWater = !isWater;
    }
}
