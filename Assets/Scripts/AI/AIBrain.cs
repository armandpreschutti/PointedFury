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
    public bool _initialAttack;
    public bool _isAttacking;
    public float _strafeDirection;
    public int comboCount;
    public int hitCount;

    public float targetDistance;
    public float AttackDistance = 1.5f;
    public float WatchDistance = 3f;
    public float DistanceBuffer = 1f;

    public int ComboSkill;
    public bool CanComboChain;
    public int HitTolerance;
    public float BlockReleaseTime = 1;
    public int ParrySkill;
    public int HeavyAttackSkill;
    public int EvadeSkill;
    public float InitialAttackDelay;
    public float AttackInterval;
    public float timeSinceAttack;


    // enemy management variables
    public bool isActivated;
    public bool isAttacker;
    public bool isWatcher;


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

    private void Update()
    {
        _currentState.UpdateStates();
        GetStateMachineVariables();
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
    }

    private void GetStateMachineVariables()
    {
        isHurt = StateMachine.IsHurt;
        StateMachine.MoveInput = moveInput;
    }

    public float DistanceToPlayer(Transform target)
    {
        Vector3 directionToTarget = transform.position - target.position;
        float distanceToTarget = directionToTarget.magnitude;
        return distanceToTarget;
    }

    public void AddToHitCount()
    {
        hitCount++;
    }
}




























