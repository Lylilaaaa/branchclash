using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    public Transform world_start_Pt;
    public Transform ui_end_Pt;    // 结束点的 Transform
    private Camera mainCamera;

    public Vector2 start_2D;
    public Vector2 end_2D;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        // 获取 LineRenderer 组件
        mainCamera = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();
        //world_start_Pt = transform.GetChild(0).GetChild(0);
        //ui_end_Pt = transform.GetChild(1).GetChild(0).GetComponent<RectTransform>();
    }

    private void Update()
    {
        // Vector3 startScreenPoint = mainCamera.WorldToScreenPoint(world_start_Pt.position);
        end_2D = RectTransformUtility.WorldToScreenPoint(mainCamera, ui_end_Pt.localPosition);
        //
        // startScreenPoint = new Vector3(startScreenPoint.x, startScreenPoint.y, 10f);
        Vector3 endScreenPoint = new Vector3(end_2D.x, end_2D.y,10f);

        lineRenderer.SetPosition(0, world_start_Pt.position);
        lineRenderer.SetPosition(1, ui_end_Pt.position);
    }
}
