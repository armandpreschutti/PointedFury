using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDetectionHandler : MonoBehaviour
{

    public float DestroyDelayTime = 5;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<HealthSystem> () != null)
        {
            StateMachine stateMachine = other.GetComponent<StateMachine>();
            HealthSystem healthSystem = other.GetComponent<HealthSystem> ();
            stateMachine.CurrentTarget = stateMachine.transform.gameObject;
            stateMachine.HitType = "Light";
            stateMachine.HitID = 1;
            other.GetComponent<HealthSystem>().enabled = true;
            other.GetComponent<HealthSystem>().TakeDamage(1000f, "Light");
            StartCoroutine(DestroyEntity(other.gameObject));
        }
    }

    public IEnumerator DestroyEntity(GameObject entity)
    {
        yield return new WaitForSeconds(DestroyDelayTime);
        Destroy(entity);
    }
}
