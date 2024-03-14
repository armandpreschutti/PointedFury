using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] bool _isPlayer;
    [SerializeField] bool _isAi;
    [SerializeField] DebugTester _aiStateMachine;
    [SerializeField] PlayerStateMachine _playerStateMachine;
    [SerializeField] float _maxHealth;
    [SerializeField] float _currentHealh;
    [SerializeField] float _DamageAmount;
    public Action OnDeath;

    private void Awake()
    {
        if (_isPlayer)
        {
            _playerStateMachine= GetComponent<PlayerStateMachine>();
        }
        if (_isAi)
        {
            _aiStateMachine = GetComponent<DebugTester>();
        }
    }
    private void OnEnable()
    {
        if(_playerStateMachine != null)
        {

        }
        if(_aiStateMachine != null)
        {
            _aiStateMachine.OnHit += TakeDamage;
        }
    }
    private void OnDisable()
    {
        if (_playerStateMachine != null)
        {

        }
        if (_aiStateMachine != null)
        {
            _aiStateMachine.OnHit -= TakeDamage;
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        _currentHealh = _maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int attackType)
    {
        _currentHealh -= 20;
        if(_currentHealh <= 0)
        {
            Debug.Log($"{gameObject.name} has died!");
            OnDeath?.Invoke();
        }
    }
}
