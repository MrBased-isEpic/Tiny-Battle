using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthVisual : MonoBehaviour
{
    [SerializeField] private UnityEngine.Sprite[] numberSprites;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ChangeHealthVisual(int currentHealth)
    {
        if(currentHealth >= 0 && currentHealth <= 9)
            spriteRenderer.sprite = numberSprites[currentHealth];
    }
}
