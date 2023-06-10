using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{    public float zoomSpeed = 5f;   // 相机缩放速度
    public float moveSpeed = 10f;  // 相机移动速度
    public float rotationSpeed = 5f; // 相机旋转速度

    private float zoomAmount = 0f;
    private float lastScrollWheel = 0f;

    private void Update()
    {
        // 鼠标滚轮放大缩小
        float scrollWheel = Input.GetAxisRaw("Mouse ScrollWheel");
        float deltaScrollWheel = scrollWheel - lastScrollWheel;
        if (scrollWheel != 0f)
        {
            zoomAmount -= deltaScrollWheel * zoomSpeed;
            zoomAmount = Mathf.Clamp(zoomAmount, -10f, 10f);
            transform.position = new Vector3(transform.position.x, transform.position.y + zoomAmount, transform.position.z);

            lastScrollWheel = scrollWheel;
        }
        else
        {
            // 当滚轮停止滚动时停止缩放
            zoomAmount = 0;
            zoomAmount = Mathf.Clamp(zoomAmount, -10f, 10f);
            transform.position = new Vector3(transform.position.x, transform.position.y + zoomAmount, transform.position.z);
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
