using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.InputSystem.LowLevel;

public class CameraFramingHandler : MonoBehaviour
{
    private CinemachineTargetGroup _targetGroup;
    [SerializeField] EnemyManagementSystem enemyManagementSystem;
    
  /*  public Transform _currentAttacker;
    public Transform _previousAttacker;
    public float MinimumEnemyWeight = .3f;
    public float MaximumEnemyWeight = 1f;*/

    private void Awake()
    {
        _targetGroup = GetComponent<CinemachineTargetGroup>();
    }
    private void OnEnable()
    {
        EnemyManagementSystem.OnAttackerDeath += RemoveEntityFromTargetGroup;
        EnemyManagementSystem.OnEnemyDetected += SetZoneTargets;

    }

    private void OnDisable()
    {
        EnemyManagementSystem.OnAttackerDeath -= RemoveEntityFromTargetGroup;
        EnemyManagementSystem.OnEnemyDetected -= SetZoneTargets;
    }

    public void SetZoneTargets(bool value, Transform entity)
    {
        if(value)
        {
            AddEntityToTargetGroup(entity);
        }
        else
        {
            RemoveEntityFromTargetGroup(entity);
        }
    }
    public void AddEntityToTargetGroup(Transform entity)
    {
        if (!TargetGroupContainsTransform(_targetGroup, entity))
        {
            _targetGroup.AddMember(entity, 1, 0);
        }
        else
        {
            return;
        }
    }

    private void RemoveEntityFromTargetGroup(Transform entity)
    {
        if (TargetGroupContainsTransform(_targetGroup, entity))
        {
            _targetGroup.RemoveMember(entity);
        }
        else
        {
            return;
        }
    }

    bool TargetGroupContainsTransform(CinemachineTargetGroup targetGroup, Transform target)
    {
        // Iterate through all the members of the target group
        for (int i = 0; i < targetGroup.m_Targets.Length; i++)
        {
            // Check if the transform matches the target we're looking for
            if (targetGroup.m_Targets[i].target == target)
            {
                return true;
            }
        }
        return false;
    }

    public void RotateEnemyWeights()
    {
        /*if (_stateMachine.ClosestTarget != null)
        {
            // Iterate through all the members of the target group
            for (int i = 0; i < _targetGroup.m_Targets.Length; i++)
            {
                if (_targetGroup.m_Targets[i].target.gameObject == GameObject.Find("Player"))
                {
                    _targetGroup.m_Targets[i].weight = 1f;
                }
                // Check if the transform matches the target we're looking for
                else if (_targetGroup.m_Targets[i].target == _stateMachine.ClosestTarget)
                {
                    if (_targetGroup.m_Targets[i].weight < MaximumEnemyWeight)
                    {
                        _targetGroup.m_Targets[i].weight += .1f * Time.deltaTime * 10f;
                    }
                    else
                    {
                        _targetGroup.m_Targets[i].weight = MaximumEnemyWeight;
                    }

                }
                else if (_targetGroup.m_Targets[i].target != _stateMachine.ClosestTarget)
                {
                    if (_targetGroup.m_Targets[i].weight > MinimumEnemyWeight)
                    {
                        _targetGroup.m_Targets[i].weight -= .1f * Time.deltaTime * 10f;
                    }
                    else
                    {
                        _targetGroup.m_Targets[i].weight = MinimumEnemyWeight;
                    }

                }
            }
            _targetGroup.m_Targets[1].target = _stateMachine.ClosestTarget;
        }*/

    }
}
