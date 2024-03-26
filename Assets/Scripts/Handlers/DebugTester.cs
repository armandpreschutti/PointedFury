using DG.Tweening;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DebugTester : MonoBehaviour
{
    [SerializeField] StateMachine _stateMachine;
    [SerializeField] GameObject _indicator;

    private void OnEnable()
    {
        _stateMachine.OnLightAttack1 += SetIndicator;
    }
    private void OnDisable()
    {
        _stateMachine.OnLightAttack1 -= SetIndicator;
    }

    public void SetIndicator(bool value)
    {
        _indicator.SetActive(value);
    }


}
