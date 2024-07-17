using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class PracticeEnemyManagementSystem : MonoBehaviour
{
    public GameObject[] managedEnemies/* = new GameObject[10]*/; // assuming a max of 10 enemies
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

    private void Awake()
    {
        //enemyCount = managedEnemies.Length;
        managedEnemies = new GameObject[100];
        player = GameObject.Find("Player");
        //  Debug.LogError(managedEnemies.Length);
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
        Debug.Log("Attempting to clear enemies on system");
        // Stop all running coroutines
        StopCoroutines();

        // Clear all managed enemies
        for (int i = 0; i < managedEnemies.Length; i++)
        {
            if (managedEnemies[i] != null)
            {
                // Optionally, you can destroy the enemy game objects if needed
                Destroy(managedEnemies[i]);
                managedEnemies[i] = null;
            }
        }

        // Reset enemy count and related states
        enemyCount = 0;
        currentAttacker = null;
        newAttacker = null;
        zoneActive = false;

        // Notify listeners that the zone is cleared
        OnZoneEnemiesCleared?.Invoke(false, 0);
        OnInitiateTutorialUI?.Invoke(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        //  Debug.Log("Trigger entered by: " + other.tag); // Debug log

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
                other.GetComponent<StateMachine>().EnemiesNearby.Add(player.gameObject);
             /*   foreach (GameObject enemy in managedEnemies)
                {
                    if (enemy != null)
                    {
                        OnEnemyDetected?.Invoke(true, enemy.transform.Find("PlayerCameraTarget"));
                        enemy.GetComponent<AIBrain>().enabled = true;
                        enemy.GetComponent<StateMachine>().CurrentTarget = player.gameObject;
                        enemy.GetComponent<StateMachine>().EnemiesNearby.Add(player.gameObject);
                    }
                }*/
                
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
            // Debug.Log("Set Attacker coroutine started");
            yield return new WaitForSeconds(NewAttackerCheckInterval);
            SetAttacker();
        }
    }

    private IEnumerator CleanEnemyListCoroutine()
    {
        while (zoneActive)
        {
            //Debug.Log("Enemy cleaner coroutine started");
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
                    i--; // Adjust index due to removal
                }
            }

            if (enemyCount == 1)
            {
                OnLastEnemy?.Invoke(true, 1);
            }

            if (enemyCount == 0)
            {
                OnZoneEnemiesCleared?.Invoke(false, 0);
                OnInitiateTutorialUI?.Invoke(false);
                zoneActive = false;
                StopCoroutines();
                this.enabled = false;
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
            //Debug.Log("Added enemy: " + enemy.name + ". Total enemies: " + enemyCount);
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
            // Debug.Log("Closest Enemy picked");
            currentAttacker = ClosestEnemy();
        }
        else if (enemyCount > 1 && (SecondClosestEnemy().GetComponent<HealthSystem>().CurrentHealth > (ClosestEnemy().GetComponent<HealthSystem>().MaxHealth / 2)))
        {
            // Debug.Log("Random Enemy picked");
            currentAttacker = SecondClosestEnemy();
        }
        else if (enemyCount > 2 && (ThirdClosestEnemy().GetComponent<HealthSystem>().CurrentHealth > (ClosestEnemy().GetComponent<HealthSystem>().MaxHealth / 2)))
        {
            // Debug.Log("Random Enemy picked");
            currentAttacker = ThirdClosestEnemy();
        }
        else /*if (currentAttacker.GetComponent<HealthSystem>() != null && currentAttacker.GetComponent<HealthSystem>().CurrentHealth < 50 && enemyCount > 2 && SecondClosestEnemy().GetComponent<HealthSystem>().CurrentHealth < 50)*/
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
