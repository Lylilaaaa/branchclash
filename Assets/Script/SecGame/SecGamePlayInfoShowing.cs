using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SecGamePlayInfoShowing : MonoBehaviour
{
    public Slider debuffWood;
    public Slider debuffIron;
    public Slider debuffElec;
    public Slider proWood;
    public Slider proIron;
    public Slider proElec;
    
    public int[] _debufflist;
    public int[] weaponTotalBlood;
    public int[] weaponTotalProtect;
    
    public Image[] debuffSliderImage;
    public TextMeshProUGUI[] layerMainShowing;
    public TextMeshProUGUI[] protectShowing;
    
    public float fillSpeedDebuff = 1f;
    public float fillSpeedProtect = 1f;

    public bool dataFilled;
    public bool dataRefresh;

    private string[] weaponList = new[] { "wood", "iron", "elec" };
    
    // Start is called before the first frame update
    void Start()
    {
        dataFilled = false;
        dataRefresh = false;
        
        for(int i = 0;i<3;i++)
        {
            TextMeshProUGUI VARIABLE = layerMainShowing[i];
            VARIABLE.text = (GlobalVar._instance.chosenDownNodeData.nodeLayer+1).ToString() + " layer main\nlevel "+weaponList[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dataFilled == false && CurNodeDataSummary._instance.gamePlayInitData)
        {
            dataFilled = true;
            
            _debufflist = CurNodeDataSummary._instance.debuffList;

            weaponTotalBlood = CurNodeDataSummary._instance.weaponBloodList;
            weaponTotalProtect = CurNodeDataSummary._instance.protectList;

            protectShowing[0].text = "+" + weaponTotalProtect[0].ToString();
            protectShowing[1].text = "+" + weaponTotalProtect[1].ToString();
            protectShowing[2].text = "+" + weaponTotalProtect[2].ToString();
            
            debuffWood.maxValue = 1;
            debuffIron.maxValue = 1;
            debuffElec.maxValue = 1;
            proWood.maxValue = 1;
            proIron.maxValue = 1;
            proElec.maxValue = 1;
            proWood.minValue = proIron.minValue = proElec.minValue = 0;
            proWood.value = proIron.value = proElec.value =debuffWood.value = debuffIron.value = debuffElec.value = 0;
            _doWithSlides(0, debuffWood, proWood);
            _doWithSlides(1,debuffIron,proIron);
            _doWithSlides(2,debuffElec,proElec);
        }
    }
    private void _doWithSlides(int index, Slider debufSlider, Slider protectSlider)
    {
        if (weaponTotalBlood[index] != 0)
        {

            float temp = Mathf.Round(((float)_debufflist[index] / (float)weaponTotalBlood[index])*100)/100;

            if (_debufflist[index] != 0) //wood有血，有debuff
            {
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
                }
                else //wood有血，没有debuff，没有protect
                {
                    Color tempColor = debuffSliderImage[index].color;
                    tempColor.a = 1;
                    debufSlider.value = 0;
                    protectSlider.value = 0;
                }
            }
        }
        else //没有血
        {
            //CurNodeDataSummary._instance.debuffListData[index] = 0;
            if (_debufflist[index] != 0) //没有血，有debuff
            {
                //CurNodeDataSummary._instance.debuffListData[index] = 1;
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
                }
                else
                {
                    Color tempColor = debuffSliderImage[index].color;
                    tempColor.a = 1;
                    debufSlider.value = 0;
                    protectSlider.value = 0;
                }
            }
   
        }
    }
    
    IEnumerator FillProgressBar(Slider slider,float targetValue,int index)
    {
        //slider.value = 0;
        //print(slider.gameObject.transform.name+" target value is: "+targetValue);
        while (slider.value < targetValue)
        {
            //print(slider.value);
            //print(slider.gameObject.transform.name+" value is: "+slider.value);
            slider.value += fillSpeedDebuff * Time.deltaTime; // 增加Slider的值
            
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
        //slider.value = 0;
        
        if (targetValue >= slider.maxValue)
        {
            targetValue = slider.maxValue;
        }
        //print(slider.gameObject.transform.name+" target value is: "+targetValue);
        while (slider.value < targetValue)
        {
            //print(slider.value);
            //print(slider.gameObject.transform.name+" value is: "+slider.value);
            slider.value += fillSpeedProtect * Time.deltaTime; // 增加Slider的值
            
            yield return new WaitForSeconds(0.01f);
        }
        
        //Debug.Log("Progress bar filled222!");
    }

    public void AddDebuffAndRefresh(int index)
    {
        float addNum = 1;
        TowerData addTowerData = new TowerData();
        if (index == 0)
        {
            addTowerData = GlobalVar._instance.woodTowerData;
        }
        else if(index == 1)
        {
            addTowerData = GlobalVar._instance.ironTowerData;
        }
        else if (index == 2)
        {
            addTowerData = GlobalVar._instance.elecTowerData;
        }
        addNum = (addTowerData.baseBulletAttack*addTowerData.baseBulletNumberPerSecond)/2;
        CurNodeDataSummary._instance.curDebuffList[index] += (int) addNum;
        dataRefresh = true;
        
        TreeNodeDataInit._instance.AddDownNodeData();
        
        _debufflist = CurNodeDataSummary._instance.curDebuffList;
        weaponTotalBlood = CurNodeDataSummary._instance.weaponBloodList;
        weaponTotalProtect = CurNodeDataSummary._instance.protectList;

        _changeCertainSlider(index);
    }

    private void _changeCertainSlider(int index)
    {
        if (index == 0)
        {
            proWood.value = 0;
            _doWithSlides(0, debuffWood, proWood);
        }
        else if (index == 1)
        {
            proIron.value = 0;
            _doWithSlides(1,debuffIron,proIron);
        }
        else if (index == 2)
        {
            proElec.value = 0;
            _doWithSlides(2,debuffElec,proElec);
        }
    }
}
