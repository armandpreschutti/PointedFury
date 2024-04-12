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
    [SerializeField] GameObject _attackImpactVFX;
    [SerializeField] GameObject _blockImpactVFX;
    [SerializeField] GameObject _parryVFX;
    //[SerializeField] GameObject _dashVFX;

    private void OnEnable()
    {
        _stateMachine.OnLightAttack += SetIndicator;
        _stateMachine.OnHitLanded += PlayAttackImpactVFX;
        _stateMachine.OnBlockSuccessful += PlayBlockImpactVFX;
        _stateMachine.OnParrySuccessful += PlayParryVFX;
        //_stateMachine.OnDashSuccessful += PlayDashVFX;
    }
    private void OnDisable()
    {
        _stateMachine.OnLightAttack -= SetIndicator;
        _stateMachine.OnHitLanded -= PlayAttackImpactVFX;
        _stateMachine.OnBlockSuccessful -= PlayBlockImpactVFX;
        _stateMachine.OnParrySuccessful -= PlayParryVFX;
       // _stateMachine.OnDashSuccessful -= PlayDashVFX;
    }

    public void Update()
    {
        
    }
    public void SetIndicator(bool value)
    {
        switch (_stateMachine.AttackType)
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
    public void PlayAttackImpactVFX(float value)
    {
        CreateVFXOneShot(_attackImpactVFX, _vfxOrigin);
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
