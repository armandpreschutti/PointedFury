using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    [Tooltip("Acceleration and deceleration")]
    public float SpeedChangeRate = 10.0f;
    [Tooltip("How fast the character charges towards a direction while light attacking")]
    public float LightAttackChargeSpeed;
    [Tooltip("How fast the character charges towards a direction while heavy attacking")]
    public float HeavyAttackChargeSpeed;
    [Tooltip("How fast the character charges towards a direction while parrying")]
    public float ParryChargeSpeed;
    [Tooltip("How fast the character leaps towards while dodging")]
    public float DashSpeed;
    [Tooltip("How much gravity is applied to the player")]
    public float Gravity;

    public float rotationSpeed;

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
    [Tooltip("The amount of damage a player will inflict with light attacks")]
    public float LightAttackDamage;
    [Tooltip("The amount of damage a player will inflict with heavy attacks")]
    public float HeavyAttackDamage;
    [Tooltip("What layers the character detects enemies on")]
    public LayerMask EnemyLayers;
    [Tooltip("The amount of time after an attack to exit attack state")]
    public float AttackTimeout;
    [Tooltip("The minimum distance between player and target")]
    public float CombatDistance;
    [Tooltip("The amount of force added to landed attacks")]
    public float KnockBackPower;
    [Tooltip("A list of enemies detected via sphere casr")]
    public List<GameObject> EnemiesNearby = new List<GameObject>();

    [Tooltip("Is the state machine controlled by Ai")]
    public bool IsAI;
    // Debug State Variables

    public Vector3 moveDirection;
    // Player state variables
    private bool _isGrounded = true;
    private bool _isFighting = false;
    private float _speed;
    private float _targetSpeed;
    private float _verticalVelocity;
    private bool _isAttacking;
    private bool _isPostAttack = false;
    private bool _isCharging = false;
    private bool _isHurt = false;
    private bool _isLightHitLanded = false;
    private bool _isHeavyHitLanded = false;
    private bool _isKnockedBack = false;
    private bool _isDashing = false; 
    private bool _isDashMoving = false;
    private bool _isBlocking = false;
    private bool _isBlockSuccess = false;
    private bool _isParried = false;
    private bool _isParrySuccess = false;
    private bool _isParrying = false;
    private bool _isParryable = false;
    private bool _isStunned = false;
    private bool _isDead = false;


    public string DebugCurrentSuperState;
    public string DebugCurrentSubState;

    private float _verticalSpeed;

    // Player fighting variables
    private GameObject _currentTarget;
    private int _lightAttackID = 0;
    private int _heavyAttackID = 0;
    private bool _canComboAttack = false;
    private bool _isComboAttacking = false;
    private int _hitID = 0;
    private string _attackType;
    private string _hitType;
    private Vector3 _attackDirection;
    private Vector3 _incomingAttackDirection;

    // Player input variables
    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private bool _isLightAttackPressed = false;
    private bool _isHeavyAttackPressed = false;
    private bool _isDashPressed = false;
    private bool _isBlockPressed = false;
    private bool _isParryPressed = false; 

    // Player components
    private Animator _animator;
    private CharacterController _controller;

    // Player state machine
    private BaseState _currentState;
    private StateFactory _states;
    public BaseState CurrentState { get { return _currentState; } set { _currentState = value; } }

    // Player state events
    public Action<bool> OnFall;
    public Action<bool> OnGrounded;
    public Action<bool> OnFight;
    public Action<bool,string> OnLightAttack;
    public Action<bool,string> OnHeavyAttack;
    public Action<bool> OnAttack;
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
    [HideInInspector] public int AnimIDLightAttackID;
    [HideInInspector] public int AnimIDHeavyAttack;
    [HideInInspector] public int AnimIDHeavyAttackID;
    [HideInInspector] public int AnimIDPostAttack;
    [HideInInspector] public int AnimIDHurt;
    [HideInInspector] public int AnimIDDash;
    [HideInInspector] public int AnimIDBlock;
    [HideInInspector] public int AnimIDCombo;
    [HideInInspector] public int AnimIDParry;
    [HideInInspector] public int AnimIDStunned;
    [HideInInspector] public int AnimIDDeath;
    [HideInInspector] public int AnimationIDAttackType;
    [HideInInspector] public float _transitionTime;
    

    // Player action events
    public Action OnAttackContact;
    public Action<float, string> OnLightAttackRecieved;
    public Action<float, string> OnHeavyAttackRecieved;
    public Action <bool, string> OnAttackWindUp;
    public Action OnLightAttackGiven;
    public Action OnHeavyAttackGiven;
    public Action OnAttemptParty;
    public Action OnTestAction;
    public Action OnBlockSuccessful;
    public Action OnParrySuccessful;
    public Action OnDashSuccessful;
    public Action OnDeath;

    // Getters and setters
    // Input
    public Vector2 MoveInput { get { return _moveInput; } set { _moveInput = value; } }
    public Vector2 LookInput { get { return _lookInput; } set { _lookInput = value; } }
    public bool IsLightAttackPressed { get { return _isLightAttackPressed; } set { _isLightAttackPressed = value; } }
    public bool IsHeavyAttackPressed { get { return _isHeavyAttackPressed; } set { _isHeavyAttackPressed = value; } }
    public bool IsDashPressed { get { return _isDashPressed; } set { _isDashPressed = value; } }
    public bool IsBlockPressed { get { return _isBlockPressed; } set { _isBlockPressed = value; } }   
    public bool IsParryPressed {  get { return _isParryPressed; } set { _isParryPressed= value; } }

    // Speed
    public float Speed { get { return _speed; } }
    public float TargetSpeed { get { return _targetSpeed; } set { _targetSpeed = value; } }
    public float VerticalVelocity { get { return _verticalVelocity; } set { _verticalVelocity = value; } }

    // State
    public Animator Animator { get { return _animator; } set { _animator = value; } }
    public CharacterController Controller { get { return _controller; } set { _controller = value; } }
    public bool IsGrounded { get { return _isGrounded; } }
    public bool IsFighting { get { return _isFighting; } set { _isFighting = value; } }
    public bool IsAttacking { get { return _isAttacking; } set { _isAttacking = value; } }
    public bool IsPostAttack { get { return _isPostAttack; } set { _isPostAttack = value; } }   
    public bool IsHurt { get { return _isHurt; } set { _isHurt = value; } }
    public bool IsCharging { get { return _isCharging; } set { _isCharging = value; } }
    public bool IsLightHitLanded { get { return _isLightHitLanded; } set { _isLightHitLanded = value; } }
    public bool IsHeavyHitLanded { get { return _isHeavyHitLanded; } set { _isHeavyHitLanded = value; } }
    public bool IsKnockedBack { get { return _isKnockedBack; } set { _isKnockedBack= value; } }
    public bool IsDashing { get { return _isDashing; } set { _isDashing = value; } }
    public bool IsDashMoving { get { return _isDashMoving; } set { _isDashMoving = value; } }
    public bool IsBlocking { get { return _isBlocking; } set { _isBlocking = value; } }
    public bool IsBlockSuccess { get { return _isBlockSuccess; } set { _isBlockSuccess = value; } }
    public bool IsParrying { get { return _isParrying; } set { _isParrying = value; } }
    public bool IsParrySucces { get { return _isParrySuccess; } set { _isParrySuccess= value; } }
    public bool IsParried { get { return _isParried; } set { _isParried = value; } }
    public bool IsParryable { get { return _isParryable; }set { _isParryable = value; } }
    public bool IsStunned { get { return _isStunned; } set { _isStunned = value; } }    
    public bool IsDead { get { return _isDead; } set { _isDead = value; } }

    // Fighting
    public GameObject CurrentTarget { get { return _currentTarget; } set { _currentTarget = value; } }
    public Transform ClosestTarget;
    public Transform SecondClosestTarget;
    public Transform ThirdClosestTarget;
    public int LightAttackID { get { return _lightAttackID; } set { _lightAttackID = value; } }
    public int HeavyAttackID { get { return _heavyAttackID; } set { _heavyAttackID = value; } }
    public int HitID { get { return _hitID; } set { _hitID = value; } }
    public string AttackType { get { return _attackType; } set { _attackType = value; } }
    public string HitType { get { return _hitType; } set { _hitType = value; } }
    public bool CanComboAttack { get { return _canComboAttack; } set { _canComboAttack = value; } }
    public bool IsComboAttacking { get { return _isComboAttacking;} set { _isComboAttacking = value; } }
    public Vector3 AttackDirection { get { return _attackDirection; } set { _attackDirection= value; } }
    public Vector3 IncomingAttackDirection { get { return _incomingAttackDirection; } set { _incomingAttackDirection = value; } }

    private void Awake()
    {
        SetComponentValues();
        InitilaizeStateMachine();
    }

    private void Start()
    {
        AssignAnimationIDs();
    }

    private void Update()
    {
        _currentState.UpdateStates();

        GroundedCheck();
        SetPlayerSpeed();
        CheckIsFighting();
        SetMovementAnimationSpeed();
        SimulateGravity();
    }

    private void GroundedCheck()
    {
        // Perform a sphere check to determine if the player is grounded
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
        _isGrounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
    }

    private void SimulateGravity()
    {
        // Apply gravity
        if (!IsGrounded)
        {
            _verticalSpeed = Gravity;
        }
        else
        {
            _verticalSpeed= 0;
        }
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
    /* public void CombatMovement()
     {
         if (!_isAttacking && !_isHurt && !_isBlocking && !_isStunned && !_isDashing && !_isPostAttack && !_isParrying)
         {
             moveDirection = new Vector3(InputDirection().x * TargetSpeed, _verticalSpeed, InputDirection().z * TargetSpeed);
             if (moveDirection != Vector3.zero)
             {
                 _controller.Move(moveDirection * Time.deltaTime);
             }

             if (_currentTarget != null)
             {
                 Vector3 direction = _currentTarget.transform.position - transform.position;
                 direction.y = 0f; // Ignore y component
                 if (direction != Vector3.zero)
                 {
                     Quaternion targetRotation = Quaternion.LookRotation(direction);
                     transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                 }
             }
             else
             {
                 Vector3 direction = InputDirection();
                 direction.y = 0f; // Ignore y component
                 if (direction != Vector3.zero)
                 {
                     Quaternion targetRotation = Quaternion.LookRotation(direction);
                     transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                 }
                 return;
             }
         }
         else
         {
             return;
         }
     }*/
    public void CombatMovement()
    {
        if (!_isAttacking && !_isHurt && !_isBlocking && !_isStunned && !_isDashing && !_isPostAttack && !_isParrying)
        {
            moveDirection = new Vector3(InputDirection().x * TargetSpeed, _verticalSpeed, InputDirection().z * TargetSpeed);
            if (moveDirection != Vector3.zero)
            {
                _controller.Move(moveDirection * Time.deltaTime);
            }

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
        if (!_isHurt && !_isBlocking && !_isStunned && !_isDashing && !_isPostAttack && !_isAttacking && !_isParrying) 
        {
            if (!IsAI)
            {
                Vector3 forwardDirection = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized;
                transform.LookAt(transform.position + forwardDirection);
                moveDirection = new Vector3(InputDirection().x * TargetSpeed, _verticalSpeed, InputDirection().z * TargetSpeed);
                _controller.Move(moveDirection * Time.deltaTime/* * TargetSpeed*/);
            }
            else
            {
                return;
            }
        }
        else
        {
            return;
        }
    }

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
        _currentState = _states.CombatState();
        _currentState.EnterStates();
    }

    public void ParryMovement()
    {
        // Calculate the direction towards the target
        Vector3 directionToTarget = _currentTarget.transform.position - transform.position;

        // Check the distance to the target
        float distanceToTarget = directionToTarget.magnitude;

        // If the distance is greater than stopDistance, move towards the target
        if (distanceToTarget > 0)
        {
            // Calculate the movement direction based on the forward direction of the character
            Vector3 moveDirection = transform.forward * ParryChargeSpeed;

            // Move the character using the CharacterController
            _controller.Move(moveDirection * Time.deltaTime);
        }
    }
    public void AttackMovement()
    {
        if(_currentTarget == null)
        {
            // Calculate the movement direction based on the forward direction of the character
            Vector3 moveDirection = transform.forward * (_attackType == "Light" ? LightAttackChargeSpeed : HeavyAttackChargeSpeed);

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
                    Vector3 moveDirection = transform.forward * (_attackType == "Light" ? LightAttackChargeSpeed : HeavyAttackChargeSpeed);

                    // Move the character using the CharacterController
                    _controller.Move(moveDirection * Time.deltaTime);
                }
                else
                {
                    return;
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
        _controller.Move(transform.forward * Time.deltaTime * DashSpeed);
    }

    public void SetIncomingAttackDirection()
    {
        transform.LookAt(_incomingAttackDirection);
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
                transform.LookAt(transform.position + transform.forward);
                // return;
            }
        }
        /*if (_currentTarget != null)
        {
            Vector3 direction = _currentTarget.transform.position - transform.position;
            direction.y = 0f; // Ignore y component
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            if (_moveInput != Vector2.zero)
            {
                Vector3 direction = new Vector3(_moveInput.x, 0f, _moveInput.y);
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            else
            {
                // No input, maintain current rotation
                Quaternion targetRotation = Quaternion.LookRotation(transform.forward);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }*/
    }
    public void SetParryDirection()
    {
        if (_currentTarget != null)
        {
            transform.LookAt(_currentTarget.transform.position);
        }
        else
        {
            return;
        }
    }

    public void SetDashDirection()
    {
        transform.LookAt(transform.position + InputDirection());
    }

    public void SetHitKnockBack()
    {
        _controller.Move((transform.position - _incomingAttackDirection).normalized * KnockBackPower * Time.deltaTime);
    }

    public void SetStunnedKnockback()
    {
        _controller.Move((transform.position - _incomingAttackDirection).normalized * (KnockBackPower)* Time.deltaTime);
    }

    public Vector3 InputDirection()
    {

        var camera = Camera.main;
        var forward = IsAI? transform.forward : camera.transform.forward;
        var right = IsAI ? transform.right: camera.transform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();
        Vector3 inputDirection = forward * _moveInput.y + right * _moveInput.x;
        inputDirection = inputDirection.normalized;
        return inputDirection;
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
            return Vector2.zero;
        }
    }

    public Vector3 TargetOffset(Transform target)
    {
        Vector3 position;
        position = target.position;
        return Vector3.MoveTowards(position, transform.position, .95f);
    }

    public void OnAttackAnimationBegin()
    {
        _isParryable = true;
    }

    public void OnAttackAnimationCharge()
    {
        OnAttackWindUp?.Invoke(true, _attackType);
        _isCharging = true;
    }

    public void OnAttackAnimationContact()
    {
        OnAttackContact?.Invoke();
        _isParryable = false;
        _isCharging = false;
    }

    public void OnAttackAnimationComplete()
    {
        _isAttacking = false;
    }
    
    public void OnPostAttackAnimationComplete()
    {
        _isPostAttack = false;
    }

    public void OnStunAnimationKnockbackComplete()
    {
        _isKnockedBack = false;
    }

    public void OnParryAnimationCharge()
    {
        _isCharging = true;
    }

    public void OnParryAnimationContact()
    {
        _isCharging = false;
    }

    public void OnParryAnimationComplete()
    {
        _isParrying = false;    
    }

    public void OnStunAnimationComplete()
    {
        _isStunned = false;
    }

    public void OnHurtAnimationKnockbackComplete()
    {
        _isKnockedBack= false;
    }
    
    public void OnHurtAnimationComplete()
    {
        _isHurt = false;
    }

    public void OnBlockAnimationComplete()
    {
        _isBlocking = false;
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
        _isDashing = false;
    }

    public void TakeHit(string hitType, int attackID, Vector3 attackerPosition, float attackDamage)
    {
        _incomingAttackDirection = attackerPosition;

        if(_isDashing || _isParrying)
        {
            return;
        }
        else
        {
            _hitID = attackID;
            _hitType = hitType;
            if (!IsDead)
            {
                if (_isBlocking && HitType != "Heavy")
                {
                    _isBlockSuccess = true;
                    OnBlockSuccessful?.Invoke();
                }
                else if (hitType == "Light" && !_isParrying)
                { 
                    OnLightAttackRecieved?.Invoke(attackDamage, "Light");
                    _isLightHitLanded = true;
                }
                else if (hitType == "Heavy" && !_isParrying)
                {
                    OnHeavyAttackRecieved?.Invoke(attackDamage, "Heavy");
                    OnTestAction?.Invoke();
                    _isHeavyHitLanded = true;
                }
            }          
        }
    }
    public void GiveHit(string attackType)
    {
        if(attackType == "Light")
        {
            OnLightAttackGiven?.Invoke();
        }
        else if(attackType == "Heavy")
        {
            OnHeavyAttackGiven?.Invoke();
        }
    }

    public void TakeParry(Vector3 attackerPosition)
    {
        _incomingAttackDirection = attackerPosition;
        _isParried = true;
    }
    
    public void GiveParry(Vector3 attackerPosition)
    {
        _incomingAttackDirection = attackerPosition;
        IsParrySucces = true;
        OnParrySuccessful?.Invoke();
    }
    private void AssignAnimationIDs()
    {
        AnimIDSpeed = Animator.StringToHash("Speed");
        AnimIDGrounded = Animator.StringToHash("Grounded");
        AnimIDFight = Animator.StringToHash("Fight");
        AnimIDInputX = Animator.StringToHash("InputX");
        AnimIDInputY = Animator.StringToHash("InputY");
        AnimIDLightAttack = Animator.StringToHash("LightAttack");
        AnimIDLightAttackID = Animator.StringToHash("LightAttackID");
        AnimIDHeavyAttack = Animator.StringToHash("HeavyAttack");
        AnimIDHeavyAttackID = Animator.StringToHash("HeavyAttackID");
        AnimIDPostAttack = Animator.StringToHash("PostAttack");
        AnimIDHurt = Animator.StringToHash("Hurt");
        AnimIDDash = Animator.StringToHash("Dash");
        AnimIDBlock = Animator.StringToHash("Block");
        AnimIDParry = Animator.StringToHash("Parry");
        AnimIDStunned = Animator.StringToHash("Stunned");
        AnimIDDeath = Animator.StringToHash("Death");
    }

    public void SetCombatMovementAnimationValues()
    {
        _animator.SetFloat(AnimIDInputX, TargetRelativeInput().x * TargetSpeed);
        _animator.SetFloat(AnimIDInputY, TargetRelativeInput().z * TargetSpeed);
    }

    public void SetFreeRoamMovementAnimationValues()
    {
        _animator.SetFloat(AnimIDInputX, _moveInput.x * TargetSpeed);
        _animator.SetFloat(AnimIDInputY, _moveInput.y * TargetSpeed);
    }

    private void SetMovementAnimationSpeed()
    {
        AnimationBlend = Mathf.Lerp(AnimationBlend, _targetSpeed, Time.deltaTime * SpeedChangeRate);
        if (AnimationBlend < 0.01f) AnimationBlend = 0f;
        _animator.SetFloat(AnimIDSpeed, AnimationBlend);
    }

    public void CheckIsFighting()
    {
        if (EnemiesNearby.Count > 0)
        {
            //_isFighting = true;
            _animator.SetBool(AnimIDFight, true);
        }
        else
        {
            //_isFighting = false;
            _animator.SetBool(AnimIDFight, false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
       // Gizmos.DrawSphere(transform.position + InputDirection(), .2f);
        Gizmos.DrawSphere(new Vector3(transform.position.x, GroundedOffset, transform.position.z), GroundedRadius);
        Gizmos.DrawRay(transform.position + Vector3.up, InputDirection() * 1f);
        if (_currentTarget != null)
        {
            Gizmos.DrawSphere(_currentTarget.transform.position, 1f);
        }
        Gizmos.DrawWireSphere(transform.position, 5f);
    }
}
