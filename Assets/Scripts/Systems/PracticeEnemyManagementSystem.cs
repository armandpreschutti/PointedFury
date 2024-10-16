using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class PracticeEnemyManagementSystem : MonoBehaviour
{
    public GameObject[] managedEnemies;
    public int enemyCount = 0;
    public GameObject player;
    public GameObject newAttacker;
    public GameObject currentAttacker;
    public bool zoneActive;
    public static Action<bool, Transform> OnEnemyDetected;
    public static Action<Transform> OnAttackerDeath;
    public static Action<Transform> OnTargetGroupFound;
    public static Action<bool, int> OnZoneEntered;
    public static Action<bool, int> OnZoneEnemiesCleared;
    public static Action<GameObject> OnRemoveUnusedAttacker;
    public Action<bool> OnInitiateTutorialUI;
    public static Action<bool, int> OnLastEnemy;
    public bool attackPicked;
    public bool attackerSelected;

    public float NewAttackerCheckInterval = 0.1f;
    public float EnemyDeathCheckInterval = 4f;

    private Coroutine attackerCoroutine;
    private Coroutine cleanerCoroutine;
    public GameObject spawnVFX;

    private void Awake()
    {
        managedEnemies = new GameObject[100];
        player = GameObject.Find("Player");
    }

    private void OnEnable()
    {
        PracticeConfigController.OnClearEnemies += ClearEnemies;
    }

    private void OnDisable()
    {
        PracticeConfigController.OnClearEnemies -= ClearEnemies;
    }

    public void ClearEnemies()
    {
        StopCoroutines();

        for (int i = 0; i < managedEnemies.Length; i++)
        {
            if (managedEnemies[i] != null)
            {
                OnAttackerDeath?.Invoke(managedEnemies[i].transform.Find("PlayerCameraTarget").transform);
                managedEnemies[i] = null;
            }
        }

        enemyCount = 0;
        currentAttacker = null;
        newAttacker = null;
        zoneActive = false;

        OnZoneEnemiesCleared?.Invoke(false, 0);
        OnInitiateTutorialUI?.Invoke(false);

        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < objectsWithTag.Length; i++)
        {
            Instantiate(spawnVFX, objectsWithTag[i].transform.position, Quaternion.identity);
            Destroy(objectsWithTag[i]);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (!Array.Exists(managedEnemies, e => e == other.gameObject))
            {
                AddEnemy(other.gameObject);
                zoneActive = true;
                OnZoneEntered?.Invoke(true, enemyCount);
                OnEnemyDetected?.Invoke(true, other.transform.Find("PlayerCameraTarget"));
                other.GetComponent<AIBrain>().enabled = true;
                other.GetComponent<StateMachine>().CurrentTarget = player.gameObject;

                StateMachine enemyStateMachine = other.GetComponent<StateMachine>();
                if (enemyStateMachine.EnemiesNearby == null)
                {
                    enemyStateMachine.EnemiesNearby = new GameObject[1];
                    enemyStateMachine.EnemiesNearby[0] = other.gameObject;
                }
                else
                {
                    Array.Resize(ref enemyStateMachine.EnemiesNearby, enemyStateMachine.EnemiesNearby.Length + 1);
                    enemyStateMachine.EnemiesNearby[enemyStateMachine.EnemiesNearby.Length - 1] = other.gameObject;
                }
                SetAttacker();
                StartCoroutines();
            }
        }
    }

    private void StartCoroutines()
    {
        if (attackerCoroutine != null) StopCoroutine(attackerCoroutine);
        if (cleanerCoroutine != null) StopCoroutine(cleanerCoroutine);

        attackerCoroutine = StartCoroutine(SetAttackerCoroutine());
        cleanerCoroutine = StartCoroutine(CleanEnemyListCoroutine());
    }

    private IEnumerator SetAttackerCoroutine()
    {
        while (zoneActive)
        {
            yield return new WaitForSeconds(NewAttackerCheckInterval);
            SetAttacker();
        }
    }

    private IEnumerator CleanEnemyListCoroutine()
    {
        while (zoneActive)
        {
            yield return new WaitForSeconds(EnemyDeathCheckInterval);
            CleanEnemyList();
        }
    }

    public void SetAttacker()
    {
        if (zoneActive)
        {
            if (enemyCount > 0)
            {
                ResetEnemyStates(managedEnemies);
                SetNewAttacker();
                currentAttacker.GetComponent<AIBrain>().isAttacker = true;
                currentAttacker.GetComponent<AIBrain>().isWatcher = false;
            }
            else
            {
                newAttacker = null;
                currentAttacker = null;
                return;
            }
        }
    }

    public void CleanEnemyList()
    {
        if (zoneActive)
        {
            for (int i = 0; i < enemyCount; i++)
            {
                GameObject enemy = managedEnemies[i];
                if (enemy.GetComponent<StateMachine>().IsDead || enemy.GetComponent<StateMachine>() == null)
                {
                    if (enemy == currentAttacker)
                    {
                        SetAttacker();
                    }
                    OnAttackerDeath?.Invoke(enemy.transform.Find("PlayerCameraTarget").transform);
                    RemoveEnemy(enemy);
                    i--;
                }
            }

            if (enemyCount == 1)
            {
                OnLastEnemy?.Invoke(true, 1);
            }

            if (enemyCount == 0)
            {
                OnZoneEnemiesCleared?.Invoke(false, 0);
                zoneActive = false;
                StopCoroutines();
            }
        }
    }

    private void StopCoroutines()
    {
        if (attackerCoroutine != null) StopCoroutine(attackerCoroutine);
        if (cleanerCoroutine != null) StopCoroutine(cleanerCoroutine);
    }

    private void AddEnemy(GameObject enemy)
    {
        if (enemyCount < managedEnemies.Length)
        {
            managedEnemies[enemyCount] = enemy;
            enemyCount++;
        }
        else
        {
            // Debug.LogWarning("Managed enemies array is full. Cannot add more enemies.");
        }
    }

    private void RemoveEnemy(GameObject enemy)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            if (managedEnemies[i] == enemy)
            {
                managedEnemies[i] = managedEnemies[--enemyCount];
                managedEnemies[enemyCount] = null;
                break;
            }
        }
    }

    public void ResetEnemyStates(GameObject[] enemies)
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.GetComponent<AIBrain>().isAttacker = false;
                enemy.GetComponent<AIBrain>().isWatcher = true;
            }
        }
    }

    public void SetNewAttacker()
    {
        if ((ClosestEnemy().GetComponent<HealthSystem>().CurrentHealth > (ClosestEnemy().GetComponent<HealthSystem>().MaxHealth / 2)
            || enemyCount == 1))
        {
            currentAttacker = ClosestEnemy();
        }
        else if (enemyCount > 1 && (SecondClosestEnemy().GetComponent<HealthSystem>().CurrentHealth > (ClosestEnemy().GetComponent<HealthSystem>().MaxHealth / 2)))
        {
            currentAttacker = SecondClosestEnemy();
        }
        else if (enemyCount > 2 && (ThirdClosestEnemy().GetComponent<HealthSystem>().CurrentHealth > (ClosestEnemy().GetComponent<HealthSystem>().MaxHealth / 2)))
        {
            currentAttacker = ThirdClosestEnemy();
        }
        else
        {
            currentAttacker = ClosestEnemy();
        }
    }

    public GameObject ClosestEnemy()
    {
        float[] distances = new float[3] { Mathf.Infinity, Mathf.Infinity, Mathf.Infinity };
        Transform[] closestTargets = new Transform[3] { null, null, null };

        foreach (GameObject enemy in managedEnemies)
        {
            if (enemy != null)
            {
                float distance = Vector3.Distance(player.transform.position, enemy.transform.position);

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

    public GameObject SecondClosestEnemy()
    {
        float[] distances = new float[3] { Mathf.Infinity, Mathf.Infinity, Mathf.Infinity };
        Transform[] closestTargets = new Transform[3] { null, null, null };

        foreach (GameObject enemy in managedEnemies)
        {
            if (enemy != null)
            {
                float distance = Vector3.Distance(player.transform.position, enemy.transform.position);

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

        return closestTargets[1]?.gameObject;
    }

    public GameObject ThirdClosestEnemy()
    {
        float[] distances = new float[3] { Mathf.Infinity, Mathf.Infinity, Mathf.Infinity };
        Transform[] closestTargets = new Transform[3] { null, null, null };

        foreach (GameObject enemy in managedEnemies)
        {
            if (enemy != null)
            {
                float distance = Vector3.Distance(player.transform.position, enemy.transform.position);

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

        return closestTargets[2]?.gameObject;
    }
}
