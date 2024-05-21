using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitSFXHandler : MonoBehaviour
{
    public EntitySFXSO sfxSO;
    public StateMachine _stateMachine;
    public GameObject _oneShotPrefab;

    private void Awake()
    {
        _stateMachine = GetComponent<StateMachine>();
    }
    private void OnEnable()
    {
        _stateMachine.OnAttackWindUp += PlayAttackWhooshSFX;
        _stateMachine.OnLightAttackRecieved += PlayAttackImpactSFX;
        _stateMachine.OnHeavyAttackRecieved += PlayAttackImpactSFX;
        _stateMachine.OnBlockSuccessful += PlayBlockImpactSFX;
        _stateMachine.OnParryRecieved += PlayParrySFX;
        _stateMachine.OnDash += PlayDashSFX;
        _stateMachine.OnDeath += PlayDeathSFX;
    }

    private void OnDisable()
    {
        _stateMachine.OnAttackWindUp -= PlayAttackWhooshSFX;
        _stateMachine.OnLightAttackRecieved -= PlayAttackImpactSFX;
        _stateMachine.OnHeavyAttackRecieved -= PlayAttackImpactSFX;
        _stateMachine.OnBlockSuccessful -= PlayBlockImpactSFX;
        _stateMachine.OnParryRecieved -= PlayParrySFX;
        _stateMachine.OnDash -= PlayDashSFX;
        _stateMachine.OnDeath -= PlayDeathSFX;
    }

    public void PlayAttackWhooshSFX(bool value, string attackType)
    {
        if(attackType == "Light")
        {
            CreateSFXOneShot(sfxSO._lightAttackWhooshSFX);
        }
        else if(attackType == "Heavy")
        {
            CreateSFXOneShot(sfxSO._heavyAttackWhooshSFX);
        }
        else
        {
            Debug.Log("Attack type not known");
            return;
        }

    }

    public void PlayAttackImpactSFX(float value, string attackType)
    {
       if(attackType == "Light")
        {
            CreateSFXOneShot(sfxSO._lightAttackImpactSFX);
        }
        else if(attackType == "Heavy")
        {
            CreateSFXOneShot(sfxSO._heavyAttackImpactSFX);
        }
        else
        {
            Debug.Log("Attack type not known");
            return;
        }
    }

    public void PlayBlockImpactSFX(float damage, string actionType)
    {
        CreateSFXOneShot(sfxSO._blockImpactSFX);
    }

    public void PlayParrySFX(float damage, string actionType)
    {
        CreateSFXOneShot(sfxSO._parrySFX);
    }

    public void PlayDashSFX(bool value)
    {
        CreateSFXOneShot(sfxSO._dashSFX);
    }

    public void PlayDeathSFX()
    {
        CreateSFXOneShot(sfxSO._deathSFX);
    }

    public void CreateSFXOneShot(List<AudioClip> possibleClips)
    {
        if(possibleClips.Count != 0)
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
