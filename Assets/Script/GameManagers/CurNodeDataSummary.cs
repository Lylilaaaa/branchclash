using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using UnityEngine;

public class CurNodeDataSummary : MonoBehaviour
{
    public static CurNodeDataSummary _instance;
    [Header("--------InitSettings--------")]
    private TowerData wData;
    private TowerData iData;
    private TowerData eData;

    [Header("--------GlobalDataRead--------")]
    public NodeData thisNodeData;
    private NodeData previousNodeData;
    public DownNodeData thisDownNodeData;
    private DownNodeData previousDownNodeData;
    private string[][] _mapStruct;
    
    [Header("--------NumCount--------")]
    public Dictionary<int,int> woodCount; //grade, count
    public Dictionary<int,int> ironCount;
    public Dictionary<int,int> elecCount;
    public Dictionary<int,int> wproCount;
    public Dictionary<int,int> iproCount;
    public Dictionary<int,int> eproCount;
    public List<int> DictionaryCount;
    public int[] debuffList;
    
    public int[] curDebuffList;
    public int[] majorDebuffList;
    
    public int[] protectList;
    public int[] weaponBloodList;
    
    public float[] debuffListData;
    public float[] majorDebuffListData;
    public float[] protecListData;
    public float[] majorProtecListData;
    
    [Header("--------ProcessingBool--------")]
    public bool _initData = false;
    public bool gamePlayInitData = false;
    public int choseNodeType;

    [Header("--------LevelReCord--------")]
    public float homeMaxHealth;
    public float homeCurHealth;
    public float moneyLeft;
    public int monsterCount;
    
    private void Awake()
    {
        _instance = this;
    }

    public void ReStart()
    {
        _initData = false;
        gamePlayInitData = false;
        thisNodeData = GlobalVar._instance.chosenNodeData;
        thisDownNodeData = GlobalVar._instance.chosenDownNodeData;
        wData = GlobalVar._instance.woodTowerData;
        iData = GlobalVar._instance.ironTowerData;
        eData = GlobalVar._instance.elecTowerData;

        debuffListData = new float[3];
        curDebuffList = new int[3];
        majorDebuffList = new int[3];
        majorDebuffListData = new float[3];
        protecListData = new float[3];
        weaponBloodList = new int[3];
        majorProtecListData = new float[3];
        
        woodCount = new Dictionary<int, int>();
        ironCount = new Dictionary<int, int>();
        elecCount = new Dictionary<int, int>();
        wproCount = new Dictionary<int, int>();
        iproCount = new Dictionary<int, int>();
        eproCount = new Dictionary<int, int>();
    }

    private void Update()
    {
        thisNodeData = GlobalVar._instance.chosenNodeData;
        thisDownNodeData = GlobalVar._instance.chosenDownNodeData;
        if (GlobalVar.CurrentGameState == GlobalVar.GameState.Viewing)
        {
            if ((previousNodeData != thisNodeData || previousDownNodeData != thisDownNodeData)&& _initData)
            {
                _countDicInit();
                if (choseNodeType == 0)
                {
                    debuffList = thisNodeData.towerDebuffList;
                }
                else
                {
                    debuffList = thisDownNodeData.debuffData;
                }
                GlobalVar._instance._getMapmapList();
                _mapStruct = GlobalVar._instance.mapmapList;
                _checkTypeIndex();
            }

            //chose the down node for the first time!
            else if (GlobalVar._instance.dataPrepared && !_initData)
            {
                _countDicInit();
                if (choseNodeType == 0)
                {
                    debuffList = thisNodeData.towerDebuffList;
                }
                else
                {
                    debuffList = thisDownNodeData.debuffData;
                }
                GlobalVar._instance._getMapmapList();
                _mapStruct = GlobalVar._instance.mapmapList;
                _checkTypeIndex();
                _initData = true;
            }
        }

        //ENTER GAMEPLAY!!
        if (GlobalVar._instance.thisUserData != null)
        {
            if (GlobalVar._instance.thisUserData.role == 0)
            {
                //enter the game play mode, need refresh
                if (GlobalVar.CurrentGameState == GlobalVar.GameState.GamePlay && !_initData && !gamePlayInitData)
                {
                    _countDicInit();
                    debuffList = thisNodeData.towerDebuffList;
                    _mapStruct = GlobalVar._instance.mapmapList;
                    _checkTypeIndex();
                    _initData = true;
                    gamePlayInitData = true;
                    //weaponBloodList =_checkWeaponTotalBlood();
                }
            }
            else
            {
                //enter the sec game play mode, need to refresh the up tree main node
                if (GlobalVar.CurrentGameState == GlobalVar.GameState.GamePlay && !_initData && !gamePlayInitData)
                {
                    _countDicInit();
                    debuffList = thisDownNodeData.debuffData;
                    GlobalVar._instance.chosenNodeData = GlobalVar._instance._checkUpNodeMain(thisDownNodeData.nodeLayer+1);
                    GlobalVar._instance._getMapmapList();
                    _mapStruct = GlobalVar._instance.mapmapList;
                    _checkTypeIndex();
                    weaponBloodList = GetMainMaxWeaponLevelBlood();
                    protectList = GetMainProtectBlood();
                    _initData = true;
                    gamePlayInitData = true;
                }
            }
        }
        previousNodeData = thisNodeData;
        previousDownNodeData = thisDownNodeData;

    }

