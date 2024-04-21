using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        UserInput.OnResetLevelPressed += ResetLevel;
        UserInput.OnToggleHealthSystemsPressed += ToggleHealthSystems;
        UserInput.OnDisableEnemiesPresssed += ToggleEnemyAI;
        UserInput.OnResetGamePressed += ResetGame;
        PauseMenuController.OnGamePaused += PauseGame;
    }
    private void OnDisable()
    {
        UserInput.OnResetLevelPressed -= ResetLevel;
        UserInput.OnToggleHealthSystemsPressed-= ToggleHealthSystems;
        UserInput.OnDisableEnemiesPresssed -= ToggleEnemyAI;
        UserInput.OnResetGamePressed -= ResetGame;
        PauseMenuController.OnGamePaused -= PauseGame;
    }
    public void ResetLevel()
    {
        SceneManager.LoadScene("CombatGym");
    }
    public void ResetGame()
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
    public void ToggleEnemyAI()
    {
        /*   disableEnemies = !disableEnemies;
           AIBrain[] aiBrains = FindObjectsOfType<AIBrain>();
           foreach (AIBrain entity in aiBrains)
           {
               entity.enabled = disableEnemies;
           }*/
        SceneManager.LoadScene("Debug");
    }
}
