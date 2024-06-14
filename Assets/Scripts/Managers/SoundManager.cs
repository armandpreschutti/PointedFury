using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioClip generalMusicClip;
    public AudioClip bossMusicClip;
    public float musicVolume;
    public AudioSource ambientSource;
    public AudioClip ambientClip;
    public float ambientVolume;

    private void OnEnable()
    {
        TutoiralManager.OnBeginLevel += PlayGeneralMusic;
        TutoiralManager.OnBeginLevel += PlayAmbience;
        TutoiralManager.OnBeginBossFight += PlayBossMusic;
        CutSceneTriggerHandler.onStartCutscene += StopMusic;
        WinConditionHandler.OnLevelPassed += StopMusic;
    }
    private void OnDisable()
    {
        TutoiralManager.OnBeginLevel -= PlayGeneralMusic;
        TutoiralManager.OnBeginLevel -= PlayAmbience;
        TutoiralManager.OnBeginBossFight -= PlayBossMusic;
        CutSceneTriggerHandler.onStartCutscene -= StopMusic;
        WinConditionHandler.OnLevelPassed -= StopMusic;
    }
    public void PlayGeneralMusic()
    {
        musicSource.clip = generalMusicClip;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    public void PlayBossMusic()
    {
        musicSource.clip = bossMusicClip;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }
    public void PlayAmbience()
    {
        ambientSource.clip = ambientClip;
        ambientSource.volume = ambientVolume;
        ambientSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
        ambientSource.Stop();
    }
}
