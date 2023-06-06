using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    public float rotationSpeed = 50f;   // 旋转速度
    public bool rotateX;
    public bool rotateY;
    public bool rotateZ;

    private Vector3 rotationAxes = new Vector3(0f, 0f, 0f);  // 旋转轴

    private void Start()
    {
        if (rotateX == true)
        {
            rotationAxes += new Vector3(1f, 0f, 0f);
        }
        else if (rotateY == true)
        {
            rotationAxes += new Vector3(0f, 1f, 0f);
        }
        else if (rotateZ == true)
        {
            rotationAxes += new Vector3(0f, 0f, 1f);
        }
    }

    private void Update()
    {
        // 根据旋转轴和旋转速度进行旋转
        transform.Rotate(rotationAxes * rotationSpeed * Time.deltaTime);
    }
}
