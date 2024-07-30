using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDetectionHandler : MonoBehaviour
{

    public float DestroyDelayTime = 5;

    private void OnTriggerEnter(Collider other)
    {
       
        if(other.GetComponent<StateMachine>() != null) 
        {
            StateMachine stateMachine = other.GetComponent<StateMachine>();
            stateMachine.CurrentTarget = stateMachine.transform.gameObject;
            stateMachine.HitType = "Light";
            stateMachine.HitID = 1;
        }
        if (other.GetComponent<HealthSystem>() != null)
        {

            HealthSystem healthSystem = other.GetComponent<HealthSystem>();
            healthSystem.enabled = true;
            healthSystem.TakeDamage(1000f, "Light");
        }
        SetGravityForAllChildren(other.gameObject, new Vector3(0, -5, 0));
        StartCoroutine(DestroyEntity(other.gameObject));
    }

    public IEnumerator DestroyEntity(GameObject entity)
    {
        yield return new WaitForSeconds(DestroyDelayTime);
        Destroy(entity);
    }

    public static void SetGravityForAllChildren(GameObject parent, Vector3 gravity)
    {
        // Get all Rigidbody components in the parent and its children
        Rigidbody[] rigidbodies = parent.GetComponentsInChildren<Rigidbody>();

        // Iterate through each Rigidbody using a for loop and set the gravity
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].drag = 10;
           // rigidbodies[i].AddForce(gravity, ForceMode.Acceleration);
        }
    }
}
