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
    }

    private void OnDisable()
    {
        _stateMachine.OnAttackContact -= LandAttackOnEnemy;
        _stateMachine.OnAttackContact -= BreakObject;
    }

    private void Update()
    {
        for (int i = _enemyTargets.Count - 1; i >= 0; i--)
        {
            GameObject enemy = _enemyTargets[i];
            if (enemy == null || enemy.GetComponent<StateMachine>() == null || enemy.GetComponent<StateMachine>().IsDead)
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
        if (other.CompareTag(_enemyTag))
        {
            _enemyTargets.Add(other.gameObject);
        }

        if (other.CompareTag(_breakableTag))
        {
            _breakableObjects.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_enemyTargets.Contains(other.gameObject))
        {
            _enemyTargets.Remove(other.gameObject);
        }

        if (_breakableObjects.Contains(other.gameObject))
        {
            _breakableObjects.Remove(other.gameObject);
        }
    }

    private void LandAttackOnEnemy()
    {
        for (int i = 0; i < _enemyTargets.Count; i++)
        {
            GameObject hit = _enemyTargets[i];
            if (hit != null && hit.GetComponent<StateMachine>() != null)
            {
                hit.GetComponent<StateMachine>().TakeHit(
                    _stateMachine.AttackType,
                    _stateMachine.AttackType == "Light" ? _stateMachine.LightAttackID : _stateMachine.HeavyAttackID,
                    _stateMachine.transform.position,
                    _stateMachine.AttackType == "Light" ? _stateMachine.LightAttackDamage : _stateMachine.HeavyAttackDamage
                );
                _stateMachine.GiveHit(_stateMachine.AttackType);
            }
        }
    }

    private void BreakObject()
    {
        for (int i = 0; i < _breakableObjects.Count; i++)
        {
            GameObject obj = _breakableObjects[i];
            if (obj != null && obj.GetComponent<Fracture>() != null)
            {
                obj.GetComponent<Fracture>().CauseFracture();

                GameObject fragmentParent = GameObject.Find($"{obj.name}Fragments");
                if (fragmentParent != null)
                {
                    Rigidbody[] fragments = fragmentParent.GetComponentsInChildren<Rigidbody>();
                    for (int j = 0; j < fragments.Length; j++)
                    {
                        Rigidbody fragment = fragments[j];
                        if (fragment != null)
                        {
                            fragment.AddForce(
                                (fragment.transform.position - transform.parent.position).normalized * ObjectBreakForce,
                                ForceMode.Impulse
                            );
                        }
                    }
                }

                Destroy(obj.gameObject);
            }
        }
    }
}
