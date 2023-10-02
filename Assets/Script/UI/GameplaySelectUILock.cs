using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameplaySelectUILock : MonoBehaviour
{
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            GlobalVar._instance.gamePlaySelect = true;
        }
        else
        {
            GlobalVar._instance.gamePlaySelect = false;
        }

        // if (!EventSystem.current.IsPointerOverGameObject())
        // {
        //     GlobalVar._instance.gamePlaySelect = false;
        // }
        // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // RaycastHit hit;
        //
        // if (!Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("UI")))
        // {
        //     // 鼠标指针不在UI物体上
        //     GlobalVar._instance.gamePlaySelect = false;
        // }
        // else
        // {
        //     // 鼠标指针在UI物体上
        //     GlobalVar._instance.gamePlaySelect = true;
        // }
    }
}
