using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance;

    [SerializeField] private Camera cam;
    private Vector2 MousePos;
    private Vector2 MouseWorldPosRounded;
    private float MouseHoldTime = 0.2f;
    private float mouseHoldTimer;

    public event EventHandler OnMouseWorldPosChanged;
    public event EventHandler OnMouseClicked;
    public event EventHandler OnMouseHeldDown;
    public event EventHandler OnMouseReleased;
    public event EventHandler OnMousePressed;
    public event EventHandler OnPauseButtonPressed;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        mouseHoldTimer = MouseHoldTime;
    }

    private void Update()
    {
        switch(GameManager.Instance.GetCurrentState())
        {
            case GameManager.State.Preparation:
                TrackMouseClick();
                TrackMouseWorldPosition();
                break;
            case GameManager.State.GamePlaying:
                TrackMouseClick();
                TrackMouseWorldPosition();
                TrackKeyboard();
                break;
            case GameManager.State.GamePaused:
                TrackKeyboard();
                break;
        }
    }

    private void TrackMouseClick() // Tracks whatever the left mouse button is doing
    {
        if(Input.GetMouseButtonDown(0))
        {
            OnMousePressed?.Invoke(this, EventArgs.Empty);
        }
        if (Input.GetMouseButton(0))
        {
            if(mouseHoldTimer > 0)
            {
                mouseHoldTimer -= Time.deltaTime;
            }
            else
            {
                OnMouseHeldDown?.Invoke(this, EventArgs.Empty);
            }      
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (mouseHoldTimer > 0)
            {
                OnMouseClicked?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                OnMouseReleased?.Invoke(this, EventArgs.Empty);
            }
            mouseHoldTimer = MouseHoldTime;
        }
    }
    private void TrackMouseWorldPosition() // Tracks the cursor position with respect to the grid world
    {
        MousePos = Input.mousePosition;
        Vector2 NewMouseWorldPos = cam.ScreenToWorldPoint(new Vector3(MousePos.x, MousePos.y, 0));  
        NewMouseWorldPos.x = Mathf.Round(NewMouseWorldPos.x);
        NewMouseWorldPos.y = Mathf.Round(NewMouseWorldPos.y);

        if(NewMouseWorldPos != MouseWorldPosRounded)
        {
            //Debug.Log(NewMouseWorldPos);
            MouseWorldPosRounded = NewMouseWorldPos;
            OnMouseWorldPosChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    public Vector2 GetMouseWorldPositionRounded() // returns the current grid's position on which the mouse is hovering
    {
        return MouseWorldPosRounded; 
    }
    private void TrackKeyboard() // Tracks keyboard buttons
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            OnPauseButtonPressed?.Invoke(this, EventArgs.Empty);
        }
    }
}
