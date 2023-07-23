using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class UIScrollCamLock : MonoBehaviour
{
    public bool onGameObj;
    
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            CameraController._instance.camLock = true;
            onGameObj = true;
        }
        else
        {
            CameraController._instance.camLock = false;
            onGameObj = false;
        }
    }
}
