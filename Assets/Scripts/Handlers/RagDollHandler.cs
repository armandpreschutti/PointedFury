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
    public Rigidbody HeadArea;
    public Rigidbody BodyArea;
    public float LightAttackImpactForce = 100;
    public float HeavyAttackImpactForce = 200;
    public float ParryImpactForce = 300;

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
/*        if (baseRb.velocity.magnitude <= 0)
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

    public void EnableRagDoll(Vector3 direction, /*float force,*/ string attackType, int attackID)
    {
        //Debug.LogWarning($"Player used a {attackType} {attackID} attack");

        anim.enabled = false;
        controller.enabled = false;
        for (int i = ragdollRbs.Length - 1; i >= 0; i--)
        {
            Rigidbody rb = ragdollRbs[i];
            rb.isKinematic = false;
            
        }
        // Normalize the direction to ensure it's a unit vector
        //Vector3 normalizedDirection = direction.normalized;
        Vector3 normalizedDirection = ImpactDirection(attackType, attackID);

        // Calculate the force vector
        Vector3 forceVector = normalizedDirection * ImpactForce(attackType);

        // Apply the force to the Rigidbody
        ImpactArea(attackType, attackID).AddForce(forceVector, ForceMode.Impulse);
        stateMachine.enabled = false;
    }

    public Vector3 ImpactDirection(string attackType, int attackID)
    {
        if(attackType == "Light")
        {
            Vector3 returnDirection = Vector3.zero;
            //return Vector3.zero;
            switch(attackID)
            {
                case 1:
                    returnDirection = -stateMachine.transform.forward.normalized;
                    break;
                case 2:
                    returnDirection = stateMachine.transform.right.normalized;
                    break;
                case 3:
                    returnDirection = -stateMachine.transform.right.normalized;
                    break;
                case 4:
                    returnDirection = -stateMachine.transform.forward.normalized;
                    break;
                case 5:
                    returnDirection = -stateMachine.transform.forward.normalized;
                    break;
                default:
                     break;
            }
            return returnDirection;
        }
        else if(attackType == "Heavy")
        {
            Vector3 returnDirection = Vector3.zero;
            switch (attackID)
            {
                case 1:
                    returnDirection = (-stateMachine.transform.forward + (stateMachine.transform.up / 4)).normalized;
                    break;
                case 2:
                    returnDirection = (-stateMachine.transform.forward + (stateMachine.transform.up / 4)).normalized;
                    break;
                case 3:
                    returnDirection = (-stateMachine.transform.right + -stateMachine.transform.forward + (stateMachine.transform.up / 4)).normalized;

                    break;
                case 4:
                    returnDirection = (-stateMachine.transform.forward + (stateMachine.transform.up / 4)).normalized;
                    break;
                case 5:
                    returnDirection = (stateMachine.transform.right + -stateMachine.transform.forward + (stateMachine.transform.up / 4)).normalized;

                    break;
                default:
                    break;
            }
            return returnDirection;
        }
        else if(attackType == "Parry")
        {
            Vector3 returnDirection = Vector3.zero;
            switch (attackID)
            {
                case 1:
                    returnDirection = (-stateMachine.transform.forward + (stateMachine.transform.up / 4)).normalized;
                    break;
                case 2:
                    returnDirection = stateMachine.transform.right.normalized;
                    break;
                case 3:
                    returnDirection = (-stateMachine.transform.forward + stateMachine.transform.up).normalized; 
                    break;
                case 4:
                    returnDirection = (-stateMachine.transform.forward + (stateMachine.transform.up /4)).normalized;
                    break;
                default:
                    break;
            }
            return returnDirection;
        }
        else
        {
            return -stateMachine.transform.forward.normalized;
        }
    }

    public Rigidbody ImpactArea(string attackType, int attackID)
    {
        if (attackType == "Light")
        {
            Rigidbody impactedArea = null;
            //return Vector3.zero;
            switch (attackID)
            {
                case 1:
                    impactedArea = HeadArea;
                    break;
                case 2:
                    impactedArea = BodyArea;
                    break;
                case 3:
                    impactedArea = BodyArea;
                    break;
                case 4:
                    impactedArea = HeadArea;
                    break;
                case 5:
                    impactedArea = HeadArea;
                    break;
                default:
                    break;
            }
            return impactedArea;
        }
        else if (attackType == "Heavy")
        {
            Rigidbody impactedArea = null;
            switch (attackID)
            {
                case 1:
                    impactedArea = HeadArea;
                    break;
                case 2:
                    impactedArea = BodyArea;
                    break;
                case 3:
                    impactedArea = BodyArea;
                    break;
                case 4:
                    impactedArea = HeadArea;
                    break;
                case 5:
                    impactedArea = HeadArea;
                    break;
                default:
                    break;
            }
            return impactedArea;
        }
        else if (attackType == "Parry")
        {
            Rigidbody impactedArea = null;
            switch (attackID)
            {
                case 1:
                    impactedArea = BodyArea;
                    break;
                case 2:
                    impactedArea = HeadArea;
                    break;
                case 3:
                    impactedArea = HeadArea;
                    break;
                case 4:
                    impactedArea = HeadArea;
                    break;
                default:
                    break;
            }
            return impactedArea;
        }
        else
        {
            return null;
        }
    }
    public float ImpactForce(string attackType)
    {
        if (attackType == "Light")
        {
            return LightAttackImpactForce;
        }
        else if (attackType == "Heavy")
        {
            return HeavyAttackImpactForce;
        }
        else if (attackType == "Parry")
        {
            return ParryImpactForce;
        }
        else
        {
            return 0f;
        }
    }
}
