using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class GameOverHandler : MonoBehaviour
{
    public PlayableDirector playableDirector;
    
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
        SceneManager.LoadScene("TitleMenu");
    }
}
