using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class GameOverHandler : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public string SceneName;

    private void Awake()
    {
        SceneName = SceneManager.GetActiveScene().name;
    }

    private void OnEnable()
    {
        StateMachine.OnGameOver += PlayerDeath;

    }
    private void OnDisable()
    {
        StateMachine.OnGameOver -= PlayerDeath;
    }

    public void PlayerDeath(string value)
    {
        if (value == "Player")
        {
            playableDirector.Play();
        }
    }

    public void GameOver()
    {
        //SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
        SceneManager.LoadScene(SceneName);
    }
}
