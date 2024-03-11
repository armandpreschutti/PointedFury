using DG.Tweening;
using System;
using System.Collections;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
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

    // Player jump variables
    [Space(10)]
    [Tooltip("The height the player can jump")]
    public float JumpHeight = 1.2f;
    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    public float Gravity = -15.0f;
    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    public float JumpDuration = 0.15f;

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



    private GameObject _currentTarget;

    // Debug State Variables
    public string DebugCurrentSuperState;
    public string DebugCurrentSubState;

    // Player state variables
    private bool _isGrounded = true;
    private bool _isFighting = false;
    private float _speed;
    private float _targetSpeed;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    private float _verticalVelocity;
    private float _jumpDurationDelta;
    private bool _isAttacking = false;

    // Player fighting variables
    private bool _fightTimeout;
    public float _fightTimeoutDelta;
    private int _attackType = 0;

    // Player input variables
    private PlayerControls _playerControls;
    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private bool _isJumpPressed = false;
    private bool _isSprintPressed = false;
    private bool _isFightPressed = false;
    private bool _isLightAttackPressed = false;

    //DOTween
    public Tween attackRotateTween;
    public Tween attackMoveTween;
    public Sequence attackSequence;

    // Player components
    private Animator _animator;
    private CharacterController _controller;
    private GameObject _mainCamera;

    // Player state machine
    private PlayerBaseState _currentState;
    private PlayerStateFactory _states;
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }

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
    public float Speed { get { return _speed; } }
    public float TargetSpeed { get { return _targetSpeed; } set { _targetSpeed = value; } }
    public float VerticalVelocity { get { return _verticalVelocity; } set { _verticalVelocity = value; } }
    public float JumpDurationDelta { get { return _jumpDurationDelta; } set { _jumpDurationDelta = value; } }
    public Vector2 MoveInput { get { return _moveInput; } set { _moveInput = value; } }
    public Vector2 LookInput { get { return _lookInput; } set { _lookInput = value; } }
    public bool IsJumpPressed { get { return _isJumpPressed; } set { _isJumpPressed = value; } }
    public bool IsSprintPressed { get { return _isSprintPressed; } set { _isSprintPressed = value; } }
    public bool IsFightPressed { get { return _isFightPressed; } set { _isFightPressed= value; } }
    public bool IsLightAttackPressed { get { return _isLightAttackPressed; } set { _isLightAttackPressed = value; } }
    public Animator Animator { get { return _animator; } set { _animator = value; } }
    public bool IsGrounded { get { return _isGrounded; } }
    public bool IsFighting { get { return _isFighting; } set { _isFighting = value; } }
    public bool IsAttacking { get { return _isAttacking; } set { _isAttacking = value; } }
    public float FIghtTimeoutDelta { get { return _fightTimeoutDelta; } set { _fightTimeoutDelta = value; } }
    public bool FightTimeoutActive { get { return _fightTimeout; } set { _fightTimeout = value; } }
    public int AttackType { get { return _attackType; } set { _attackType = value; } }

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

    // Set jump input value
    public void SetJumpInput(bool value)
    {
        _isJumpPressed = value;
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
            Vector3 inputDirection = new Vector3(_moveInput.x, 0.0f, _moveInput.y).normalized;

            if (_moveInput != Vector2.zero)
            {
                // Calculate the target rotation based on input direction and camera orientation
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f); // Rotate player to face input direction relative to camera position
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
        attackRotateTween = transform.DOLookAt(transform.position + forwardDirection, .2f);
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
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
        _playerControls = new PlayerControls();
    }

    // Initialize player state machine
    private void InitilaizeStateMachine()
    {
        // Initialize the player state machine with default state
        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();
    }

    // Set input values
    private void SetInputValues()
    {
        // Set up input actions for player controls
        _playerControls.Player.Sprint.performed += ctx => SetSprintInput(ctx.ReadValueAsButton());
        _playerControls.Player.Jump.performed += ctx => SetJumpInput(ctx.ReadValueAsButton());
        _playerControls.Player.Move.performed += ctx => SetMoveInput(ctx.ReadValue<Vector2>());
        _playerControls.Player.Look.performed += ctx => SetLookInput(ctx.ReadValue<Vector2>());
        _playerControls.Player.Fight.performed += ctx => SetFightInput(ctx.ReadValueAsButton());
        _playerControls.Player.LightAttack.performed += ctx => SetLightAttackInput(ctx.ReadValueAsButton());
    }

    public void EnemyDetection()
    {

        if(_moveInput != Vector2.zero)
        {
            RaycastHit info;
            if(Physics.SphereCast(transform.position, 1f, InputDirection(), out info, 10f,  EnemyLayers))
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

    
    public void RotateTowardTarget(float duration)
    {
        if (_currentTarget != null)
        {
            attackRotateTween = transform.DOLookAt(_currentTarget.transform.position, .2f);
        }
        else
        {
            if (_moveInput != Vector2.zero)
            {

                attackRotateTween = transform.DOLookAt(transform.position + InputDirection(), .2f);
            }
            else
            {
                return;
            }
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


    public Vector3 TargetOffset(Transform target)
    {
        Vector3 position;
        position = target.position;
        return Vector3.MoveTowards(position, transform.position, 1f);
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
}
