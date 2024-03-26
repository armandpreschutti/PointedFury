using DG.Tweening;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DebugTester : MonoBehaviour
{
    [SerializeField] StateMachine _stateMachine;
    [SerializeField] GameObject _leftHandFX;
    [SerializeField] GameObject _rightHandFX;
    [SerializeField] GameObject _leftFootFX;
    [SerializeField] GameObject _rightFootFX;
    private void OnEnable()
    {
        _stateMachine.OnLightAttack += SetIndicator;
    }
    private void OnDisable()
    {
        _stateMachine.OnLightAttack -= SetIndicator;
    }

    public void SetIndicator(bool value)
    {
        switch (_stateMachine.AttackType)
        {
            case 1:
                _rightHandFX.SetActive(value);
                break;
            case 2:
                _leftHandFX.SetActive(value);
                break;
            case 3:
                _leftFootFX.SetActive(value);
                break;
            case 4:
                _leftHandFX.SetActive(value);
                break;
            case 5:
                _leftFootFX.SetActive(value);
                break;
            case 6:
                _rightHandFX.SetActive(value);
                break;
            case 7:
                _rightFootFX.SetActive(value);
                break;
            default:
                break;
        }
    }

    public void SetAttackType()
    {
       
    }

}
