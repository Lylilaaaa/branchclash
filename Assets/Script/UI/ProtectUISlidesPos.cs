using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProtectUISlidesPos : MonoBehaviour
{
    public int weaponType;
    private Vector3 _debuffStartLocalPos;
    private Vector3 _debuffMidPtLocalPos;
    //private Vector3 _debuffEndLocalPos;
    public TextMeshProUGUI debuffData;
    public TextMeshProUGUI protectData;
    //private Vector3 offset;
    //private Vector3 previousLocalPos;
    void Awake()
    {
        debuffData = transform.parent.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        protectData = transform.parent.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
        //previousLocalPos = transform.localPosition;
    }
    
    void Update()
    {
        //_debuffEndLocalPos = transform.parent.GetChild(2).localPosition;
        _debuffStartLocalPos = transform.parent.GetChild(1).localPosition;
        _debuffMidPtLocalPos = transform.parent.GetChild(0).localPosition;
        
        //tempRT.localScale = targetV3;
        transform.localPosition = _debuffMidPtLocalPos - _debuffStartLocalPos + _debuffMidPtLocalPos;
        
        //offset = transform.localPosition - previousLocalPos;
        //debuffData.gameObject.transform.localPosition += offset;
        //protectData.gameObject.transform.localPosition += offset;

        float persentage = 0f;
        if (transform.parent.parent.GetComponent<Slider>().value == 0)
        {
            persentage = 0f;
        }
        else if (transform.parent.parent.GetComponent<Slider>().maxValue == 0)
        {
            
        }
        else
        {
            persentage = transform.parent.parent.GetComponent<Slider>().value / transform.parent.parent.GetComponent<Slider>().maxValue;
        }
        
        transform.localScale = new Vector3( persentage, transform.localScale.y,transform.localScale.z);

        float tempFloat = CurNodeDataSummary._instance.debuffListData[weaponType];
        //print("tempFloat: "+tempFloat);
        if (tempFloat > 1)
        {
            tempFloat = 1;
        }
        debuffData.text = "-"+(tempFloat* 100).ToString() + "%";
        tempFloat = CurNodeDataSummary._instance.protecListData[weaponType];
        if (tempFloat > 1)
        {
            tempFloat = 1;
        }
        protectData.text = "+"+(tempFloat * 100).ToString() + "%";
        
        //previousLocalPos = transform.localPosition;
    }
}
