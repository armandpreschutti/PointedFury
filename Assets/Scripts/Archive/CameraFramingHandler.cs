using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class CameraFramingHandler : MonoBehaviour
{
    /*private CinemachineTargetGroup _targetGroup;
    private StateMachine _stateMachine;
    public Transform _currentAttacker;
    public Transform _previousAttacker;
    public float MinimumEnemyWeight =.3f;
    public float MaximumEnemyWeight = 1f;
    private void Awake()
    {
        _targetGroup= GetComponent<CinemachineTargetGroup>();
        _stateMachine = GameObject.Find("Player").GetComponent<StateMachine>();
    }
    private void OnEnable()
    {
       *//* EnemyManagementSystem.OnNewAttacker += AddEntityToGroup;
        EnemyManagementSystem.OnAttackerDeath += RemoveEntityFromGroup;
        EnemyManagementSystem.OnRemoveUnusedAttacker += RemoveEntityFromGroup;*//*
    }

    private void OnDisable()
    {
       *//* EnemyManagementSystem.OnNewAttacker -= AddEntityToGroup;
        EnemyManagementSystem.OnAttackerDeath -= RemoveEntityFromGroup;
        EnemyManagementSystem.OnRemoveUnusedAttacker -= RemoveEntityFromGroup;*//*
    }
    private void Update()
    {
        if(_stateMachine.ClosestTarget != null)
        {
            // Iterate through all the members of the target group
            for (int i = 0; i < _targetGroup.m_Targets.Length; i++)
            {
                if(_targetGroup.m_Targets[i].target.gameObject == GameObject.Find("Player"))
                {
                    _targetGroup.m_Targets[i].weight = 1f;
                }
                // Check if the transform matches the target we're looking for
                else if (_targetGroup.m_Targets[i].target == _stateMachine.ClosestTarget)
                {
                    if(_targetGroup.m_Targets[i].weight < MaximumEnemyWeight)
                    {
                        _targetGroup.m_Targets[i].weight += .1f * Time.deltaTime * 10f;
                    }
                    else
                    {
                        _targetGroup.m_Targets[i].weight = MaximumEnemyWeight;
                    }

                }
                else*//* if(_targetGroup.m_Targets[i].target != _stateMachine.ClosestTarget)*//*
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
            *//*_targetGroup.m_Targets[1].target = _stateMachine.ClosestTarget;*//*
        }


    }
    private void AddEntityToGroup(GameObject newAttacker, GameObject currentAttacker)
    {      
        if (!TargetGroupContainsTransform(_targetGroup, newAttacker.transform))
        {
            _targetGroup.AddMember(newAttacker.transform, 1, 0);
            if(_previousAttacker!= null)
            {
                if (TargetGroupContainsTransform(_targetGroup, _previousAttacker))
                {
                    _targetGroup.RemoveMember(_previousAttacker);
                }
                _currentAttacker = _previousAttacker;
            }
            else
            {
               
                _previousAttacker = _currentAttacker;
            }
            _currentAttacker = newAttacker.transform;
        }
    }

    private void RemoveEntityFromGroup(GameObject entity)
    {
        if(TargetGroupContainsTransform(_targetGroup, entity.transform))
        {
            _targetGroup.RemoveMember(entity.transform);
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
   */
}
