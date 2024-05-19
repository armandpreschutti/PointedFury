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
        if(_stateMachine.AttackType != "Light" && _stateMachine.IsParryable)
        {
            _attackIndicator.SetActive(true);
            _attackIndicator.GetComponent<MeshRenderer>().material.color = Color.red;
        }
        else if (_stateMachine.IsEvadable)
        {
            _attackIndicator.SetActive(true);
            _attackIndicator.GetComponent<MeshRenderer>().material.color = Color.green;
        }
        else
        {
            _attackIndicator.SetActive(false);
            _attackIndicator.GetComponent<MeshRenderer>().material.color = Color.white;
        }
        

    }
}
