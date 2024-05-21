using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTargetHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(GameObject.Find("Player").transform.Find("PlayerCameraTarget").transform != null)
        {
            GetComponent<CinemachineVirtualCamera>().m_Follow = GameObject.Find("Player").transform.Find("PlayerCameraTarget").transform;
        }
        else
        {
            Debug.LogError("Could not find Player Camera Target");
        }
      
    }

  
}
