using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YourNodeContent : MonoBehaviour
{
    private int childCount;
    private RectTransform _rectTransform;
    private bool has;
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        has = false;
    }

    private void LateUpdate()
    {
        //print(childCount);
        childCount = transform.childCount;
        if (has == false && childCount!=0)
        {
            
            Vector2 sizeDelta = _rectTransform.sizeDelta;
    
            float newHeight = 20f*childCount; 
            float newWidth = sizeDelta.x; 
            sizeDelta.y = newHeight;
            sizeDelta.x = newWidth;
            
            _rectTransform.sizeDelta = sizeDelta;
            has = true;
        }

        positionFix();
    }

    private void positionFix()
    {
        int cur_child = 0;
        Transform cur_childTrans;
        while (cur_child < childCount)
        {
            cur_childTrans = transform.GetChild(cur_child);
            cur_childTrans.transform.localPosition = new Vector3(0, -10-cur_child*20f, 0);
            cur_child += 1;
        }
    }
}
