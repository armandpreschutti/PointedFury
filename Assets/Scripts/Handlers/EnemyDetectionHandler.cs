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
            //_stateMachine.IsFighting = true;

            foreach (GameObject enemy in _stateMachine.EnemiesNearby)
            {
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
            //_stateMachine.IsFighting = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object exiting the trigger area is in the list
        if (_stateMachine.EnemiesNearby.Contains(other.gameObject))
        {
            // Remove the object from the list of objects in the trigger area
            _stateMachine.EnemiesNearby.Remove(other.gameObject);
            /*if (_stateMachine.EnemiesNearby.Count == 0)
            {
                _stateMachine.IsFighting = false;
            }*/
        }
    }

    public void ProximityDetection()
    {
        float closestDistance = Mathf.Infinity;
        closestTarget = null;

        // Iterate through all hits to find the closest collider
        foreach (GameObject hit in _stateMachine.EnemiesNearby)
        {
            float distance = Vector3.Distance(transform.position, hit.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = hit.transform;
                _stateMachine.ClosestTarget = closestTarget;
            }
        }        
    }

    public void StickDetection()
    {
        if(closestTarget != null)
        {
            if (_stateMachine.MoveInput != Vector2.zero)
            {
                RaycastHit info;
                if (Physics.SphereCast(transform.position, 1f, _stateMachine.InputDirection(), out info, EnemyDetectionRadius, EnemyLayers))
                {
                    if (_stateMachine.EnemiesNearby.Contains(info.transform.gameObject))
                    {
                        _stateMachine.CurrentTarget = info.transform.gameObject;
                    }
                }
            }
            else
            {
               _stateMachine.CurrentTarget = closestTarget.gameObject;
            }
        }
        else
        {
            _stateMachine.CurrentTarget = null;
        }
    }
}
