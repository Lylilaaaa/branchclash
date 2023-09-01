using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CanvasRayCastMouseSound : MonoBehaviour
{
    //private bool _isUI;
    public GraphicRaycaster raycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;
    // Start is called before the first frame update
    void Awake()
    {
        eventSystem = EventSystem.current;
        raycaster = GetComponent<GraphicRaycaster>();
        pointerEventData = new PointerEventData(eventSystem);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !_checkButtonUI())
        {
            SoundManager._instance.PlayEffectSound(SoundManager._instance.mouseDownSound);
        }
        else if(Input.GetMouseButtonDown(0) && _checkButtonUI())
        {
            SoundManager._instance.PlayEffectSound(SoundManager._instance.uiMouseDownSound);
        }
    }
    private bool _checkButtonUI()
    {
        print("hi here!");
        // 检测鼠标悬浮的位置
        pointerEventData.position = Input.mousePosition;

        // 创建一个列表来存储射线检测的结果
        var results = new List<RaycastResult>();

        // 执行射线检测，将结果存储在results列表中
        raycaster.Raycast(pointerEventData, results);

        // 检查是否有UI Button在鼠标位置下
        bool isMouseOverUIButton = false;

        foreach (var result in results)
        {
            //print(result);
            if (result.gameObject.GetComponent<Button>() != null)
            {
                // 发现UI Button
                isMouseOverUIButton = true;
                break; // 如果发现一个按钮，就可以退出循环
            }
        }

        return isMouseOverUIButton;
    }
}
