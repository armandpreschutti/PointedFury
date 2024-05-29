using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagementSystem : MonoBehaviour
{
    public GameObject[] managedEnemies/* = new GameObject[10]*/; // assuming a max of 10 enemies
    public int enemyCount = 0;

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
        managedEnemies = new GameObject[10];
        Debug.LogError(managedEnemies.Length);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.tag); // Debug log

        if (other.CompareTag("Enemy"))
        {
            if (!Array.Exists(managedEnemies, e => e == other.gameObject))
            {
                
                AddEnemy(other.gameObject);
                Debug.Log("Enemy added: " + other.gameObject.name); // Debug log
            }
        }

        if (other.CompareTag("Player"))
        {
            if (enemyCount != 0)
            {
                zoneActive = true;
                OnZoneEntered?.Invoke(true, enemyCount);
                OnInitiateTutorialUI?.Invoke(true);

                foreach (GameObject enemy in managedEnemies)
                {
                    if (enemy != null)
                    {
                        OnEnemyDetected?.Invoke(true, enemy.transform.Find("PlayerCameraTarget"));
                        enemy.GetComponent<AIBrain>().enabled = true;
                        enemy.GetComponent<StateMachine>().CurrentTarget = other.gameObject;
                        enemy.GetComponent<StateMachine>().EnemiesNearby.Add(other.gameObject);
                    }
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
            Debug.Log("Set Attacker coroutine started");
            yield return new WaitForSeconds(NewAttackerCheckInterval);
            SetAttacker();
        }
    }

    private IEnumerator CleanEnemyListCoroutine()
    {
        while (zoneActive)
        {
            Debug.Log("Enemy cleaner coroutine started");
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
            Debug.Log("Added enemy: " + enemy.name + ". Total enemies: " + enemyCount);
        }
        else
        {
            Debug.LogWarning("Managed enemies array is full. Cannot add more enemies.");
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
        if (ClosestEnemy() != currentAttacker || enemyCount == 1)
        {
            newAttacker = ClosestEnemy();
            currentAttacker = newAttacker;
            Debug.Log(currentAttacker.name);
        }
        else
        {
            newAttacker = managedEnemies[UnityEngine.Random.Range(0, enemyCount)];
            if (newAttacker == ClosestEnemy())
            {
                SetNewAttacker();
            }
            else
            {
                currentAttacker = newAttacker;
                Debug.Log(currentAttacker.name);
            }
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
                float distance = Vector3.Distance(transform.position, enemy.transform.position);

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
    /*  public List<GameObject> managedEnemies = new List<GameObject>();
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

      private void OnTriggerEnter(Collider other)
      {
          if (other.CompareTag("Enemy"))
          {
              if (!managedEnemies.Contains(other.gameObject))
              {
                  managedEnemies.Add(other.gameObject);
              }
          }
          if (other.CompareTag("Player"))
          {
              if (managedEnemies.Count != 0)
              {
                  zoneActive = true;
                  OnZoneEntered?.Invoke(true, managedEnemies.Count);
                  OnInitiateTutorialUI?.Invoke(true);
                  foreach (GameObject enemy in managedEnemies)
                  {
                      OnEnemyDetected?.Invoke(true, enemy.transform.Find("PlayerCameraTarget"));
                      enemy.GetComponent<AIBrain>().enabled = true;
                      enemy.GetComponent<StateMachine>().CurrentTarget = other.gameObject;
                      enemy.GetComponent<StateMachine>().EnemiesNearby.Add(other.gameObject);
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
              Debug.Log("Set Attacker coroutine started");
              yield return new WaitForSeconds(NewAttackerCheckInterval);
              SetAttacker();
          }
      }

      private IEnumerator CleanEnemyListCoroutine()
      {
          while (zoneActive)
          {
              Debug.Log("Enemy cleaner co routine started");
              yield return new WaitForSeconds(EnemyDeathCheckInterval);
              CleanEnemyList();
          }
      }

      public void SetAttacker()
      {
          if (zoneActive)
          {
              if (managedEnemies.Count > 0)
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
              foreach (GameObject enemy in managedEnemies)
              {
                  if (enemy.GetComponent<StateMachine>().IsDead || enemy.GetComponent<StateMachine>() == null)
                  {
                      if (enemy == currentAttacker)
                      {
                          SetAttacker();
                      }
                      OnAttackerDeath?.Invoke(enemy.transform.Find("PlayerCameraTarget").transform);
                      managedEnemies.Remove(enemy);
                  }
              }

              if (managedEnemies.Count == 1)
              {
                  OnLastEnemy?.Invoke(true, 1);
              }

              if (managedEnemies.Count == 0)
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
          if (ClosestEnemy() != currentAttacker || managedEnemies.Count == 1)
          {
              newAttacker = ClosestEnemy();
              currentAttacker = newAttacker;
              Debug.Log(currentAttacker.name);
          }
          else
          {
              newAttacker = managedEnemies[UnityEngine.Random.Range(0, managedEnemies.Count)];
              if(newAttacker == ClosestEnemy())
              {
                  SetNewAttacker();
              }
              else
              {
                  currentAttacker = newAttacker;
                  Debug.Log(currentAttacker.name);
              }
          }
      }

      public GameObject ClosestEnemy()
      {
          float[] distances = new float[3] { Mathf.Infinity, Mathf.Infinity, Mathf.Infinity };
          Transform[] closestTargets = new Transform[3] { null, null, null };

          // Iterate through all hits to find the closest colliders
          foreach (GameObject enemy in managedEnemies)
          {
              float distance = Vector3.Distance(transform.position, enemy.transform.position);

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
                      closestTargets[i] = enemy.transform;
                      break; // Exit the loop after updating the closest target
                  }
              }
          }

          // Assign closest, second closest, and third closest targets
          return closestTargets[0].gameObject;
      }*/
}
 
