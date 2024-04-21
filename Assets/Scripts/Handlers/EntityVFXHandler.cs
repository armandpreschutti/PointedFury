using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EntityVFXHandler : MonoBehaviour
{
    [SerializeField] StateMachine _stateMachine;
    [SerializeField] ParticleSystem _leftHandVFX;
    [SerializeField] ParticleSystem _rightHandVFX;
    [SerializeField] ParticleSystem _leftFootVFX;
    [SerializeField] ParticleSystem _rightFootVFX;

    [SerializeField] Transform _vfxOrigin;
    [SerializeField] GameObject _lightAttackImpactVFX;
    [SerializeField] GameObject _heavyAttackImpactVFX;
    [SerializeField] GameObject _blockImpactVFX;
    [SerializeField] GameObject _parryVFX;

    private void OnEnable()
    {
        _stateMachine.OnAttackWindUp += SetIndicator;
        _stateMachine.OnLightAttack += SetIndicator;
        _stateMachine.OnHeavyAttack += SetIndicator;
        _stateMachine.OnLightAttackRecieved += PlayAttackImpactVFX;
        _stateMachine.OnHeavyAttackRecieved += PlayAttackImpactVFX;
        _stateMachine.OnBlockSuccessful += PlayBlockImpactVFX;
        _stateMachine.OnParrySuccessful += PlayParryVFX;
    }
    private void OnDisable()
    {
        _stateMachine.OnAttackWindUp -= SetIndicator;
        _stateMachine.OnLightAttack -= SetIndicator;
        _stateMachine.OnHeavyAttack -= SetIndicator;
        _stateMachine.OnLightAttackRecieved -= PlayAttackImpactVFX;
        _stateMachine.OnHeavyAttackRecieved -= PlayAttackImpactVFX;
        _stateMachine.OnBlockSuccessful -= PlayBlockImpactVFX;
        _stateMachine.OnParrySuccessful -= PlayParryVFX;
    }
    public void SetIndicator(bool value, string attackType)
    {
        if(attackType == "Light")
        {
            if (value)
            {
                switch (_stateMachine.LightAttackID)
                {
                    case 1:
                        _rightHandVFX.Play();
                        break;
                    case 2:
                        _leftFootVFX.Play();
                        break;
                    case 3:
                        _rightHandVFX.Play();
                        break;
                    case 4:
                        _rightFootVFX.Play();
                        break;
                    case 5:
                        _leftHandVFX.Play();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (_stateMachine.LightAttackID)
                {
                    case 1:
                        _rightHandVFX.Stop();
                        break;
                    case 2:
                        _leftFootVFX.Stop();
                        break;
                    case 3:
                        _rightHandVFX.Stop();
                        break;
                    case 4:
                        _rightFootVFX.Stop();
                        break;
                    case 5:
                        _leftHandVFX.Stop();
                        break;
                    default:
                        break;
                }
            }

        }
        else if (attackType == "Heavy")
        {
            if (value)
            {
                _leftFootVFX.Play();
                _rightFootVFX.Play();
            }
            else
            {
                _leftFootVFX.Stop();
                _rightFootVFX.Stop();
            }
        }
        else
        {
            Debug.LogError("EnityVFXHandler could not determine AttackType");
        }
       
    }
    public void PlayAttackImpactVFX(float value, string attackType)
    {
        if (attackType == "Light")
        {
            CreateVFXOneShot(_lightAttackImpactVFX, _vfxOrigin);

        }
        else if (attackType == "Heavy")
        {
            CreateVFXOneShot(_heavyAttackImpactVFX, _vfxOrigin);

        }
        else
        {
            Debug.Log("Attack type not known");
            return;
        }
    }

    public void PlayBlockImpactVFX()
    {
        CreateVFXOneShot(_blockImpactVFX, _vfxOrigin);
    }

    public void PlayParryVFX()
    {
        CreateVFXOneShot(_parryVFX, _vfxOrigin);
    }
    public void CreateVFXOneShot(GameObject vfxPrefab, Transform origin)
    {
        Instantiate(vfxPrefab, origin);
    }
}
