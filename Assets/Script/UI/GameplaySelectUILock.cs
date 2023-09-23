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
    }
}
