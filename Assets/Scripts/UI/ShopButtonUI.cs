using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButtonUI : MonoBehaviour
{
    [SerializeField] private OverviewModeUI overviewModeUI;
    [SerializeField] private Button BuildingShopButton;
    [SerializeField] private Button DefenseShopButton;
    [SerializeField] private BuildingShopUI buildingShopUI;
    private void Awake()
    {
        BuildingShopButton.onClick.AddListener(()=>
        {
            overviewModeUI.EnterBuildingShopUI();
        });
        DefenseShopButton.onClick.AddListener(() =>
        {
            overviewModeUI.EnterDefenseShopUI();
        });
    }
}
