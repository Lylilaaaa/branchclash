using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController _instance;
    public float zoomSpeed = 5f; // 相机缩放速度
    public float moveSpeed = 10f; // 相机移动速度
    public float CursorZoomSpeed = 3f;
    public float rotationSpeed = 5f; // 相机旋转速度
    public float approachDistance = 10f;
    public bool canMove;
    public bool camLock = false;
    
    private float zoomAmount = 0f;
    private float lastScrollWheel = 0f;


    private void Awake()
    {
        _instance = this;
    }

    private void Update()
    {
        if (camLock == false)
        {
            // 鼠标滚轮放大缩小
            float scrollWheel = Input.GetAxisRaw("Mouse ScrollWheel");
            float deltaScrollWheel = scrollWheel - lastScrollWheel;
            if (scrollWheel != 0f)
            {
                zoomAmount -= deltaScrollWheel * zoomSpeed;
                zoomAmount = Mathf.Clamp(zoomAmount, -10f, 10f);
                transform.localPosition = transform.localPosition - transform.forward * zoomAmount;
    
                lastScrollWheel = scrollWheel;
            }
            else
            {
                // 当滚轮停止滚动时停止缩放
                zoomAmount = 0;
                zoomAmount = Mathf.Clamp(zoomAmount, -10f, 10f);
                transform.localPosition = transform.localPosition - transform.forward * zoomAmount;
                lastScrollWheel = 0f;
            }
    
            // 按住中键移动视角
            if (Input.GetMouseButton(2))
            {
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");
                Vector3 moveDirection = new Vector3(-mouseX, -mouseY, 0f) * moveSpeed * Time.deltaTime;
                transform.Translate(moveDirection, Space.Self);
            }
    
            // 按住右键转动视角
            if (Input.GetMouseButton(1))
            {
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");
                Vector3 rotation = new Vector3(-mouseY, mouseX, 0f) * rotationSpeed;
                transform.eulerAngles += rotation;
            }
        }

    }

    public void LookUpNode(Transform targetNode)
    {
        //print(targetNode.position);
        if (targetNode.name.Substring(targetNode.name.Length-3,3)=="red")
        {
            targetNode = targetNode.GetComponent<CursorOutlinesDown>().previewLevelInfoPenal.transform.GetChild(0).GetChild(0);
        }
        else
        {
            targetNode = targetNode.GetComponent<CursorOutlines>().previewLevelInfoPenal.transform.GetChild(0).GetChild(0);
        }
        Vector3 targetPosition = targetNode.position - transform.forward * approachDistance;
        Quaternion targetRotation = Quaternion.LookRotation(targetNode.position - transform.position);

        StartCoroutine(MoveCamera(targetPosition, targetRotation));
    }

    private System.Collections.IEnumerator MoveCamera(Vector3 targetPosition, Quaternion targetRotation)
    {
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f || Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            if (canMove)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime*10);
                transform.position = Vector3.Lerp(transform.position, targetPosition, CursorZoomSpeed * Time.deltaTime*10);
            }
            yield return null;
        }
        yield return null;
    }
}
