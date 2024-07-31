using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBillboardHandler : MonoBehaviour
{
    Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }
   /* // Update is called once per frame
    void Update()
    {
        transform.rotation = _camera.transform.rotation;
        *//* transform.rotation = Quaternion.LookRotation(transform.position - _camera.transform.position);*//*
    }*/
}
