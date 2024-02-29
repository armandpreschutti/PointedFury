using System;
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

    // Player state variables
    private bool _isGrounded = true;
    private float _speed;
    private float _targetSpeed;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    private float _verticalVelocity;
    private float _jumpDurationDelta;

    // Player fighting variables
    public Transform FightTarget;

    // Player input variables
    private PlayerControls _playerControls;
    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private bool _isJumpPressed = false;
    private bool _isSprintPressed = false;
    private bool _isFightPressed = false;

    // Player components
    private Animator _animator;
    private CharacterController _controller;
    private GameObject _mainCamera;

    // Player state machine
    private PlayerBaseState _currentState;
    private PlayerStateFactory _states;
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }

    // Player events
    public Action<bool> OnJump;
    public Action<bool> OnFall;
    public Action<bool> OnGrounded;
    public Action<bool> OnFight;

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
    public Animator Animator { get { return _animator; } set { _animator = value; } }
    public bool IsGrounded { get { return _isGrounded; } }

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
    }

    // Move the player
    public void FreeMovement()
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

        Vector3 inputDirection = new Vector3(_moveInput.x, 0.0f, _moveInput.y).normalized;

        if (_moveInput != Vector2.zero)
        {
            // Calculate the target rotation based on input direction and camera orientation
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f); // Rotate player to face input direction relative to camera position
        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
        _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
    }

    public void FightMovement()
    {
        Vector3 targetPosition = FightTarget.position;
        transform.LookAt(targetPosition); // Face the target

        // Get the forward vector of the camera without vertical component
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        // Calculate movement direction based on player input relative to camera
        Vector3 moveInput = new Vector3(_moveInput.x, 0.0f, _moveInput.y);
        Vector3 moveDirection = Quaternion.LookRotation(cameraForward) * moveInput;
        moveDirection.Normalize();

        // Calculate movement speed and apply movement
        Vector3 movement = moveDirection * (_speed * Time.deltaTime);
        _controller.Move(movement + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
        /*Vector3 target = FightTarget.position;
        transform.LookAt(target);
        Vector3 moveDirection = new Vector3(_moveInput.x, 0.0f, _moveInput.y).normalized;
        _controller.Move(moveDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);*/

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
    }
}
