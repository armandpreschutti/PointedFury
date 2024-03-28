using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class AIBrain : MonoBehaviour
{
    [SerializeField] StateMachine _stateMachine;
    [SerializeField] Animator _anim;
    [SerializeField] GameObject _target;
    [SerializeField] Vector2 _randomInput;
    [SerializeField] CharacterController _characterController;
    [SerializeField] float _targetDistance;
    [SerializeField] Vector3 _moveDirection;
    [SerializeField] Vector3 _inputDirection;
    [SerializeField] bool _isMeleeRange;
    [SerializeField] float _distanceToTarget;
    [SerializeField] Vector2 _moveInput;
    [SerializeField] int _attackChance = 0;
    [SerializeField] int Difficulty = 0;
    [SerializeField] float _closeInDistance;
    [SerializeField] bool _isAttacking;


    public Action<int> OnHit;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _stateMachine =GetComponent<StateMachine>();
    }
    private void OnEnable()
    {

    }
    private void OnDisable()
    {

    }
    private void Start()
    {
        _target = GameObject.Find("Player").gameObject;
        StartCoroutine(SetAttackAttempt());
        StartCoroutine(GetRandomDirection());
    }

    private void Update()
    {
        CloseFightDistance();
        if (_stateMachine.IsStunned || _stateMachine.IsHurt || _stateMachine.IsDead)
        {
            _isAttacking = false;
            _stateMachine.IsParryable = false;
        }

        _stateMachine.MoveInput = _moveInput;
    }

    public void CloseFightDistance()
    {
        if (_stateMachine.CurrentTarget != null)
        {
            // Calculate the direction towards the target
            Vector3 directionToTarget = transform.position - _stateMachine.CurrentTarget.transform.position;

            // Check the distance to the target
            _distanceToTarget = directionToTarget.magnitude;

            _isMeleeRange = _distanceToTarget < _closeInDistance ? true : false;
        }
        else
        {
            return;
        }
    }
 

    public IEnumerator GetRandomDirection()
    {
        yield return new WaitForSeconds(.25f);
        //Debug.Log("New chance assigned");

        if(!_isAttacking)
        {
            if (!_isMeleeRange)
            {
                _moveInput = new Vector2(0, 1);
                _stateMachine.IsLightAttackPressed = false;
            }
            else
            {
                _moveInput = new Vector2(UnityEngine.Random.Range(-1, 2), 0);
            }
        }
        else
        {
            _moveInput = Vector2.zero;
        }
        
        //_randomInput = new Vector2(UnityEngine.Random.Range(-1, 2), UnityEngine.Random.Range(-1, 2));
        //_stateMachine.MoveInput = _randomInput;
        StartCoroutine(GetRandomDirection());
    }

    public IEnumerator SetAttackAttempt()
    {
        Debug.LogError("Enemy is thinking about attacking");
        _attackChance = UnityEngine.Random.Range(1, 11);
        yield return new WaitForSeconds(.25f);

        if (_attackChance > Difficulty && !_isAttacking && !_stateMachine.IsStunned)
        {
            StartCoroutine(SetParryState());
            
        }
        else
        {
            StartCoroutine(SetAttackAttempt());
        }

    }
    public IEnumerator SetParryState()
    {
        Debug.LogError("Enemy is going to attack");

        _stateMachine.IsParryable = true;
        _isAttacking = true;
        yield return new WaitForSeconds(1f);
        if (_isAttacking)
        {
            _stateMachine.IsLightAttackPressed = true;
            _isAttacking = false;
        }
        Debug.LogError("Enemy is finished attacking");
        StartCoroutine(SetAttackAttempt());
    }
}
