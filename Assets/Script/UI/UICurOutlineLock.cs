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
            Camera.main.gameObject.GetComponent<CameraController>().camLock = true;
            GlobalVar._instance.global_OL = false;
        }
        else
        {
            Camera.main.gameObject.GetComponent<CameraController>().camLock = false;
            GlobalVar._instance.global_OL = true;
        }
    }
}
