using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarHandler : MonoBehaviour
{
    /*[SerializeField] HealthSystem _healthSystem;
    [SerializeField]  Slider _healthBarSlider;
    public bool IsAI;

    private void Awake()
    {
        if(!IsAI)
        {
            _healthSystem = GameObject.Find("Player").GetComponent<HealthSystem>();
        }
        else
        {
            _healthBarSlider.gameObject.SetActive(false);
        }
        

    }
    private void OnEnable()
    {
        if (_healthSystem != null)
        {
            _healthSystem.OnDamage += SetHealthBarValue;
            _healthSystem.OnDeath += DisableEnemyHealthBar;
        }

    }
    private void OnDisable()
    {
        if (_healthSystem != null)
        {
            _healthSystem.OnDamage -= SetHealthBarValue;
            _healthSystem.OnDeath -= DisableEnemyHealthBar;
        }
    }

    public void SetHealthBarValue()
    {
        if (IsAI)
        {
            _healthBarSlider.gameObject.SetActive(true);
        }
        _healthBarSlider.value = _healthSystem.CurrentHealth; 
    }
    public void DisableEnemyHealthBar()
    {
        if (IsAI)
        {
            _healthBarSlider.gameObject.SetActive(false);
        }
    }*/
}
