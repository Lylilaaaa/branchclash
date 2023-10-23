using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProtectUISlidesPos_Home : MonoBehaviour
{
    public int weaponType;
    private Vector3 _debuffStartLocalPos;
    private Vector3 _debuffMidPtLocalPos;
    // private TextMeshProUGUI debuffData_Real;
    // private TextMeshProUGUI debuffData;
    // private TextMeshProUGUI protectData;
    public enum debuffSlidesType
    {
        Current,
        Major
    }
    public debuffSlidesType thisSlideType;

    void Awake()
    {
        //debuffData = transform.parent.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        //protectData = transform.parent.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        //debuffData_Real = transform.parent.GetChild(4).GetComponent<TextMeshProUGUI>();
        if (thisSlideType == debuffSlidesType.Current)
        {
            //debuffData_Real.text = "-("+CurNodeDataSummary._instance.curDebuffList[weaponType].ToString()+")";
        }
        else
        {
            //debuffData_Real.text = "-("+CurNodeDataSummary._instance.majorDebuffList[weaponType].ToString()+")";
        }
        
        
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
        float tempFloat;
        if (thisSlideType == debuffSlidesType.Current)
        {
            tempFloat = CurNodeDataSummary._instance.debuffListData[weaponType];
        }
        else
        {
            tempFloat = CurNodeDataSummary._instance.majorDebuffListData[weaponType];
        }
        //print("tempFloat: "+tempFloat);
        if (tempFloat > 1)
        {
            tempFloat = 1;
        }
        //debuffData.text = "-"+(tempFloat* 100).ToString() + "%";
        //tempFloat = CurNodeDataSummary._instance.protecListData[weaponType];
        if (thisSlideType == debuffSlidesType.Current)
        {
            tempFloat = CurNodeDataSummary._instance.protecListData[weaponType];
        }
        else
        {
            tempFloat = CurNodeDataSummary._instance.majorProtecListData[weaponType];
        }
        if (tempFloat > 1)
        {
            tempFloat = 1;
        }
        //protectData.text = "+"+(tempFloat * 100).ToString() + "%";
    }
}
