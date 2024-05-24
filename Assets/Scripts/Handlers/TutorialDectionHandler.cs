using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDetectionHandler : MonoBehaviour
{
    public Action<bool> OnInitiateTutorialUI;
    public GameObject breakableObject;
 
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            OnInitiateTutorialUI?.Invoke(true);
        }
    }

    private void Update()
    {
        if(breakableObject == null)
        {
            OnInitiateTutorialUI?.Invoke(false);
            Destroy(this);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            OnInitiateTutorialUI?.Invoke(false);
        }
    }
}
