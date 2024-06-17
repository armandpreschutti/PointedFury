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
     //   _stateMachine.OnAttemptParry += LandFinisher;
    }

    private void OnDisable()
    {
        _stateMachine.OnAttackContact -= LandAttackOnEnemy;
        _stateMachine.OnAttackContact -= BreakObject;
       // _stateMachine.OnAttemptParry -= LandFinisher;
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

    private void LandFinisher()
    {
        foreach (GameObject hit in _enemyTargets)
        {
            if (hit.GetComponent<StateMachine>() != null)
            {
                if (hit.GetComponent<StateMachine>().IsNearDeath)
                {
                    hit.GetComponent<StateMachine>().TakeFinisher(_stateMachine.transform.position, _stateMachine.FinishingPosition);
                    _stateMachine.GiveFinisher();
                }
            }
        }
    }
    private void BreakObject()
    {
        foreach (GameObject obj in _breakableObjects)
        {
            if(_stateMachine.AttackType == "Heavy")
            {
               
                obj.GetComponent<Collider>().enabled = false;
                obj.GetComponent<Rigidbody>().isKinematic = false;

                obj.GetComponent<Fracture>().CauseFracture();
                GameObject fragmentParent = GameObject.Find($"{obj.name}Fragments");
                Rigidbody[] fragments = fragmentParent.GetComponentsInChildren<Rigidbody>();
                foreach (Rigidbody fragment in fragments)
                {

                    fragment.GetComponent<Rigidbody>().AddForce((fragment.transform.position + transform.position).normalized * .1f, ForceMode.Impulse);
                }
                Destroy(obj);




            }

        }
    }

    public GameObject ClosestNearDeathEnemy()
    {
        float[] distances = new float[3] { Mathf.Infinity, Mathf.Infinity, Mathf.Infinity };
        Transform[] closestTargets = new Transform[3] { null, null, null };

        foreach (GameObject enemy in _enemyTargets)
        {
            if (enemy != null)
            {
                float distance = Vector3.Distance(transform.parent.position, enemy.transform.position);

                for (int i = 0; i < distances.Length; i++)
                {
                    if (distance < distances[i])
                    {
                        for (int j = distances.Length - 1; j > i; j--)
                        {
                            distances[j] = distances[j - 1];
                            closestTargets[j] = closestTargets[j - 1];
                        }

                        distances[i] = distance;
                        closestTargets[i] = enemy.transform;
                        break;
                    }
                }
            }
        }

        return closestTargets[0]?.gameObject;
    }


}
