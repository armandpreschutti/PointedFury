using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    // Player input variables
    private StateMachine _stateMachine;
    private PlayerControls _playerControls;

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
        
        if (_stateMachine.AttackType == 0 || _stateMachine.CanComboAttack)
        {
            _stateMachine.IsLightAttackPressed = value;
        }
        else
        {
            return;
        }
    }

    // Set input values
    private void SetInputValues()
    {
        // Set up input actions for player controls
        _playerControls.Player.Move.performed += ctx => SetMoveInput(ctx.ReadValue<Vector2>());
        _playerControls.Player.Look.performed += ctx => SetLookInput(ctx.ReadValue<Vector2>());
        _playerControls.Player.LightAttack.performed += ctx => SetLightAttackInput(ctx.ReadValueAsButton());
    }
}
