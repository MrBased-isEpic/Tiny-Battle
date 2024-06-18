using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefenseShopUI : MonoBehaviour
{
    [SerializeField] private OverviewModeUI overviewModeUI;
    [SerializeField] private Button RocketShootDefenseCellButton;
    [SerializeField] private Button AreaEffectDefenseCellButton;
    [SerializeField] private Button LaserShootDefenseCellButton;
    private void Awake()
    {
        RocketShootDefenseCellButton.onClick.AddListener(()=>
        {
            GameManager.Instance.player.SetOverviewDraggedCell(GridManager.Instance.GetBuildingCellFromArray(GridManager.Instance.GetIndexOfBuildingCell(GridManager.BuildingCellEnum.RocketShootDefenseCell)).transform);
            Hide();
        });
        AreaEffectDefenseCellButton.onClick.AddListener(()=>
        {
            GameManager.Instance.player.SetOverviewDraggedCell(GridManager.Instance.GetBuildingCellFromArray(GridManager.Instance.GetIndexOfBuildingCell(GridManager.BuildingCellEnum.AreaEffectDefenseCell)).transform);
            Hide();
        });
        LaserShootDefenseCellButton.onClick.AddListener(()=>
        {
            GameManager.Instance.player.SetOverviewDraggedCell(GridManager.Instance.GetBuildingCellFromArray(GridManager.Instance.GetIndexOfBuildingCell(GridManager.BuildingCellEnum.LaserShootDefenseCell)).transform);
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
