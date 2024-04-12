using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagementSystem : MonoBehaviour
{
    public List<GameObject> managedEnemies = new List<GameObject>();
    public GameObject currentAttacker;
    public bool zoneActive;

    private void Update()
    {
        CleanEnemyList();
    }
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
            zoneActive = true;
            foreach (GameObject enemy in managedEnemies)
            {
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
            zoneActive = false;
            foreach (GameObject enemy in managedEnemies)
            {
                enemy.GetComponent<AIBrain>().isActivated = false;
                enemy.GetComponent<StateMachine>().CurrentTarget = null;
                enemy.GetComponent<StateMachine>().EnemiesNearby.Remove(other.gameObject);
                enemy.GetComponent<StateMachine>().IsFighting = false;
            }
        }
       
    }
    private void Start()
    {
        StartCoroutine(PickAttacker());
    }

    public IEnumerator PickAttacker()
    {
        yield return new WaitForSeconds(5f);
        if (zoneActive)
        {
            foreach (GameObject enemy in managedEnemies)
            {
                enemy.GetComponent<AIBrain>().isAttacker = false;
                enemy.GetComponent<AIBrain>().isWatcher = true;
            }
            currentAttacker = managedEnemies[Random.Range(0, managedEnemies.Count)];
            currentAttacker.GetComponent<AIBrain>().isAttacker = true;
            currentAttacker.GetComponent<AIBrain>().isWatcher = false;
            foreach (GameObject enemy in managedEnemies)
            {
                enemy.GetComponent<AIBrain>().ChangeEnemyMaterial();
            }
        }        

        StartCoroutine(PickAttacker());
    }

    public void CleanEnemyList()
    {
        if (zoneActive)
        {
            foreach (GameObject enemy in managedEnemies)
            {
                if (enemy.GetComponent<StateMachine>().IsDead)
                {
                    managedEnemies.Remove(enemy);
                }
            }
        }        
    }
}
