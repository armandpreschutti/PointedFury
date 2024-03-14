using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    // Player movement variables
    [Header("Player Movement")]
    [Tooltip("Move speed of the character in m/s")]
    public float MoveSpeed = 4.0f;
    [Tooltip("Sprint speed of the character in m/s")]
    public float SprintSpeed = 7.5f;
    [Tooltip("Fight speed of the character in m/s")]
    public float FightSpeed = 2f;
    [Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;
    [Tooltip("Acceleration and deceleration")]
    public float SpeedChangeRate = 10.0f;

    // Player grounded variables
    [Header("Player Grounded")]
    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = -0.14f;
    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.28f;
    [Tooltip("What layers the character uses as ground")]
    public LayerMask GroundLayers;


    [Header("Player Combat")]
    [Tooltip("What layers the character detects enemies on")]
    public LayerMask EnemyLayers;
    [Tooltip("The amount of time after an attack to exit attack state")]
    public float AttackTimeout;

    // Debug State Variables
    public string DebugCurrentSuperState;
    public string DebugCurrentSubState;

    // Player state variables
    private bool _isGrounded = true;
    private bool _isFighting = false;
    private float _speed;
    private float _targetSpeed;
    private float _verticalVelocity;
    private float _jumpDurationDelta;
    private bool _isAttacking = false;

    // Player fighting variables
    private GameObject _currentTarget;
    private bool _fightTimeout;
    public float _fightTimeoutDelta;
    private int _attackType = 0;

    // Player input variables
    private PlayerControls _playerControls;
    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private bool _isSprintPressed = false;
    private bool _isFightPressed = false;
    private bool _isLightAttackPressed = false;


    // Player components
    private Animator _animator;
    private CharacterController _controller;

    // Player state machine
    private BaseState _currentState;
    private StateFactory _states;
    public BaseState CurrentState { get { return _currentState; } set { _currentState = value; } }

    // Player state events
    public Action<bool> OnJump;
    public Action<bool> OnFall;
    public Action<bool> OnGrounded;
    public Action<bool> OnFight;
    public Action<bool> OnAttack;
    public Action<bool> OnRun;
    public Action<bool> OnIdle;

    // Player action events
    public Action OnAttackContact;

    // Getters and setters
    // Input
    public Vector2 MoveInput { get { return _moveInput; } set { _moveInput = value; } }
    public Vector2 LookInput { get { return _lookInput; } set { _lookInput = value; } }
    public bool IsSprintPressed { get { return _isSprintPressed; } set { _isSprintPressed = value; } }
    public bool IsFightPressed { get { return _isFightPressed; } set { _isFightPressed = value; } }
    public bool IsLightAttackPressed { get { return _isLightAttackPressed; } set { _isLightAttackPressed = value; } }

    // 
    public float Speed { get { return _speed; } }
    public float TargetSpeed { get { return _targetSpeed; } set { _targetSpeed = value; } }
    public float VerticalVelocity { get { return _verticalVelocity; } set { _verticalVelocity = value; } }

    // State
    public Animator Animator { get { return _animator; } set { _animator = value; } }
    public bool IsGrounded { get { return _isGrounded; } }
    public bool IsFighting { get { return _isFighting; } set { _isFighting = value; } }
    public bool IsAttacking { get { return _isAttacking; } set { _isAttacking = value; } }

    // Fighting
    public int AttackType { get { return _attackType; } set { _attackType = value; } }
    public float FIghtTimeoutDelta { get { return _fightTimeoutDelta; } set { _fightTimeoutDelta = value; } }
    public bool FightTimeoutActive { get { return _fightTimeout; } set { _fightTimeout = value; } }

    // Debug
    public Vector3 debugFloat;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        SetComponentValues();
        InitilaizeStateMachine();
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

    // Start is called before the first frame update
    private void Start()
    {
        SetInputValues();
    }

    // Update is called once per frame
    private void Update()
    {
        // Update the current state of the player
        _currentState.UpdateStates();

        // Check if the player is grounded
        GroundedCheck();
        SetPlayerSpeed();
    }

    // Set sprint input value
    public void SetSprintInput(bool value)
    {
        _isSprintPressed = value;
    }

    // Set move input value
    public void SetMoveInput(Vector2 value)
    {
        _moveInput = value;
    }

    // Set look input value
    public void SetLookInput(Vector2 value)
    {
        _lookInput = value;
    }

    public void SetFightInput(bool value)
    {
        _isFightPressed = value;

        // DEBUG ONLY, DELETE WHEN NO LONGER NEEDED
        _currentTarget = null;
    }

    public void SetLightAttackInput(bool value)
    {
        _isLightAttackPressed = value;
    }

    public void SetPlayerSpeed()
    {
        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
        float speedOffset = 0.1f;

        if (currentHorizontalSpeed < _targetSpeed - speedOffset || currentHorizontalSpeed > _targetSpeed + speedOffset)
        {
            // Smoothly adjust player speed to the target speed
            _speed = Mathf.Lerp(currentHorizontalSpeed, _targetSpeed * _moveInput.magnitude, Time.deltaTime * SpeedChangeRate);
            _speed = Mathf.Round(_speed * 1000f) / 1000f; // Round speed to 3 decimal places
        }
        else
        {
            _speed = _targetSpeed;
        }
    }

    // Move the player
    public void FreeRoamMovement()
    {
        if (!IsAttacking)
        {
            if (_moveInput != Vector2.zero)
            {
                transform.DOLookAt(transform.position + InputDirection(), .2f);
            }
            else
            {
                transform.DOLookAt(_currentTarget.transform.position, .2f);
            }
        }
        else
        {
            return;
        }

    }

    public void ThirdPersonMovement()
    {
        // Calculate the forward direction based on the camera's forward direction
        Vector3 forwardDirection = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized;
        //attackRotateTween = transform.DOLookAt(transform.position + forwardDirection, .2f);
        transform.DOLookAt(transform.position + forwardDirection, .2f);
    }

    // Check if the player is grounded
    private void GroundedCheck()
    {
        // Perform a sphere check to determine if the player is grounded
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
        _isGrounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
    }

    // Set component values
    private void SetComponentValues()
    {
        // Set references to components and initialize player controls
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
        _playerControls = new PlayerControls();
    }

    // Initialize player state machine
    private void InitilaizeStateMachine()
    {
        // Initialize the player state machine with default state
        _states = new StateFactory(this);
        _currentState = _states.FreeRoam();
        _currentState.EnterState();
    }

    // Set input values
    private void SetInputValues()
    {
        // Set up input actions for player controls
        _playerControls.Player.Sprint.performed += ctx => SetSprintInput(ctx.ReadValueAsButton());
        _playerControls.Player.Move.performed += ctx => SetMoveInput(ctx.ReadValue<Vector2>());
        _playerControls.Player.Look.performed += ctx => SetLookInput(ctx.ReadValue<Vector2>());
        _playerControls.Player.Fight.performed += ctx => SetFightInput(ctx.ReadValueAsButton());
        _playerControls.Player.LightAttack.performed += ctx => SetLightAttackInput(ctx.ReadValueAsButton());
    }

    public void EnemyDetection()
    {
        if (!IsAttacking)
        {
            if (_moveInput != Vector2.zero)
            {
                RaycastHit info;
                if (Physics.SphereCast(transform.position, 1f, InputDirection(), out info, 5f, EnemyLayers))
                {
                    _currentTarget = info.transform.gameObject;
                }
            }
            else
            {
                // Perform a spherecast to detect all colliders on the specified layer within the detection radius
                RaycastHit[] hits = Physics.SphereCastAll(transform.position, 5f, transform.forward, Mathf.Infinity, EnemyLayers);

                float closestDistance = Mathf.Infinity;
                Transform closestTarget = null;

                // Iterate through all hits to find the closest collider
                foreach (RaycastHit hit in hits)
                {
                    float distance = Vector3.Distance(transform.position, hit.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestTarget = hit.transform;
                    }
                }
                // Set the closest target as the target for your player
                if (closestTarget != null)
                {
                    _currentTarget = closestTarget.gameObject;
                    // You can add any additional logic here, such as locking onto the target or performing an action
                }
            }
        }
    }


    public void RotateTowardTarget(float duration)
    {
        if (_currentTarget != null)
        {
            //attackRotateTween = transform.DOLookAt(_currentTarget.transform.position, duration);
            transform.DOLookAt(_currentTarget.transform.position, duration);
        }
        else
        {
            if (_moveInput != Vector2.zero)
            {
                //attackRotateTween = transform.DOLookAt(transform.position + InputDirection(), duration);
                transform.DOLookAt(transform.position + InputDirection(), duration);
            }
            else
            {
                return;
            }
        }
    }

    public Vector3 InputDirection()
    {
        var camera = Camera.main;
        var forward = camera.transform.forward;
        var right = camera.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 inputDirection = forward * _moveInput.y + right * _moveInput.x;
        inputDirection = inputDirection.normalized;
        return inputDirection;
    }

    public void OnAttackAnimationContact()
    {
        OnAttackContact.Invoke();

    }
    public void OnAttackAnimationComplete()
    {
        IsAttacking = false;
    }
    public void OnAttackAnimationRecover()
    {
        IsFighting = false;
    }

    public void SetAttackType()
    {
        switch (_attackType)
        {
            case 0:
                _attackType = 1;
                break;
            case 1:
                _attackType = 2;
                break;
            case 2:
                _attackType = 0;
                break;
            default:
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(transform.position + InputDirection(), .2f);

        Gizmos.DrawRay(transform.position + Vector3.up, InputDirection() * 1f);

        if (_currentTarget != null)
        {
            Gizmos.DrawSphere(_currentTarget.transform.position, 1f);
        }
        Gizmos.DrawWireSphere(transform.position, 5f);
    }
}
