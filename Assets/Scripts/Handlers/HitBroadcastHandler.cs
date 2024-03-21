using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBroadcastHandler : MonoBehaviour
{
    [SerializeField] StateMachine _stateMachine;
    [SerializeField] List<GameObject> _hitTargets = new List<GameObject>();
    [SerializeField] LayerMask _enemyLayer;

    private void Awake()
    {
        _stateMachine = GetComponentInParent<StateMachine>();
    }

    private void OnEnable()
    {
        _stateMachine.OnAttackContact += LandAttack;
    }

    private void OnDisable()
    {
        _stateMachine.OnAttackContact -= LandAttack;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Add the object to the list of objects in the trigger area
        _hitTargets.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object exiting the trigger area is in the list
        if (_hitTargets.Contains(other.gameObject))
        {
            // Remove the object from the list of objects in the trigger area
            _hitTargets.Remove(other.gameObject);
        }
    }

    private void LandAttack()
    {
        foreach(GameObject hit in _hitTargets)
        {
           // Debug.Log("LandAttack() called correctly on HitBroadcastHandler");
            if(hit.GetComponent<StateMachine>() != null) 
            {
                hit.GetComponent<StateMachine>().TakeHit(_stateMachine.AttackType);
            }
            //Debug.Log($"Player has attacked {hit.name}");
            //hit.GetComponent<DebugTester>().TakeHit(_StateMachine.AttackType);
            //Debug.Log(_playerStateMachine.AttackType);
        }
    }
}
