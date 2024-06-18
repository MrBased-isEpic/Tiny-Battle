using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingShopUI : MonoBehaviour
{
    [SerializeField] private OverviewModeUI overviewModeUI;
    [SerializeField] private Button DefenseBuildingButton;
    [SerializeField] private Button GarageBuildingButton;
    [SerializeField] private Button MoneyGenBuildingButton;
    private void Awake()
    {
        DefenseBuildingButton.onClick.AddListener(()=>
        {
            GameManager.Instance.player.SetOverviewDraggedCell(GridManager.Instance.GetBuildingCellFromArray(GridManager.Instance.GetIndexOfBuildingCell(GridManager.BuildingCellEnum.LaserShootDefenseCell)).transform);
            Hide();
        });
        GarageBuildingButton.onClick.AddListener(()=>
        {
            GameManager.Instance.player.SetOverviewDraggedCell(GridManager.Instance.GetBuildingCellFromArray(GridManager.Instance.GetIndexOfBuildingCell(GridManager.BuildingCellEnum.GarageCell)).transform);
            Hide();
        });
        MoneyGenBuildingButton.onClick.AddListener(()=>
        {
            GameManager.Instance.player.SetOverviewDraggedCell(GridManager.Instance.GetBuildingCellFromArray(GridManager.Instance.GetIndexOfBuildingCell(GridManager.BuildingCellEnum.MoneyGenCell)).transform);
            Hide();
        });
    }  

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
