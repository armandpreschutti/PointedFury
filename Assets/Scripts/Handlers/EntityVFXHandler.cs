using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EntityVFXHandler : MonoBehaviour
{
    [SerializeField] StateMachine _stateMachine;
    [SerializeField] GameObject _leftHandVFX;
    [SerializeField] GameObject _rightHandVFX;
    [SerializeField] GameObject _leftFootVFX;
    [SerializeField] GameObject _rightFootVFX;

    [SerializeField] Transform _vfxOrigin;
    [SerializeField] GameObject _lightAttackImpactVFX;
    [SerializeField] GameObject _heavyAttackImpactVFX;
    [SerializeField] GameObject _blockImpactVFX;
    [SerializeField] GameObject _parryVFX;

    private void OnEnable()
    {
        _stateMachine.OnLightAttack += SetIndicator;
        _stateMachine.OnHeavyAttack += SetIndicator;
        _stateMachine.OnLightHitLanded += PlayLightAttackImpactVFX;
        _stateMachine.OnHeavyHitLanded += PlayLightAttackImpactVFX;
        _stateMachine.OnBlockSuccessful += PlayBlockImpactVFX;
        _stateMachine.OnParrySuccessful += PlayParryVFX;
    }
    private void OnDisable()
    {
        _stateMachine.OnLightAttack -= SetIndicator;
        _stateMachine.OnHeavyAttack -= SetIndicator;
        _stateMachine.OnLightHitLanded -= PlayLightAttackImpactVFX;
        _stateMachine.OnHeavyHitLanded -= PlayLightAttackImpactVFX;
        _stateMachine.OnBlockSuccessful -= PlayBlockImpactVFX;
        _stateMachine.OnParrySuccessful -= PlayParryVFX;
    }
    public void SetIndicator(bool value)
    {
        if(_stateMachine.AttackType == "Light")
        {
            switch (_stateMachine.LightAttackID)
            {
                case 1:                    
                    _rightHandVFX.SetActive(value);
                    break;
                case 2:
                    _leftHandVFX.SetActive(value);
                    break;
                case 3:
                    _leftFootVFX.SetActive(value);
                    break;
                case 4:
                    _leftHandVFX.SetActive(value);
                    break;
                case 5:
                    _leftFootVFX.SetActive(value);
                    break;
                case 6:
                    _rightHandVFX.SetActive(value);
                    break;
                case 7:
                    _rightFootVFX.SetActive(value);
                    break;
                default:
                    break;
            }
        }
        else if (_stateMachine.AttackType == "Heavy")
        {
            _rightHandVFX.SetActive(value);
            _leftHandVFX.SetActive(value);
            _leftFootVFX.SetActive(value);
            _rightFootVFX.SetActive(value);
            _leftHandVFX.GetComponent<TrailRenderer>().endColor = value ? Color.yellow : Color.white;
            _rightHandVFX.GetComponent<TrailRenderer>().endColor = value ? Color.yellow : Color.white;
            _leftFootVFX.GetComponent<TrailRenderer>().endColor = value ? Color.yellow : Color.white;
            _rightFootVFX.GetComponent<TrailRenderer>().endColor = value ? Color.yellow : Color.white;
            _leftHandVFX.GetComponent<TrailRenderer>().startColor = value ? Color.red : Color.white;
            _rightHandVFX.GetComponent<TrailRenderer>().startColor = value ? Color.red : Color.white;
            _leftFootVFX.GetComponent<TrailRenderer>().startColor = value ? Color.red : Color.white;
            _rightFootVFX.GetComponent<TrailRenderer>().startColor = value ? Color.red : Color.white;
        }
        else
        {
            Debug.LogError("EnityVFXHandler could not determine AttackType");
        }
       
    }
    public void PlayLightAttackImpactVFX(float value)
    {
        CreateVFXOneShot(_lightAttackImpactVFX, _vfxOrigin);
    }
    public void PlayHeavyAttackImpactVFX(float value)
    {
        CreateVFXOneShot(_heavyAttackImpactVFX, _vfxOrigin);
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
