using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class AIBrain : MonoBehaviour
{
    private AIBaseState _currentState;
    private AIStateFactory _states;
    public AIBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }

    public float _stateTime;
    public StateMachine StateMachine;
    public GameObject _currentTarget;

    public string DebugSuperState;
    public string DebugSubState;
    // enemy mobility variables
    public float distanceToTarget;
    public Vector2 moveInput;

    public bool _isAttackRange;
    public bool _isWatchRange;
    public bool isHurt;
    public bool _isStunned;
    public bool _isAttacking;
    public float _strafeDirection;
    public int comboCount;
    public int hitCount;
    public float _blockReleaseTime;
    public float targetDistance;
    public float AttackDistance = 1.5f;
    public float WatchDistance = 3f;
    public float DistanceBuffer = 1f;

    public int ComboSkill;
    public int BlockSkill;
    public int BlockBreakSkill;
    public int HitTolerance;

    // enemy management variables
    public bool isActivated;
    public bool isAttacker;
    public bool isWatcher;

    // other variables
    bool isMeleeRange;
    bool _isPaused;

    private void Awake()
    {
        SetComponentValues();
        InitilaizeStateMachine();
    }
    private void OnEnable()
    {
        StateMachine.OnHurt += AddToHitCount;
    }
    private void OnDisable()
    {
        StateMachine.OnHurt -= AddToHitCount;
    }
    private void Start()
    {
        // AssignAnimationIDs();
    }

    private void Update()
    {
        _currentState.UpdateStates();
        GetStateMachineVariables();

        /*  GroundedCheck();
          SetPlayerSpeed();
          CheckIsFighting();
          SetMovementAnimationSpeed();
          SimulateGravity();*/
    }
    // Initialize player state machine
    private void InitilaizeStateMachine()
    {
        // Initialize the player state machine with default state
        _states = new AIStateFactory(this);
        _currentState = _states.WatcherRootState();
        _currentState.EnterStates();
    }

    private void SetComponentValues()
    {
        StateMachine = GetComponent<StateMachine>();
        _currentTarget = FindAnyObjectByType<UserInput>().gameObject;
        StateMachine.EnemiesNearby.Add(_currentTarget);
        StateMachine.CurrentTarget = _currentTarget;

    }

    private void GetStateMachineVariables()
    {
        isHurt = StateMachine.IsHurt;
        StateMachine.MoveInput = moveInput;
        isMeleeRange = IsMeleeRange();
    }

    public float DistanceToPlayer(Transform target)
    {
        Vector3 directionToTarget = transform.position - target.position;
        float distanceToTarget = directionToTarget.magnitude;
        return distanceToTarget;
    }

    public bool IsMeleeRange()
    {
        if (DistanceToPlayer(_currentTarget.transform) <= AttackDistance + DistanceBuffer)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddToHitCount()
    {
        hitCount++;
    }
}




























