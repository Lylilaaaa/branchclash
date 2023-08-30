using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
    public NodeData previousNodeData;
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
    public bool dictionaryFinish = false;
    public bool _initData = false;
    public bool changeData = false;
    public bool gamePlayInitData = false;

    [Header("--------LevelReCord--------")]
    public float homeMaxHealth;
    public float homeCurHealth;
    public float moneyLeft;
    public int monsterCount;
    
    private void Awake()
    {
        _instance = this;
        dictionaryFinish = false;
        _initData = false;
        changeData = false;
        gamePlayInitData = false;
        thisNodeData = GlobalVar._instance.chosenNodeData;
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
        debuffList = thisNodeData.towerDebuffList;
    }

    private void Update()
    {
        if (GlobalVar.CurrentGameState == GlobalVar.GameState.Viewing)
        {
            thisNodeData = GlobalVar._instance.chosenNodeData;
            if (previousNodeData != thisNodeData && changeData == false && _initData == true)
            {
                _countDicInit();
                debuffList = thisNodeData.towerDebuffList;
                _mapStruct = GlobalVar._instance.mapmapList;
                _checkTypeIndex();
                changeData = true;
            }
            if (GlobalVar._instance.mapmapList != null && _initData == false)
            {
                _countDicInit();
                debuffList = thisNodeData.towerDebuffList;
                _mapStruct = GlobalVar._instance.mapmapList;
                _checkTypeIndex();
                _initData = true;
            }
    
            previousNodeData = thisNodeData;
        }
        if (GlobalVar.CurrentGameState == GlobalVar.GameState.GamePlay && gamePlayInitData == false)
        {
            _countDicInit();
            debuffList = thisNodeData.towerDebuffList;
            _mapStruct = GlobalVar._instance.mapmapList;
            // foreach (var Mystring in _mapStruct)
            // {
            //     string thisRow="";
            //     foreach (var VARIABLE  in Mystring )
            //     {
            //         thisRow+=VARIABLE;
            //     }
            //     print(thisRow);
            // }
            _checkTypeIndex();
            gamePlayInitData = true;
            protectList = _checkProtect();
            //weaponBloodList =_checkWeaponTotalBlood();
        }
        
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
        dictionaryFinish = true;
        // foreach (int key in woodCount.Keys)
        // {
        //     print("wood level "+key+": "+woodCount[key]);
        // }
        // foreach (int key in ironCount.Keys)
        // {
        //     print("iron level "+key+": "+ironCount[key]);
        // }
        // foreach (int key in wproCount.Keys)
        // {
        //     print("wpro level "+key+": "+wproCount[key]);
        // }
        DictionaryCount.Add(woodCount.Count);
        DictionaryCount.Add(ironCount.Count);
        DictionaryCount.Add(elecCount.Count);
        DictionaryCount.Add(wproCount.Count);
        DictionaryCount.Add(iproCount.Count);
        DictionaryCount.Add(eproCount.Count);
    }

    private int[] _checkProtect()
    {
        int[] _proList = new int[3];
        foreach (int grade in wproCount.Keys)
        {
            _proList[0] += CheckProtectBlood("wpro", grade);
        }
        foreach (int grade in iproCount.Keys)
        {
            _proList[1] += CheckProtectBlood("ipro", grade);
        }
        foreach (int grade in eproCount.Keys)
        {
            _proList[2] += CheckProtectBlood("epro", grade);
        }

        return _proList;
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
    public int CheckProtectBlood(string proType,int grade)
    {
        if (wproCount.ContainsKey(grade))
        {
            int count = 0;
            string countString = "";
            switch (proType)
            {
                case "wpro":
                    count = wproCount[grade];
                    break;
                case "ipro":
                    count = iproCount[grade];
                    break;
                case "elec":
                    count = eproCount[grade];
                    break;
                default:
                    Console.WriteLine("Unknown");
                    break;
            }
            return count*grade;
        }

        return 0;
    }
    // private int[] _checkWeaponTotalBlood()
    // {
    //     int[] _weaponBlood = new int[3];
    //     foreach (int grade in woodCount.Keys)
    //     {
    //         _weaponBlood[0] += woodCount[grade]*grade;
    //     }
    //     foreach (int grade in ironCount.Keys)
    //     {
    //         _weaponBlood[1] += ironCount[grade] * grade;
    //     }
    //     foreach (int grade in elecCount.Keys)
    //     {
    //         _weaponBlood[2] += elecCount[grade] * grade;
    //     }
    //
    //     return _weaponBlood;
    // }
}
