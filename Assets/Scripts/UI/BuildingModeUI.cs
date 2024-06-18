using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingModeUI : MonoBehaviour
{
    [SerializeField] private Button BaseBuildingButton;
    [SerializeField] private Button RoadBuildingButton;
    [SerializeField] private Button RoadDestroyingButton;

    public event EventHandler OnBaseBuildingModeButtonClicked;
    public event EventHandler OnRoadBuildingModeButtonClicked;
    public event EventHandler OnRoadDestroyingButtonClicked;
    private void Awake()
    {
        BaseBuildingButton.onClick.AddListener(() =>
        {
            OnBaseBuildingModeButtonClicked.Invoke(this, EventArgs.Empty);
        });
        RoadBuildingButton.onClick.AddListener(() =>
        {
            OnRoadBuildingModeButtonClicked.Invoke(this, EventArgs.Empty);
            ShowRoadDestroyingButton();
        });
        RoadDestroyingButton.onClick.AddListener(() =>
        {
            OnRoadDestroyingButtonClicked.Invoke(this, EventArgs.Empty);
            HideRoadDestroyingButton();
        });
    }
    private void Start()
    {
        HideRoadDestroyingButton();
    }
    public void ShowRoadDestroyingButton()
    {
        RoadBuildingButton.gameObject.SetActive(false);
        RoadDestroyingButton.gameObject.SetActive(true);
    }
    public void HideRoadDestroyingButton()
    {
        RoadBuildingButton.gameObject.SetActive(true);
        RoadDestroyingButton.gameObject.SetActive(false);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
