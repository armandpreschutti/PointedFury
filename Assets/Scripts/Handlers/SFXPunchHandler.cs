using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPunchHandler : MonoBehaviour
{
    public List<AudioClip> _punchSFX;
    public List<AudioClip> _attackImpactSFX;
    public StateMachine _stateMachine;
    public GameObject _oneShotPrefab;

    private void OnEnable()
    {
        _stateMachine.OnLightAttack += PlayPunchSFX;
        _stateMachine.OnAttackSuccess += PlayAttackImpactSFX;
    }

    private void OnDisable()
    {
        _stateMachine.OnLightAttack -= PlayPunchSFX;
        _stateMachine.OnAttackSuccess -= PlayAttackImpactSFX;
    }

    public void PlayPunchSFX(bool value)
    {
        if(value)
        {
            CreateSFXOneShot(_punchSFX);
        }        
    }

    public void PlayAttackImpactSFX()
    {
        CreateSFXOneShot(_attackImpactSFX);
    }

    public void CreateSFXOneShot(List<AudioClip> possibleClips)
    {
        GameObject oneShotPrefab = _oneShotPrefab;
        AudioClip clipToPlay = possibleClips[Random.Range(0, possibleClips.Count)];
        oneShotPrefab.GetComponent<AudioSource>().clip = clipToPlay;
        Instantiate(oneShotPrefab);
    }
}
