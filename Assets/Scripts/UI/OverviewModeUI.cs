using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OverviewModeUI : MonoBehaviour
{
    [SerializeField] private Transform ShopButtonUI;
    [SerializeField] private Transform BuildingShopUI;
    [SerializeField] private Transform DefenseShopUI;
    [SerializeField] private MoneyCounter moneyCounter;

    private void OnEnable()
    {
        GameManager.Instance.onMoneyValueChanged += GameManager_onMoneyValueChanged;
        moneyCounter.ShowMoneyAmount(GameManager.Instance.Money);
    }

    private void OnDisable()
    {
        GameManager.Instance.onMoneyValueChanged -= GameManager_onMoneyValueChanged;
    }
    
    private void GameManager_onMoneyValueChanged(object sender, EventArgs e)
    {
        moneyCounter.ShowMoneyAmount(GameManager.Instance.Money);
    }

    public void EnterShopButtonsUI()
    {
        ShopButtonUI.gameObject.SetActive(true);
        DefenseShopUI.gameObject.SetActive(false);
        BuildingShopUI.gameObject.SetActive(false);
    }
    public void EnterBuildingShopUI()
    {
        ShopButtonUI.gameObject.SetActive(false);
        DefenseShopUI.gameObject.SetActive(false);
        BuildingShopUI.gameObject.SetActive(true);
    }
    public void EnterDefenseShopUI()
    {
        ShopButtonUI.gameObject.SetActive(false);
        DefenseShopUI.gameObject.SetActive(true);
        BuildingShopUI.gameObject.SetActive(false);
    }
}
