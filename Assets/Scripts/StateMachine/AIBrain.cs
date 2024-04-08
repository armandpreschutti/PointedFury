using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class AIBrain : MonoBehaviour
{
    [SerializeField] StateMachine _stateMachine;
    [SerializeField] float _distanceToTarget;
    [SerializeField] Vector2 _moveInput;
    [SerializeField] float _attackingDistance;
    [SerializeField] float _watchingDistance;

    // debug variables
    [SerializeField] SkinnedMeshRenderer _enemySkin;
    [SerializeField] Material _attackerMaterial;
    [SerializeField] Material _watcherMaterial;

    // statevariables
    float _closeInCheckTime;
    public float CloseInCheckInterval = .1f;
    float _strafeCheckTime;
    public float StrafeCheckInterval = 4f;
    float _attackCheckTime;
    public float AttackCheckInterval =3f;
    float _blockCheckTime;
    public float BlockCheckInterval = .25f;   

    // enemy stats
    public int AttackSkill;
    public int ComboSkill;
    public int BlockSkill;

    // enemy management variables
    public bool isActivated;
    public bool isAttacker;
    public bool isWatcher;

    private void Awake()
    {
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
        _attackCheckTime = AttackCheckInterval;
        _closeInCheckTime = CloseInCheckInterval;
        _blockCheckTime = BlockCheckInterval;
        //StartCoroutine(SetAttackState());
        //StartCoroutine(SetMovementState());
    }

    private void Update()
    {
        CloseInLoop();
        StrafeLoop();
        AttackLoop();
        BlockLoop();
        CheckFightDistance();

        _stateMachine.MoveInput = _moveInput;
    }

    public void CheckFightDistance()
    {
        if (_stateMachine.CurrentTarget != null)
        {
            Vector3 directionToTarget = transform.position - _stateMachine.CurrentTarget.transform.position;

            _distanceToTarget = directionToTarget.magnitude;
        }
        else
        {
            return;
        }
    } 

    public void SetAttackState()
    {
        if (isActivated)
        {
            if (isAttacker && !_stateMachine.IsAttacking)
            {
                Attack();

            }
            else
            {
                return;
            }
        }

    }

    public void Attack()
    {
        if (!_stateMachine.IsAttacking)
        {
            int attackChance = UnityEngine.Random.Range(1, AttackSkill + 1);
            if (attackChance < AttackSkill)
            {
                _stateMachine.IsLightAttackPressed = true;
            }
        }
        else if (_stateMachine.IsPostAttack)
        {
            int comboChance = UnityEngine.Random.Range(1, ComboSkill+ 1);
            if (comboChance < ComboSkill)
            {
                _stateMachine.IsLightAttackPressed = true;
            }
        }
        else
        {
            return;
        }
    }

    public void ChangeEnemyMaterial()
    {
      /*  if (isActivated)
        {
            if (isAttacker)
            {
                _enemySkin.material = _attackerMaterial;
            }
            else
            {
                _enemySkin.material = _watcherMaterial;
            }
        }
        else
        {
            return;
        }*/
        
    }
    public void StrafeLoop()
    {
        _strafeCheckTime -= Time.deltaTime;

        // Check if the timer has reached zero
        if (_strafeCheckTime <= 0)
        {
           
            // Reset the timer
            _strafeCheckTime = StrafeCheckInterval;

            if (isActivated)
            {
                float strafeDirection = UnityEngine.Random.Range(-1, 2);
                if (strafeDirection == -1)
                {
                    _moveInput.x = -1;
                }
                else if (strafeDirection == 1)
                {
                    _moveInput.x = 1;
                }
                else
                {
                    _moveInput.x = 0;
                }
            }
        }
    }

    public void CloseInLoop()
    {


        _closeInCheckTime -= Time.deltaTime;

        // Check if the timer has reached zero
        if (_closeInCheckTime <= 0)
        {

            // Reset the timer
            _closeInCheckTime = CloseInCheckInterval;

            if (isActivated)
            {
                if (isWatcher)
                {
                    if (_distanceToTarget > _watchingDistance + .5f)
                    {
                        _moveInput.y = 1f;
                    }
                    else if (_distanceToTarget < _watchingDistance - .5f)
                    {
                        _moveInput.y = -1f;
                    }
                    else
                    {
                        _moveInput.y = 0f;
                    }
                }
                else if (isAttacker)
                {
                    if (_distanceToTarget > _attackingDistance + .5f)
                    {
                        _moveInput.y = 1f;
                    }
                    else if (_distanceToTarget < _attackingDistance - .5f)
                    {
                        _moveInput.y = -1f;
                    }
                    else
                    {
                        _moveInput.y = 0f;
                    }
                }
            }
            else
            {
                _moveInput = Vector2.zero;
            }
        }
    }
    public void AttackLoop()
    {
        _attackCheckTime -= Time.deltaTime;

        // Check if the timer has reached zero
        if (_attackCheckTime <= 0)
        {

            // Reset the timer
            _attackCheckTime = AttackCheckInterval;

            if (isActivated)
            {
                if (isAttacker)
                {
                    Attack();

                }
                else
                {
                    return;
                }
            }
            // Perform actions when timer reaches zero (You can put your desired actions here)
            //Debug.Log("Timer reached zero. Resetting...");
        }
    }

    public void BlockLoop()
    {
        _blockCheckTime -= Time.deltaTime;

        // Check if the timer has reached zero
        if (_blockCheckTime <= 0)
        {

            // Reset the timer
            _blockCheckTime = BlockCheckInterval;

            if (isActivated)
            {
                int blockChance = UnityEngine.Random.Range(1, BlockSkill + 1);
                if (_stateMachine.CurrentTarget.GetComponent<StateMachine>().IsAttacking && _stateMachine.CurrentTarget.GetComponent<StateMachine>().CurrentTarget == this.gameObject && blockChance < BlockSkill)
                {
                    _stateMachine.IsBlockPressed = true;

                }
                else
                {
                    _stateMachine.IsBlockPressed = false;
                }
            }
        }
    }
}
