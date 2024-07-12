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
       // _stateMachine.OnAttackWindUp += SetIndicator;
        _stateMachine.OnLightAttack += SetIndicator;
        _stateMachine.OnHeavyAttack += SetIndicator;
        _stateMachine.OnLightAttackRecieved += PlayAttackImpactVFX;
        _stateMachine.OnHeavyAttackRecieved += PlayAttackImpactVFX;
        //_stateMachine.OnBlockSuccessful += PlayBlockImpactVFX;
        _stateMachine.OnParryRecieved += PlayParryVFX;
        _stateMachine.OnDash += PlayDashVFX;
        _stateMachine.OnDeflectSuccessful += PlayDeflectVFX;
    }
    private void OnDisable()
    {
//        _stateMachine.OnAttackWindUp -= SetIndicator;
        _stateMachine.OnLightAttack -= SetIndicator;
        _stateMachine.OnHeavyAttack -= SetIndicator;
        _stateMachine.OnLightAttackRecieved -= PlayAttackImpactVFX;
        _stateMachine.OnHeavyAttackRecieved -= PlayAttackImpactVFX;
        //_stateMachine.OnBlockSuccessful -= PlayBlockImpactVFX;
        _stateMachine.OnParryRecieved -= PlayParryVFX;
        _stateMachine.OnDeflectSuccessful -= PlayDeflectVFX;
    }
    public void SetIndicator(bool value, string attackType)
    {
        if(attackType == "Light")
        {
            _leftFootVFX.GetComponent<TrailRenderer>().startColor= Color.white;
            _leftFootVFX.GetComponent<TrailRenderer>().endColor = Color.white;
            _rightFootVFX.GetComponent<TrailRenderer>().startColor = Color.white;
            _rightFootVFX.GetComponent<TrailRenderer>().endColor = Color.white;
            _leftHandVFX.GetComponent<TrailRenderer>().startColor = Color.white;
            _leftHandVFX.GetComponent<TrailRenderer>().endColor = Color.white;
            _rightHandVFX.GetComponent<TrailRenderer>().startColor = Color.white;
            _rightHandVFX.GetComponent<TrailRenderer>().endColor = Color.white;
            _leftFootVFX.GetComponent<TrailRenderer>().endColor = Color.white;
            _leftFootVFX.GetComponent<TrailRenderer>().endColor = Color.white;
            switch (_stateMachine.LightAttackID)
            {
                case 1:
                    _leftHandVFX.SetActive(value);
                    break;
                case 2:
                    _rightFootVFX.SetActive(value);
                    break;
                case 3:
                    _leftHandVFX.SetActive(value);
                    break;
                case 4:
                    _leftFootVFX.SetActive(value);
                    break;
                case 5:
                    _rightHandVFX.SetActive(value);
                    break;
                default:
                    break;
            }
        }
        else if(attackType == "Heavy")
        {
            _leftFootVFX.GetComponent<TrailRenderer>().startColor = Color.yellow;
            _leftFootVFX.GetComponent<TrailRenderer>().endColor = Color.red;
            _rightFootVFX.GetComponent<TrailRenderer>().startColor = Color.yellow;
            _rightFootVFX.GetComponent<TrailRenderer>().endColor = Color.red;
            _leftHandVFX.GetComponent<TrailRenderer>().startColor = Color.yellow;
            _leftHandVFX.GetComponent<TrailRenderer>().endColor = Color.red;
            _rightHandVFX.GetComponent<TrailRenderer>().startColor = Color.yellow;
            _rightHandVFX.GetComponent<TrailRenderer>().endColor = Color.red;
            switch (_stateMachine.HeavyAttackID)
            {
                case 1:
                    _rightFootVFX.SetActive(value);
                    break;
                case 2:
                    _rightHandVFX.SetActive(value);
                    break;
                case 3:
                    _leftFootVFX.SetActive(value);
                    break;
                case 4:
                    _rightFootVFX.SetActive(value);
                    break;
                case 5:
                    _rightHandVFX.SetActive(value);
                    break;
                default:
                    break;
            }
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

    public void PlayBlockImpactVFX(float damage, string actionType)
    {
        CreateVFXOneShot(_blockImpactVFX, _vfxOrigin);
    }

    public void PlayParryVFX(float damage, string actionType)
    {
        CreateVFXOneShot(_parryVFX, _vfxOrigin);
    }

    public void PlayDeflectVFX()
    {
        CreateVFXOneShot(_lightAttackImpactVFX, _vfxOrigin);
    }
    public void CreateVFXOneShot(GameObject vfxPrefab, Transform origin)
    {
        Instantiate(vfxPrefab, origin);
    }
    public void PlayDashVFX(bool value)
    {
        _leftFootVFX.GetComponent<TrailRenderer>().startColor = Color.white;
        _leftFootVFX.GetComponent<TrailRenderer>().endColor = Color.white;
        _rightFootVFX.GetComponent<TrailRenderer>().startColor = Color.white;
        _rightFootVFX.GetComponent<TrailRenderer>().endColor = Color.white;
        _leftHandVFX.GetComponent<TrailRenderer>().startColor = Color.white;
        _leftHandVFX.GetComponent<TrailRenderer>().endColor = Color.white;
        _rightHandVFX.GetComponent<TrailRenderer>().startColor = Color.white;
        _rightHandVFX.GetComponent<TrailRenderer>().endColor = Color.white;
        _leftFootVFX.GetComponent<TrailRenderer>().endColor = Color.white;
        _leftFootVFX.GetComponent<TrailRenderer>().endColor = Color.white;
        _leftFootVFX.SetActive(value);
       // _leftHandVFX.SetActive(value);
        _rightFootVFX.SetActive(value);
       // _rightHandVFX.SetActive(value);
    }
}
