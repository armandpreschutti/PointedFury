using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUpHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<HealthSystem>() != null)
        {
            other.GetComponent<HealthSystem>().RefillHealth();
            Destroy(gameObject);
        }
    }
}
