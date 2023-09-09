using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ViewingDebuffProtect_Home : MonoBehaviour
{
    public Slider debuffWood;
    public Slider debuffIron;
    public Slider debuffElec;
    public Slider proWood;
    public Slider proIron;
    public Slider proElec;
    public CursorOutlinesDown cursorOLDown;
    public string[][] _mapStruct;
    
    public enum debuffSlidesType
    {
        Current,
        Major
    }
    public debuffSlidesType thisSlideType;
    

    public int[] _debufflist;

    public bool _counted = false;
    private string _at, _sp, _ra;

    public int[] weaponTotalBlood;
    public int[] weaponTotalProtect;

    public Image[] debuffSliderImage;

    public float fillSpeedDebuff = 1f;
    public float fillSpeedProtect = 1f;   
    public Dictionary<int,int> woodCount; //grade, count
    public Dictionary<int,int> ironCount;
    public Dictionary<int,int> elecCount;
    public Dictionary<int,int> wproCount;
    public Dictionary<int,int> iproCount;
    public Dictionary<int,int> eproCount; 

    
    void Start()
    {

        _counted = false;
        weaponTotalBlood = new int[3];
        weaponTotalProtect = new int[3];

        //_wood[0].text = _debufflist[0]
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalVar._instance.finalNodePrepared&&cursorOLDown.correspondMajorNodeData != null && cursorOLDown.majorDownNodeData!= null)
        {
            if (thisSlideType == debuffSlidesType.Current)
            {
                _debufflist = cursorOLDown.thisDownNodeData.debuffData;
            }
            else
            {
                _debufflist = cursorOLDown.majorDownNodeData.debuffData;
            }
        }

        if (!cursorOLDown._canDisappear && !_counted && cursorOLDown.correspondMajorNodeData != null && cursorOLDown.majorDownNodeData!= null ) 
        {
            _counted = true;
            if (thisSlideType == debuffSlidesType.Current)
            {
                CurNodeDataSummary._instance.curDebuffList = _debufflist; 
                Debug.Log("Cur curDebuffList is: "+CurNodeDataSummary._instance.curDebuffList[0]+","+CurNodeDataSummary._instance.curDebuffList[1]+","+CurNodeDataSummary._instance.curDebuffList[2]);
            }
            else
            {
                CurNodeDataSummary._instance.majorDebuffList = _debufflist; 
                Debug.Log("MajorDebuffList is: "+CurNodeDataSummary._instance.majorDebuffList[0]+","+CurNodeDataSummary._instance.majorDebuffList[1]+","+CurNodeDataSummary._instance.majorDebuffList[2]);
            }
            _getMapmapList(cursorOLDown.correspondMajorNodeData.mapStructure);
            (woodCount,ironCount,elecCount,wproCount,iproCount,eproCount) =  CurNodeDataSummary._instance._checkTypeIndex(_mapStruct);
            int[] towerCount = new int[3];
            int[] protectCount = new int[3];
            foreach (var VARIABLE in woodCount.Keys)
            {
                towerCount[0] += woodCount[VARIABLE];
            }
            foreach (var VARIABLE in ironCount.Keys)
            {
                towerCount[1] += ironCount[VARIABLE];
            }
            foreach (var VARIABLE in elecCount.Keys)
            {
                towerCount[2] += elecCount[VARIABLE];
            }
            foreach (var VARIABLE in wproCount.Keys)
            {
                protectCount[0] +=wproCount[VARIABLE];
            }
            foreach (var VARIABLE in iproCount.Keys)
            {
                protectCount[1] += iproCount[VARIABLE];
            }
            foreach (var VARIABLE in eproCount.Keys)
            {
                protectCount[2] += eproCount[VARIABLE];
            }
            weaponTotalBlood = CurNodeDataSummary._instance.GetMainMaxWeaponLevelBlood(woodCount, ironCount, elecCount);
            Debug.Log("weaponTotalBlood: "+weaponTotalBlood[0]+", "+weaponTotalBlood[1]+", "+weaponTotalBlood[2]);

            float[] debuffPresentage = new float[3];
            for (int i = 0; i < 3; i++)
            {
                if (weaponTotalBlood[i] != 0)
                {
                    debuffPresentage[i] = (float)_debufflist[i] / (float)weaponTotalBlood[i];
                }
                else
                {
                    debuffPresentage[i] = 1;
                } 
            }
            weaponTotalProtect =  CurNodeDataSummary._instance.GetMainProtectBlood(wproCount, iproCount, eproCount);
            Debug.Log("weaponTotalProtect: "+weaponTotalProtect[0]+", "+weaponTotalProtect[1]+", "+weaponTotalProtect[2]);
            debuffWood.maxValue = 1;
            debuffIron.maxValue = 1;
            debuffElec.maxValue = 1;
            
            debuffWood.minValue = debuffIron.minValue = debuffElec.minValue = 0;
            proWood.minValue = proIron.minValue = proElec.minValue = 0;
            proWood.value = proIron.value = proElec.value =debuffWood.value = debuffIron.value = debuffElec.value = 0;
            proWood.maxValue = 1;
            proIron.maxValue = 1;
            proElec.maxValue = 1;
            _doWithSlides(0, debuffWood, proWood);
            _doWithSlides(1,debuffIron,proIron);
            _doWithSlides(2,debuffElec,proElec);
        }
    }

    private void _getMapmapList(string mapmapString)
    {
        string totalString = mapmapString;
        string[] rows = totalString.Split("/n");
        string[][] stringList = new string[rows.Length][];
        for (int i = 0; i < rows.Length; i++)
        {
            stringList[i] = rows[i].Split(',');
        }
        for (int i = 0; i < stringList.Length / 2; i++)
        {
            string[] temp = stringList[i];
            stringList[i] = stringList[stringList.Length - 1 - i];
            stringList[stringList.Length - 1 - i] = temp;
        }
        
        for (int i = 0; i < stringList.Length; i++)
        {
            if (stringList[i] != null)
            {
                string[] row = stringList[i];
                List<string> newRow = new List<string>();

                for (int j = 0; j < row.Length; j++)
                {
                    if (row[j] != null&&row[j] != "")
                    {
                        newRow.Add(row[j]);
                    }
                }
                // ????????
                stringList[i] = newRow.ToArray();
            }
        }
        _mapStruct = stringList;
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
            if (thisSlideType == debuffSlidesType.Current)
            {
                CurNodeDataSummary._instance.debuffListData[index] = temp; 
            }
            else
            {
                CurNodeDataSummary._instance.majorDebuffListData[index] = temp; 
            }
            
            //print(CurNodeDataSummary._instance.debuffListData[index] );
            if (_debufflist[index] != 0) //wood???????debuff
            {
                if (thisSlideType == debuffSlidesType.Current)
                {
                    CurNodeDataSummary._instance.protecListData[index] = Mathf.Round(((float)weaponTotalProtect[index] /(float) _debufflist[index]) * 100) / 100;
                }
                else
                {
                    CurNodeDataSummary._instance.majorProtecListData[index] = Mathf.Round(((float)weaponTotalProtect[index] /(float) _debufflist[index]) * 100) / 100;
                }
                StartCoroutine(FillProgressBar(debufSlider,temp,index));
            }
            else //wood????????debuff
            {
                if (weaponTotalProtect[index] != 0) //wood????????debuff????protect
                {
                    Color tempColor = debuffSliderImage[index].color;
                    tempColor.a = 0;
                    debufSlider.value = 1;
                    StartCoroutine(FillProgressBar2(protectSlider,1f));
                    if (thisSlideType == debuffSlidesType.Current)
                    {
                        CurNodeDataSummary._instance.protecListData[index] = 1;
                    }
                    else
                    {
                        CurNodeDataSummary._instance.majorProtecListData[index] = 1; 
                    }
                    //CurNodeDataSummary._instance.protecListData[index] = 1;
                }
                else //wood????????debuff?????protect
                {
                    Color tempColor = debuffSliderImage[index].color;
                    tempColor.a = 1;
                    debufSlider.value = 0;
                    protectSlider.value = 0;
                    if (thisSlideType == debuffSlidesType.Current)
                    {
                        CurNodeDataSummary._instance.protecListData[index] = 0;
                    }
                    else
                    {
                        CurNodeDataSummary._instance.majorProtecListData[index] = 0; 
                    }
                }
            }
        }
        else //????
        {
            if (thisSlideType == debuffSlidesType.Current)
            {
                CurNodeDataSummary._instance.debuffListData[index] = 0; 
            }
            else
            {
                CurNodeDataSummary._instance.majorDebuffListData[index] = 0; 
            }
            //CurNodeDataSummary._instance.debuffListData[index] = 0;
            if (_debufflist[index] != 0) //????????debuff
            {
                if (thisSlideType == debuffSlidesType.Current)
                {
                    CurNodeDataSummary._instance.debuffListData[index] = 1; 
                }
                else
                {
                    CurNodeDataSummary._instance.majorDebuffListData[index] = 1; 
                }
                //CurNodeDataSummary._instance.debuffListData[index] = 1;
                StartCoroutine(FillProgressBar(debufSlider,1,index));
            }
            else //?????????debuff
            {
                if (weaponTotalProtect[index] != 0) //?????????debuff????protect
                {
                    Color tempColor = debuffSliderImage[index].color;
                    tempColor.a = 0;
                    debufSlider.value = 1;
                    StartCoroutine(FillProgressBar2(protectSlider,1f));
                    //CurNodeDataSummary._instance.protecListData[index] = 1;
                    if (thisSlideType == debuffSlidesType.Current)
                    {
                        CurNodeDataSummary._instance.protecListData[index] = 1;
                    }
                    else
                    {
                        CurNodeDataSummary._instance.majorProtecListData[index] = 1; 
                    }
                }
                else
                {
                    Color tempColor = debuffSliderImage[index].color;
                    tempColor.a = 1;
                    debufSlider.value = 0;
                    protectSlider.value = 0;
                    //CurNodeDataSummary._instance.protecListData[index] = 0;
                    if (thisSlideType == debuffSlidesType.Current)
                    {
                        CurNodeDataSummary._instance.protecListData[index] = 0;
                    }
                    else
                    {
                        CurNodeDataSummary._instance.majorProtecListData[index] = 0; 
                    }
                }
            }
   
        }
    }

    
    IEnumerator FillProgressBar(Slider slider,float targetValue,int index)
    {
        slider.value = 0;
        //print(slider.gameObject.transform.name+" target value is: "+targetValue);
        while (slider.value < targetValue)
        {
            //print(slider.value);
            //print(slider.gameObject.transform.name+" value is: "+slider.value);
            slider.value += fillSpeedDebuff * Time.deltaTime; // ????Slider???
            
            yield return new WaitForSeconds(0.01f);
        }
        
        //Debug.Log("Progress bar filled!");
        if (index == 0)
        {
            StartCoroutine(FillProgressBar2(proWood,Mathf.Round((float)weaponTotalProtect[0] /(float) _debufflist[0]* 100) / 100));
        }
        else if (index == 1)
        {
            StartCoroutine(FillProgressBar2(proIron,Mathf.Round((float)weaponTotalProtect[1] /(float) _debufflist[1]* 100) / 100));
        }
        else if (index == 2)
        {
            StartCoroutine(FillProgressBar2(proElec,Mathf.Round((float)weaponTotalProtect[2] /(float) _debufflist[2]* 100) / 100));
        }
        
    }
    IEnumerator FillProgressBar2(Slider slider,float targetValue)
    {
        slider.value = 0;
        
        if (targetValue >= slider.maxValue)
        {
            targetValue = slider.maxValue;
        }
        //print(slider.gameObject.transform.name+" target value is: "+targetValue);
        while (slider.value < targetValue)
        {
            //print(slider.value);
            //print(slider.gameObject.transform.name+" value is: "+slider.value);
            slider.value += fillSpeedProtect * Time.deltaTime; // ????Slider???
            
            yield return new WaitForSeconds(0.01f);
        }
        
        //Debug.Log("Progress bar filled222!");
    }


}
