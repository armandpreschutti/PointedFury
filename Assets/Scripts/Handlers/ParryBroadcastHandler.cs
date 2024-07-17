using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ParryBroadcastHandler : MonoBehaviour
{
    StateMachine _stateMachine;
    GameObject[] _hitTargets = new GameObject[100]; // Assume a maximum size for the array
    [SerializeField] string _enemyTag;
    private int _hitTargetCount = 0;

    private void Awake()
    {
        _stateMachine = GetComponentInParent<StateMachine>();
    }

    private void OnEnable()
    {
        _stateMachine.OnAttemptParry += AttemptParry;
        _stateMachine.OnAttemptEvade += AttemptEvade;
        _stateMachine.OnParryContact += GiveParry;
        _stateMachine.OnDeflectSuccessful += GiveDeflect;
    }

    private void OnDisable()
    {
        _stateMachine.OnAttemptParry -= AttemptParry;
        _stateMachine.OnAttemptEvade -= AttemptEvade;
        _stateMachine.OnParryContact -= GiveParry;
        _stateMachine.OnDeflectSuccessful -= GiveDeflect;
    }

    private void Update()
    {
        for (int i = _hitTargetCount - 1; i >= 0; i--)
        {
            if (_hitTargets[i] == null || _hitTargets[i].GetComponent<StateMachine>().IsDead)
            {
                RemoveAt(i);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_enemyTag) && _hitTargetCount < _hitTargets.Length)
        {
            _hitTargets[_hitTargetCount++] = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < _hitTargetCount; i++)
        {
            if (_hitTargets[i] == other.gameObject)
            {
                RemoveAt(i);
                break;
            }
        }
    }

    private void RemoveAt(int index)
    {
        for (int i = index; i < _hitTargetCount - 1; i++)
        {
            _hitTargets[i] = _hitTargets[i + 1];
        }
        _hitTargets[--_hitTargetCount] = null;
    }

    private void AttemptParry()
    {
        for (int i = 0; i < _hitTargetCount; i++)
        {
            GameObject hit = _hitTargets[i];
            if (hit != null && hit.GetComponent<StateMachine>() != null && hit.GetComponent<StateMachine>().IsParryable && !_stateMachine.IsParrying)
            {
                _stateMachine.BeginParry(hit.transform.position);
            }
        }
    }

    private void AttemptEvade()
    {
        for (int i = 0; i < _hitTargetCount; i++)
        {
            GameObject hit = _hitTargets[i];
            if (hit != null && hit.GetComponent<StateMachine>() != null && hit.GetComponent<StateMachine>().IsEvadable)
            {
                _stateMachine.BeginEvade();
            }
        }
    }

    private void GiveParry()
    {
        for (int i = 0; i < _hitTargetCount; i++)
        {
            GameObject hit = _hitTargets[i];
            if (hit != null && hit.GetComponent<StateMachine>() != null && !hit.GetComponent<StateMachine>().IsEvading)
            {
                hit.GetComponent<StateMachine>().TakeParry(_stateMachine.transform.position, _stateMachine.ParryDamage, _stateMachine.ParryID);
            }
        }
    }

    private void GiveDeflect()
    {
        for (int i = 0; i < _hitTargetCount; i++)
        {
            GameObject hit = _hitTargets[i];
            if (hit != null && hit.GetComponent<StateMachine>() != null && !hit.GetComponent<StateMachine>().IsEvading)
            {
                hit.GetComponent<StateMachine>().TakeDeflect(_stateMachine.transform.position);
            }
        }
    }
}
