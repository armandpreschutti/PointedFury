using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;
using static TreeEditor.TreeEditorHelper;

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
    [Tooltip("How fast the character charges towards a direction while attacking")]
    public float ChargeSpeed;
    [Tooltip("How fast the character leaps towards while dodging")]
    public float DashSpeed;

    // Player grounded variables
    [Header("Player Grounded")]
    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = -0.14f;
    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.28f;
    [Tooltip("What layers the character uses as ground")]
    public LayerMask GroundLayers;


    // Player combat variables
    [Header("Player Combat")]
    [Tooltip("What layers the character detects enemies on")]
    public LayerMask EnemyLayers;
    [Tooltip("The amount of time after an attack to exit attack state")]
    public float AttackTimeout;
    [Tooltip("The minimum distance between player and target")]
    public float CombatDistance;
    [Tooltip("The amount of force added to landed attacks")]
    public float KnockBackPower;

    // Debug State Variables


    // Player state variables
    private bool _isGrounded = true;
    private bool _isFighting = false;
    private float _speed;
    private float _targetSpeed;
    private float _verticalVelocity;
    private bool _isAttacking;
    private bool _isLightAttacking = false;
    private bool _isCharging = false;
    private bool _isHurt = false;
    private bool _isHitLanded = false;
    private bool _isKnockedBack = false;
    private bool _isDashing = false; 
    private bool _isDashMoving = false;
    private bool _isDodging = false;
    private bool _isDodgeSuccess = false;

    [SerializeField] bool _isAI;
    public string DebugCurrentSuperState;
    public string DebugCurrentSubState;

    // Player fighting variables
    private GameObject _currentTarget;
    private bool _fightTimeout;
    public float _fightTimeoutDelta;
    private int _attackType = 0;
    private bool _canComboAttack = false;
    private bool _isComboAttacking = false;
    private int _hitType = 0;
    private int _dodgeType = 1;

    // Player input variables
    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private bool _isLightAttackPressed = false;
    private bool _isDashPressed = false;
    private bool _isDodgePressed = false;

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
    public Action<bool> OnLightAttack;
    public Action<bool> OnMove;
    public Action<bool> OnIdle;
    public Action<bool> OnHurt;

    // Animation Variables
    [HideInInspector] public float AnimationBlend;
    [HideInInspector] public int AnimIDSpeed;
    [HideInInspector] public int AnimIDGrounded;
    [HideInInspector] public int AnimIDFight;
    [HideInInspector] public int AnimIDInputX;
    [HideInInspector] public int AnimIDInputY;
    [HideInInspector] public int AnimIDLightAttack;
    [HideInInspector] public int AnimIDHurt;
    [HideInInspector] public int AnimIDDash;
    [HideInInspector] public int AnimIDDodge;
    [HideInInspector] public int AnimIDCombo;
    [HideInInspector] public int AnimationIDAttackType;
    [HideInInspector] public float _transitionTime;
    

    // Player action events
    public Action OnAttackContact;
    public Action OnHitLanded;
    public Action OnCounterSuccess;

    // Getters and setters
    // Input
    public Vector2 MoveInput { get { return _moveInput; } set { _moveInput = value; } }
    public Vector2 LookInput { get { return _lookInput; } set { _lookInput = value; } }
    public bool IsLightAttackPressed { get { return _isLightAttackPressed; } set { _isLightAttackPressed = value; } }
    public bool IsDashPressed { get { return _isDashPressed; } set { _isDashPressed = value; } }
    public bool IsDodgePressed { get { return _isDodgePressed; } set { _isDodgePressed = value; } }   

    // Speed
    public float Speed { get { return _speed; } }
    public float TargetSpeed { get { return _targetSpeed; } set { _targetSpeed = value; } }
    public float VerticalVelocity { get { return _verticalVelocity; } set { _verticalVelocity = value; } }

    // State
    public Animator Animator { get { return _animator; } set { _animator = value; } }
    public bool IsGrounded { get { return _isGrounded; } }
    public bool IsFighting { get { return _isFighting; } set { _isFighting = value; } }
    public bool IsAttacking { get { return _isAttacking; } set { _isAttacking = value; } }
    public bool IsLightAttacking { get { return _isLightAttacking; } set { _isLightAttacking = value; } }
    public bool IsHurt { get { return _isHurt; } set { _isHurt = value; } }
    public bool IsCharging { get { return _isCharging; } }
    public bool IsHitLanded { get { return _isHitLanded; } set { _isHitLanded = value; } }
    public bool IsKnockedBack { get { return _isKnockedBack; } set { _isKnockedBack= value; } }
    public bool IsDashing { get { return _isDashing; } set { _isDashing = value; } }
    public bool IsDashMoving { get { return _isDashMoving; } set { _isDashMoving = value; } }
    public bool IsDodging { get { return _isDodging; } set { _isDodging = value; } }
    public bool IsDodgeSuccess { get { return _isDodgeSuccess; } set { _isDodgeSuccess = value; } }

    // Fighting
    public GameObject CurrentTarget { get { return _currentTarget; } }
    public int AttackType { get { return _attackType; } set { _attackType = value; } }
    public int HitType { get { return _hitType; } set { _hitType = value; } }
    public float FightTimeoutDelta { get { return _fightTimeoutDelta; } set { _fightTimeoutDelta = value; } }
    public bool FightTimeoutActive { get { return _fightTimeout; } set { _fightTimeout = value; } }
    public bool CanComboAttack { get { return _canComboAttack; } set { _canComboAttack = value; } }
    public bool IsComboAttacking { get { return _isComboAttacking;} set { _isComboAttacking = value; } }

    // Debug
    public Vector3 _debugVector;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        SetComponentValues();
        InitilaizeStateMachine();
    }

    private void Start()
    {
        AssignAnimationIDs();
    }

    // Update is called once per frame
    private void Update()
    {
        // Update the current state of the player
        _currentState.UpdateStates();

        // Check if the player is grounded
        GroundedCheck();
        SetPlayerSpeed();
        _debugVector = TargetRelativeInput();
        SetMovementAnimationValues();
        SetMovementAnimationSpeed();
    }

    // Check if the player is grounded
    private void GroundedCheck()
    {
        // Perform a sphere check to determine if the player is grounded
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
        _isGrounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
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
    public void FightMovement()
    {
        if (!_isAttacking && !_isHurt && !_isDodging)
        {
            _controller.Move(InputDirection() * Time.deltaTime * TargetSpeed);
            if (_currentTarget != null)
            {
                Vector3 direction = _currentTarget.transform.position - transform.position;
                direction.y = 0f; // Ignore y component
                if (direction != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(direction);
                }
            }
            else
            {
                Vector3 direction = InputDirection();
                direction.y = 0f; // Ignore y component
                if (direction != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(direction);
                }
                return;
            }
        }
        else
        {
            return;
        }
    }

    public void FreeRoamMovement()
    {
        Vector3 forwardDirection = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized;
        transform.DOLookAt(transform.position + forwardDirection, .2f);
        _controller.Move(InputDirection() * Time.deltaTime * TargetSpeed);
    }

    // Set component values
    private void SetComponentValues()
    {
        // Set references to components and initialize player controls
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
    }

    // Initialize player state machine
    private void InitilaizeStateMachine()
    {
        // Initialize the player state machine with default state
        _states = new StateFactory(this);
        _currentState = _states.FreeRoam();
        _currentState.EnterStates();

    }

    public void EnemyDetection()
    {
        if (_moveInput != Vector2.zero)
        {
            RaycastHit info;
            if(!_isAI)
            {
                if (Physics.SphereCast(transform.position, 1f, InputDirection(), out info, 10f, EnemyLayers))
                {
                    _currentTarget = info.transform.gameObject;
                }
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
            else
            {
                _currentTarget = null;
            }
        }
    }
    public void LightAttackMovement()
    {
        if(_currentTarget == null)
        {
            // Calculate the movement direction based on the forward direction of the character
            Vector3 moveDirection = transform.forward * ChargeSpeed;

            // Move the character using the CharacterController
            _controller.Move(moveDirection * Time.deltaTime);
        }
        else
        {
            if (_currentTarget != null)
            {
                // Calculate the direction towards the target
                Vector3 directionToTarget = _currentTarget.transform.position - transform.position;

                // Check the distance to the target
                float distanceToTarget = directionToTarget.magnitude;

                // If the distance is greater than stopDistance, move towards the target
                if (distanceToTarget > CombatDistance)
                {
                    // Calculate the movement direction based on the forward direction of the character
                    Vector3 moveDirection = transform.forward * ChargeSpeed;

                    // Move the character using the CharacterController
                    _controller.Move(moveDirection * Time.deltaTime);
                }
            }
            else
            {
                return;
            }
        }
    }

    public void DashMovement()
    {
        if(_moveInput != Vector2.zero)
        {
            _controller.Move(InputDirection() * Time.deltaTime * DashSpeed);
        }
        else
        {
            if(_currentTarget != null)
            {
                _controller.Move((transform.position - _currentTarget.transform.position).normalized * Time.deltaTime * DashSpeed);
            }
            else
            {
                return;
            }
        }

    }

    public void SetAttackDirection()
    {
        if (_currentTarget != null)
        {
            transform.LookAt(_currentTarget.transform.position);
        }
        else
        {
            if (_moveInput != Vector2.zero)
            {
                transform.LookAt(transform.position + InputDirection());
            }
            else
            {
                return;
            }
           
        }
        
    }

    public void SetHitKnockback()
    {
        _controller.Move((transform.position - _currentTarget.transform.position).normalized * KnockBackPower * Time.deltaTime);
    }

    public Vector3 InputDirection()
    {

        var camera = Camera.main;
        var forward = _isAI? transform.forward : camera.transform.forward;
        var right = _isAI ? transform.right: camera.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 inputDirection = forward * _moveInput.y + right * _moveInput.x;
        inputDirection = inputDirection.normalized;
        return inputDirection;
    }

    public int DodgeType()
    {
        if(_dodgeType == 1)
        {
            _dodgeType = 2;
        }
        else 
        {
            _dodgeType = 1;

        }
        return _dodgeType;
    }

    public Vector3 TargetRelativeInput()
    {
        if(_currentTarget!= null && _moveInput != Vector2.zero)
        {
            // Calculate the direction from player to target
            Vector3 directionToTarget = (_currentTarget.transform.position - transform.position).normalized;

            // Calculate the angle between input direction and direction to target
            float angle = Vector3.SignedAngle(directionToTarget, InputDirection(), Vector3.up);

            // Create a vector3 based on angle to represent the direction relative to the target
            Vector3 relativeDirection = Quaternion.Euler(0, angle, 0) * Vector3.forward;

            return relativeDirection;
        }
        else
        {
            return Vector3.zero;
        }
       
    }

    public Vector3 TargetOffset(Transform target)
    {
        Vector3 position;
        position = target.position;
        return Vector3.MoveTowards(position, transform.position, .95f);
    }
    public void OnAttackAnimationCharge()
    {
        _isCharging = true;
    }
    public void OnAttackAnimationContact()
    {
        OnAttackContact?.Invoke();

        _isCharging = false;
    }
    public void OnAttackAnimationComplete()
    {
        _isLightAttacking = false;
    }
    public void OnAttackAnimationRecover()
    {
        _isFighting = false;
    }
    
    public void OnHurtAnimationKnockbackComplete()
    {
        _isKnockedBack= false;
    }
    public void OnHurtAnimationComplete()
    {
        _animator.SetBool(AnimIDHurt, false);
        _isHurt = false;
    }
    public void OnDodgeAnimationComplete()
    {
        _animator.SetBool(AnimIDDodge, false);
        _isDodging = false;
    }
    public void OnDashMoveStart()
    {
        _isDashMoving = true;
    }
    public void OnDashMoveComplete()
    {
        _isDashMoving = false;
    }
    public void OnDashAnimationComplete()
    {
        //_animator.SetBool(AnimIDDodge, false);
        _isDashing = false;
    }
    public void TakeHit(int attackType)
    {
        if (!_isDodgePressed)
        {
            _hitType = attackType;
            _isHitLanded = true;
            OnHitLanded?.Invoke();
        }
        else
        {
            _isDodgeSuccess = true;
            OnCounterSuccess?.Invoke();
            return;
        }
    }

    private void AssignAnimationIDs()
    {
        AnimIDSpeed = Animator.StringToHash("Speed");
        AnimIDGrounded = Animator.StringToHash("Grounded");
        AnimIDFight = Animator.StringToHash("Fight");
        AnimIDInputX = Animator.StringToHash("InputX");
        AnimIDInputY = Animator.StringToHash("InputY");
        AnimIDLightAttack = Animator.StringToHash("LightAttack");
        AnimIDHurt = Animator.StringToHash("Hurt");
        AnimIDDash = Animator.StringToHash("Dash");
        AnimIDDodge = Animator.StringToHash("Dodge");
    }

    private void SetMovementAnimationValues()
    {
        _animator.SetFloat(AnimIDInputX, TargetRelativeInput().x);
        _animator.SetFloat(AnimIDInputY, TargetRelativeInput().z);
    }
    private void SetMovementAnimationSpeed()
    {
        AnimationBlend = Mathf.Lerp(AnimationBlend, _targetSpeed, Time.deltaTime * SpeedChangeRate);
        if (AnimationBlend < 0.01f) AnimationBlend = 0f;
        _animator.SetFloat(AnimIDSpeed, AnimationBlend);
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
