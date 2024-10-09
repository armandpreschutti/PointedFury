using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class UserInput : MonoBehaviour
{
    // Player input variables
    private StateMachine _stateMachine;
    public StaminaSystem staminaSystem;
    private PlayerControls _playerControls;
    public bool isPaused;
    public static Action OnPausePressed;
    public static Action OnResetLevelPressed;
    public static Action OnToggleHealthSystemsPressed;
    public static Action OnResetGamePressed;
    public static Action OnDisableEnemiesPresssed;
    public static Action OnInputError;
    public Vector2 _targetMoveInput;
    public float _moveInputSmoothTime = 0.1f; // Adjust this value to change the smoothing speed
    public Vector2 _currentMoveInputVelocity;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _stateMachine = GetComponent<StateMachine>();
        if(GetComponent<StaminaSystem>() != null)
        {
            staminaSystem = GetComponent<StaminaSystem>();
        }
    }

    // This function is called when the object becomes enabled and active
    private void OnEnable()
    {
        TutoiralManager.OnEnableControl += EnablePlayerControl;
        PracticeManager.OnEnableControl += EnablePlayerControl;
        PauseMenuController.OnGamePaused += PausePlayerControl;
        _playerControls.Enable();
        SubscribeToActions();
    }

    // This function is called when the behaviour becomes disabled
    private void OnDisable()
    {
        TutoiralManager.OnEnableControl -= EnablePlayerControl;
        PracticeManager.OnEnableControl -= EnablePlayerControl;
        PauseMenuController.OnGamePaused -= PausePlayerControl;
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
    public void EnablePlayerControl()
    {
        Debug.Log("Function Called on UserInput");
        this.enabled = true;
    }
    public void PausePlayerControl(bool value)
    {
       isPaused = value;
    }

    // Set move input value
    public void SetMoveInput(Vector2 value)
    {
        if (!isPaused)
        {
            _targetMoveInput = value;
        }
        else
        {
            _targetMoveInput = Vector2.zero;
        }

    }

    // Set look input value
    public void SetLookInput(Vector2 value)
    {
        if(!isPaused && !_stateMachine.IsAttacking)
        {
            _stateMachine.LookInput = value;
        }
        else
        {
            _stateMachine.LookInput = Vector2.zero;
        }

    }

    public void SetLightAttackInput(bool value)
    {
        if(!isPaused)
        {
            if (!_stateMachine.IsHurt && !_stateMachine.IsStunned && !_stateMachine.IsHeavyAttackPressed && !staminaSystem.isDepleted)
            {
                _stateMachine.IsLightAttackPressed = true;
            }
            else if (staminaSystem.isDepleted)
            {
                OnInputError?.Invoke();
            }
            else
            {
                return;
            }
        }
        
    }

    public void SetHeavyAttackInput(bool value)
    {
        if (!isPaused)
        {
            if (!_stateMachine.IsHurt && !_stateMachine.IsStunned && !_stateMachine.IsLightAttackPressed & !staminaSystem.isDepleted)
            {
                _stateMachine.IsHeavyAttackPressed = true;
            }
            else if (staminaSystem.isDepleted)
            {
                OnInputError?.Invoke();
            }
            else
            {
                return;
            }
        }
       
    }

    public void SetEvadeInput(bool value)
    {
        if (!isPaused)
        {
            if (!_stateMachine.IsEvading && !_stateMachine.IsStunned && !_stateMachine.IsParrying && !_stateMachine.IsDeflecting && !_stateMachine.IsDashing)
            {
                _stateMachine.OnAttemptEvade?.Invoke();
            }
            else
            {
                return;
            }
        }
     
    }
    public void SetDashInput(bool value)
    {
        if (!isPaused)
        {
            if (!_stateMachine.IsHurt && !_stateMachine.IsStunned && !_stateMachine.IsDashing && !staminaSystem.isDepleted)
            {
                _stateMachine.IsDashPressed = value;
            }
            else if (staminaSystem.isDepleted)
            {
                OnInputError?.Invoke();
            }
        }
    }

    public void SetSprintInput(bool value)
    {
        if (!isPaused)
        {
            _stateMachine.IsSprintPressed = value;
        }
    }

    public void SetBlockInput(bool value)
    {
        if (!isPaused)
        {
            _stateMachine.IsBlockPressed = value;
        }
    }

    public void SetParryInput()
    {
        if(!isPaused)
        {
            if (/*!_stateMachine.IsEvading && */!_stateMachine.IsDeflecting)
            {
                _stateMachine.OnAttemptParry?.Invoke();
            }
            else if (staminaSystem.isDepleted)
            {
                OnInputError?.Invoke();
            }
            else
            {
                return;
            }
        }
    }

    public void SetResetLevelInput()
    {
        OnResetLevelPressed?.Invoke();
    }


    // Set input values
    private void SubscribeToActions()
    {
        // Set up input actions for player controls
        _playerControls.Player.Move.performed += ctx => SetMoveInput(ctx.ReadValue<Vector2>());
        _playerControls.Player.Look.performed += ctx => SetLookInput(ctx.ReadValue<Vector2>());
        _playerControls.Player.Dash.performed += ctx => SetDashInput(ctx.ReadValueAsButton());
        _playerControls.Player.Sprint.performed += ctx => SetSprintInput(ctx.ReadValueAsButton());
        _playerControls.Player.Evade.performed += ctx => SetEvadeInput(ctx.ReadValueAsButton());
        _playerControls.Player.Block.performed += ctx => SetBlockInput(ctx.ReadValueAsButton());
        _playerControls.Player.Parry.performed += ctx => SetParryInput();
        _playerControls.Player.LightAttack.performed += ctx => SetLightAttackInput(ctx.ReadValueAsButton());
        _playerControls.Player.HeavyAttack.performed += ctx => SetHeavyAttackInput(ctx.ReadValueAsButton());
    }

    // Set input values
    private void UnsubscribeFromActions()
    {
        // Set up input actions for player controls
        _playerControls.Player.Move.performed -= ctx => SetMoveInput(ctx.ReadValue<Vector2>());
        _playerControls.Player.Look.performed -= ctx => SetLookInput(ctx.ReadValue<Vector2>());
        _playerControls.Player.Dash.performed -= ctx => SetDashInput(ctx.ReadValueAsButton());
        _playerControls.Player.Evade.performed -= ctx => SetEvadeInput(ctx.ReadValueAsButton());
        _playerControls.Player.Block.performed -= ctx => SetBlockInput(ctx.ReadValueAsButton());
        _playerControls.Player.Parry.performed -= ctx => SetParryInput();
        _playerControls.Player.LightAttack.performed -= ctx => SetLightAttackInput(ctx.ReadValueAsButton());
        _playerControls.Player.HeavyAttack.performed -= ctx => SetHeavyAttackInput(ctx.ReadValueAsButton());
    }
}
