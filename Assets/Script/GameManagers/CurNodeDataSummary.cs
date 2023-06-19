using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class CurNodeDataSummary : MonoBehaviour
{
    public static CurNodeDataSummary _instance;
    public NodeData thisNodeData;
    public NodeData previousNodeData;
    private string[][] _mapStruct;
    public Dictionary<int,int> woodCount; //grade, count
    public Dictionary<int,int> ironCount;
    public Dictionary<int,int> elecCount;
    public Dictionary<int,int> wproCount;
    public Dictionary<int,int> iproCount;
    public Dictionary<int,int> eproCount;
    public bool dictionaryFinish = false;
    public int[] debuffList;
    private TowerData wData;
    private TowerData iData;
    private TowerData eData;
    private bool _initData = false;
    public string wproCountShown;
    private void Awake()
    {
        _instance = this;
    }
    private void Start()
    {
        thisNodeData = GlobalVar._instance.chosenNodeData;
        wData = GlobalVar._instance.woodTowerData;
        iData = GlobalVar._instance.ironTowerData;
        eData = GlobalVar._instance.elecTowerData;
        
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
            if (previousNodeData != thisNodeData)
            {
                _countDicInit();
                debuffList = thisNodeData.towerDebuffList;
                _mapStruct = GlobalVar._instance.mapmapList;
                _checkTypeIndex();
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

        wproCountShown=CheckProtectBlood("wpro", 1);
    }

    private void _countDicInit()
    {
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
                    int towerGrade = _mapStruct[i][j].Length - 4;
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
                    else
                    {
                        Debug.LogError("incorrect Map String!!!");
                    }
                }
            }
        }
        foreach (var kvp in woodCount)
        {
            int grade = kvp.Key;
            int count = kvp.Value;
            //print($"Grade: {grade}, Count: {count}");
        }
        dictionaryFinish = true;
    }

    public (string, string,string) CheckAttackSpeedRange(string towerType,int grade)
    {
        int attack = 0;
        int speed = 0;
        int range = 0;
        string attackString = "";
        string speedString = "";
        string rangeString = "";
        switch (towerType)
        {
            case "wood":
                if (grade <= wData.gradeSpeedToAttack)
                {
                    speed = wData.baseBulletNumberPerSecond + grade * wData.upgradeSpeedRate;
                    attack = wData.baseBulletAttack;
                }
                else if(grade > wData.gradeSpeedToAttack)
                {
                    speed = wData.baseBulletNumberPerSecond + wData.gradeSpeedToAttack * wData.upgradeSpeedRate;
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
                    speed = iData.baseBulletNumberPerSecond + grade * iData.upgradeSpeedRate;
                    attack = iData.baseBulletAttack;
                }
                else if(grade > iData.gradeSpeedToAttack)
                {
                    speed = iData.baseBulletNumberPerSecond + iData.gradeSpeedToAttack * iData.upgradeSpeedRate;
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
                    speed = eData.baseBulletNumberPerSecond + grade * eData.upgradeSpeedRate;
                    attack = eData.baseBulletAttack;
                }
                else if(grade > eData.gradeSpeedToAttack)
                {
                    speed = eData.baseBulletNumberPerSecond + eData.gradeSpeedToAttack * eData.upgradeSpeedRate;
                    attack = eData.baseBulletAttack + (grade-eData.gradeSpeedToAttack)*eData.upgradeAttackRate;
                }
                range = eData.basicRange;
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
    public string CheckProtectBlood(string proType,int grade)
    {
        if (wproCount.ContainsKey(grade))
        {
            int count = 0;
            string countString = "";
            switch (proType)
            {
                case "wpro":
                    count = wproCount[grade];
                    countString = count.ToString();
                    break;
                case "ipro":
                    count = iproCount[grade];
                    countString = count.ToString();
                    break;
                case "elec":
                    count = eproCount[grade];
                    countString = count.ToString();
                    break;
                default:
                    Console.WriteLine("Unknown");
                    break;
            }
            return countString;
        }

        return null;

    }
    public int CheckTypeCount(string towerType, int grade)
    {
        int towerCount=0;
        switch (towerType)
        {
            case "wood":
                towerCount = woodCount[grade];
                break;
            case "iron":
                towerCount = ironCount[grade];
                break;
            case "elec":
                towerCount = elecCount[grade];
                break;
            case "wpro":
                towerCount = wproCount[grade];
                break;
            case "epro":
                towerCount = eproCount[grade];
                break;
            case "ipro":
                towerCount = iproCount[grade];
                break;
            default:
                Console.WriteLine("Unknown");
                break;
        }
        return towerCount;
    }
}
