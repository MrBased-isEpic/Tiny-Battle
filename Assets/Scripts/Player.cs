using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Events
    public event EventHandler OnPlayerModeChanged;
    public BaseCell SelectedCell;
    public event EventHandler OnRoadBuilt;

    // Player State Initialization
    private PlayerBaseState PlayerPlayingMode = new PlayerPlayingMode();
    private PlayerBaseState PlayerBuildingMode = new PlayerBuildingMode();
    private PlayerBaseState PlayerOverviewMode = new PlayerOverviewMode();

    private PlayerBaseState PlayerCurrentState;

    public int money {get ; private set;}

    // Player State enum
    public enum Mode
    {
        PlayingMode,
        BuildingMode,
        OverviewMode
    }

    private Mode mode;
    private void Start()
    {
        SwitchMode(Mode.PlayingMode);
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
    }

    private void GameManager_OnStateChanged(object sender, EventArgs e)
    {
        if(GameManager.Instance.GetCurrentState() == GameManager.State.Preparation)
        {
            PlayerBuildingMode state = PlayerBuildingMode as PlayerBuildingMode;
            state.isFirstRoad = true;
        }
    }

    private void Update()
    {
        PlayerCurrentState.UpdateState(this);
    }
    public void SwitchMode(Mode mode) // Public Function to change the player state
    {
        if(GameManager.Instance.state == GameManager.State.GamePlaying && mode == Mode.BuildingMode)
        {
            return;
        }


        this.mode = mode;
        OnPlayerModeChanged?.Invoke(this,EventArgs.Empty);

        if(PlayerCurrentState != null)
        {
            PlayerCurrentState.EndState(this);
        }

        switch(mode)
        {
            case Mode.PlayingMode:    
                PlayerCurrentState = PlayerPlayingMode;
                PlayerCurrentState.StartState(this);
                break;
            case Mode.BuildingMode:
                PlayerCurrentState = PlayerBuildingMode;
                PlayerCurrentState.StartState(this);
                break;
            case Mode.OverviewMode:
                PlayerCurrentState = PlayerOverviewMode;
                PlayerOverviewMode.StartState(this);
                break;
        }

    }

    public void SetOverviewDraggedCell(Transform buildingCell)
    {
        if(mode == Mode.OverviewMode)
        {
            PlayerOverviewMode OverviewState = PlayerCurrentState as PlayerOverviewMode;
            Transform cellTransform = buildingCell;
            OverviewState.SetDraggedBuilding(buildingCell);
        }
    }
    public Transform InstantiateObject(Transform transform, Vector2 position)
    {
        return Instantiate(transform, position, Quaternion.identity);
    }
    public void DestroyObject(GameObject gameObject)
    {
        Destroy(gameObject);
    }
    public Mode GetMode()
    {
        return mode;
    }
    public void SetSelectedCell(BaseCell cell) // Tells the BaseCell to show the selectedCellVisual
    {
        if (cell != null)
        {
            if (SelectedCell != null)
            {
                SelectedCell.HideSelectedVisual();
            }

            SelectedCell = cell;
            SelectedCell.ShowSelectedVisual();
        }
    }
    public void AddMoney(int amount)
    {
        money += amount;
    }
    public void TakeMoney(int amount)
    {
        money -= amount;
    }
    public void RoadBuilt()
    {
        OnRoadBuilt?.Invoke(this, EventArgs.Empty);
    }
}
