using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocusPointHandler : MonoBehaviour
{
    void Update()
    {
        // Get the main camera
        Camera mainCamera = Camera.main;

        // Calculate the center of the screen in viewport coordinates
        Vector3 viewportCenter = new Vector3(0.5f, 0.5f, mainCamera.nearClipPlane);

        // Convert the viewport coordinates to world coordinates
        Vector3 worldCenter = mainCamera.ViewportToWorldPoint(viewportCenter);

        // Set the position of the transform to the calculated world center
        transform.position = worldCenter;
    }
}
