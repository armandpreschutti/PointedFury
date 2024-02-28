using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkourController : MonoBehaviour
{
  /*  [SerializeField] float climbSpeed = 5f;
    [SerializeField] float climbRaycastDistance = 1f;
    [SerializeField] Vector3 raycastOffset;
    [SerializeField] LayerMask climbLayerMask;
    [SerializeField] bool canClimb;

    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Perform the raycast
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, climbRaycastDistance))
        {
            // If the raycast hits something, draw a debug line from the object's position to the hit point
            Debug.DrawRay(transform.position + raycastOffset, transform.forward * hit.distance, Color.green);
            canClimb = true;
           
        }
        else
        {
            // If the raycast doesn't hit anything, draw a debug line to the maximum raycast distance
            Debug.DrawRay(transform.position + raycastOffset, transform.forward * climbRaycastDistance, Color.red);
            canClimb = false;
        }
    }*/
}
