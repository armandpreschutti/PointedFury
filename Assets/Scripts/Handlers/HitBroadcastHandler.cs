using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBroadcastHandler : MonoBehaviour
{
    [SerializeField] StateMachine _stateMachine;
    [SerializeField] List<GameObject> _hitTargets = new List<GameObject>();
    [SerializeField] string _enemyTag;
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


    private void Update()
    {
        foreach (GameObject enemy in _hitTargets)
        {
            if (enemy.GetComponent<StateMachine>().IsDead)
            {
                _hitTargets.Remove(enemy);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // Add the object to the list of objects in the trigger area
        if(other.CompareTag(_enemyTag))   
        {
            _hitTargets.Add(other.gameObject);
        }
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
            if(hit.GetComponent<StateMachine>() != null) 
            {
                //_stateMachine.OnAttackSuccess?.Invoke();
                hit.GetComponent<StateMachine>().TakeHit(_stateMachine.AttackType, _stateMachine.AttackID, _stateMachine.transform.position, _stateMachine.AttackDamage);
            }
        }
    }
}
