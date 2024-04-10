using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class UserInput : MonoBehaviour
{
    // Player input variables
    private StateMachine _stateMachine;
    private PlayerControls _playerControls;

    public Action OnPausePressed;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _stateMachine = GetComponent<StateMachine>();
    }

    // This function is called when the object becomes enabled and active
    private void OnEnable()
    {
        _playerControls.Enable();
    }

    // This function is called when the behaviour becomes disabled
    private void OnDisable()
    {
        _playerControls.Disable();
    }

    private void Start()
    {
        SetInputValues();
    }

    // Set move input value
    public void SetMoveInput(Vector2 value)
    {
        _stateMachine.MoveInput = value;
    }

    // Set look input value
    public void SetLookInput(Vector2 value)
    {
        _stateMachine.LookInput = value;
    }

    public void SetLightAttackInput(bool value)
    {
        
        if(!_stateMachine.IsHurt && !_stateMachine.IsDashing && !_stateMachine.IsStunned && !_stateMachine.IsAttacking && !_stateMachine.IsBlocking)
        {
            _stateMachine.IsLightAttackPressed = true;
        }
       
        else
        {
            return;
        }
    }
    public void SetDashInput(bool value)
    {
        if (!_stateMachine.IsDashing && !_stateMachine.IsAttacking && _stateMachine.MoveInput != Vector2.zero && !_stateMachine.IsHurt && !_stateMachine.IsStunned)
        {
            _stateMachine.IsDashPressed = value;
        }

        else
        {
            return;
        }
    }
    public void SetBlockInput(bool value)
    {
        _stateMachine.IsBlockPressed = value;      
    }

    public void SetParryInput()
    {
        if (!_stateMachine.IsAttacking && !_stateMachine.IsDashing)
        {
            _stateMachine.OnAttemptParty?.Invoke();
        }
    }

    public void SetPauseInput()
    {
        Debug.Log("Button Hooked up Correctly");
        OnPausePressed?.Invoke();
    }

    // Set input values
    private void SetInputValues()
    {
        // Set up input actions for player controls
        _playerControls.Player.Move.performed += ctx => SetMoveInput(ctx.ReadValue<Vector2>());
        _playerControls.Player.Look.performed += ctx => SetLookInput(ctx.ReadValue<Vector2>());
        _playerControls.Player.LightAttack.performed += ctx => SetLightAttackInput(ctx.ReadValueAsButton());
        _playerControls.Player.Dash.performed += ctx => SetDashInput(ctx.ReadValueAsButton());
        _playerControls.Player.Block.performed += ctx => SetBlockInput(ctx.ReadValueAsButton());
        _playerControls.Player.Parry.performed += ctx => SetParryInput();
        _playerControls.Player.Pause.performed += ctx => SetPauseInput();
    }
}
