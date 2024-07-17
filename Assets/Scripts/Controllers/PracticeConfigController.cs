using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeConfigController : MonoBehaviour
{
    private PracticeControls _practiceControls;
    public bool isPaused;
    public static Action OnSpawnEnemy;
    public static Action OnEnableHealthSystems;
    public static Action OnCycleEnemyTypes;
    public static Action OnClearEnemies;

    private void Awake()
    {
        _practiceControls = new PracticeControls();
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
        if(!isPaused)
        {
            //Debug.Log("Player is spawning enemy");
            OnSpawnEnemy?.Invoke();
        }
    }

    private void EnableHealthSystemsInput()
    {
        if(!isPaused)
        {
            //Debug.Log("Player is setting Health System");
            OnEnableHealthSystems?.Invoke();
        }
    }

    private void CycleEnemyTypesInput()
    {
        if(!isPaused)
        {
            //Debug.Log("Player is cycling enemy types");
            OnCycleEnemyTypes?.Invoke();
        }
    }

    private void ClearEnemiesInput()
    {
        if(!isPaused)
        {
            Debug.Log("Player has cleared enemies");
            OnClearEnemies?.Invoke();
        }
    }

    private void SubscribeToActions()
    {
        _practiceControls.Player.SpawnEnemy.performed += ctx => SpawnEnemyInput();
        _practiceControls.Player.EnableHealthSystems.performed += ctx => EnableHealthSystemsInput();
        _practiceControls.Player.CycleEnemyTypes.performed+= ctx => CycleEnemyTypesInput();
        _practiceControls.Player.ClearEnemies.performed+= ctx => ClearEnemiesInput();

    }

    private void UnsubscribeToActions()
    {
        _practiceControls.Player.SpawnEnemy.performed -= ctx => SpawnEnemyInput();
        _practiceControls.Player.EnableHealthSystems.performed -= ctx => EnableHealthSystemsInput();
        _practiceControls.Player.CycleEnemyTypes.performed -= ctx => CycleEnemyTypesInput();
        _practiceControls.Player.ClearEnemies.performed -= ctx => ClearEnemiesInput();
    }
}
