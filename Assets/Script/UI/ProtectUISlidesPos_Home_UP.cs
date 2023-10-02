using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProtectUISlidesPos_Home_UP : MonoBehaviour
{
    private Vector3 _debuffStartLocalPos;
    private Vector3 _debuffMidPtLocalPos;
    
    // Update is called once per frame
    void Update()
    {
        _debuffStartLocalPos = transform.parent.GetChild(1).localPosition;
        _debuffMidPtLocalPos = transform.parent.GetChild(0).localPosition;

        //tempRT.localScale = targetV3;
        transform.localPosition = _debuffMidPtLocalPos - _debuffStartLocalPos + _debuffMidPtLocalPos;
        
        float persentage = 0f;
        if (transform.parent.parent.GetComponent<Slider>().maxValue == 0)
        {
            persentage = 0f;
        }
        else
        {
            persentage = transform.parent.parent.GetComponent<Slider>().value /
                                       transform.parent.parent.GetComponent<Slider>().maxValue;
        }
        transform.localScale = new Vector3( persentage, transform.localScale.y,transform.localScale.z);
    }
}
