using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuController : MonoBehaviour
{
    private PauseControls _pauseControls;

    [SerializeField] UserInput _userInput;
    [SerializeField] GameObject _pauseMenu;

    public static Action<bool> OnGamePaused;

    private void Awake()
    {
        _userInput = GameObject.Find("Player").GetComponent<UserInput>();
        _pauseControls = new PauseControls();
    }

    private void Start()
    {
        SetInputValues(); 
    }
    private void OnEnable()
    {
        _pauseControls.Enable();
        _userInput.OnPausePressed += SetPauseMenuState;
    }
    private void OnDisable()
    {
        _pauseControls.Disable();
        _userInput.OnPausePressed -= SetPauseMenuState;
    }
    public void SetPauseMenuState()
    {
        
        _pauseMenu.SetActive(!_pauseMenu.activeSelf);
        Time.timeScale = _pauseMenu.activeSelf ? 0.0f : 1.0f;
        _userInput.enabled = !_pauseMenu.activeSelf;
        OnGamePaused?.Invoke(_pauseMenu.activeSelf);

    }


    // Set input values
    private void SetInputValues()
    {
        _pauseControls.Pause.Return.performed += ctx => SetPauseMenuState();
    }
}
