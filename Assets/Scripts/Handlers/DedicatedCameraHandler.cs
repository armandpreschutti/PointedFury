using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DedicatedCameraHandler : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _dedicatedCamera;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _dedicatedCamera.enabled = true;
            other.GetComponent<PlayerCameraController>().enabled = false;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _dedicatedCamera.enabled = false;
            other.GetComponent<PlayerCameraController>().enabled = true;
        }
    }
}
