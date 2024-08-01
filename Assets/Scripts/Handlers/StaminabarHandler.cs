using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminabarHandler : MonoBehaviour
{
    public StaminaSystem _staminaSystem;
    public Slider _staminaBarSlider;
    public bool IsAI;
    public Animator animator;

    public Transform target; // The target game object
    public RectTransform _rectTransform; // The health bar UI element
    public Vector3 offset; // Offset for the health bar position
    private Camera mainCamera;

    private void Awake()
    {
        _staminaSystem = GameObject.Find("Player").GetComponent<StaminaSystem>();

        if (!IsAI)
        {
            //_staminaSystem = GameObject.Find("Player").GetComponent<StaminaSystem>();

        }
        else
        {
/*            _staminaBarSlider.gameObject.SetActive(false);
            _staminaBarSlider.maxValue = _staminaSystem.CurrentStamina;
            _rectTransform = GetComponent<RectTransform>();*/
        }
    }

    private void OnEnable()
    {
        _staminaSystem.OnDeath += DisableEnemyHealthBar;
        _staminaSystem.OnReplenish += SetStaminaBarValue;
        _staminaSystem.OnEnableStamina += EnableStaminaBar;
        _staminaSystem.OnDisableStamina += DisableStaminaBar;
        UserInput.OnInputError += StartBarAnimation;
    }

    private void OnDisable()
    {
        _staminaSystem.OnDeath -= DisableEnemyHealthBar;
        _staminaSystem.OnReplenish -= SetStaminaBarValue;
        _staminaSystem.OnEnableStamina -= EnableStaminaBar;
        _staminaSystem.OnDisableStamina -= DisableStaminaBar;
        UserInput.OnInputError -= StartBarAnimation;
    }

    void Start()
    {
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (IsAI)
        {
            if (_staminaBarSlider.IsActive())
            {
                // Convert the target's world position to screen space
                Vector3 screenPosition = mainCamera.WorldToScreenPoint(target.position + new Vector3(0f, 1.8f, 0f));

                // Clamp the screen position to ensure it's within the screen bounds
                screenPosition.x = Mathf.Clamp(screenPosition.x, 0, Screen.width);
                screenPosition.y = Mathf.Clamp(screenPosition.y, 0, Screen.height);

                // Set the health bar's position
                _rectTransform.position = screenPosition;
            }
        }

        SetStaminaBarValue();
        SetStaminaBarColor();
    }

    public void StartBarAnimation()
    {
        animator.Play("Exhausted", 0, 0);
    }


    public void SetStaminaBarValue()
    {
        _staminaBarSlider.value = _staminaSystem._currentStamina;
    }
    public void SetStaminaBarColor()
    {
        Image fillRect = _staminaBarSlider.fillRect.GetComponent<Image>();

        if (_staminaSystem._currentStamina > 50)
        {
            fillRect.color = Color.white;
        }
        else if (_staminaSystem._currentStamina > 20)
        {
            fillRect.color = Color.yellow;
        }
        else
        {
            fillRect.color = Color.red;
        }
    }
     public void ResetHealthBarValue()
     {
         if (IsAI)
         {
             _staminaBarSlider.gameObject.SetActive(true);
         }
         _staminaBarSlider.value = _staminaSystem.CurrentStamina;
     }
     public void DisableEnemyHealthBar()
     {
         if (IsAI)
         {
             _staminaBarSlider.gameObject.SetActive(false);
         }
     }

     public void EnableStaminaBar()
     {
         _staminaBarSlider.value = _staminaSystem.CurrentStamina;
         _staminaBarSlider.gameObject.SetActive(true);
     }

     public void DisableStaminaBar()
     {
         _staminaBarSlider.gameObject.SetActive(false);
     }
}
