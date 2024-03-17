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


    // Player combat variables
    [Header("Player Combat")]
    [Tooltip("What layers the character detects enemies on")]
    public LayerMask EnemyLayers;
    [Tooltip("The amount of time after an attack to exit attack state")]
    public float AttackTimeout;
/*    [Tooltip("Array of light attacks in combo")]
    public List<LightAttackSO> LightAttackCombo;*/


    // Debug State Variables
    public string DebugCurrentSuperState;
    public string DebugCurrentSubState;

    // Player state variables
    private bool _isGrounded = true;
    private bool _isFighting = false;
    private float _speed;
    private float _targetSpeed;
    private float _verticalVelocity;
    private bool _isAttacking;
    private bool _isLightAttacking1 = false;
    private bool _isLightAttacking2 = false;
    private bool _isLightAttacking3 = false;
    private bool _isLightAttacking4 = false;
    private bool _isLightAttacking5 = false;
    private bool _isLightAttacking6 = false;
    private bool _isLightAttacking7 = false;

    // Player fighting variables
    private GameObject _currentTarget;
    private bool _fightTimeout;
    public float _fightTimeoutDelta;
    private int _attackType = 0;
    private bool _canComboAttack = false;
    private bool _isComboAttacking = false;

    // Player input variables
    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private bool _isLightAttackPressed = false;


    // Player components
    private Animator _anim;
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
    public Action<bool> OnLightAttack1;
    public Action<bool> OnLightAttack2;
    public Action<bool> OnLightAttack3;
    public Action<bool> OnLightAttack4; 
    public Action<bool> OnLightAttack5;
    public Action<bool> OnLightAttack6;
    public Action<bool> OnLightAttack7;
    public Action<bool> OnRun;
    public Action<bool> OnIdle;

    // Animation Variables
    [SerializeField] public float AnimationBlend;
    [SerializeField] public int AnimIDSpeed;
    [SerializeField] public int AnimIDGrounded;
    [SerializeField] public int AnimIDFight;
    [SerializeField] public int AnimIDInputX;
    [SerializeField] public int AnimIDInputY;
    [SerializeField] public int AnimIDLightAttack1;
    [SerializeField] public int AnimIDLightAttack2;
    [SerializeField] public int AnimIDLightAttack3;
    [SerializeField] public int AnimIDLightAttack4;
    [SerializeField] public int AnimIDLightAttack5;
    [SerializeField] public int AnimIDLightAttack6;
    [SerializeField] public int AnimIDLightAttack7;
    [SerializeField] public int AnimIDCombo;
    [SerializeField] public int AnimationIDAttackType;
    //[SerializeField] private bool _isFighting;
    [SerializeField] private float _transitionTime;


    // Player action events
    public Action OnAttackContact;
