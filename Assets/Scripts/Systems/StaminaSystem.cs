using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaSystem : MonoBehaviour
{
    public StateMachine _stateMachine;
    public float MaxStamina = 100;
    public float MinStamina = 0;
    public float _currentStamina;
    public float ReplenishRate;
    public float ReplenishDelay;
    public float DepletionThreshold;
    public float ReplenishThreshold;

    public float CurrentStamina { get { return _currentStamina; } }
    public bool isDepleted;

    public Action OnReplenish;
    public Action OnEnableStamina;
    public Action OnDisableStamina;
    public Action OnDeath;
    public Action OnExhaustedAttempt;

    public float HeavyAttackCost;
    public float LightAttackCost;
    public float ParryCost;
    public float SprintCost;

    public float stateTime;

    private void Awake()
    {
        _stateMachine = GetComponent<StateMachine>();
    }

    private void OnEnable()
    {
        _stateMachine.OnHeavyAttack += SpendStamina;
        _stateMachine.OnLightAttack += SpendStamina;
        _stateMachine.OnParry += SpendStamina;
    }

    private void OnDisable()
    {
        _stateMachine.OnHeavyAttack -= SpendStamina;
        _stateMachine.OnLightAttack -= SpendStamina;
        _stateMachine.OnParry -= SpendStamina;
    }

    private void Start()
    {
        _currentStamina = MaxStamina;
    }

    private void Update()
    {
        if (!_stateMachine.IsDead)
        {
            ActivateStamina();
            ResetDepletionCheck();
        }

        if (_currentStamina < MaxStamina)
        {
            stateTime += Time.deltaTime;
        }
    }

    public void ActivateStamina()
    {
        if (_currentStamina < MaxStamina && stateTime > ReplenishDelay)
        {
            _currentStamina += Time.deltaTime * ReplenishRate;
        }

        if (_currentStamina >= MaxStamina)
        {
            _currentStamina = MaxStamina;
        }

    }

    public void ResetDepletionCheck()
    {
        if (_currentStamina > ReplenishThreshold)
        {
            isDepleted = false;
        }
        else if (_currentStamina <= DepletionThreshold)
        {
            isDepleted = true;
        }
    }

    public void SpendStamina(bool value, string actionType)
    {
        if (value)
        {
            if (!isDepleted)
            {
                if (_currentStamina >= GetActionCost(actionType))
                {
                    stateTime = 0f;
                    _currentStamina -= GetActionCost(actionType);
                }
                else
                {
                    OnExhaustedAttempt?.Invoke();
                    _currentStamina = 0;
                    isDepleted = true;
                }
            }
            else
            {

            }
        }
    }

    public void ResetStamina(bool value)
    {
        _currentStamina = MaxStamina;
        isDepleted = false;
    }

    public void RefillStamina()
    {
        _currentStamina = MaxStamina;
        isDepleted = false;
        OnReplenish?.Invoke();
    }

    public void EnableStamina()
    {
        _currentStamina = MaxStamina;
        isDepleted = false;
        OnEnableStamina?.Invoke();
    }

    public void DisableStamina()
    {
        OnDisableStamina?.Invoke();
    }

    public float GetActionCost(string actionType)
    {
        float cost;

        switch (actionType)
        {
            case "Heavy":
                cost = HeavyAttackCost;
                break;
            case "Light":
                cost = LightAttackCost;
                break;
            case "Parry":
                cost = ParryCost;
                break;
            default:
                cost = 0.0f;
                break;
        }

        return cost;
    }
}