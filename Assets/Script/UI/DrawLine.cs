using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    public enum Usage { Screen_World = 0,World_World};
    public Usage m_Usage = Usage.Screen_World;
    public Transform world_start_Pt;
    public Transform ui_end_Pt;    // 结束点的 Transform
    public Transform world_end_Pt;
    private Camera mainCamera;

    public Vector2 start_2D;
    public Vector2 end_2D;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        if (m_Usage == Usage.Screen_World)
        {
            mainCamera = Camera.main;
            lineRenderer = GetComponent<LineRenderer>();
        }
        else if (m_Usage == Usage.World_World)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
        // 获取 LineRenderer 组件
        //world_start_Pt = transform.GetChild(0).GetChild(0);
        //ui_end_Pt = transform.GetChild(1).GetChild(0).GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (m_Usage == Usage.Screen_World)
        {
            // Vector3 startScreenPoint = mainCamera.WorldToScreenPoint(world_start_Pt.position);
            end_2D = RectTransformUtility.WorldToScreenPoint(mainCamera, ui_end_Pt.localPosition);
            //
            // startScreenPoint = new Vector3(startScreenPoint.x, startScreenPoint.y, 10f);
            //Vector3 endScreenPoint = new Vector3(end_2D.x, end_2D.y,10f);

            lineRenderer.SetPosition(0, world_start_Pt.position);
            lineRenderer.SetPosition(1, ui_end_Pt.position);
        }
        else if (m_Usage == Usage.World_World)
        {
            lineRenderer.SetPosition(0, world_start_Pt.position);
            lineRenderer.SetPosition(1, world_end_Pt.position);
        }

    }
}
