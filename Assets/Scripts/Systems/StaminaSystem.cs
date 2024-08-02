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
    public float HeavyChargeAttackCost;
    public float LightAttackCost;
    public float LightChargeAttackCost;
    public float ParryCost;
    public float SprintCost;
    public float DashCost;
    public float EvadeReward;
    public float DeflectReward;
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
        _stateMachine.OnDash += SpendDashStamina;
        _stateMachine.OnEvade += RewardEvade;
    }

    private void OnDisable()
    {
        _stateMachine.OnHeavyAttack -= SpendStamina;
        _stateMachine.OnLightAttack -= SpendStamina;
        _stateMachine.OnParry -= SpendStamina;
        _stateMachine.OnDash -= SpendDashStamina;
        _stateMachine.OnEvade -= RewardEvade;
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
            SprintDepletionCheck();
        }

        if (_currentStamina < MaxStamina)
        {
            stateTime += Time.deltaTime;
        }
    }

    public void ActivateStamina()
    {
        if(!_stateMachine.IsSprinting)
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

    public void SprintDepletionCheck()
    {
        if (_stateMachine.IsSprinting && ! isDepleted)
        {
            stateTime = 0f;
            _currentStamina -= Time.deltaTime * SprintCost;
            if(_currentStamina <= DepletionThreshold)
            {
                OnExhaustedAttempt?.Invoke();
            }
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

    public void SpendDashStamina(bool value)
    {
        if (value)
        {
            if (!isDepleted)
            {
                if (_currentStamina >= GetActionCost("Dash"))
                {
                    stateTime = 0f;
                    _currentStamina -= GetActionCost("Dash");
                }
                else
                {
                    //OnExhaustedAttempt?.Invoke();
                    _currentStamina = 0;
                    isDepleted = true;
                }
            }

        }
    }

    public void RewardEvade(bool value)
    {
        if (value)
        {
            _currentStamina += EvadeReward;
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
                if(_stateMachine.HeavyAttackID == 0)
                {
                    cost = HeavyChargeAttackCost;
                }
                else
                {
                    cost = HeavyAttackCost;
                }               
                break;
            case "Light":
                if (_stateMachine.LightAttackID == 0)
                {
                    cost = LightChargeAttackCost;
                }
                else
                {
                    cost = LightAttackCost;
                }
                break;
            case "Parry":
                cost = ParryCost;
                break;

            case "Dash":
                cost = DashCost;
                break;
            default:
                cost = 0.0f;
                break;
        }

        return cost;
    }
}