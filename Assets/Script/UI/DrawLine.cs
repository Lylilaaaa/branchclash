using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    public Transform world_start_Pt;
    public Transform endPoint;    // 结束点的 Transform
    private Camera mainCamera;

    private LineRenderer lineRenderer;

    private void Start()
    {
        // 获取 LineRenderer 组件
        mainCamera = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();
        world_start_Pt = transform.GetChild(0).GetChild(0);
        endPoint = transform.GetChild(1).GetChild(0);
    }

    private void Update()
    {
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(world_start_Pt.position);
        lineRenderer.SetPosition(0, screenPosition);
        lineRenderer.SetPosition(1, endPoint.position);
    }
}
