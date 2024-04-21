using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuController : MonoBehaviour
{
    private PauseControls _pauseControls;

    [SerializeField] GameObject _pauseMenu;

    public static Action<bool> OnGamePaused;

    private void Awake()
    {
        _pauseControls = new PauseControls();
    }

    private void OnEnable()
    {
        UserInput.OnPausePressed += SetPauseMenuState;
    }
    private void OnDisable()
    {
       // _pauseControls.Disable();
        UserInput.OnPausePressed -= SetPauseMenuState;
    }
    public void SetPauseMenuState()
    {
        _pauseMenu.SetActive(!_pauseMenu.activeSelf);
        OnGamePaused?.Invoke(_pauseMenu.activeSelf);
    }

}
