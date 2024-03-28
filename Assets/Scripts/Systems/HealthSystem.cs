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

    private void Awake()
    {
       _stateMachine = GetComponent<StateMachine>();
       //_healthBar = GetComponentInChildren<Slider>();
    }
    private void OnEnable()
    {
        _stateMachine.OnHitLanded += TakeDamage;
        _stateMachine.OnDeath += ResetHealth;
    }
    private void OnDisable()
    {
        _stateMachine.OnHitLanded -= TakeDamage;
        _stateMachine.OnDeath -= ResetHealth;
    }
    // Start is called before the first frame update
    void Start()
    {
        _currentHealh = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //_healthBar.transform.LookAt(Camera.main.transform.position);
    }

    public void TakeDamage()
    {
        if(!_stateMachine.IsDead)
        {
            _currentHealh -= 20;
            if (_currentHealh <= MinHealth)
            {
                Debug.Log($"{gameObject.name} has died!");
                _stateMachine.IsDead = true;
            }
        }
        
    }
    public void ResetHealth(bool value)
    {
        _currentHealh = MaxHealth;
    }
}
