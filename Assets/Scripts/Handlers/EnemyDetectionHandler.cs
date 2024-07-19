using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectionHandler : MonoBehaviour
{
    [SerializeField] StateMachine _stateMachine;
    //[SerializeField] List<GameObject> _enemiesNearby = new List<GameObject>(); // No longer using a list
    [SerializeField] string _enemyTag;

    [Tooltip("The radius of the enemy detection zone when not aiming")]
    public float EnemyDetectionRadius;
    [Tooltip("What layers the character detects enemies on")]
    public LayerMask EnemyLayers;
    public Transform closestTarget;
    public Transform secondClosestTarget;
    public Transform thirdClosestTarget;

    private void Awake()
    {
        _stateMachine = GetComponentInParent<StateMachine>();
    }
    private void OnEnable()
    {
        PracticeConfigController.OnClearEnemies += ClearAllEnemies;
    }

    private void OnDisable()
    {
        PracticeConfigController.OnClearEnemies -= ClearAllEnemies;
    }
    private void Update()
    {
        if (!_stateMachine.IsParrying)
        {
            ProximityDetection();
            StickDetection();
        }

        // Remove dead enemies from the array
        for (int i = 0; i < _stateMachine.EnemiesNearby.Length; i++)
        {
            GameObject enemy = _stateMachine.EnemiesNearby[i];
            if (enemy != null && enemy.GetComponent<StateMachine>().IsDead)
            {
                RemoveFromArray(i);
                i--; // Adjust index because array length decreases after removal
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_enemyTag))
        {
            AddToArray(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (ArrayContains(other.gameObject))
        {
            RemoveFromArray(ArrayIndexOf(other.gameObject));
            if (_stateMachine.EnemiesNearby.Length == 0)
            {
                _stateMachine.IsFighting = false;
            }
        }
    }

    private void AddToArray(GameObject enemy)
    {
        // Resize the array and add the new enemy
        Array.Resize(ref _stateMachine.EnemiesNearby, _stateMachine.EnemiesNearby.Length + 1);
        _stateMachine.EnemiesNearby[_stateMachine.EnemiesNearby.Length - 1] = enemy;
    }

    private void RemoveFromArray(int index)
    {
        // Shift elements to remove the enemy at the specified index
        for (int i = index; i < _stateMachine.EnemiesNearby.Length - 1; i++)
        {
            _stateMachine.EnemiesNearby[i] = _stateMachine.EnemiesNearby[i + 1];
        }
        Array.Resize(ref _stateMachine.EnemiesNearby, _stateMachine.EnemiesNearby.Length - 1);
    }

    private bool ArrayContains(GameObject enemy)
    {
        return Array.IndexOf(_stateMachine.EnemiesNearby, enemy) != -1;
    }

    private int ArrayIndexOf(GameObject enemy)
    {
        return Array.IndexOf(_stateMachine.EnemiesNearby, enemy);
    }

    public void ProximityDetection()
    {
        if (/*!_stateMachine.IsAttacking &&*/ !_stateMachine.IsDeflecting)
        {
            float[] distances = new float[3] { Mathf.Infinity, Mathf.Infinity, Mathf.Infinity };
            Transform[] closestTargets = new Transform[3] { null, null, null };

            for (int i = 0; i < _stateMachine.EnemiesNearby.Length; i++)
            {
                GameObject hit = _stateMachine.EnemiesNearby[i];
                if (hit != null)
                {
                    float distance = Vector3.Distance(transform.position, hit.transform.position);
                    for (int j = 0; j < distances.Length; j++)
                    {
                        if (distance < distances[j])
                        {
                            for (int k = distances.Length - 1; k > j; k--)
                            {
                                distances[k] = distances[k - 1];
                                closestTargets[k] = closestTargets[k - 1];
                            }
                            distances[j] = distance;
                            closestTargets[j] = hit.transform;
                            break;
                        }
                    }
                }
            }

            closestTarget = closestTargets[0];
            _stateMachine.ClosestTarget = closestTargets[0];
            secondClosestTarget = closestTargets[1];
            _stateMachine.SecondClosestTarget = closestTargets[1];
            thirdClosestTarget = closestTargets[2];
            _stateMachine.ThirdClosestTarget = closestTargets[2];
        }
    }

    public void StickDetection()
    {
        if (/*!_stateMachine.IsAttacking &&*/ !_stateMachine.IsDeflecting)
        {
            if (closestTarget != null)
            {
                if (_stateMachine.MoveInput != Vector2.zero && _stateMachine.EnemiesNearby.Length > 1)
                {
                    RaycastHit info;
                    if (Physics.SphereCast(transform.position, 1f, _stateMachine.InputDirection(), out info, EnemyDetectionRadius * 2, EnemyLayers))
                    {
                        if (ArrayContains(info.transform.gameObject))
                        {
                            _stateMachine.CurrentTarget = info.transform.gameObject;
                        }
                    }
                }
                else
                {
                    if (!_stateMachine.IsParrying)
                    {
                        _stateMachine.CurrentTarget = closestTarget.gameObject;
                    }
                }
            }
            else
            {
                _stateMachine.CurrentTarget = null;
            }
        }
    }

    public void ClearAllEnemies()
    {
        _stateMachine.EnemiesNearby = new GameObject[0];
        _stateMachine.IsFighting = false;
    }
}
