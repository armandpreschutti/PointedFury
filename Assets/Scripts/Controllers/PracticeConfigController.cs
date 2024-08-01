using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PracticeConfigController : MonoBehaviour
{
    private PracticeControls _practiceControls;
    public bool isPaused;
    public static Action OnSpawnEnemy;
    public static Action OnToggleVitalsSystems;
    public static Action OnCycleEnemyTypes;
    public static Action OnClearEnemies;

    public Image vitalsToggleButton;
    public Image enemyTypeButton;

    public Sprite VitalsDisabledButton;
    public Sprite VitalsEnabledButton;
    public Sprite WeakEnemyButton;
    public Sprite MediumEnemyButton;
    public Sprite HeavyEnemyButton;
    public Sprite BossEnemyButton;
    public GameObject PracticeSettingsControls;
    public GameObject PracticeSettingsButton;
    public int enemyTypeIndex;
    public bool isVitalsActive;

    private void Awake()
    {
        _practiceControls = new PracticeControls();
        enemyTypeIndex = 1;
        if (isVitalsActive)
        {
            vitalsToggleButton.sprite = VitalsEnabledButton;
        }
        else
        {
            vitalsToggleButton.sprite = VitalsDisabledButton;
        }
    }

    private void OnEnable()
    {
        _practiceControls.Enable();
        PauseMenuController.OnGamePaused += PausePlayerControl;
        SubscribeToActions();
    }

    private void OnDisable()
    {
        _practiceControls.Disable();
        PauseMenuController.OnGamePaused -= PausePlayerControl;
        UnsubscribeToActions();
    }

    public void PausePlayerControl(bool value)
    {
        isPaused = value;
    }

    private void SpawnEnemyInput()
    {
        if(!isPaused && PracticeSettingsControls.activeSelf)
        {
            //Debug.Log("Player is spawning enemy");
            OnSpawnEnemy?.Invoke();
        }
    }

    private void EnableVitalsSystemsInput()
    {
        if(!isPaused && PracticeSettingsControls.activeSelf)
        {
            //Debug.Log("Player is setting Health System");
            SetVitalsToggleButton();
            OnToggleVitalsSystems?.Invoke();
        }
    }

    private void CycleEnemyTypesInput()
    {
        if(!isPaused && PracticeSettingsControls.activeSelf)
        {
            //Debug.Log("Player is cycling enemy types");
            SetEnemyTypeButton();
            OnCycleEnemyTypes?.Invoke();
        }
    }

    private void ClearEnemiesInput()
    {
        if(!isPaused && PracticeSettingsControls.activeSelf)
        {
            Debug.Log("Player has cleared enemies");
            OnClearEnemies?.Invoke();

        }
    }

    public void TogglePracticeControls(bool value)
    {
        if (value)
        {
            PracticeSettingsButton.SetActive(false);
            PracticeSettingsControls.SetActive(true);
        }
        else
        {
            PracticeSettingsButton.SetActive(true);
            PracticeSettingsControls.SetActive(false);
        }
    }

    private void SubscribeToActions()
    {
        _practiceControls.Player.SpawnEnemy.performed += ctx => SpawnEnemyInput();
        _practiceControls.Player.ToggleHealthSystems.performed += ctx => EnableVitalsSystemsInput();
        _practiceControls.Player.CycleEnemyTypes.performed+= ctx => CycleEnemyTypesInput();
        _practiceControls.Player.ClearEnemies.performed+= ctx => ClearEnemiesInput();
        _practiceControls.Player.TogglePracticeControls.performed += ctx => TogglePracticeControls(ctx.ReadValueAsButton());
    }

    private void UnsubscribeToActions()
    {
        _practiceControls.Player.SpawnEnemy.performed -= ctx => SpawnEnemyInput();
        _practiceControls.Player.ToggleHealthSystems.performed -= ctx => EnableVitalsSystemsInput();
        _practiceControls.Player.CycleEnemyTypes.performed -= ctx => CycleEnemyTypesInput();
        _practiceControls.Player.ClearEnemies.performed -= ctx => ClearEnemiesInput();
        _practiceControls.Player.TogglePracticeControls.performed -= ctx => TogglePracticeControls(ctx.ReadValueAsButton());
    }

    public void SetEnemyTypeButton()
    {
        switch (enemyTypeIndex)
        {
            case 0:
                enemyTypeButton.sprite = WeakEnemyButton;
                enemyTypeIndex = 1;
                break;
            case 1:
                enemyTypeButton.sprite = MediumEnemyButton;
                enemyTypeIndex = 2;
                break;
            case 2:
                enemyTypeButton.sprite = HeavyEnemyButton;
                enemyTypeIndex = 3;
                break;
            case 3:
                enemyTypeButton.sprite = BossEnemyButton;
                enemyTypeIndex = 0;
                break;
            default:
                enemyTypeButton.sprite = WeakEnemyButton;
                enemyTypeIndex = 1;
                break;
        }
    }
    public void SetVitalsToggleButton()
    {
        isVitalsActive = !isVitalsActive;

        if (isVitalsActive)
        {
            vitalsToggleButton.sprite = VitalsEnabledButton;
        }
        else
        {
            vitalsToggleButton.sprite = VitalsDisabledButton;
        }
    }
}
