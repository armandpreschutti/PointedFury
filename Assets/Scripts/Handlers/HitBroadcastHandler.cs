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
    public float ObjectBreakForce;

    private void Awake()
    {
        _stateMachine = GetComponentInParent<StateMachine>();
    }

    private void OnEnable()
    {
        _stateMachine.OnAttackContact += LandAttackOnEnemy;
        _stateMachine.OnAttackContact += BreakObject;
        // _stateMachine.OnAttemptParry += LandFinisher;
    }

    private void OnDisable()
    {
        _stateMachine.OnAttackContact -= LandAttackOnEnemy;
        _stateMachine.OnAttackContact -= BreakObject;
        // _stateMachine.OnAttemptParry -= LandFinisher;
    }

    private void Update()
    {
        for (int i = _enemyTargets.Count - 1; i >= 0; i--)
        {
            GameObject enemy = _enemyTargets[i];
            if (enemy.GetComponent<StateMachine>().IsDead || enemy.GetComponent<StateMachine>() == null)
            {
                _enemyTargets.RemoveAt(i);
            }
        }

        for (int i = _breakableObjects.Count - 1; i >= 0; i--)
        {
            GameObject obj = _breakableObjects[i];
            if (obj == null)
            {
                _breakableObjects.RemoveAt(i);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Add the object to the list of enemies in the trigger area
        if (other.CompareTag(_enemyTag))
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
        for (int i = 0; i < _enemyTargets.Count; i++)
        {
            GameObject hit = _enemyTargets[i];
            hit.GetComponent<StateMachine>().TakeHit(_stateMachine.AttackType, _stateMachine.AttackType == "Light" ? _stateMachine.LightAttackID : _stateMachine.HeavyAttackID, _stateMachine.transform.position, _stateMachine.AttackType == "Light" ? _stateMachine.LightAttackDamage : _stateMachine.HeavyAttackDamage);
            _stateMachine.GiveHit(_stateMachine.AttackType);
        }
    }

    /*private void LandFinisher()
    {
        for (int i = 0; i < _enemyTargets.Count; i++)
        {
            GameObject hit = _enemyTargets[i];
            if (hit.GetComponent<StateMachine>() != null)
            {
                if (hit.GetComponent<StateMachine>().IsNearDeath)
                {
                    hit.GetComponent<StateMachine>().TakeFinisher(_stateMachine.transform.position, _stateMachine.FinishingPosition);
                    _stateMachine.GiveFinisher();
                }
            }
        }
    }*/

    private void BreakObject()
    {
        for (int i = 0; i < _breakableObjects.Count; i++)
        {
            GameObject obj = _breakableObjects[i];
            if (_stateMachine.AttackType == "Heavy")
            {
                obj.GetComponent<Collider>().enabled = false;
                obj.GetComponent<Rigidbody>().isKinematic = false;

                obj.GetComponent<Fracture>().CauseFracture();
                
                GameObject fragmentParent = GameObject.Find($"{obj.name}Fragments");
                Rigidbody[] fragments = fragmentParent.GetComponentsInChildren<Rigidbody>();
                for (int j = 0; j < fragments.Length; j++)
                {
                    Rigidbody fragment = fragments[j];
                    fragment.GetComponent<Rigidbody>().AddForce((fragment.transform.position + transform.position).normalized * ObjectBreakForce, ForceMode.Impulse);
                    fragment.gameObject.layer = LayerMask.NameToLayer("Debris");
                }

                _breakableObjects.RemoveAt(i);
            }
        }
    }
}
