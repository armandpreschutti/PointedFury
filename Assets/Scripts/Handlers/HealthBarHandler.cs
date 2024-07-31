using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarHandler : MonoBehaviour
{
    public HealthSystem _healthSystem;
    public Slider _healthBarSlider;
    public bool IsAI;

    public Transform target; // The target game object
    public RectTransform _rectTransform; // The health bar UI element
    public Vector3 offset; // Offset for the health bar position
    private Camera mainCamera;

    private void Awake()
    {
        if (!IsAI)
        {
            _healthSystem = GameObject.Find("Player").GetComponent<HealthSystem>();

        }
        else
        {
            _healthBarSlider.gameObject.SetActive(false);
            _healthBarSlider.maxValue = _healthSystem.MaxHealth;
            _rectTransform = GetComponent<RectTransform>();
        }


    }

    private void OnEnable()
    {
        _healthSystem.OnDamage += SetHealthBarValue;
        _healthSystem.OnDeath += DisableEnemyHealthBar;
        _healthSystem.OnReplish += SetHealthBarValue;
        _healthSystem.OnEnableHealth += EnableHealthBar;
        _healthSystem.OnDisableHealth += DisableHealthBar;


    }
    private void OnDisable()
    {
        _healthSystem.OnDamage -= SetHealthBarValue;
        _healthSystem.OnDeath -= DisableEnemyHealthBar;
        _healthSystem.OnReplish -= SetHealthBarValue;
        _healthSystem.OnEnableHealth -= EnableHealthBar;
        _healthSystem.OnDisableHealth -= DisableHealthBar;
    }

    void Start()
    {
        mainCamera = Camera.main;
    }
    void Update()
    {
        if (IsAI)
        {
            if (_healthBarSlider.IsActive())
            {
                // Convert the target's world position to screen space
                Vector3 screenPosition = mainCamera.WorldToScreenPoint(target.position + new Vector3(0f,1.8f,0f));

                // Clamp the screen position to ensure it's within the screen bounds
                screenPosition.x = Mathf.Clamp(screenPosition.x, 0, Screen.width);
                screenPosition.y = Mathf.Clamp(screenPosition.y, 0, Screen.height);

                // Set the health bar's position
                _rectTransform.position = screenPosition;
            }
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
    }

    public void EnableHealthBar()
    {
        _healthBarSlider.value = _healthSystem.CurrentHealth;
        _healthBarSlider.gameObject.SetActive(true);
    }

    public void DisableHealthBar()
    {
        _healthBarSlider.gameObject.SetActive(false);
    }
}
