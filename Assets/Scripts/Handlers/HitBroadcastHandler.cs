using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBroadcastHandler : MonoBehaviour
{
    [SerializeField] PlayerStateMachine _playerStateMachine;
    [SerializeField] List<GameObject> _hitTargets = new List<GameObject>();
    [SerializeField] LayerMask _enemyLayer;

    private void OnEnable()
    {
        _playerStateMachine.OnAttackContact += LandAttack;
    }

    private void OnDisable()
    {
        _playerStateMachine.OnAttackContact -= LandAttack;
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
            /*Debug.Log(hit.name);*/
            hit.GetComponent<DebugTester>().PlayHurtAnimation(_playerStateMachine.AttackType);
            //Debug.Log(_playerStateMachine.AttackType);
        }
    }
}
