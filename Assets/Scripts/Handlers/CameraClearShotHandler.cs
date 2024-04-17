using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraClearShotHandler : MonoBehaviour
{
    public LayerMask transparentLayer;
    // Update is called once per frame
    void Update()
    { 
        // Get the main camera
        Camera mainCamera = Camera.main;

        // Get the center of the screen in viewport coordinates (0.5, 0.5)
        Vector3 viewportCenter = new Vector3(0.5f, 0.5f, 0f);

        // Convert the viewport coordinates to a ray
        Ray ray = mainCamera.ViewportPointToRay(viewportCenter);

        // Declare a RaycastHit variable to store information about the hit
        RaycastHit hit;

        // Check if the ray hits any game object
        if (Physics.Raycast(ray, out hit, 20f,transparentLayer))
        {
            // Get the material of the object
            Material objectMaterial = hit.transform.GetComponent<MeshRenderer>().material;
            // Adjust the alpha value of the material's color
            Color color = objectMaterial.color;
            color.a = 0f;
            objectMaterial.color = color;
        }
        
    }
}
