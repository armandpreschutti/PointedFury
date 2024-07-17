using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectionHandler : MonoBehaviour
{

    [SerializeField] StateMachine _stateMachine;
    //[SerializeField] List<GameObject> _enemiesNearby = new List<GameObject>();
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

    private void Update()
    {
        if(!_stateMachine.IsParrying)
        {
            ProximityDetection();
            StickDetection();
        }       
        if(_stateMachine.EnemiesNearby.Count > 0)
        {
            // _stateMachine.IsFighting = true;

            for (int i = 0; i < _stateMachine.EnemiesNearby.Count; i++)
            {
                GameObject enemy = _stateMachine.EnemiesNearby[i];
                if (enemy.GetComponent<StateMachine>().IsDead)
                {
                    _stateMachine.EnemiesNearby.Remove(enemy);
                }
            }
        }        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // Add the object to the list of objects in the trigger area
        if (other.CompareTag(_enemyTag))
        {
            _stateMachine.EnemiesNearby.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object exiting the trigger area is in the list
        if (_stateMachine.EnemiesNearby.Contains(other.gameObject))
        {
            // Remove the object from the list of objects in the trigger area
            _stateMachine.EnemiesNearby.Remove(other.gameObject);
            if(_stateMachine.EnemiesNearby.Count == 0f)
            {
                _stateMachine.IsFighting = false;
            }
        }
    }

    public void ProximityDetection()
    {
        if (!_stateMachine.IsAttacking && !_stateMachine.IsDeflecting)
        {
            float[] distances = new float[3] { Mathf.Infinity, Mathf.Infinity, Mathf.Infinity };
            Transform[] closestTargets = new Transform[3] { null, null, null };

            // Iterate through all hits to find the closest colliders
            for (int i1 = 0; i1 < _stateMachine.EnemiesNearby.Count; i1++)
            {
                GameObject hit = _stateMachine.EnemiesNearby[i1];
                float distance = Vector3.Distance(transform.position, hit.transform.position);

                // Update closest targets array if a closer target is found
                for (int i = 0; i < distances.Length; i++)
                {
                    if (distance < distances[i])
                    {
                        // Shift elements to make space for the new closest target
                        for (int j = distances.Length - 1; j > i; j--)
                        {
                            distances[j] = distances[j - 1];
                            closestTargets[j] = closestTargets[j - 1];
                        }

                        // Assign the new closest target
                        distances[i] = distance;
                        closestTargets[i] = hit.transform;
                        break; // Exit the loop after updating the closest target
                    }
                }
            }

            // Assign closest, second closest, and third closest targets
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
        if (!_stateMachine.IsAttacking && !_stateMachine.IsDeflecting)
        {
            if (closestTarget != null)
            {
                if (_stateMachine.MoveInput != Vector2.zero && _stateMachine.EnemiesNearby.Count > 1)
                {
                    RaycastHit info;
                    if (Physics.SphereCast(transform.position, 1f, _stateMachine.InputDirection(), out info, EnemyDetectionRadius * 2, EnemyLayers))
                    {
                        if (_stateMachine.EnemiesNearby.Contains(info.transform.gameObject))
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
}
