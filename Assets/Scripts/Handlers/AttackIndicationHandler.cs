using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackIndicationHandler : MonoBehaviour
{
    [SerializeField] StateMachine _stateMachine;
    [SerializeField] GameObject _attackIndicator;

    private void Update()
    {
        if(_stateMachine.AttackType == "Heavy" && _stateMachine.IsParryable)
        {
            _attackIndicator.SetActive(true);
            _attackIndicator.GetComponent<MeshRenderer>().material.color = Color.red;
        }
        else if (_stateMachine.IsEvadable && _stateMachine.IsParrying)
        {
            _attackIndicator.SetActive(true);
            _attackIndicator.GetComponent<MeshRenderer>().material.color = Color.green;
        }
        else if(_stateMachine.CurrentTarget.GetComponent<StateMachine>().CurrentTarget != null && _stateMachine.CurrentTarget.GetComponent<StateMachine>().CurrentTarget == gameObject)
        {
            _attackIndicator.SetActive(true);
            _attackIndicator.GetComponent<MeshRenderer>().material.color = Color.white;
        }
        else
        {
            _attackIndicator.SetActive(false);
        }
        

    }
}
