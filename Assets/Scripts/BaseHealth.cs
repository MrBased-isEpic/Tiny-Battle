using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseHealth : MonoBehaviour
{
    //Reference
    //UI elements
    [SerializeField] private HealthVisual healthVisual;

    //Public Events
    public event EventHandler HealthIsZero;
    public event EventHandler HealthIsNotZero;

    //Health Variables
    private int MaxHealth = 0;
    private int currentHealth;


    //Polish Variable


    private void Awake()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
    }

    private void Start()
    {
        //if(GameManager.Instance.GetCurrentState() == GameManager.State.Preparation || GameManager.Instance.GetCurrentState() == GameManager.State.GamePlaying)
        //{
        //    Debug.Log(gameObject + "Reset Health in Start");
        //    ResetHealth();
        //}
    }

    public void GiveDamage(int damage)
    {
        if(currentHealth <= 0)
        {
            HealthIsZero?.Invoke(this, EventArgs.Empty);
            return;
        }
        currentHealth -= damage;
        healthVisual.ChangeHealthVisual(currentHealth);
    }

    private void GameManager_OnStateChanged(object sender, EventArgs e)
    {
        if(GameManager.Instance.GetCurrentState() == GameManager.State.Preparation || GameManager.Instance.GetCurrentState() == GameManager.State.GamePlaying)
        {
            ResetHealth();
        }
    }
    public void SetMaxHealth(int health)
    {
        MaxHealth = health;     
        currentHealth = MaxHealth;
        ResetHealth();
        healthVisual.ChangeHealthVisual(currentHealth); 
    }

    private void ResetHealth()
    {
        currentHealth = MaxHealth;
        GiveDamage(0);
        HealthIsNotZero?.Invoke(this, EventArgs.Empty);
    }
}
