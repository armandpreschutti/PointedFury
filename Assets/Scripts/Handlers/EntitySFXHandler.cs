using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitSFXHandler : MonoBehaviour
{
    public List<AudioClip> _lightPunchSFX;
    public List<AudioClip> _heavyPunchSFX;
    public List<AudioClip> _attackImpactSFX;
    public List<AudioClip> _blockImpactSFX;
    public List<AudioClip> _parrySFX;
    public List<AudioClip> _dashSFX;
    public List<AudioClip> _deathSFX;

    public StateMachine _stateMachine;
    public GameObject _oneShotPrefab;

    private void Awake()
    {
        _stateMachine = GetComponent<StateMachine>();
    }
    private void OnEnable()
    {
        _stateMachine.OnLightAttack += PlayLightAttackWhooshSFX;
        _stateMachine.OnHeavyAttack += PlayHeavyAttackWhoosh;
        _stateMachine.OnLightHitLanded += PlayLightAttackImpactSFX;
        _stateMachine.OnHeavyHitLanded += PlayLightAttackImpactSFX;
        _stateMachine.OnBlockSuccessful += PlayBlockImpactSFX;
        _stateMachine.OnParrySuccessful += PlayParrySFX;
        _stateMachine.OnDashSuccessful += PlayDashSFX;
        _stateMachine.OnDeath += PlayDeathSFX;
    }

    private void OnDisable()
    {
        _stateMachine.OnLightAttack -= PlayLightAttackWhooshSFX;
        _stateMachine.OnHeavyAttack -= PlayHeavyAttackWhoosh;
        _stateMachine.OnLightHitLanded -= PlayLightAttackImpactSFX;
        _stateMachine.OnHeavyHitLanded -= PlayLightAttackImpactSFX;
        _stateMachine.OnBlockSuccessful -= PlayBlockImpactSFX;
        _stateMachine.OnParrySuccessful -= PlayParrySFX;
        _stateMachine.OnDashSuccessful -= PlayDashSFX;
        _stateMachine.OnDeath -= PlayDeathSFX;
    }

    public void PlayLightAttackWhooshSFX(bool value)
    {
        if (value)
        {
            CreateSFXOneShot(_lightPunchSFX);
        }
    }

    public void PlayHeavyAttackWhoosh(bool value) 
    {
        if (value) 
        {
            CreateSFXOneShot(_heavyPunchSFX);
        }
    }

    public void PlayLightAttackImpactSFX(float value)
    {
        CreateSFXOneShot(_attackImpactSFX);
    }

    public void PlayBlockImpactSFX()
    {
        CreateSFXOneShot(_blockImpactSFX);
    }

    public void PlayParrySFX()
    {
        CreateSFXOneShot(_parrySFX);
    }

    public void PlayDashSFX()
    {
        CreateSFXOneShot(_dashSFX);
    }

    public void PlayDeathSFX()
    {
        CreateSFXOneShot(_deathSFX);
    }

    public void CreateSFXOneShot(List<AudioClip> possibleClips)
    {
        if(possibleClips == null)
        {
            GameObject oneShotPrefab = _oneShotPrefab;
            AudioClip clipToPlay = possibleClips[Random.Range(0, possibleClips.Count)];
            oneShotPrefab.GetComponent<AudioSource>().clip = clipToPlay;
            Instantiate(oneShotPrefab);
        }
        else
        {
            return;
        }

    }
}
