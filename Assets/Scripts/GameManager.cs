using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] public Player player;
    [SerializeField] public Transform PlayerTruck;
    [SerializeField] public Transform[] EnemyTrucks;
    public event EventHandler OnStateChanged;
    public event EventHandler onMoneyValueChanged;
    // Game Stats
    [SerializeField] private int StartMoneyAmount;
    [SerializeField] public int RoadCellCost;
    public int Money 
    {
        get;
        private set;
    }
    // Map Objects references
    [SerializeField] public List<BuildingCell> mapBuildingCells {get; private set;} = new List<BuildingCell>();
    [SerializeField] public List<RoadCell> mapRoadCells{get; private set;} = new List<RoadCell>();
    [SerializeField] public List<Truck> mapTrucks{get;private set;} = new List<Truck>();
    // UI Elements
    [SerializeField] private OverviewModeUI OverviewUI;
    [SerializeField] public BuildingModeUI buildingModeUI;
    [SerializeField] private TimerUI timerUI;
    [SerializeField] private Button PreparationReadyButton;
    [SerializeField] private ErrorTextUI errorTextUI;
    public enum State
    {
        CountdownToPreparation,
        Preparation,
        GamePlaying,
        GamePaused
    }
    public State state {get; private set;} = State.CountdownToPreparation;

    //Countdown to Prep Variables
    private float timer;
    [SerializeField]private float timerToPrepMax;
    [SerializeField]private float timerToGameMax;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SwitchState(State.CountdownToPreparation);
        PreparationReadyButton.onClick.AddListener(()=> 
        {
            if(CheckIfReadyForGame())
            {
                SwitchState(State.GamePlaying);
            }
        }
        );

        GameInput.Instance.OnPauseButtonPressed += GameInput_OnPauseButtonPressed;
        Money = StartMoneyAmount;
    }

    private void GameInput_OnPauseButtonPressed(object sender, EventArgs e)
    {
        if(GetCurrentState() == State.GamePlaying)
        {
            SwitchState(State.GamePaused);
        }
        else
        {
            SwitchState(State.GamePlaying);
        }
    }

    private void Update()
    {
        switch(state)
        {
            case State.CountdownToPreparation:
                timer -= Time.deltaTime;

                if(timer < 0 )
                {
                    SwitchState(State.Preparation);
                }
                break;
            case State.Preparation:
                break;
            case State.GamePlaying:
                break;
        }
    }

    private void SwitchState(State state) // Function to Switch Game States
    {
        this.state = state;

        switch(state)
        {
            case State.CountdownToPreparation:
                timerUI.gameObject.SetActive(true);
                timer = timerToPrepMax;
                PreparationReadyButton.gameObject.SetActive(false);
                break;
            case State.Preparation:
                GridManager.Instance.GenerateGrid();
                timerUI.gameObject.SetActive(false);
                PreparationReadyButton.gameObject.SetActive(true);
                break;
            case State.GamePlaying:
                player.SwitchMode(Player.Mode.PlayingMode);
                PreparationReadyButton.gameObject.SetActive(false);
                break;
            case State.GamePaused:
                player.SwitchMode(Player.Mode.PlayingMode);
                break;
        }
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }
    public State GetCurrentState()
    {
        return state;
    }
    public void AddBuildingCellToList(BuildingCell buildingCell)
    {
        mapBuildingCells.Add(buildingCell);
    }
    public void RemoveBuildingCellFromList(BuildingCell buildingCell)
    {
        mapBuildingCells.Remove(buildingCell);
    }
    public void AddRoadCellToList(RoadCell roadCell)
    {
        mapRoadCells.Add(roadCell);
    }
    public void AddTruckToList(Truck truck)
    {
        mapTrucks.Add(truck);
    }

    public void RemoveRoadCellFromList(RoadCell roadCell)
    {
        mapRoadCells.Remove(roadCell);
    }

    public void TurnOnOverviewVisuals()
    {
        foreach(BuildingCell buildingCell in mapBuildingCells)
        {
            buildingCell.TurnOnHealthVisual();
        }
        foreach(Truck truck in mapTrucks)
        {
            truck.TurnOnHealthVisual();
        }
        OverviewUI.gameObject.SetActive(true);
        OverviewUI.EnterShopButtonsUI();
    }
    public void TurnOffOverviewVisuals()
    {
        foreach(BuildingCell buildingCell in mapBuildingCells)
        {
            buildingCell.TurnOffHealthVisual();
        }
        foreach (Truck truck in mapTrucks)
        {
            truck.TurnOffHealthVisual();
        }
        OverviewUI.gameObject.SetActive(false);
    }

    public void TurnOnBuildingModeVisuals()
    {
        buildingModeUI.Show();
    }
    public void TurnOffBuildingModeVisuals()
    {
        buildingModeUI.Hide();
    }
    public BuildingModeUI GetBuildingModeUI()
    {
        return buildingModeUI;
    }
    public int GetTimeLeftRounded()
    {
        return (int)timer;
    }
    private bool CheckIfReadyForGame()
    {
        bool isReady = true;
        foreach(BuildingCell buildingCell in mapBuildingCells)
        {
            if(!buildingCell.IsReady())
            {
                errorTextUI.DisplayError(buildingCell.ErrorMessage);
                isReady = false;
            }      
        }
        return isReady;
    }
    public void RefreshVisuals()
    {
        foreach(BuildingCell buildingCell in mapBuildingCells)
        {
            buildingCell.CheckRoutes();
        }
    }

    public void AddToMoney(int amount)
    {
        Money += amount;
        onMoneyValueChanged?.Invoke(this,EventArgs.Empty);
    }
    public void TakeFromMoney(int amount)
    {
        Money -= amount;
        onMoneyValueChanged?.Invoke(this,EventArgs.Empty);
    }
}
