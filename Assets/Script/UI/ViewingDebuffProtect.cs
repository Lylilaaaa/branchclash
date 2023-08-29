using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;



public class ViewingDebuffProtect : MonoBehaviour
{
    public Slider debuffWood;
    public Slider debuffIron;
    public Slider debuffElec;
    public Slider proWood;
    public Slider proIron;
    public Slider proElec;

    public int[] _debufflist;

    private bool _counted = false;
    private string _at, _sp, _ra;

    public int[] weaponTotalBlood;
    public int[] weaponTotalProtect;

    public Image[] debuffSliderImage;
    // public int woodTotalBlood=0;
    // public int ironTotalBlood=0;
    // public int elecTotalBlood=0;
    
    // public int woodTotalProtect=0;
    // public int ironTotalProtect=0;
    // public int elecTotalProtect=0;

    public float fillSpeed = 0.01f;
    
    void Awake()
    {
        _debufflist = CurNodeDataSummary._instance.debuffList;
        _counted = false;
        weaponTotalBlood = new int[3];
        weaponTotalProtect = new int[3];

        //_wood[0].text = _debufflist[0]
    }

    // Update is called once per frame
    void Update()
    {
        if (CurNodeDataSummary._instance.dictionaryFinish == true && _counted ==false)
        {
            _counted = true;
            if (CurNodeDataSummary._instance.woodCount != null)
            {
                foreach (int grade in CurNodeDataSummary._instance.woodCount.Keys)
                {
                    (_at,_sp,_ra) = CurNodeDataSummary._instance.CheckAttackSpeedRange("wood", grade);
                    weaponTotalBlood[0] += CurNodeDataSummary._instance.woodCount[grade]*int.Parse(_at);
                }
            }
            if (CurNodeDataSummary._instance.ironCount != null)
            {
                foreach (int grade in CurNodeDataSummary._instance.ironCount.Keys)
                {
                    (_at,_sp,_ra) = CurNodeDataSummary._instance.CheckAttackSpeedRange("iron", CurNodeDataSummary._instance.ironCount[grade]);
                    weaponTotalBlood[1] += int.Parse(_at);
                }
            }
            if (CurNodeDataSummary._instance.elecCount != null)
            {
                foreach (int grade in CurNodeDataSummary._instance.elecCount.Keys)
                {
                    (_at,_sp,_ra) = CurNodeDataSummary._instance.CheckAttackSpeedRange("iron", CurNodeDataSummary._instance.elecCount[grade]);
                    weaponTotalBlood[2] += int.Parse(_at);
                }
            }
            if (CurNodeDataSummary._instance.wproCount != null)
            {
                foreach (int grade in CurNodeDataSummary._instance.wproCount.Keys)
                {
                    weaponTotalProtect[0] += CurNodeDataSummary._instance.wproCount[grade];
                }
            }
            if (CurNodeDataSummary._instance.iproCount != null)
            {
                foreach (int grade in CurNodeDataSummary._instance.iproCount.Keys)
                {
                    weaponTotalProtect[1] += CurNodeDataSummary._instance.iproCount[grade];
                }
            }
            if (CurNodeDataSummary._instance.eproCount != null)
            {
                foreach (int grade in CurNodeDataSummary._instance.eproCount.Keys)
                {
                    weaponTotalProtect[2] += CurNodeDataSummary._instance.eproCount[grade];
                }
            }
            
            debuffWood.maxValue = 1;
            debuffIron.maxValue = 1;
            debuffElec.maxValue = 1;
            
            debuffWood.minValue = debuffIron.minValue = debuffElec.minValue = 0;
            proWood.minValue = proIron.minValue = proElec.minValue = 0;
            proWood.maxValue = 1;
            proIron.maxValue = 1;
            proElec.maxValue = 1;
            _doWithSlides(0, debuffWood, proWood);
            _doWithSlides(1,debuffIron,proIron);
            _doWithSlides(2,debuffElec,proElec);
        }
    }

