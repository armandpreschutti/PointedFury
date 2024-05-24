using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HitBroadcastHandler : MonoBehaviour
{
    StateMachine _stateMachine;
    public List<GameObject> _enemyTargets = new List<GameObject>();
    public List<GameObject> _breakableObjects = new List<GameObject>();
    public string _enemyTag;
    public string _breakableTag;

    private void Awake()
    {
        _stateMachine = GetComponentInParent<StateMachine>();
    }

    private void OnEnable()
    {
        _stateMachine.OnAttackContact += LandAttackOnEnemy;
        _stateMachine.OnAttackContact += BreakObject;
    }

    private void OnDisable()
    {
        _stateMachine.OnAttackContact -= LandAttackOnEnemy;
        _stateMachine.OnAttackContact -= BreakObject;
    }

    private void Update()
    {
        foreach (GameObject enemy in _enemyTargets)
        {
            if (enemy.GetComponent<StateMachine>().IsDead || enemy.GetComponent<StateMachine>() == null)
            {
                _enemyTargets.Remove(enemy);
            }
        }
        foreach (GameObject obj in _breakableObjects)
        {
            if (obj == null)
            {
                _enemyTargets.Remove(obj);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        
        // Add the object to the list of enemies in the trigger area
        if(other.CompareTag(_enemyTag))   
        {
            _enemyTargets.Add(other.gameObject);
        }

        // Add the object to the list of objects in the trigger area
        if (other.CompareTag(_breakableTag))
        {
            _breakableObjects.Add(other.gameObject);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object exiting the trigger area is in the list
        if (_enemyTargets.Contains(other.gameObject))
        {
            // Remove the object from the list of objects in the trigger area
            _enemyTargets.Remove(other.gameObject);
        }
        // Check if the object exiting the trigger area is in the list
        if (_breakableObjects.Contains(other.gameObject))
        {
            // Remove the object from the list of objects in the trigger area
            _breakableObjects.Remove(other.gameObject);
        }
    }

    private void LandAttackOnEnemy()
    {
        foreach(GameObject hit in _enemyTargets)
        {
            hit.GetComponent<StateMachine>().TakeHit(_stateMachine.AttackType, _stateMachine.AttackType == "Light" ? _stateMachine.LightAttackID : _stateMachine.HeavyAttackID, _stateMachine.transform.position, _stateMachine.AttackType == "Light" ? _stateMachine.LightAttackDamage : _stateMachine.HeavyAttackDamage);
            _stateMachine.GiveHit(_stateMachine.AttackType);
        }
    }
    private void BreakObject()
    {
        foreach (GameObject obj in _breakableObjects)
        {
            if(_stateMachine.AttackType == "Heavy")
            {
                Destroy(obj);
            }

        }
    }

}
