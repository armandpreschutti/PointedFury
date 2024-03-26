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
    private void Update()
    {
        CloseFightDistance();
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

            /*// If the distance is greater than stopDistance, move towards the target
            if (_distanceToTarget < _stateMachine.CombatDistance)
            {
                
            }
            else { 
            }*/
            _isMeleeRange = _distanceToTarget < _closeInDistance ? true : false;
        }
        else
        {
            return;
        }
    }
    private void Start()
    {
        _target = GameObject.Find("Player").gameObject;
        StartCoroutine(GetRandomDirection());
    }


    public IEnumerator GetRandomDirection()
    {
        yield return new WaitForSeconds(.25f);
        Debug.Log("New chance assigned");

        _attackChance = UnityEngine.Random.Range(1, 11);
        if (!_isMeleeRange )
        {
            _moveInput = new Vector2(0, 1);
            _stateMachine.IsLightAttackPressed = false;
        }
        else
        {
            if(_attackChance > Difficulty)
            {
                _moveInput = Vector2.zero;
                _stateMachine.IsLightAttackPressed = true;
            }
            else
            {
                _moveInput = new Vector2(UnityEngine.Random.Range(-1, 2), 0);
            }
            
        }
        //_randomInput = new Vector2(UnityEngine.Random.Range(-1, 2), UnityEngine.Random.Range(-1, 2));
        //_stateMachine.MoveInput = _randomInput;
        StartCoroutine(GetRandomDirection());
    }
}
