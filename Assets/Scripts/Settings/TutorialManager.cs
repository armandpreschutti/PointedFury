using Cinemachine;
using System;

using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class TutoiralManager : MonoBehaviour
{
    public GameObject player;
    public CinemachineBrain cinemachineBrain;
    public PlayableDirector playableDirector;
    public TimelineAsset introCutScene;
    public TimelineAsset preBossCutScene;
    public TimelineAsset postBossCutScene;
    public static Action OnEnableControl;
    public static Action OnBeginLevel;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += LevelStarted;
        WinConditionHandler.OnLevelPassed += LevelCompleted;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= LevelStarted;
        WinConditionHandler.OnLevelPassed -= LevelCompleted;
    }

    public void LevelStarted(Scene currentScene, LoadSceneMode mode)
    {
        PlayIntroCutScene();
    }

    public void LevelCompleted()
    {
        PlayFadeOut();
    }

    public void PlayIntroCutScene()
    {
        cinemachineBrain.m_DefaultBlend.m_Time = 1f;
        player.GetComponent<UserInput>().enabled = false;
        playableDirector.playableAsset = introCutScene;
        playableDirector.Play();
    }

    public void PlayFadeOut()
    {
        Time.timeScale = 1f;
        playableDirector.extrapolationMode = DirectorWrapMode.Hold;
        cinemachineBrain.m_DefaultBlend.m_Time= 0f;
        player.GetComponent<UserInput>().enabled = false;
        playableDirector.playableAsset = postBossCutScene;
        playableDirector.Play();
    }

    public void EnableControl()
    {
        OnEnableControl?.Invoke();
    }
    public void BeginLevel()
    {
        OnBeginLevel?.Invoke();
    }
}
