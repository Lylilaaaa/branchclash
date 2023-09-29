using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //public static CameraController _instance;
    public float zoomSpeed = 5f; // 相机缩放速度
    public float moveSpeed = 10f; // 相机移动速度
    public float CursorZoomSpeed = 3f;
    public float rotationSpeed = 5f; // 相机旋转速度
    public bool canMove;
    public bool camLock = false;
    public Collider boundaryCollider;
    
    private float zoomAmount = 0f;
    private float lastScrollWheel = 0f;
    
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
                Vector3 zoomVector = -transform.forward * zoomAmount;
                Vector3 newPosition = transform.localPosition + zoomVector;

                // 检查新位置是否与碰撞体相交
                if (IsCollidingWithBoundary(newPosition))
                {
                    transform.localPosition = newPosition;
                }

                lastScrollWheel = scrollWheel;
                
                // transform.localPosition = transform.localPosition - transform.forward * zoomAmount;
                //
                // lastScrollWheel = scrollWheel;
            }
            else
            {
                // 当滚轮停止滚动时停止缩放
                zoomAmount = 0;
                zoomAmount = Mathf.Clamp(zoomAmount, -10f, 10f);
                Vector3 zoomVector = -transform.forward * zoomAmount;
                Vector3 newPosition = transform.localPosition + zoomVector;

                // 检查新位置是否与碰撞体相交
                if (IsCollidingWithBoundary(newPosition))
                {
                    transform.localPosition = newPosition;
                }

                lastScrollWheel = 0f;
                
                // transform.localPosition = transform.localPosition - transform.forward * zoomAmount;
                // lastScrollWheel = 0f;
            }
    
            // 按住中键移动视角
            if (Input.GetMouseButton(2))
            {
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");
                Vector3 moveDirection = new Vector3(-mouseX, -mouseY, 0f) * moveSpeed * Time.deltaTime;
                Vector3 newPosition = transform.localPosition + moveDirection;
                
                //Vector3 moveDirection = new Vector3(-mouseX, -mouseY, 0f) * moveSpeed * Time.deltaTime;
                if (IsCollidingWithBoundary(newPosition))
                {
                    transform.Translate(moveDirection, Space.Self);
                }
                
                //transform.Translate(moveDirection, Space.Self);
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
            targetNode = targetNode.GetComponent<CursorOutlinesDown>().cameraPos;
        }
        else
        {
            targetNode = targetNode.GetComponent<CursorOutlines>().cameraPos;
        }

        Vector3 targetPosition = targetNode.position;
        Quaternion targetRotation = targetNode.rotation;
        //transform.rotation = targetRotation;
        //transform.position = targetPosition;

        // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime*10);
        // transform.position = Vector3.Lerp(transform.position, targetPosition, CursorZoomSpeed * Time.deltaTime*10);
        StartCoroutine(MoveCamera(targetPosition, targetRotation));
    }

    private System.Collections.IEnumerator MoveCamera(Vector3 targetPosition, Quaternion targetRotation)
    {
        float timeCount = 0f;
        //
        while ( Quaternion.Angle(transform.rotation, targetRotation) > 0.01f ||Vector3.Distance(transform.position, targetPosition) > 0.1f || timeCount < 1f)
        {
            if (canMove)
            {
                timeCount += Time.deltaTime;
                //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lerpSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime*10);
                //transform.rotation = targetRotation;
                transform.position = Vector3.Lerp(transform.position, targetPosition, CursorZoomSpeed * Time.deltaTime*10);
            }
            yield return null;
        }
        yield return null;
    }
    private bool IsCollidingWithBoundary(Vector3 position)
    {
        if (boundaryCollider == null)
        {
            return false;
        }

        Bounds bounds = boundaryCollider.bounds;
        Vector3 min = bounds.min;
        Vector3 max = bounds.max;
        //print("min:"+min);
        //print("max:"+max);
        //print(position);
        //print(position.x >= min.x && position.x <= max.x &&
              // position.y >= min.y && position.y <= max.y &&
              // position.z >= min.z && position.z <= max.z);
        return position.x >= min.x && position.x <= max.x &&
               position.y >= min.y && position.y <= max.y &&
               position.z >= min.z && position.z <= max.z;
    }

}