    public int[] GetMainProtectBlood()
    {
        int[] weaponTotalProtect = new int[3];
        if (wproCount != null)
        {
            foreach (int grade in wproCount.Keys)
            {
                weaponTotalProtect[0] += wproCount[grade]* _gradeToProtect(grade)*GlobalVar._instance.ProWood.baseProtect/2;
            }
        }
        if (iproCount != null)
        {
            foreach (int grade in iproCount.Keys)
            {
                weaponTotalProtect[1] += iproCount[grade] * _gradeToProtect(grade)*GlobalVar._instance.ProIron.baseProtect/2;
            }
        }
        if (eproCount != null)
        {
            foreach (int grade in eproCount.Keys)
            {
                weaponTotalProtect[2] += eproCount[grade] * _gradeToProtect(grade)*GlobalVar._instance.ProElec.baseProtect/2;
            }
        }
        return weaponTotalProtect;
    }
    
    public int[] GetMainMaxWeaponLevelBlood()
    { 
        string _at, _sp, _ra;
        int[] weaponTotalBlood = new int[3];
        if (woodCount != null)
        {
            int maxGrade = 0;
            foreach (int grade in woodCount.Keys)
            {
                if (grade >= maxGrade)
                {
                    maxGrade = grade;
                }
            }
            (_at,_sp,_ra) = CheckAttackSpeedRange("wood", maxGrade);
            weaponTotalBlood[0] = int.Parse(_at) * int.Parse(_sp);
        }
        if (ironCount != null)
        {
            int maxGrade = 0;
            foreach (int grade in ironCount.Keys)
            {
                if (grade >= maxGrade)
                {
                    maxGrade = grade;
                }
            }
            (_at,_sp,_ra) = CheckAttackSpeedRange("iron", maxGrade);
            weaponTotalBlood[1] = int.Parse(_at) * int.Parse(_sp);
        }
        if (elecCount != null)
        {
            int maxGrade = 0;
            foreach (int grade in elecCount.Keys)
            {
                if (grade >= maxGrade)
                {
                    maxGrade = grade;
                }
            }
            (_at,_sp,_ra) = CheckAttackSpeedRange("elec", maxGrade);
            weaponTotalBlood[2] = int.Parse(_at) * int.Parse(_sp);
        }

        return weaponTotalBlood;
    }
    private int _gradeToProtect(int grade)
    {
        int tempInt = 0;
        for (int i = 0; i < grade; i++)
        {
            tempInt = (tempInt * 2) + 1;
        }

        return tempInt;
    }
    
    private void _countDicInit()
    {
        DictionaryCount = new List<int>();
        woodCount = new Dictionary<int, int>();
        ironCount = new Dictionary<int, int>();
        elecCount = new Dictionary<int, int>();
        wproCount = new Dictionary<int, int>();
        iproCount = new Dictionary<int, int>();
        eproCount = new Dictionary<int, int>();
    } 
    
