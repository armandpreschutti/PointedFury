using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class PracticeManager : MonoBehaviour
{
    public GameObject player;
    public CinemachineBrain cinemachineBrain;
    public PlayableDirector playableDirector;
    public TimelineAsset introCutScene;
    public static Action OnEnableControl;
    public static Action OnBeginLevel;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += LevelStarted;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= LevelStarted;
    }

    public void LevelStarted(Scene currentScene, LoadSceneMode mode)
    {
        PlayIntroCutScene();
    }


    public void PlayIntroCutScene()
    {
        cinemachineBrain.m_DefaultBlend.m_Time = 1f;
        player.GetComponent<UserInput>().enabled = false;
        playableDirector.playableAsset = introCutScene;
        playableDirector.Play();
    }

    public void EnableControl()
    {
        Debug.Log("Function called on practice manager");
        OnEnableControl?.Invoke();
        player.GetComponent<UserInput>().enabled = true;
        player.GetComponent<CharacterController>().enabled = true;
        cinemachineBrain.m_DefaultBlend.m_Time = 1f;
    }
    public void BeginLevel()
    {
        OnBeginLevel?.Invoke();
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("TitleMenu");
    }
}
