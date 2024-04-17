using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagementSystem : MonoBehaviour
{
    public List<GameObject> managedEnemies = new List<GameObject>();
    public GameObject newAttacker;
    public GameObject currentAttacker;
    public GameObject previousAttacker;
    public bool zoneActive;

    public static Action<Transform> OnEnemyDetected;
    public static Action<Transform> OnAttackerDeath;

    public static Action<Transform> OnTargetGroupFound;
    public static Action<bool> OnZoneEntered;
    public static Action<bool> OnZoneEnemiesCleared;
    public static Action<GameObject, GameObject> OnNewAttacker;
    public static Action<GameObject> OnRemoveUnusedAttacker;
    public static Action<bool, Transform> OnLastEnemyStanding;

    // statevariables
    float _newAttackerCheckTime;
    public float NewAttackerCheckInterval = .1f;
    float _enemyDeathCheckTime;
    public float EnemyDeathCheckInterval = 4f;


    private void Update()
    {
        SetAttacker();
        CleanEnemyList();
/*        GameObject.Find("Player").GetComponent<StateMachine>().IsFighting = zoneActive ? true : false;*/
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (!managedEnemies.Contains(other.gameObject))
            {
               // OnEnemyDetected?.Invoke(other.transform.Find("PlayerCameraTarget").transform);
                managedEnemies.Add(other.gameObject);
            }
        }
        if (other.CompareTag("Player"))
        {
            //other.GetComponent<StateMachine>().IsFighting = true;
            zoneActive = true;
            OnZoneEntered?.Invoke(true);
            //OnTargetGroupFound?.Invoke(transform.Find("TargetGroup"));
            foreach (GameObject enemy in managedEnemies)
            {
                OnEnemyDetected?.Invoke(enemy.transform.Find("PlayerCameraTarget"));
                enemy.GetComponent<AIBrain>().isActivated = true;
                enemy.GetComponent<StateMachine>().CurrentTarget = other.gameObject;
                enemy.GetComponent<StateMachine>().EnemiesNearby.Add(other.gameObject);
                enemy.GetComponent<StateMachine>().IsFighting = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<StateMachine>().IsFighting = false;
            zoneActive = false;
            OnZoneEntered?.Invoke(false);
            //OnTargetGroupFound?.Invoke(null);
            foreach (GameObject enemy in managedEnemies)
            {
                enemy.GetComponent<AIBrain>().isActivated = false;
                enemy.GetComponent<StateMachine>().CurrentTarget = null;
                enemy.GetComponent<StateMachine>().EnemiesNearby.Remove(other.gameObject);
                enemy.GetComponent<StateMachine>().IsFighting = false;
            }
        }

    }

    public void SetAttacker()
    {
        if (zoneActive)
        {
            _newAttackerCheckTime -= Time.deltaTime;
            
            if (_newAttackerCheckTime < 0 || currentAttacker == null)
            {
                if (managedEnemies.Count > 0)
                {
                    _newAttackerCheckTime = NewAttackerCheckInterval;
                    ResetEnemyStates(managedEnemies);
                    SetNewAttacker();
                    currentAttacker.GetComponent<AIBrain>().isAttacker = true;
                    currentAttacker.GetComponent<AIBrain>().isWatcher = false;
                    OnNewAttacker?.Invoke(currentAttacker, previousAttacker);
                }
                else
                {
                    newAttacker = null;
                    currentAttacker = null;
                    previousAttacker = null;
                    return;
                }

            }
        }
    }

    public void CleanEnemyList()
    {
        if (zoneActive)
        {
            _enemyDeathCheckTime -= Time.deltaTime;
            if (_enemyDeathCheckTime < 0)
            {
                _enemyDeathCheckTime = EnemyDeathCheckInterval;
                foreach (GameObject enemy in managedEnemies)
                {
                    if (enemy.GetComponent<StateMachine>().IsDead)
                    {
                        if (enemy == currentAttacker)
                        {
                            SetAttacker();
                        }
                        OnAttackerDeath?.Invoke(enemy.transform.Find("PlayerCameraTarget").transform);
                        managedEnemies.Remove(enemy);
                    }
                }
                if (managedEnemies.Count == 0)
                {
                    OnZoneEnemiesCleared?.Invoke(false);
                }
            }
        }
    }
    public void ResetEnemyStates(List<GameObject> enemies)
    {
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<AIBrain>().isAttacker = false;
            enemy.GetComponent<AIBrain>().isWatcher = true;
        }
    }

    public void SetNewAttacker()
    {
        newAttacker = managedEnemies[UnityEngine.Random.Range(0, managedEnemies.Count)];
        if(newAttacker != currentAttacker)
        {
            if(currentAttacker == null)
            {
                currentAttacker = newAttacker; 
                if(previousAttacker == null)
                {
                    previousAttacker = currentAttacker;
                }
            }
            else
            {
                previousAttacker = currentAttacker;
                currentAttacker = newAttacker;
            }

        }
    }
}
