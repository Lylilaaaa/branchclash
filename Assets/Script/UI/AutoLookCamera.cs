using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoLookCamera : MonoBehaviour
{
    public Transform mainCameraTrans;

    private void Start()
    {
        mainCameraTrans = Camera.main.transform;
    }

    private void LateUpdate()
    {
        if (mainCameraTrans != null)
        {
            transform.LookAt(transform.position + mainCameraTrans.rotation*Vector3.forward,mainCameraTrans.rotation*Vector3.up);
        }
    }
}
