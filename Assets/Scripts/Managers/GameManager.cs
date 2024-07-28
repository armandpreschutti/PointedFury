using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class GameManager : MonoBehaviour
{
    public static GameManager gameInstance;
    public static GameManager GetInstance()
    {
        return gameInstance;
    }

    public bool healthSystemsActivated;
    public bool disableEnemies;
  

    private void Awake()
    {
        if (gameInstance == null)
        {
            gameInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += SetTimeScele;
        PauseMenuController.OnGamePaused += PauseGame;
        PauseMenuController.OnReturnToMenu += ReturnToMenu;
    }
    private void OnDisable()
    {
        PauseMenuController.OnGamePaused -= PauseGame;
        PauseMenuController.OnReturnToMenu -= ReturnToMenu;
    }

    public void SetTimeScele(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1.0f;
    }
    public void ResetLevel()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public void ReturnToMenu(bool value)
    {
        SceneManager.LoadScene("TitleMenu");
    }
    public void PauseGame(bool value)
    {
        Time.timeScale = value ? 0.0f : 1.0f;
        StateMachine[] pausedEntities = FindObjectsOfType<StateMachine>();
        foreach(StateMachine entity in pausedEntities)
        {
            entity.enabled = !value;
        }
    }
    public void ToggleHealthSystems() 
    {
        healthSystemsActivated = !healthSystemsActivated;
        HealthSystem[] HealthSystems = FindObjectsOfType<HealthSystem>();
        foreach (HealthSystem entity in HealthSystems)
        {
            entity.enabled = healthSystemsActivated;
        }
    }
}
