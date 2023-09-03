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
            Camera.main.gameObject.GetComponent<CameraController>().camLock = true;
            onGameObj = true;
        }
        else
        {
            Camera.main.gameObject.GetComponent<CameraController>().camLock = false;
            onGameObj = false;
        }
    }
}