    private void _doWithSlides(int index, Slider debufSlider, Slider protectSlider)
    {
        if (weaponTotalBlood[index] != 0)
        {
            // print(debufSlider.gameObject.transform.name+": the blood is not zero!");
            // print(_debufflist[index].ToString()+weaponTotalBlood[index].ToString());
            //print(_debufflist[index] / (float)weaponTotalBlood[index]);
            float temp = Mathf.Round(((float)_debufflist[index] / (float)weaponTotalBlood[index])*100)/100;
            //print(temp);
            CurNodeDataSummary._instance.debuffListData[index] = temp;
            print(CurNodeDataSummary._instance.debuffListData[index] );
            if (_debufflist[index] != 0) //wood有血，有debuff
            {
                CurNodeDataSummary._instance.protecListData[index] =
                    Mathf.Round(weaponTotalProtect[index] / _debufflist[index] * 100) / 100;
                StartCoroutine(FillProgressBar(debufSlider,temp,index));
            }
            else //wood有血，没有debuff
            {
                if (weaponTotalProtect[index] != 0) //wood有血，没有debuff，有protect
                {
                    Color tempColor = debuffSliderImage[index].color;
                    tempColor.a = 0;
                    debufSlider.value = 1;
                    StartCoroutine(FillProgressBar2(protectSlider,1f));
                    CurNodeDataSummary._instance.protecListData[index] = 1;
                }
                else //wood有血，没有debuff，没有protect
                {
                    Color tempColor = debuffSliderImage[index].color;
                    tempColor.a = 1;
                    debufSlider.value = 0;
                    protectSlider.value = 0;
                    CurNodeDataSummary._instance.protecListData[index] = 0;
                }
            }
        }
        else //没有血
        {
            CurNodeDataSummary._instance.debuffListData[index] = 0;
            if (_debufflist[index] != 0) //没有血，有debuff
            {
                StartCoroutine(FillProgressBar(debufSlider,1,index));
            }
            else //没有血，没有debuff
            {
                if (weaponTotalProtect[index] != 0) //没有血，没有debuff，有protect
                {
                    Color tempColor = debuffSliderImage[index].color;
                    tempColor.a = 0;
                    debufSlider.value = 1;
                    StartCoroutine(FillProgressBar2(protectSlider,1f));
                    CurNodeDataSummary._instance.protecListData[index] = 1;
                }
                else
                {
                    Color tempColor = debuffSliderImage[index].color;
                    tempColor.a = 1;
                    debufSlider.value = 0;
                    protectSlider.value = 0;
                    CurNodeDataSummary._instance.protecListData[index] = 0;
                }
            }
   
        }
    }

    
    IEnumerator FillProgressBar(Slider slider,float targetValue,int index)
    {
        slider.value = 0;
        print(slider.gameObject.transform.name+" target value is: "+targetValue);
        while (slider.value < targetValue)
        {
            //print(slider.value);
            print(slider.gameObject.transform.name+" value is: "+slider.value);
            slider.value += fillSpeed * Time.deltaTime; // 增加Slider的值
            
            yield return new WaitForSeconds(0.01f);
        }
        
        Debug.Log("Progress bar filled!");
        if (index == 0)
        {
            StartCoroutine(FillProgressBar2(proWood,Mathf.Round(weaponTotalProtect[0]/_debufflist[0]* 100) / 100));
        }
        else if (index == 1)
        {
            StartCoroutine(FillProgressBar2(proIron,Mathf.Round(weaponTotalProtect[1]/_debufflist[1]* 100) / 100));
        }
        else if (index == 2)
        {
            StartCoroutine(FillProgressBar2(proElec,Mathf.Round(weaponTotalProtect[2]/_debufflist[2]* 100) / 100));
        }
        
    }
    IEnumerator FillProgressBar2(Slider slider,float targetValue)
    {
        slider.value = 0;
        
        if (targetValue >= slider.maxValue)
        {
            targetValue = slider.maxValue;
        }
        print(slider.gameObject.transform.name+" target value is: "+targetValue);
        while (slider.value < targetValue)
        {
            //print(slider.value);
            print(slider.gameObject.transform.name+" value is: "+slider.value);
            slider.value += fillSpeed * Time.deltaTime; // 增加Slider的值
            
            yield return new WaitForSeconds(0.01f);
        }
        
        Debug.Log("Progress bar filled222!");
    }


}
