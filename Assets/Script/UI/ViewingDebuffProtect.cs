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

    
    public float fillSpeedDebuff = 1f;
    public float fillSpeedProtect = 1f;

    public int[] currentProt;
    public int[] previousProt;

    public int[] curDebuff;
    public int[] previousDebuff;
    
    void Awake()
    {
        _debufflist = CurNodeDataSummary._instance.debuffList;
        _counted = false;
        weaponTotalBlood = new int[3];
        weaponTotalProtect = new int[3];
        Debug.Log("HIIIIIIIIIIIIIIIII debuff is restart!!!!!!!!!!!!!!!!!!!!!!");
        currentProt = previousProt = CurNodeDataSummary._instance.protectList;
        curDebuff = previousDebuff = CurNodeDataSummary._instance.debuffList;
        //_wood[0].text = _debufflist[0]
    }

    
    // public void GetChosenNodeInfo()
    // {
    //     _countDicInit();
    //     debuffList = GlobalVar._instance.chosenDownNodeData.debuffData;
    //     GlobalVar._instance._getMapmapList();
    //     _mapStruct = GlobalVar._instance.mapmapList;
    //     (woodCount,ironCount,elecCount,wproCount,iproCount,eproCount) = _checkTypeIndex(_mapStruct);
    // }
    
    
    //Update is called once per frame
    void Update()
    {
        _debufflist = CurNodeDataSummary._instance.debuffList;
        if (_counted == false && CurNodeDataSummary._instance._initData&&GlobalVar.CurrentGameState != GlobalVar.GameState.ChooseField)
        {
            debuffProSlidesInitTrigger();
        }

        if (_counted == false && CurNodeDataSummary._instance._initData && GlobalVar.CurrentGameState == GlobalVar.GameState.ChooseField)
        {
            debuffProSlidesInitTrigger();
        }

        if (GlobalVar.CurrentGameState == GlobalVar.GameState.ChooseField)
        {
            currentProt = CurNodeDataSummary._instance.protectList;
            curDebuff = CurNodeDataSummary._instance.debuffList;
            if (previousProt != currentProt || curDebuff!=previousDebuff)
            {
                print("update the protect and debuff list!");
                debuffProSlidesInitTrigger();
            }

            previousDebuff = curDebuff;
            previousProt = currentProt;
        }
        
    }

    private void debuffProSlidesInitTrigger()
    {
        _counted = true;
        int[] towerCount = new int[3];
        int[] protectCount = new int[3];
        foreach (var VARIABLE in CurNodeDataSummary._instance.woodCount.Keys)
        {
            towerCount[0] += CurNodeDataSummary._instance.woodCount[VARIABLE];
        }
        foreach (var VARIABLE in CurNodeDataSummary._instance.ironCount.Keys)
        {
            towerCount[1] += CurNodeDataSummary._instance.ironCount[VARIABLE];
        }
        foreach (var VARIABLE in CurNodeDataSummary._instance.elecCount.Keys)
        {
            towerCount[2] += CurNodeDataSummary._instance.elecCount[VARIABLE];
        }
        foreach (var VARIABLE in CurNodeDataSummary._instance.wproCount.Keys)
        {
            protectCount[0] +=CurNodeDataSummary._instance.wproCount[VARIABLE];
        }
        foreach (var VARIABLE in CurNodeDataSummary._instance.iproCount.Keys)
        {
            protectCount[1] += CurNodeDataSummary._instance.iproCount[VARIABLE];
        }
        foreach (var VARIABLE in CurNodeDataSummary._instance.eproCount.Keys)
        {
            protectCount[2] += CurNodeDataSummary._instance.eproCount[VARIABLE];
        }
        weaponTotalBlood = CurNodeDataSummary._instance.GetMainMaxWeaponLevelBlood(CurNodeDataSummary._instance.woodCount, CurNodeDataSummary._instance.ironCount, CurNodeDataSummary._instance.elecCount);
        Debug.Log("for debuff and protect slider: weaponTotalBlood: "+weaponTotalBlood[0]+", "+weaponTotalBlood[1]+", "+weaponTotalBlood[2] +"in exhibit");
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
        weaponTotalProtect =  CurNodeDataSummary._instance.GetMainProtectBlood(CurNodeDataSummary._instance.wproCount, CurNodeDataSummary._instance.iproCount, CurNodeDataSummary._instance.eproCount);
        Debug.Log("for debuff and protect slider: weaponTotalProtect: "+weaponTotalProtect[0]+", "+weaponTotalProtect[1]+", "+weaponTotalProtect[2]+"in exhibit");
        
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
            //print(CurNodeDataSummary._instance.debuffListData[index] );
            if (_debufflist[index] != 0) //wood???????debuff
            {
                print("for debuff and protect slider: weapon type"+index+", the Protect/Debuff is: "+((float)(weaponTotalProtect[index] /(float) _debufflist[index])));
                CurNodeDataSummary._instance.protecListData[index] = Mathf.Round(((float)weaponTotalProtect[index] /(float) _debufflist[index]) * 100) / 100;
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
                    CurNodeDataSummary._instance.protecListData[index] = 1;
                }
                else //wood????????debuff?????protect
                {
                    Color tempColor = debuffSliderImage[index].color;
                    tempColor.a = 1;
                    debufSlider.value = 0;
                    protectSlider.value = 0;
                    CurNodeDataSummary._instance.protecListData[index] = 0;
                }
            }
        }
        else //????
        {
            CurNodeDataSummary._instance.debuffListData[index] = 0;
            if (_debufflist[index] != 0) //????????debuff
            {
                CurNodeDataSummary._instance.debuffListData[index] = 1;
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
        //print(slider.gameObject.transform.name+" target value is: "+targetValue);
        while (slider.value < targetValue)
        {
            print("for debuff and protect slider: for weapon "+index+" the debuff slider increasing: "+slider.value);
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
            print("for debuff and protect slider, the curSummary data is: "+CurNodeDataSummary._instance.protecListData[3]);
            print("for debuff and protect slider: "+Mathf.Round((float)weaponTotalProtect[2] /(float) _debufflist[2]* 100) / 100);
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
        print("for debuff and protect slider: "+ slider.gameObject.transform.name+" target value is: "+targetValue);
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
