using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioClip musicClip;
    public float musicVolume;
    public AudioSource ambientSource;
    public AudioClip ambientClip;
    public float ambientVolume;

    private void OnEnable()
    {
        TutoiralManager.OnBeginLevel += PlayMusic;
        TutoiralManager.OnEnableControl += PlayAmbience;
    }
    private void OnDisable()
    {
        TutoiralManager.OnBeginLevel -= PlayMusic;
        TutoiralManager.OnEnableControl -= PlayAmbience;
    }
    public void PlayMusic()
    {
        musicSource.clip = musicClip;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }
    public void PlayAmbience()
    {
        ambientSource.clip = ambientClip;
        ambientSource.volume = ambientVolume;
        ambientSource.Play();
    }
}
