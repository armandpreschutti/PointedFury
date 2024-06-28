using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFracturingHandler : MonoBehaviour
{
    GameObject parent;
    Rigidbody[] children;

    public void SetDebrisLayer()
    {
        parent = GameObject.Find($"{gameObject.name}Fragments");
        children = parent.GetComponentsInChildren<Rigidbody>();
        for (int i = 0; i < children.Length; i++)
        {
            Rigidbody fragment = children[i];
            fragment.gameObject.layer = LayerMask.NameToLayer("Debris");
            fragment.gameObject.tag = "Untagged";
        }
        Destroy(this.gameObject);

    }
   
}