    private void _checkTypeIndex()
    {
        for (int i = 0; i < _mapStruct.Length; i++)
        {
            for (int j = 0; j < _mapStruct[i].Length; j++)
            {
                if (_mapStruct[i][j].Length >= 5)
                {
                    string mapType = _mapStruct[i][j].Substring(0, 4);
                    int towerGrade = int.Parse(_mapStruct[i][j].Substring(4, _mapStruct[i][j].Length-4));
                    if (mapType == "wood")
                    {
                        if (!woodCount.ContainsKey(towerGrade))
                        {
                            woodCount.Add(towerGrade, 1);
                        }
                        else
                        {
                            woodCount[towerGrade] += 1;
                        }
                    }
                    else if(mapType == "iron")
                    {
                        if (!ironCount.ContainsKey(towerGrade))
                        {
                            ironCount.Add(towerGrade, 1);
                        }
                        else
                        {
                            ironCount[towerGrade] += 1;
                        }
                    }
                    else if (mapType == "elec")
                    {
                        _mapStruct[i][j + 1] = "eleC"+towerGrade;
                        if (!elecCount.ContainsKey(towerGrade))
                        {
                            elecCount.Add(towerGrade, 1);
                        }
                        else
                        {
                            elecCount[towerGrade] += 1;
                        }
                    }
                    else if (mapType == "wpro")
                    {
                        if (!wproCount.ContainsKey(towerGrade))
                        {
                            wproCount.Add(towerGrade, 1);
                        }
                        else
                        {
                            wproCount[towerGrade] += 1;
                        }
                    }
                    else if (mapType == "ipro")
                    {
                        if (!iproCount.ContainsKey(towerGrade))
                        {
                            iproCount.Add(towerGrade, 1);
                        }
                        else
                        {
                            iproCount[towerGrade] += 1;
                        }
                    }
                    else if (mapType == "epro")
                    {
                        if (!eproCount.ContainsKey(towerGrade))
                        {
                            eproCount.Add(towerGrade, 1);
                        }
                        else
                        {
                            eproCount[towerGrade] += 1;
                        }
                    }
                    else if (mapType == "eleC")
                    {
                        continue;
                    }
                    else
                    {
                        Debug.LogError("incorrect Map String!!!");
                    }
                }
            }
        }
        DictionaryCount.Add(woodCount.Count);
        DictionaryCount.Add(ironCount.Count);
        DictionaryCount.Add(elecCount.Count);
        DictionaryCount.Add(wproCount.Count);
        DictionaryCount.Add(iproCount.Count);
        DictionaryCount.Add(eproCount.Count);
    }
    
    public (string, string,string) CheckAttackSpeedRange(string towerType,int grade)
    {
        int attack = 0;
        float speed = 0;
        int range = 0;
        string attackString = "";
        string speedString = "";
        string rangeString = "";
        switch (towerType)
        {
            case "wood":
                if (grade <= wData.gradeSpeedToAttack)
                {
                    speed = wData.baseBulletNumberPerSecond + (grade-1) * wData.upgradeSpeedRate;
                    attack = wData.baseBulletAttack;
                }
                else if(grade > wData.gradeSpeedToAttack)
                {
                    speed = wData.baseBulletNumberPerSecond + (wData.gradeSpeedToAttack-1) * wData.upgradeSpeedRate;
                    attack = wData.baseBulletAttack + (grade-wData.gradeSpeedToAttack)*wData.upgradeAttackRate;
                }
                range = wData.basicRange;
                attackString = attack.ToString();
                speedString = speed.ToString();
                rangeString = range.ToString();
                break;
            case "iron":
                if (grade <= iData.gradeSpeedToAttack)
                {
                    speed = iData.baseBulletNumberPerSecond + (grade-1) * iData.upgradeSpeedRate;
                    attack = iData.baseBulletAttack;
                }
                else if(grade > iData.gradeSpeedToAttack)
                {
                    speed = iData.baseBulletNumberPerSecond + (iData.gradeSpeedToAttack-1) * iData.upgradeSpeedRate;
                    attack = iData.baseBulletAttack + (grade-iData.gradeSpeedToAttack)*iData.upgradeAttackRate;
                }
                range = iData.basicRange;
                attackString = attack.ToString();
                speedString = speed.ToString();
                rangeString = "full map";
                break;
            case "elec":
                if (grade <= eData.gradeSpeedToAttack)
                {
                    speed = eData.baseBulletNumberPerSecond + (grade-1) * eData.upgradeSpeedRate;
                    attack = eData.baseBulletAttack;
                }
                else if(grade > eData.gradeSpeedToAttack)
                {
                    speed = eData.baseBulletNumberPerSecond + (eData.gradeSpeedToAttack-1) * eData.upgradeSpeedRate;
                    attack = eData.baseBulletAttack + (grade-eData.gradeSpeedToAttack)*eData.upgradeAttackRate;
                }
                range = eData.basicRange;
                if (grade < eData.gradeRange2)
                {
                    range = eData.basicRange;//10
                }
                else if(grade >= eData.gradeRange2 &&grade < eData.gradeRange3)
                {
                    range = eData.basicRange+18;//28
                }
                else
                {
                    range = eData.basicRange+18+26;//54
                }
                attackString = attack.ToString();
                speedString = speed.ToString();
                rangeString = range.ToString();
                break;
            default:
                Console.WriteLine("Unknown");
                break;
        }
        return (attackString,speedString,rangeString);
    }

}
