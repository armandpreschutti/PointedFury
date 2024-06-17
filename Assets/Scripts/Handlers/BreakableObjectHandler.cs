using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObjectHandler : MonoBehaviour
{
    public Rigidbody[] ragdollRbs;
    
   // public GameObject debugObject;

    private void Awake()
    {
        ragdollRbs = GetComponentsInChildren<Rigidbody>();
    }
  /*  private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Breakable")
        {
            Debug.Log("Collision was detected correctly");

        }
        else
        {

            Debug.Log("Collisiong was detected inncorrectly");
        }
    }
*/
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Breaker")
        {
            foreach (Rigidbody rb in ragdollRbs)
            {
                rb.isKinematic = false;
                rb.AddForce((rb.transform.position - other.transform.position) * 5f, ForceMode.Force);
            }
        }
    }
}
