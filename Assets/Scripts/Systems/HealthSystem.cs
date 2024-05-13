using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public StateMachine _stateMachine;
    public float MaxHealth = 100;
    public float MinHealth = 0;
    public float _currentHealh;
    
    public Action OnDeath;
    public Action OnDamage;

    public float CurrentHealth { get { return _currentHealh; } }

    private void Awake()
    {
       _stateMachine = GetComponent<StateMachine>();
    }

    private void OnEnable()
    {
        _stateMachine.OnLightAttackRecieved += TakeDamage;
        _stateMachine.OnHeavyAttackRecieved += TakeDamage;
    }

    private void OnDisable()
    {
        _stateMachine.OnLightAttackRecieved -= TakeDamage;
        _stateMachine.OnHeavyAttackRecieved -= TakeDamage;
    }

    void Start()
    {
        _currentHealh = MaxHealth;
    }

    public void TakeDamage(float damage, string attackType)
    {
        if(!_stateMachine.IsDead)
        {
            _currentHealh -= damage;
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
