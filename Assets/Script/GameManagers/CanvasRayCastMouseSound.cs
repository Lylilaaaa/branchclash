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
        // ������������λ��
        pointerEventData.position = Input.mousePosition;

        // ����һ���б����洢���߼��Ľ��
        var results = new List<RaycastResult>();

        // ִ�����߼�⣬������洢��results�б���
        raycaster.Raycast(pointerEventData, results);

        // ����Ƿ���UI Button�����λ����
        bool isMouseOverUIButton = false;

        foreach (var result in results)
        {
            //print(result);
            if (result.gameObject.GetComponent<Button>() != null)
            {
                // ����UI Button
                isMouseOverUIButton = true;
                break; // �������һ����ť���Ϳ����˳�ѭ��
            }
        }

        return isMouseOverUIButton;
    }
}
