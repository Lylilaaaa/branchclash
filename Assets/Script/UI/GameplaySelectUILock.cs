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
        //     // ���ָ�벻��UI������
        //     GlobalVar._instance.gamePlaySelect = false;
        // }
        // else
        // {
        //     // ���ָ����UI������
        //     GlobalVar._instance.gamePlaySelect = true;
        // }
    }
}
