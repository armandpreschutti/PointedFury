using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackIndicationHandler : MonoBehaviour
{
    public StateMachine _stateMachine;
    public GameObject _attackIndicator;
    public bool showParryIndicator;
    public bool showEvadeIndicator;

    private void Update()
    {
        if(_stateMachine.AttackType == "Heavy" && _stateMachine.IsParryable && showParryIndicator)
        {
            _attackIndicator.SetActive(true);
            _attackIndicator.GetComponent<MeshRenderer>().material.color = Color.red;
        }
        else if (_stateMachine.IsEvadable /*&& _stateMachine.IsParrying*/ && showEvadeIndicator)
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
