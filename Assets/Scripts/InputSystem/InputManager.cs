using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;

    public static InputManager Instance
    {
        get { return instance; }
    }

    private PlayerControls playerControls;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public Vector2 GetPlayerMovement()
    {
        return playerControls.Player.Movement.ReadValue<Vector2>();
    }

    public Vector2 GetMousePos()
    {
        return playerControls.Player.MousePosition.ReadValue<Vector2>();
    }

    public bool getLeftClickStatus()
    {
        bool pressed = playerControls.Player.Attack.ReadValue<float>() == 1 ? true : false;
        return pressed;
    }

    public bool DashButtonPressed()
    {
        return playerControls.Player.Dash.triggered;
    }

    public bool PauseResumeButtonPressed()
    {
        return playerControls.Player.PauseResume.triggered;
    }

    
}
