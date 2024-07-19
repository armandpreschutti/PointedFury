using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseMenuController : MonoBehaviour
{
    private PauseControls _pauseControls;
    public bool isPaused;
    [SerializeField] GameObject _pauseMenu;
    public GameObject firstSelected;
    public GameObject ControllerMap;
    public GameObject Buttons;
    public EventSystem eventSystem;

    public static Action<bool> OnGamePaused;
    public static Action<bool> OnReturnToMenu;

    private void Awake()
    {
        _pauseControls = new PauseControls();
    }

    private void OnEnable()
    {
        //UserInput.OnPausePressed += SetPauseMenuState;
        _pauseControls.Enable();
        SubscribeToActions();
    }
    private void OnDisable()
    {
        // _pauseControls.Disable();
        //UserInput.OnPausePressed -= SetPauseMenuState;
        _pauseControls.Disable();
        UnsubscribeFromActions();
    }
    public void SetPauseMenuState()
    {
        _pauseMenu.SetActive(!_pauseMenu.activeSelf);
        OnGamePaused?.Invoke(_pauseMenu.activeSelf);
        isPaused = _pauseMenu.activeSelf;
        eventSystem.SetSelectedGameObject(firstSelected);
        if(_pauseMenu.activeSelf == false)
        {
            Buttons.SetActive(true);
            ControllerMap.SetActive(false);
        }
    }

    public void SetPauseNavigationInput()
    {

    }

    public void ReturnToMenu()
    {
        OnReturnToMenu?.Invoke(true);
    }

    public void EnterControllerLayout()
    {

    }
    public void ExitControllerLayout()
    {

    }
    // Set input values
    private void SubscribeToActions()
    {
        // Set up input actions for player controls
        _pauseControls.Player.Pause.performed += ctx => SetPauseMenuState(/*ctx.ReadValue<Vector2>()*/);
       // _pauseControls.Player.Navigate.performed += ctx => SetPauseNavigationInput();
    }

    // Set input values
    private void UnsubscribeFromActions()
    {
        // Set up input actions for player controls
        _pauseControls.Player.Pause.performed -= ctx => SetPauseMenuState(/*ctx.ReadValue<Vector2>()*/);
        _pauseMenu.SetActive(!_pauseMenu.activeSelf);
        OnGamePaused?.Invoke(_pauseMenu.activeSelf);
        isPaused = _pauseMenu.activeSelf;
        if (isPaused)
        {
            eventSystem.SetSelectedGameObject(firstSelected);
        }
        //  _pauseControls.Player.Navigate.performed -= ctx => SetPauseNavigationInput();
    }
}
