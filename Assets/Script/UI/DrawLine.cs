using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    public Transform startPoint;  // 起始点的 Transform
    public Transform endPoint;    // 结束点的 Transform

    private LineRenderer lineRenderer;

    private void Start()
    {
        // 获取 LineRenderer 组件
        lineRenderer = GetComponent<LineRenderer>();
        startPoint = transform.GetChild(0).GetChild(0);
        endPoint = transform.GetChild(1).GetChild(0);

    }

    private void Update()
    {
        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(1, endPoint.position);
    }
}
