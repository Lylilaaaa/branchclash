using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MergePanalControl : MonoBehaviour
{
    public TowerBuilder tb;
    public string weaponType;
    public int weaponGrade;
    
    [Header("list")]
    public List<Texture> WeaponTexture;
    public List<Texture> ProTexture;

    [Header("TMP")]
    public TextMeshProUGUI levelString; //WOOD \n LEVEL 1
    public TextMeshProUGUI levelStringProtect; //"Wood Protector Level "+_grade.ToString()
    public TextMeshProUGUI infoStringWeapon;  //Range: 3*3 \n Damage: 1/shot \n Speed: 1s
    public TextMeshProUGUI infoStringProtect;  //"Increase wood tower's resistance to debuffs by " + _grade.ToString() + "%";
    
    [Header("GObj")]
    public GameObject moneyGObj;
    public GameObject weaponPanal;
    public GameObject protectPanal;
    
    [Header("money")]
    public TextMeshProUGUI moneyNum;
    public GameObject moneyGObjProtect;
    public TextMeshProUGUI moneyNumProtect;
    
    [Header("-----------POSITION-----------")]
    public GameObject rangePosParent;
    public RawImage rawImageDisplay;
    public RawImage rawImageDisplayProtect;
    

    
    private TowerData wData;
    private TowerData iData;
    private TowerData eData;

    private ProtectData wPro;
    private ProtectData iPro;
    private ProtectData ePro;

    private int _poolTypeIndex;
    private int _weaponTypeIndex;
    private int _performTypeIndex;
    

    private void Start()
    {
        wData = GlobalVar._instance.woodTowerData;
        iData = GlobalVar._instance.ironTowerData;
        eData = GlobalVar._instance.elecTowerData;
        wPro = GlobalVar._instance.ProWood;
        iPro = GlobalVar._instance.ProIron;
        ePro = GlobalVar._instance.ProElec;
        
        weaponType = tb.targetWeaponType;
        weaponGrade = tb.targetWeaponGrade;
    }

    private void Update()
    {
        weaponType = tb.targetWeaponType;
        weaponGrade = tb.targetWeaponGrade;
        string tempString = weaponType;
        if (weaponType.Length == 4)
        {
            if (weaponType.Substring(1) == "pro")
            {
                weaponPanal.SetActive(false);
                protectPanal.SetActive(true);
                levelStringProtect.text = tempString.ToUpper()+ "\n"+"PROTECTOR"+"\n"+"LEVEL "+weaponGrade;
                infoStringProtect.text = "Increase wood tower's resistance to debuffs by " + weaponGrade.ToString() + "%";
                if(weaponType == "wpro")
                {
                    _poolTypeIndex = 1;
                    _weaponTypeIndex = 0;
                    _performTypeIndex=outputPerformIndexPro(wPro, weaponGrade);
                }
                else if(weaponType == "epro")
                {
                    _poolTypeIndex = 1;
                    _weaponTypeIndex = 2;
                    _performTypeIndex=outputPerformIndexPro(ePro, weaponGrade);
                }
                else if(weaponType == "ipro")
                {
                    _poolTypeIndex = 1;
                    _weaponTypeIndex = 1;
                    _performTypeIndex=outputPerformIndexPro(iPro, weaponGrade);
                }
                rawImageDisplayProtect.texture = changeTexture(_poolTypeIndex,_weaponTypeIndex,_performTypeIndex) ;
                if (tb.targetWeaponPos.Count == 0)
                {
                    moneyGObjProtect.SetActive(false);
                }
                else
                {
                    moneyGObjProtect.SetActive(true);
                    if (weaponType == "wpro")
                    {
                        moneyNumProtect.text = "-" + wPro.basePrice;
                    }
                    else if (weaponType == "ipro")
                    {
                        moneyNumProtect.text = "-" + iPro.basePrice;
                    }
                    else if (weaponType == "epro")
                    {
                        moneyNumProtect.text = "-" + ePro.basePrice;
                    }
                }
            }
            else
            {
                protectPanal.SetActive(false);
                weaponPanal.SetActive(true);
                levelString.text = tempString.ToUpper()+"\n"+"LEVEL "+weaponGrade;
                //Range: 3*3 \n Damage: 1/shot \n Speed: 1s
                string attack, speed, range;

                (attack, speed, range) = CurNodeDataSummary._instance.CheckAttackSpeedRange(weaponType, weaponGrade);
                if (range == wData.basicRange.ToString())
                {
                    range = "3x3";
                    rangePosParent.transform.GetChild(0).gameObject.SetActive(true);
                    setActiveFalseExcept(0, rangePosParent.transform);
                }
                else if(range ==  "full map")
                {
                    range = "full map";
                    rangePosParent.transform.GetChild(1).gameObject.SetActive(true);
                    setActiveFalseExcept(1, rangePosParent.transform);
                }
                else if(range == (eData.basicRange).ToString())
                {
                    range = "3x4";
                    rangePosParent.transform.GetChild(2).gameObject.SetActive(true);
                    setActiveFalseExcept(2, rangePosParent.transform);
                }
                else if(range == (eData.basicRange+18).ToString())
                {
                    range = "5x6";
                    rangePosParent.transform.GetChild(3).gameObject.SetActive(true);
                    setActiveFalseExcept(3, rangePosParent.transform);
                }
                else if(range == (eData.basicRange+18+26).ToString())
                {
                    range = "7x8";
                    rangePosParent.transform.GetChild(4).gameObject.SetActive(true);
                    setActiveFalseExcept(4, rangePosParent.transform);
                }
                infoStringWeapon.text = "Range: "+range+"\n"+"Damage: "+attack+"/shot"+"\n"+"Speed: "+ 1/float.Parse(speed)+"s";
                if(weaponType == "wood")
                {
                    _poolTypeIndex = 0;
                    _weaponTypeIndex = 0;
                    _performTypeIndex=outputPerformIndex(wData, weaponGrade);
                }
                else if(weaponType == "iron")
                {
                    _poolTypeIndex = 0;
                    _weaponTypeIndex = 1;
                    _performTypeIndex=outputPerformIndex(iData, weaponGrade);
                }
                else if(weaponType == "elec")
                {
                    _poolTypeIndex = 0;
                    _weaponTypeIndex = 2;
                    _performTypeIndex=outputPerformIndex(eData, weaponGrade);
                }
                rawImageDisplay.texture = changeTexture(_poolTypeIndex,_weaponTypeIndex,_performTypeIndex) ;
                if (tb.targetWeaponPos.Count == 0)
                {
                    moneyGObj.SetActive(false);
                }
                else
                {
                    moneyGObj.SetActive(true);
                    if (weaponType == "wood")
                    {
                        moneyNum.text = "-" + wData.merge_price;
                    }
                    else if (weaponType == "iron")
                    {
                        moneyNum.text = "-" + iData.merge_price;
                    }
                    else if (weaponType == "elec")
                    {
                        moneyNum.text = "-" + eData.merge_price;
                    }
                    else
                    {
                        moneyNum.text = "-" + 0;
                    }
                }
            }

        }

    }

    private void setActiveFalseExcept(int exceptIndex,Transform parentTrans)
    {
        int childCount = parentTrans.childCount;
        if (childCount != 0)
        {
            for (int i = 0; i < childCount; i++)
            {
                if (i != exceptIndex)
                {
                    parentTrans.GetChild(i).gameObject.SetActive(false);
                    //print(parentTrans.GetChild(i).gameObject.name+": false!");
                }
            }
        }
    }
    private int outputPerformIndex(TowerData thisWeaponData,int curLevel)
    {
        if (0 < curLevel && curLevel< thisWeaponData.performGrade2)
        {
            return 0;
        }
        else if (thisWeaponData.performGrade2 <= curLevel && curLevel < thisWeaponData.performGrade3)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }
    private int outputPerformIndexPro(ProtectData thisWeaponData,int curLevel)
    {
        if (0 < curLevel && curLevel< thisWeaponData.performGrade2)
        {
            return 0;
        }
        else if (thisWeaponData.performGrade2 <= curLevel && curLevel < thisWeaponData.performGrade3)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }

    private Texture changeTexture(int pool,int type,int perform) //0,1,2
    {
        if (pool == 0)
        {
            return (WeaponTexture[perform+3*type]);
        }
        else
        {
            return (ProTexture[perform+3*type]);
        }
    }
}
