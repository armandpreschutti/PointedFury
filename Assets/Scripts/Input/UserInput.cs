using System;
using UnityEngine;


public class UserInput : MonoBehaviour
{
    // Player input variables
    private StateMachine _stateMachine;
    private PlayerControls _playerControls;

    public static Action OnPausePressed;
    public static Action OnResetLevelPressed;
    public static Action OnToggleHealthSystemsPressed;
    public static Action OnResetGamePressed;
    public static Action OnDisableEnemiesPresssed;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _stateMachine = GetComponent<StateMachine>();
    }

    // This function is called when the object becomes enabled and active
    private void OnEnable()
    {
        _playerControls.Enable();
        SubscribeToActions();
    }

    // This function is called when the behaviour becomes disabled
    private void OnDisable()
    {
        _playerControls.Disable();
        UnsubscribeFromActions();
    }

    private void Start()
    {

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
        
        if(!_stateMachine.IsHurt && !_stateMachine.IsDashing && !_stateMachine.IsStunned && !_stateMachine.IsHeavyAttackPressed/* && !_stateMachine.IsParrying*/)
        {
            _stateMachine.IsLightAttackPressed = true;
        }
       
        else
        {
            return;
        }
    }

    public void SetHeavyAttackInput(bool value)
    {

        if (!_stateMachine.IsHurt && !_stateMachine.IsDashing && !_stateMachine.IsStunned  && !_stateMachine.IsLightAttackPressed/* && !_stateMachine.IsParrying*/)
        {
            _stateMachine.IsHeavyAttackPressed = true;
        }

        else
        {
            return;
        }
    }

    public void SetDashInput(bool value)
    {
        if (!_stateMachine.IsDashing && _stateMachine.MoveInput != Vector2.zero && !_stateMachine.IsHurt && !_stateMachine.IsStunned && !_stateMachine.IsParrying)
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
        if (!_stateMachine.IsAttacking && !_stateMachine.IsDashing/* && !_stateMachine.IsHurt*/)
        { 
            _stateMachine.OnAttemptParry?.Invoke();
        }
    }

    public void SetPauseInput()
    {
        OnPausePressed?.Invoke();
    }

    public void SetResetLevelInput()
    {
        OnResetLevelPressed?.Invoke();
    }

    public void SetToggleHealthSystemsInput()
    {
        OnToggleHealthSystemsPressed.Invoke();
    }
    public void SetResetGameInput()
    {
        OnResetGamePressed?.Invoke();
    }
    public void SetDisableEnemiesInput()
    {
        OnDisableEnemiesPresssed?.Invoke();
    }

    // Set input values
    private void SubscribeToActions()
    {
        // Set up input actions for player controls
        _playerControls.Player.Move.performed += ctx => SetMoveInput(ctx.ReadValue<Vector2>());
        _playerControls.Player.Look.performed += ctx => SetLookInput(ctx.ReadValue<Vector2>());
        _playerControls.Player.Dash.performed += ctx => SetDashInput(ctx.ReadValueAsButton());
        _playerControls.Player.Block.performed += ctx => SetBlockInput(ctx.ReadValueAsButton());
        _playerControls.Player.Parry.performed += ctx => SetParryInput();
        _playerControls.Player.Pause.performed += ctx => SetPauseInput();
        _playerControls.Player.LightAttack.performed += ctx => SetLightAttackInput(ctx.ReadValueAsButton());
        _playerControls.Player.HeavyAttack.performed += ctx => SetHeavyAttackInput(ctx.ReadValueAsButton());
        _playerControls.Player.ResetLevel.performed += ctx => SetResetLevelInput();
        _playerControls.Player.ToggleHealthSystems.performed += ctx => SetToggleHealthSystemsInput();
        _playerControls.Player.ReturnToTitle.performed += ctx => SetResetGameInput();
        _playerControls.Player.DisableEnemies.performed += ctx => SetDisableEnemiesInput();
    }

    // Set input values
    private void UnsubscribeFromActions()
    {
        // Set up input actions for player controls
        _playerControls.Player.Move.performed -= ctx => SetMoveInput(ctx.ReadValue<Vector2>());
        _playerControls.Player.Look.performed -= ctx => SetLookInput(ctx.ReadValue<Vector2>());
        _playerControls.Player.Dash.performed -= ctx => SetDashInput(ctx.ReadValueAsButton());
        _playerControls.Player.Block.performed -= ctx => SetBlockInput(ctx.ReadValueAsButton());
        _playerControls.Player.Parry.performed -= ctx => SetParryInput();
        _playerControls.Player.Pause.performed -= ctx => SetPauseInput();
        _playerControls.Player.LightAttack.performed -= ctx => SetLightAttackInput(ctx.ReadValueAsButton());
        _playerControls.Player.HeavyAttack.performed -= ctx => SetHeavyAttackInput(ctx.ReadValueAsButton());
        _playerControls.Player.ResetLevel.performed -= ctx => SetResetLevelInput();
        _playerControls.Player.ToggleHealthSystems.performed -= ctx => SetToggleHealthSystemsInput();
        _playerControls.Player.ReturnToTitle.performed -= ctx => SetResetGameInput();
        _playerControls.Player.DisableEnemies.performed -= ctx => SetDisableEnemiesInput();
    }
}
