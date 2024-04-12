using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPunchHandler : MonoBehaviour
{
    public List<AudioClip> _punchSFX;
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
        _stateMachine.OnLightAttack += PlayPunchSFX;
        _stateMachine.OnHitLanded += PlayAttackImpactSFX;
        _stateMachine.OnBlockSuccessful += PlayBlockImpactSFX;
        _stateMachine.OnParrySuccessful += PlayParrySFX;
        _stateMachine.OnDashSuccessful += PlayDashSFX;
        _stateMachine.OnDeath += PlayDeathSFX;
    }

    private void OnDisable()
    {
        _stateMachine.OnLightAttack -= PlayPunchSFX;
        _stateMachine.OnHitLanded -= PlayAttackImpactSFX;
        _stateMachine.OnBlockSuccessful -= PlayBlockImpactSFX;
        _stateMachine.OnParrySuccessful -= PlayParrySFX;
        _stateMachine.OnDashSuccessful -= PlayDashSFX;
        _stateMachine.OnDeath -= PlayDeathSFX;
    }

    public void PlayPunchSFX(bool value)
    {
        if (value)
        {
            CreateSFXOneShot(_punchSFX);
        }
    }

    public void PlayAttackImpactSFX(float value)
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
        GameObject oneShotPrefab = _oneShotPrefab;
        AudioClip clipToPlay = possibleClips[Random.Range(0, possibleClips.Count)];
        oneShotPrefab.GetComponent<AudioSource>().clip = clipToPlay;
        Instantiate(oneShotPrefab);
    }
}
