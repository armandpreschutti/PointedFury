using Cinemachine;
using System;

using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class TutoiralManager : MonoBehaviour
{
    public GameObject player;
    public GameObject boss;
    public CinemachineBrain cinemachineBrain;
    public PlayableDirector playableDirector;
    public TimelineAsset introCutScene;
    public TimelineAsset preBossCutScene;
    public TimelineAsset postBossCutScene;
    public static Action OnEnableControl;
    public static Action OnBeginLevel;
    public static Action OnBeginBossFight;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += LevelStarted;
        CutSceneTriggerHandler.onStartCutscene += PreBossFightStarted;
        WinConditionHandler.OnLevelPassed += LevelCompleted;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= LevelStarted;
        CutSceneTriggerHandler.onStartCutscene -= PreBossFightStarted;
        WinConditionHandler.OnLevelPassed -= LevelCompleted;
    }

    public void LevelStarted(Scene currentScene, LoadSceneMode mode)
    {
        PlayIntroCutScene();
    }

    public void PreBossFightStarted()
    {
        PlayPreBossCutScene();
    }

    public void LevelCompleted()
    {
        PlayPostBossCutScene();
    }

    public void PlayIntroCutScene()
    {
        cinemachineBrain.m_DefaultBlend.m_Time = 1f;
        player.GetComponent<UserInput>().enabled = false;
        playableDirector.playableAsset = introCutScene;
        playableDirector.Play();
    }

   
    public void PlayPreBossCutScene()
    {
        Time.timeScale = 1f;
        playableDirector.extrapolationMode = DirectorWrapMode.None;
        cinemachineBrain.m_DefaultBlend.m_Time = 0f;
        //cinemachineBrain.m_DefaultBlend.m_Time = 1f;
        player.GetComponent<UserInput>().enabled = false;
        player.GetComponent<CharacterController>().enabled = false;
        playableDirector.playableAsset = preBossCutScene;
        playableDirector.Play();
    }

    public void PlayPostBossCutScene()
    {
        Time.timeScale = 1f;
        playableDirector.extrapolationMode = DirectorWrapMode.Hold;
        cinemachineBrain.m_DefaultBlend.m_Time = 0f;
        player.GetComponent<UserInput>().enabled = false;
        boss.GetComponent<CharacterController>().enabled = false;
        playableDirector.playableAsset = postBossCutScene;
        playableDirector.Play();
    }

    public void EnableControl()
    {
        OnEnableControl?.Invoke();
        player.GetComponent<CharacterController>().enabled = true;        
        cinemachineBrain.m_DefaultBlend.m_Time = 1f;
    }
    public void BeginLevel()
    {
        OnBeginLevel?.Invoke();
    }

    public void BeginBoosFight()
    {
        OnBeginBossFight?.Invoke();
    }
}
