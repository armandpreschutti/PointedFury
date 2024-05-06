using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public enum AIState
{
    Idle,
    Approaching,
    Strafing,
    Hurt,
    Attack,
    Block,
    Parry
}

public class AIBrainDepricated : MonoBehaviour
{
    public AIState currentState;
    public AIState previousState;
    [SerializeField] float _stateTime;

    [SerializeField] StateMachine _stateMachine;
    [SerializeField] GameObject _currentTarget;

    // enemy mobility variables
    [SerializeField] float _distanceToTarget;
    [SerializeField] Vector2 _moveInput;

    [SerializeField] bool _isAttackRange;
    [SerializeField] bool _isWatchRange;
    [SerializeField] bool _isHurt;
    [SerializeField] bool _isStunned;
    [SerializeField] bool _isAttacking;
    [SerializeField] float _strafeDirection;
    [SerializeField] int _comboCount;
    [SerializeField] int _hitCount;
    [SerializeField] float _blockReleaseTime;
    public float AttackDistance = 1.5f;
    public float WatchDistance = 3f;
    public float DistanceBuffer = 1f;

    // enemy stats
    public int AttackSkill;
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
        _stateMachine = GetComponent<StateMachine>();
    }
    private void OnEnable()
    {
        _stateMachine.OnLightAttack += AddToComboCount;
        _stateMachine.OnHurt += AddToHitCount;
    }

    private void OnDisable()
    {
        _stateMachine.OnLightAttack -= AddToComboCount;
        _stateMachine.OnHurt -= AddToHitCount;
    }
    private void Start()
    {
        _currentTarget = FindAnyObjectByType<UserInput>().gameObject;
        _stateMachine.EnemiesNearby.Add(_currentTarget);
        _stateMachine.CurrentTarget = _currentTarget;
        ChangeState(AIState.Idle);
    }

    private void Update()
    {

        SetOngoingStateVariables();
        switch (currentState)
        {
            case AIState.Idle:
                IdleState();
                break;
            case AIState.Approaching:
                ApproachingState();
                break;
            case AIState.Strafing:
                StrafingState();
                break;
            case AIState.Hurt:
                HurtState();
                break;
            case AIState.Attack:
                AttackingState();
                break;
            case AIState.Block:
                BlockingState();
                break;
            case AIState.Parry:
                ParryState();
                break;
        }
    }

    private void IdleState()
    {
        /*if (!_isHurt)
        {
            _moveInput = Vector2.zero;
            if (isAttacker)
            {
                if (TargetDistance(_currentTarget.transform) > (AttackDistance + DistanceBuffer))
                {
                    ChangeState(AIState.Approaching);
                }
                else if (_stateTime > 1f)
                {
                    ChangeState(AIState.Attack);
                }
            }
            else if (isWatcher)
            {
                if (TargetDistance(_currentTarget.transform) > WatchDistance + DistanceBuffer)
                {
                    ChangeState(AIState.Approaching);
                }
                else if (_stateTime > 3f)
                {
                    SetRandomStrafeDirection();
                    ChangeState(AIState.Strafing);
                }
            }
        }
        else
        {
            ChangeState(AIState.Hurt);
        }*/

    }

    private void ApproachingState()
    {
       /* if (!_isHurt)
        {
            _moveInput.y = 1f;
            if (isAttacker)
            {
                if (TargetDistance(_currentTarget.transform) <= AttackDistance)
                {
                    ChangeState(AIState.Idle);
                }
            }
            else
            {
                if (TargetDistance(_currentTarget.transform) <= WatchDistance)
                {
                    ChangeState(AIState.Idle);
                }
            }
        }
        else
        {
            ChangeState(AIState.Hurt);
        }*/

    }

    private void StrafingState()
    {
        _moveInput.x = _strafeDirection;
        if (!_isHurt)
        {
            if (isAttacker)
            {
                if (_stateTime > 2f || !_isAttackRange)
                {

                    ChangeState(AIState.Idle);
                }
            }
            else
            {
                if (_stateTime > 2f || _isWatchRange)
                {

                    ChangeState(AIState.Idle);
                }
            }
        }
        else
        {
            ChangeState(AIState.Hurt);
        }

    }

    private void HurtState()
    {
        if (_hitCount >= HitTolerance)
        {
            _hitCount = 0;
            ChangeState(AIState.Block);
        }

        if (!_isHurt && !_isStunned)
        {
            _hitCount = 0;
            ChangeState(AIState.Idle);
        }
    }
    private void AttackingState()
    {
        if (_isHurt || _isStunned)
        {
            _comboCount = 0;
            _stateMachine.IsLightAttackPressed = false;
            ChangeState(AIState.Hurt);
        }
        else if (_comboCount < ComboSkill)
        {
            Debug.Log("Trying to Attacking");
            _stateMachine.IsLightAttackPressed = true;
            ChangeState(AIState.Attack);
        }

        else
        {
            _comboCount = 0;
            ChangeState(AIState.Idle);
        }
    }

    private void BlockingState()
    {
        _stateMachine.IsBlockPressed = true;
        if (!_currentTarget.GetComponent<StateMachine>().IsAttacking)
        {
            _blockReleaseTime += Time.deltaTime;
            if (_blockReleaseTime >= 1f)
            {
                _stateMachine.IsBlockPressed = false;
                ChangeState(AIState.Idle);
                _blockReleaseTime = 0f;
            }
        }
    }

    private void ParryState()
    {
        if (!_stateMachine.IsParrying)
        {
            ChangeState(AIState.Idle);
        }
    }
    public void AddToHitCount()
    {
        _hitCount++;
    }

    public void AddToComboCount(bool value, string attackType)
    {
        if (value)
        {
            _comboCount++;
        }

    }

    private void ChangeState(AIState newState)
    {
        _stateTime = 0f;
        previousState = currentState;
        currentState = newState;
    }

    private float TargetDistance(Transform target)
    {
        Vector3 directionToTarget = transform.position - target.position;
        float distanceToTarget = directionToTarget.magnitude;
        return distanceToTarget;
    }

    public void SetRandomStrafeDirection()
    {
/*        _strafeDirection = UnityEngine.Random.Range(-1, 2);
        if (_strafeDirection == 0)
        {
            SetRandomStrafeDirection();
        }*/
    }
    public bool IsMeleeRange()
    {
        if (_distanceToTarget <= AttackDistance + DistanceBuffer)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetOngoingStateVariables()
    {
        _stateMachine.MoveInput = _moveInput;
        _stateTime += Time.deltaTime;
        _distanceToTarget = TargetDistance(_currentTarget.transform);
        _isAttackRange = TargetDistance(_currentTarget.transform) < (AttackDistance + DistanceBuffer) ? true : false;
        _isWatchRange = TargetDistance(_currentTarget.transform) < (WatchDistance + DistanceBuffer) ? true : false;
        _isHurt = _stateMachine.IsHurt;
        _isStunned = _stateMachine.IsStunned;
        _isAttacking = _stateMachine.IsAttacking;
    }

}