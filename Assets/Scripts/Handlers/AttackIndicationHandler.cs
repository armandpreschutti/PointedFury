using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackIndicationHandler : MonoBehaviour
{
    [SerializeField] StateMachine _stateMachine;
    [SerializeField] GameObject _attackIndicator;

    private void Update()
    {
        _attackIndicator.SetActive(_stateMachine.IsParryable);
    }
}
