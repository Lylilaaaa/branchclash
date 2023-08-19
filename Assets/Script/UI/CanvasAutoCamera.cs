using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasAutoCamera : MonoBehaviour
{
    public Camera mainCamera;
    public float desiredDistance = 1.0f;

    private void Awake()
    {
        mainCamera = Camera.main;
        if (mainCamera != null)
        {
            SetCameraDistance(mainCamera, desiredDistance);
        }
    }

    private void SetCameraDistance(Camera camera, float distance)
    {
        Vector3 cameraPosition = camera.transform.position;
        cameraPosition.z = -distance; // 设置z坐标来控制距离
        camera.transform.position = cameraPosition;
    }
}
