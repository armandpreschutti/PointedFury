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
    public float _nearDeathAmount;
    
    public Action OnDeath;
    public Action OnDamage;
    public Action OnReplish;

    public float CurrentHealth { get { return _currentHealh; } }

    private void Awake()
    {
       _stateMachine = GetComponent<StateMachine>();
    }

    private void OnEnable()
    {
        _stateMachine.OnLightAttackRecieved += TakeDamage;
        _stateMachine.OnHeavyAttackRecieved += TakeDamage;
        _stateMachine.OnFinisherRecieved += TakeDamage;
        _stateMachine.OnBlockSuccessful += TakeDamage;
        _stateMachine.OnParryRecieved += TakeDamage;
    }

    private void OnDisable()
    {
        _stateMachine.OnLightAttackRecieved -= TakeDamage;
        _stateMachine.OnHeavyAttackRecieved -= TakeDamage;
        _stateMachine.OnFinisherRecieved -= TakeDamage;
        _stateMachine.OnBlockSuccessful -= TakeDamage;
        _stateMachine.OnParryRecieved -= TakeDamage;
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
            if (_currentHealh <= MinHealth && !_stateMachine.IsDead)
            {
                _currentHealh = MinHealth;
                OnDeath?.Invoke();
                _stateMachine.IsDead = true;
                _stateMachine.OnDeath?.Invoke();
                if(GetComponent<AIBrain>() != null)
                {
                    GetComponent<AIBrain>().enabled = false;
                }
            }
            else if(_currentHealh <= _nearDeathAmount)
            {
                _stateMachine.IsNearDeath = true;
            }
        }        
    }
   
    public void ResetHealth(bool value)
    {
        _currentHealh = MaxHealth;
    }

    public void RefillHealth()
    {
        _currentHealh = MaxHealth;
        OnReplish?.Invoke();
    }
}
