using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] StateMachine _stateMachine;
    [SerializeField] float MaxHealth = 100;
    [SerializeField] float MinHealth = 0;
    [SerializeField] float _currentHealh;
    [SerializeField] Slider _healthBar;
    
    public Action OnDeath;
    public Action OnDamage;

    public float CurrentHealth { get { return _currentHealh; } }

    private void Awake()
    {
       _stateMachine = GetComponent<StateMachine>();
    }

    private void OnEnable()
    {
        _stateMachine.OnLightHitLanded += TakeDamage;
        _stateMachine.OnHeavyHitLanded += TakeDamage;
    }

    private void OnDisable()
    {
        _stateMachine.OnLightHitLanded -= TakeDamage;
        _stateMachine.OnHeavyHitLanded -= TakeDamage;
    }

    void Start()
    {
        _currentHealh = MaxHealth;
    }

    public void TakeDamage(float value)
    {
        if(!_stateMachine.IsDead)
        {
            _currentHealh -= value;
            OnDamage?.Invoke();
            if (_currentHealh <= MinHealth)
            {
                OnDeath?.Invoke();
                _stateMachine.IsDead = true;
                _stateMachine.OnDeath?.Invoke();
            }
        }        
    }

    public void ResetHealth(bool value)
    {
        _currentHealh = MaxHealth;
    }
}