/*    public Action<bool> OnComboAttack;*/

    // Getters and setters
    // Input
    public Vector2 MoveInput { get { return _moveInput; } set { _moveInput = value; } }
    public Vector2 LookInput { get { return _lookInput; } set { _lookInput = value; } }
    public bool IsLightAttackPressed { get { return _isLightAttackPressed; } set { _isLightAttackPressed = value; } }

    // Speed
    public float Speed { get { return _speed; } }
    public float TargetSpeed { get { return _targetSpeed; } set { _targetSpeed = value; } }
    public float VerticalVelocity { get { return _verticalVelocity; } set { _verticalVelocity = value; } }

    // State
    public Animator Animator { get { return _anim; } set { _anim = value; } }
    public bool IsGrounded { get { return _isGrounded; } }
    public bool IsFighting { get { return _isFighting; } set { _isFighting = value; } }
    public bool IsAttacking { get { return _isAttacking; } set { _isAttacking = value; } }
    public bool IsLightAttacking1 { get { return _isLightAttacking1; } set { _isLightAttacking1 = value; } }
    public bool IsLightAttacking2 { get { return _isLightAttacking2; } set { _isLightAttacking2 = value; } }
    public bool IsLightAttacking3 { get { return _isLightAttacking3; } set { _isLightAttacking3 = value; } }
    public bool IsLightAttacking4 { get { return _isLightAttacking4; } set { _isLightAttacking4 = value; } }
    public bool IsLightAttacking5 { get { return _isLightAttacking5; } set { _isLightAttacking5 = value; } }
    public bool IsLightAttacking6 { get { return _isLightAttacking6; } set { _isLightAttacking6 = value; } }
    public bool IsLightAttacking7 { get { return _isLightAttacking7; } set { _isLightAttacking7 = value; } }
 
    // Fighting
    public int AttackType { get { return _attackType; } set { _attackType = value; } }
    public float FightTimeoutDelta { get { return _fightTimeoutDelta; } set { _fightTimeoutDelta = value; } }
    public bool FightTimeoutActive { get { return _fightTimeout; } set { _fightTimeout = value; } }
    public bool CanComboAttack { get { return _canComboAttack; } set { _canComboAttack = value; } }
    public bool IsComboAttacking { get { return _isComboAttacking;} set { _isComboAttacking = value; } }

    // Debug
   //public Vector3 debugFloat;

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
        if (!IsLightAttacking1)
        {
            if (_moveInput != Vector2.zero)
            {
                transform.DOLookAt(transform.position + InputDirection(), .2f);
            }
            else
            {
                if(_currentTarget != null)
                {
                    transform.DOLookAt(_currentTarget.transform.position, .2f);
                }
                else
                {
                    return;
                }
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
    }

    // Set component values
    private void SetComponentValues()
    {
        // Set references to components and initialize player controls
        _anim = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
    }

    // Initialize player state machine
    private void InitilaizeStateMachine()
    {
        // Initialize the player state machine with default state
        _states = new StateFactory(this);
        _currentState = _states.FreeRoam();
        _currentState.EnterState();
    }

    public void EnemyDetection()
    {
        if (_moveInput != Vector2.zero)
        {
            RaycastHit info;
            if (Physics.SphereCast(transform.position, 1f, InputDirection(), out info, 5f, EnemyLayers))
            {
                _currentTarget = info.transform.gameObject;
            }
            else
            {
                _currentTarget = null;
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
            transform.DOLookAt(_currentTarget.transform.position, duration);
        }
        else
        {
            if (_moveInput != Vector2.zero)
            {
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

    public void OnAttackAnimationBegin()
    {

    }
    public void OnAttackAnimationContact()
    {
        OnAttackContact?.Invoke();          
    }
    public void OnAttackAnimationComplete()
    {
        ExitCurrentAttackState();     
    }
    public void OnAttackAnimationRecover()
    {
        _isFighting = false;
    }

    public void ExitCurrentAttackState()
    {
        switch (_attackType)
        {
            case 1:
                _isLightAttacking1 = false;
                break;
            case 2:
                _isLightAttacking2 = false;
                break;
            case 3:
                _isLightAttacking3 = false;
                break;
            case 4:
                _isLightAttacking4 = false;
                break;
            case 5:
                _isLightAttacking5 = false;
                break;
            case 6:
                _isLightAttacking6 = false;
                break;
            case 7:
                _isLightAttacking7 = false;
                break;
            default:
                break;
        }
    }

    private void AssignAnimationIDs()
    {
        AnimIDSpeed = Animator.StringToHash("Speed");
        AnimIDGrounded = Animator.StringToHash("Grounded");
        AnimIDFight = Animator.StringToHash("Fight");
        AnimIDInputX = Animator.StringToHash("InputX");
        AnimIDInputY = Animator.StringToHash("InputY");
        AnimIDLightAttack1 = Animator.StringToHash("LightAttack1");
        AnimIDLightAttack2 = Animator.StringToHash("LightAttack2");
        AnimIDLightAttack3 = Animator.StringToHash("LightAttack3");
        AnimIDLightAttack1 = Animator.StringToHash("LightAttack1");
        AnimIDLightAttack2 = Animator.StringToHash("LightAttack2");
        AnimIDLightAttack4 = Animator.StringToHash("LightAttack4");
        AnimIDLightAttack5 = Animator.StringToHash("LightAttack5");
        AnimIDLightAttack6 = Animator.StringToHash("LightAttack6");
        AnimIDLightAttack7 = Animator.StringToHash("LightAttack7");
    }

    private void SetMovementAnimationValues()
    {
        _anim.SetFloat(AnimIDInputX, _moveInput.x);
        _anim.SetFloat(AnimIDInputY, _moveInput.y);
    }
    private void SetMovementAnimationSpeed()
    {
        AnimationBlend = Mathf.Lerp(AnimationBlend, _targetSpeed, Time.deltaTime * SpeedChangeRate);
        if (AnimationBlend < 0.01f) AnimationBlend = 0f;
        _anim.SetFloat(AnimIDSpeed, AnimationBlend);
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
