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

    public Vector2 _targetMoveInput;
    public float _moveInputSmoothTime = 0.1f; // Adjust this value to change the smoothing speed
    public Vector2 _currentMoveInputVelocity;

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

    private void Update()
    {
        _stateMachine.MoveInput = Vector2.Lerp(_stateMachine.MoveInput, _targetMoveInput, Time.deltaTime * _moveInputSmoothTime);
        if (_stateMachine.MoveInput.magnitude < 0.01f) 
        {
            _stateMachine.MoveInput = Vector2.zero;
        }
    }


    // Set move input value
    public void SetMoveInput(Vector2 value)
    {
        _targetMoveInput = value;
    }

    // Set look input value
    public void SetLookInput(Vector2 value)
    {
        _stateMachine.LookInput = value;
    }

    public void SetLightAttackInput(bool value)
    {
        
        if(!_stateMachine.IsHurt && !_stateMachine.IsStunned && !_stateMachine.IsHeavyAttackPressed)
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

        if (!_stateMachine.IsHurt && !_stateMachine.IsStunned  && !_stateMachine.IsLightAttackPressed)
        {
            _stateMachine.IsHeavyAttackPressed = true;
        }

        else
        {
            return;
        }
    }

    public void SetEvadeInput(bool value)
    {
        if (!_stateMachine.IsEvading  && !_stateMachine.IsStunned && !_stateMachine.IsParrying)
        {
            _stateMachine.OnAttemptEvade?.Invoke();
        }
        else
        {
            return;
        }
    }
    public void SetDashInput(bool value)
    {
        if (!_stateMachine.IsHurt && !_stateMachine.IsStunned && !_stateMachine.IsDashing)
        {
            _stateMachine.IsDashPressed = value;
        }
     
    }

    public void SetBlockInput(bool value)
    {
        if(!_stateMachine.IsStunned && !_stateMachine.IsDashing)
        {
            _stateMachine.IsBlockPressed = value;
        }
    }

    public void SetParryInput()
    {
        if (!_stateMachine.IsAttacking && !_stateMachine.IsEvading)
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
        _playerControls.Player.Evade.performed += ctx => SetEvadeInput(ctx.ReadValueAsButton());
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
        _playerControls.Player.Evade.performed += ctx => SetEvadeInput(ctx.ReadValueAsButton());
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
