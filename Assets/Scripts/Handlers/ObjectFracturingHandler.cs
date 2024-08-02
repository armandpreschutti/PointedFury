using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class ObjectFracturingHandler : MonoBehaviour
{
    public Fracture fracture;
    GameObject parent;
    Rigidbody[] children;
    private void Awake()
    {
        fracture= GetComponent<Fracture>();
    }


    public void SetDebrisLayer()
    {
        parent = GameObject.Find($"{gameObject.name}Fragments");
        children = parent.GetComponentsInChildren<Rigidbody>();
        if(children != null)
        {
            for (int i = 0; i < children.Length; i++)
            {
                Rigidbody fragment = children[i];
                fragment.gameObject.layer = LayerMask.NameToLayer("Debris");
                fragment.gameObject.tag = "Untagged";
            }
        }
    }

}
