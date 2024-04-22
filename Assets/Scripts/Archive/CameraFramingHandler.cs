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
    [SerializeField] StateMachine _stateMachine;

    public float MaximumEnemyWeight;
    public float MinimumEnemyWeight;
    public float WeightRotationSpeed;

    private void Awake()
    {
        _targetGroup = GetComponent<CinemachineTargetGroup>();
        _stateMachine = GameObject.Find("Player").GetComponent<StateMachine>();
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

    private void Update()
    {
        // RotateEnemyWeights();
        SetTargetWeights();
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
            _targetGroup.AddMember(entity, MinimumEnemyWeight, 0);
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

    public void SetTargetWeights()
    {
        // Iterate through all the members of the target group
        for (int i = 0; i < _targetGroup.m_Targets.Length; i++)
        {
            if (_targetGroup.m_Targets[i].target.parent.name == "Player")
            {
                _targetGroup.m_Targets[i].weight = 1f;
            }
            else
            {
                // Check if the transform matches the target we're looking for
                if (_targetGroup.m_Targets[i].target.parent.transform == _stateMachine.ClosestTarget)
                {
                    if (_targetGroup.m_Targets[i].weight < MaximumEnemyWeight)
                    {
                        _targetGroup.m_Targets[i].weight += .1f * Time.deltaTime * WeightRotationSpeed;
                    }
                    else
                    {
                        _targetGroup.m_Targets[i].weight = MaximumEnemyWeight;
                    }
                }
                else if (_targetGroup.m_Targets[i].target.parent.transform != _stateMachine.ClosestTarget)
                {
                    if (_targetGroup.m_Targets[i].weight > MinimumEnemyWeight)
                    {
                        _targetGroup.m_Targets[i].weight -= .1f * Time.deltaTime * WeightRotationSpeed;
                    }
                    else
                    {
                        _targetGroup.m_Targets[i].weight = MinimumEnemyWeight;
                    }
                }
            }           
        }
    }
}
