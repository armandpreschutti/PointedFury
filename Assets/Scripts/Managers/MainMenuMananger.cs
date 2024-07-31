using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class MainMenuMananger : MonoBehaviour
{
    public PlayableAsset StartCutScene;
    public PlayableDirector director;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += LevelStarted;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= LevelStarted;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LevelStarted(Scene currentScene, LoadSceneMode mode)
    {
        director.playableAsset = StartCutScene;
        director.Play();
    }
}
