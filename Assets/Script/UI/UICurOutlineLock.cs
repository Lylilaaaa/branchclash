using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICurOutlineLock : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            CameraController._instance.camLock = true;
            GlobalVar._instance.global_OL = false;
        }
        else
        {
            CameraController._instance.camLock = false;
            GlobalVar._instance.global_OL = true;
        }
    }
}
