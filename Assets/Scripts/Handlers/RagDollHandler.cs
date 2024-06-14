using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagDollHandler : MonoBehaviour
{
    public Rigidbody[] ragdollRbs;
    public Collider[] colliders;
    public Rigidbody baseRb;
    public Animator anim;
    public CharacterController controller;
    public StateMachine stateMachine;

    private void Awake()
    {
        ragdollRbs = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        stateMachine = GetComponent<StateMachine>();    
        DisableRagDoll();
    }
    private void OnEnable()
    {
        stateMachine.OnEnableRagdoll += EnableRagDoll;
    }
    private void OnDisable()
    {
        stateMachine.OnEnableRagdoll -= EnableRagDoll;
    }


    // Update is called once per frame
    void Update()
    {
       /* if (baseRb.velocity.magnitude < .01)
        {
            foreach (Rigidbody rb in ragdollRbs)
            {
                rb.isKinematic = true;
            }
        }*/
    }

    public void DisableRagDoll()
    {
        foreach(Rigidbody rb in ragdollRbs)
        {
            rb.isKinematic = true;
        }
        anim.enabled = true;
        controller.enabled = true;
        stateMachine.enabled = true;
    }

    public void EnableRagDoll(Vector3 direction, float force)
    {
        
        anim.enabled = false;
        controller.enabled = false;
        foreach (Rigidbody rb in ragdollRbs)
        {
            rb.isKinematic = false;
        }
        // Normalize the direction to ensure it's a unit vector
        //Vector3 normalizedDirection = direction.normalized;
        Vector3 normalizedDirection = -stateMachine.transform.forward.normalized;

        // Calculate the force vector
        Vector3 forceVector = normalizedDirection * force;

        // Apply the force to the Rigidbody
        baseRb.AddForce(forceVector, ForceMode.Impulse);
        stateMachine.enabled = false;
        /*  baseRb.AddForce(Vector3.forward * 1000f, ForceMode.Impulse); */
    }
}
