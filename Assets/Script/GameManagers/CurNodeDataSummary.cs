using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CurNodeDataSummary : MonoBehaviour
{
    public static CurNodeDataSummary _instance;
    [Header("--------InitSettings--------")]
    private TowerData wData;
    private TowerData iData;
    private TowerData eData;
    public int mapDisPerUnit;

    [Header("--------GlobalDataRead--------")]
    public NodeData thisNodeData;
    private NodeData previousNodeData;
    public DownNodeData thisDownNodeData;
    private DownNodeData previousDownNodeData;
    public string[][] _mapStruct;
    
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
    private int[] _woodDpsRate =new []{50, 45, 40, 30, 15};
    private int[] _ironDpsRate =new []{55,55,55};
    private int[] _elecDpsRate =new []{25,25,75,75,75,125};
    private int[] _rangeWood1 = new[] {2, 0, 2, 1, 2, 3, 3, 2, 1, 1, 2, 3, 3, 2, 1, 2, 0, 2, 3, 0, 3, 2, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 2, 3, 0, 3, 3, 0, 3, 3, 0, 5, 5, 0, 3, 3, 0, 5, 5, 0, 3, 3, 0, 3, 3, 0, 3, 3, 0, 3, 3, 0, 3, 3, 0, 3, 3, 0, 3, 3, 0, 3, 3, 0, 3, 3, 0, 3, 3, 0, 3, 3, 0, 3, 3, 0, 3, 3, 0, 3, 3, 0, 3, 3, 0, 3, 3, 0, 3, 3, 0, 3, 3, 0, 3, 3, 0, 3, 3, 0, 5, 5, 0, 3, 3, 0, 5, 5, 0, 3, 3, 0, 5, 5, 0, 3, 2, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 2, 1, 2, 3, 3, 2, 1, 1, 2, 3, 3, 2, 1, 1, 2, 3, 3, 2, 1 };
    private int[] _rangeElec1 = new []{1, 0, 3, 2, 3, 4, 3, 2, 2, 2, 3, 4, 3, 2, 3, 1, 0, 2, 2, 0, 5, 2, 0, 0, 0, 0, 4, 2, 0, 0, 0, 0, 5, 2, 0, 3, 2, 0, 6, 3, 0, 8, 4, 0, 6, 3, 0, 8, 4, 0, 6, 2, 0, 3, 2, 0, 6, 2, 0, 6, 2, 0, 6, 2, 0, 6, 2, 0, 6, 2, 0, 3, 2, 0, 6, 2, 0, 6, 2, 0, 6, 2, 0, 6, 2, 0, 6, 2, 0, 3, 2, 0, 6, 2, 0, 6, 2, 0, 6, 2, 0, 6, 2, 0, 6, 2, 0, 3, 3, 0, 8, 4, 0, 6, 3, 0, 8, 4, 0, 6, 3, 0, 8, 4, 0, 3, 2, 0, 0, 0, 0, 4, 2, 0, 0, 0, 0, 4, 2, 0, 0, 0, 0, 2, 2, 3, 4, 3, 2, 2, 2, 3, 4, 3, 2, 2, 2, 3, 4, 3, 2, 1};
    private int[] _rangeElec2 = new []{2, 0, 6, 7, 6, 6, 6, 6, 6, 6, 6, 6, 6, 7, 6, 4, 0, 7, 3, 0, 8, 8, 0, 0, 0, 0, 8, 7, 0, 0, 0, 0, 8, 6, 0, 9, 4, 0, 10, 10, 0, 10, 9, 0, 10, 9, 0, 10, 9, 0, 10, 8, 0, 10, 4, 0, 11, 11, 0, 12, 11, 0, 12, 11, 0, 12, 11, 0, 11, 9, 0, 10, 4, 0, 10, 9, 0, 10, 9, 0, 10, 9, 0, 10, 9, 0, 10, 9, 0, 10, 6, 0, 12, 11, 0, 12, 11, 0, 12, 11, 0, 12, 11, 0, 12, 11, 0, 10, 5, 0, 10, 9, 0, 10, 9, 0, 10, 9, 0, 10, 9, 0, 10, 9, 0, 8, 4, 0, 0, 0, 0, 8, 7, 0, 0, 0, 0, 8, 7, 0, 0, 0, 0, 6, 4, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 4, 4};
    private int[] _rangeElec3 = new []{9, 0, 9, 12, 12, 8, 11, 12, 10, 12, 11, 8, 12, 12, 9, 7, 0, 9, 12, 0, 11, 14, 0, 0, 0, 0, 12, 14, 0, 0, 0, 0, 11, 9, 0, 11, 15, 0, 13, 17, 0, 12, 16, 0, 14, 17, 0, 12, 17, 0, 13, 11, 0, 13, 18, 0, 15, 20, 0, 14, 19, 0, 16, 20, 0, 14, 20, 0, 15, 13, 0, 15, 22, 0, 18, 24, 0, 20, 25, 0, 20, 25, 0, 20, 25, 0, 18, 16, 0, 16, 20, 0, 14, 19, 0, 16, 20, 0, 14, 19, 0, 16, 20, 0, 14, 13, 0, 14, 17, 0, 12, 16, 0, 14, 17, 0, 12, 16, 0, 14, 17, 0, 12, 11, 0, 12, 14, 0, 0, 0, 0, 12, 14, 0, 0, 0, 0, 12, 14, 0, 0, 0, 0, 10, 12, 8, 8, 11, 12, 10, 12, 11, 8, 11, 12, 10, 12, 11, 8, 8, 10, 8};
    private List<int> _woodTotalAtt;
    private List<int> _ironTotalAtt;
    private List<int> _elecTotalAtt;
    private int _monBasicBlood = 70;
    private int _monBasicNum = 3;
    private int _monInterval = 3;
    
    
    [Header("--------ProcessingBool--------")]
    public bool _initData = false;
    public bool gamePlayInitData = false;
    public int choseNodeType;

    [Header("--------LevelReCord--------")]
    public float homeMaxHealth;
    public float homeCurHealth;
    public float moneyLeft;
    public float homeDestroyData = 0;
    //public int monsterCount;
    
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

        debuffList = new int[3];
        debuffListData = new float[3];
        curDebuffList = new int[3];
        majorDebuffList = new int[3];
        majorDebuffListData = new float[3];
        protecListData = new float[3];
        weaponBloodList = new int[3];
        majorProtecListData = new float[3];
        _woodTotalAtt = new List<int>();
        _ironTotalAtt = new List<int>();
        _elecTotalAtt = new List<int>();
        
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
            if (GlobalVar._instance.finalNodePrepared)
            {
                _countDicInit();
                debuffList = thisDownNodeData.debuffData;
                GlobalVar._instance._getMapmapList();
                _mapStruct = GlobalVar._instance.mapmapList;
                (woodCount,ironCount,elecCount,wproCount,iproCount,eproCount) = _checkTypeIndex(_mapStruct);
            }
        }
        //print("WARNING THE IPROCOUNT IS: "+CurNodeDataSummary._instance.iproCount[1]);
        //ENTER GAMEPLAY!!
        if (GlobalVar._instance.thisUserAddr != null)
        {
            if (GlobalVar.CurrentGameState == GlobalVar.GameState.GamePlay || GlobalVar.CurrentGameState == GlobalVar.GameState.ChooseField)
            {
                if (GlobalVar._instance.role == 0)
                {
                    if (!gamePlayInitData)
                    {
                        _countDicInit();
                        GlobalVar._instance.chosenDownNodeData = GlobalVar._instance._checkDownNodeMain(thisNodeData.nodeLayer + 1);
                        thisDownNodeData = GlobalVar._instance.chosenDownNodeData;
                        debuffList = thisDownNodeData.debuffData;

                        Debug.Log("Chosen Down Node is: " + GlobalVar._instance.chosenDownNodeData.nodeLayer + "," + GlobalVar._instance.chosenDownNodeData.nodeIndex);
                        Debug.Log("debuff list after chose up is: " + debuffList[0] + "," + debuffList[1] + "," + debuffList[2]);
                        GlobalVar._instance._getMapmapList();
                        _mapStruct = GlobalVar._instance.mapmapList;
                        (woodCount,ironCount,elecCount,wproCount,iproCount,eproCount) = _checkTypeIndex(_mapStruct);
                        weaponBloodList = GetMainMaxWeaponLevelBlood(woodCount,ironCount,elecCount);
                        protectList = GetMainProtectBlood(wproCount,iproCount,eproCount);
                        gamePlayInitData = true;
                        GetGamPlayNodeInfo();
                    }
                }
                
                else
                {
                    //enter the sec game play mode, need to refresh the up tree main node
                    if (!gamePlayInitData)
                    {
                        _initData = true;
                        _countDicInit();
                        GlobalVar._instance.chosenNodeData =
                            GlobalVar._instance._checkUpNodeMain(thisDownNodeData.nodeLayer + 1);
                        thisNodeData = GlobalVar._instance.chosenNodeData;
                        Debug.Log("===========ReFresh UpNodeMain ================ ");
                        Debug.Log("Chosen Down Node is: " + thisNodeData.nodeLayer + "," + thisNodeData.nodeIndex);
                        _initData = true;

                        debuffList = thisDownNodeData.debuffData;
                        Debug.Log("debuff list after chose down is: " + debuffList[0] + "," + debuffList[1] + "," +
                                  debuffList[2]);

                        GlobalVar._instance._getMapmapList();
                        _mapStruct = GlobalVar._instance.mapmapList;
                        (woodCount,ironCount,elecCount,wproCount,iproCount,eproCount) = _checkTypeIndex(_mapStruct);
                        weaponBloodList = GetMainMaxWeaponLevelBlood(woodCount,ironCount,elecCount);
                        protectList = GetMainProtectBlood(wproCount,iproCount,eproCount);
                        gamePlayInitData = true;
                    }
                }
            }
        }

        previousNodeData = thisNodeData;
        previousDownNodeData = thisDownNodeData;
    }
    public void GetGamPlayNodeInfo()
    {
        _countDicInit();
        debuffList = GlobalVar._instance.chosenDownNodeData.debuffData;
        GlobalVar._instance._getMapmapList();
        _mapStruct = GlobalVar._instance.mapmapList;
        (woodCount,ironCount,elecCount,wproCount,iproCount,eproCount) = _checkTypeIndex(_mapStruct);
        _initData = true;
    }
    public void GetChosenNodeInfo()
    {
        _countDicInit();
        debuffList = GlobalVar._instance.chosenDownNodeData.debuffData;
        GlobalVar._instance._getMapmapList();
        _mapStruct = GlobalVar._instance.mapmapList;
        (woodCount,ironCount,elecCount,wproCount,iproCount,eproCount) = _checkTypeIndex(_mapStruct);
        _initData = true;
    }

    public int[] GetMainProtectBlood( Dictionary<int,int>_wproCount,Dictionary<int,int> _iproCount,Dictionary<int,int> _eproCount)
    {
        int[] weaponTotalProtect = new int[3];
        if (_wproCount != null)
        {
            foreach (int grade in _wproCount.Keys)
            {
                print("wpro grade: "+grade+" wpro num: "+_wproCount[grade]);
                weaponTotalProtect[0] += _wproCount[grade]* _gradeToProtect(grade)*GlobalVar._instance.ProWood.baseProtect;
            }
        }
        if (_iproCount != null)
        {
            foreach (int grade in _iproCount.Keys)
            {
                print("ipro grade: "+grade+" ipro num: "+_iproCount[grade]);
                weaponTotalProtect[1] += _iproCount[grade] * _gradeToProtect(grade)*GlobalVar._instance.ProIron.baseProtect;
            }
        }
        if (_eproCount != null)
        {
            foreach (int grade in _eproCount.Keys)
            {
                print("epro grade: "+grade+" epro num: "+_eproCount[grade]);
                weaponTotalProtect[2] += _eproCount[grade] * _gradeToProtect(grade)*GlobalVar._instance.ProElec.baseProtect;
            }
        }
        return weaponTotalProtect;
    }
    
    public int[] GetMainMaxWeaponLevelBlood(Dictionary<int,int> _woodCount,Dictionary<int,int> _ironCount,Dictionary<int,int>_elecCount)
    { 
        string _at, _sp, _ra;
        int[] weaponTotalBlood = new int[3];
        if (_woodCount != null)
        {
            int maxGrade = 0;
            foreach (int grade in _woodCount.Keys)
            {
                if (grade >= maxGrade)
                {
                    maxGrade = grade;
                }
            }
            (_at,_sp,_ra) = CheckAttackSpeedRange("wood", maxGrade);
            weaponTotalBlood[0] = int.Parse(_at) * int.Parse(_sp);
        }
        if (_ironCount != null)
        {
            int maxGrade = 0;
            foreach (int grade in _ironCount.Keys)
            {
                if (grade >= maxGrade)
                {
                    maxGrade = grade;
                }
            }
            (_at,_sp,_ra) = CheckAttackSpeedRange("iron", maxGrade);
            weaponTotalBlood[1] = int.Parse(_at) * int.Parse(_sp);
        }
        if (_elecCount != null)
        {
            int maxGrade = 0;
            foreach (int grade in _elecCount.Keys)
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
    
    public (Dictionary<int, int>,Dictionary<int, int>,Dictionary<int, int>,Dictionary<int, int>,Dictionary<int, int>,Dictionary<int, int>) _checkTypeIndex(string[][] mapStruct)
    {
        print("counting now!");
        Dictionary<int, int> _woodCount = new Dictionary<int, int>();
        Dictionary<int, int>_ironCount= new Dictionary<int, int>();
        Dictionary<int, int> _elecCount = new Dictionary<int, int>();
        Dictionary<int, int> _wproCount = new Dictionary<int, int>();
        Dictionary<int, int>_iproCount= new Dictionary<int, int>();
        Dictionary<int, int> _eproCount = new Dictionary<int, int>();
        for (int i = 0; i < mapStruct.Length; i++)
        {
            //print("row: "+i+" map: "+mapStruct[i]);
            //string mapRow = "";
            for (int j = 0; j < mapStruct[i].Length; j++)
            {
                if (mapStruct[i][j].Length >= 5)
                {
                    string mapType = mapStruct[i][j].Substring(0, 4);
                    int towerGrade = int.Parse(mapStruct[i][j].Substring(4, mapStruct[i][j].Length-4));
                    if (mapType == "wood")
                    {
                        if (!_woodCount.ContainsKey(towerGrade))
                        {
                            _woodCount.Add(towerGrade, 1);
                        }
                        else
                        {
                            _woodCount[towerGrade] += 1;
                        }
                    }
                    else if(mapType == "iron")
                    {
                        if (!_ironCount.ContainsKey(towerGrade))
                        {
                            _ironCount.Add(towerGrade, 1);
                        }
                        else
                        {
                            _ironCount[towerGrade] += 1;
                        }
                    }
                    else if (mapType == "elec")
                    {
                        mapStruct[i][j + 1] = "eleC"+towerGrade;
                        if (!_elecCount.ContainsKey(towerGrade))
                        {
                            _elecCount.Add(towerGrade, 1);
                        }
                        else
                        {
                            _elecCount[towerGrade] += 1;
                        }
                    }
                    else if (mapType == "wpro")
                    {
                        if (!_wproCount.ContainsKey(towerGrade))
                        {
                            _wproCount.Add(towerGrade, 1);
                        }
                        else
                        {
                            _wproCount[towerGrade] += 1;
                        }
                        print("WARNING THE IPROCOUNT IS: "+CurNodeDataSummary._instance.iproCount[1]);
                    }
                    else if (mapType == "ipro")
                    {
                        if (!_iproCount.ContainsKey(towerGrade))
                        {
                            _iproCount.Add(towerGrade, 1);
                        }
                        else
                        {
                            _iproCount[towerGrade] += 1;
                        }
                    }
                    else if (mapType == "epro")
                    {
                        //Debug.Log("A EPRO!!!!!!!!!");
                        if (!_eproCount.ContainsKey(towerGrade))
                        {
                            _eproCount.Add(towerGrade, 1);
                        }
                        else
                        {
                            _eproCount[towerGrade] += 1;
                        }
                    }
                    else if (mapType == "eleC")
                    {
                        continue;
                    }
                    else
                    {
                        Debug.Log(mapType + ": "+towerGrade);
                        Debug.LogError("incorrect Map String!!!");
                    }
                    //mapRow += mapType+",";
                }
            }
            //Debug.Log(mapRow);
        }

        // foreach (var VARIABLE in _eproCount)
        // {
        //     print(VARIABLE);
        // }
        return (_woodCount, _ironCount, _elecCount, _wproCount, _iproCount, _eproCount);
    }
    
    public (string, string,string) CheckAttackSpeedRange(string towerType,int grade)
    {
        float attack = 0;
        float speed = 0;
        int range = 0;
        string attackString = "";
        string speedString = "";
        string rangeString = "";
        switch (towerType)
        {
            case "wood":
                speed = wData.baseBulletNumberPerSecond;
                attack = (_calculateDPS(grade,_woodDpsRate,20)/speed);
                range = wData.basicRange;
                attackString = ((int)(attack)).ToString();
                speedString = speed.ToString();
                rangeString = range.ToString();
                break;
            case "iron":
                speed = iData.baseBulletNumberPerSecond;
                attack = (_calculateDPS(grade,_ironDpsRate,30)/speed);
                range = iData.basicRange;
                attackString = ((int)(attack)).ToString();
                speedString = speed.ToString();
                rangeString = "full map";
                break;
            case "elec":
                if (grade < eData.gradeRange2)
                {
                    range = eData.basicRange;
                }
                else if(grade >= eData.gradeRange2 &&grade < eData.gradeRange3)
                {
                    range = eData.basicRange+1;
                }
                else
                {
                    range = eData.basicRange+2;
                }
                speed = eData.baseBulletNumberPerSecond;
                attack = (_calculateDPS(grade,_elecDpsRate,40)/speed);
                attackString = ((int)(attack)).ToString();
                speedString = speed.ToString();
                rangeString = range.ToString();
                break;
            default:
                Console.WriteLine("Unknown");
                break;
        }
        return (attackString,speedString,rangeString);
    }
        public (float, float,float) CheckAttackSpeedRangeFloat(string towerType,int grade)
        {
            float attack = 0;
            float speed = 0;
            float range = 0;

            switch (towerType)
            {
                case "wood":
                    speed = wData.baseBulletNumberPerSecond;
                    attack = (_calculateDPS(grade,_woodDpsRate,20)/speed);
                    range = wData.basicRange * mapDisPerUnit;
                    break;
                case "iron":
                    speed = iData.baseBulletNumberPerSecond;
                    attack = (_calculateDPS(grade,_ironDpsRate,30)/speed);
                    range = iData.basicRange*mapDisPerUnit;
                    break;
                case "elec":
                    if (grade < eData.gradeRange2)
                    {
                        range = eData.basicRange;
                    }
                    else if(grade >= eData.gradeRange2 &&grade < eData.gradeRange3)
                    {
                        range = eData.basicRange+1;
                    }
                    else
                    {
                        range = eData.basicRange+2;
                    }

                    range *= mapDisPerUnit;
                    speed = eData.baseBulletNumberPerSecond;
                    attack = (_calculateDPS(grade,_elecDpsRate,40)/speed);
                    break;
                default:
                    Console.WriteLine("Unknown");
                    break;
            }
            return (attack,speed,range);
        }

    private int _calculateDPS(int grade, int[] dpsRate, int initDps)
    {
        if (grade == 0)
        {
            return 0;
        }
        if(grade == 1){
            return initDps;
        }
        for (int i = 1; i < grade; i++) {
            int index = (i - 1) % dpsRate.Length;
            initDps = (initDps * 2 + dpsRate[index])  ;
        }
        return initDps;
    }

    public int[] _gamePlayGetRowCol(Transform turrentPos)
    {
        string rowColString = turrentPos.parent.name;
        string rowString = rowColString.Substring(0, 1);
        string colString = rowColString.Substring(1, 2);
        int row, col = 0;
        if (colString.Substring(0, 1) == "0")
        {
            col = int.Parse(colString.Substring(1, 1));
        }
        else
        {
            col = int.Parse(colString);
        }

        row = int.Parse(rowString);
       print("the row for this turret is: "+row);
       print("the col for this turret is: "+col);
        return new[] { row, col };
    }

    public (int, int, int) ReCheckProtectData()
    {
        protectList = GetMainProtectBlood(wproCount, iproCount, eproCount);
        print("just before game start, the protect list is: "+protectList[0]+" , "+protectList[1]+" , "+protectList[2]);
        return (protectList[0],protectList[1],protectList[2]);
    }

    public (float,float) CheckAttackAfterDebuff(string weaponType, int weaponDebuff, int weaponGrade,float weaponBulletNumPerSec,Transform turretTransform)
    {
        float idealDps = 0;
        float idealAttack=0;
        float attackTime;
        int[] rowCol = _gamePlayGetRowCol(turretTransform);
        ReCheckProtectData();
        if (weaponType == "wood")
        {
            idealDps = _calculateDPS(weaponGrade,_woodDpsRate,20);
            (idealAttack,attackTime) = _checkTotalWeaponAttack(_woodDpsRate, _rangeWood1, rowCol[0], rowCol[1], weaponGrade, 20,
                weaponDebuff, 0,protectList[0]);
            print("the real dps of wood is: " + idealDps);
            print("the real debuff of wood is: " + weaponDebuff);
            print("the real total attack of wood is: " + idealAttack);
        }
        else if(weaponType == "iron"){
            idealDps = _calculateDPS(weaponGrade,_ironDpsRate,30);
            (idealAttack,attackTime) = _checkTotalWeaponAttack(_ironDpsRate, _rangeWood1, rowCol[0], rowCol[1], weaponGrade, 30,
                weaponDebuff, 1,protectList[1]);
            print("the real dps of iron is: " + idealDps);
            print("the real debuff of iron is: " + weaponDebuff);
            print("the real total attack of iron is: " + idealAttack);
        }
        else
        {
            idealDps = _calculateDPS(weaponGrade,_elecDpsRate,40);
            if (weaponGrade >= 0 && weaponGrade < GlobalVar._instance.elecTowerData.gradeRange2)
            {
                (idealAttack,attackTime) = _checkTotalWeaponAttack(_elecDpsRate, _rangeElec1, rowCol[0], rowCol[1], weaponGrade, 40, weaponDebuff, 2,protectList[2]);
            }
            else if (weaponGrade >= GlobalVar._instance.elecTowerData.gradeRange2 &&
                     weaponGrade < GlobalVar._instance.elecTowerData.gradeRange3)
            {
                (idealAttack,attackTime) = _checkTotalWeaponAttack(_elecDpsRate, _rangeElec2, rowCol[0], rowCol[1], weaponGrade, 40, weaponDebuff, 2,protectList[2]);
            }
            else
            {
                (idealAttack,attackTime) = _checkTotalWeaponAttack(_elecDpsRate, _rangeElec3, rowCol[0], rowCol[1], weaponGrade, 40, weaponDebuff, 2,protectList[2]);
            }
            print("the real dps of elec is: " + idealDps);
            print("the real debuff of elec is: " + weaponDebuff);
            print("the real total attack of elec is: " + idealAttack);
        }
        
        float realDps = 0;
        realDps = idealDps - (float)weaponDebuff;
        if (realDps <= 0)
        {
            realDps = 0;
        }

        //return (realDps) / (weaponBulletNumPerSec);
        return ((idealAttack / attackTime / weaponBulletNumPerSec),idealAttack);
    }

    public int GetMonsterNum(int layer)
    {
        int monNum = _monBasicNum +  layer;
        //print("this level one monster num is: "+monNum);
        return monNum;
    }

    public int GetMonsterBlood(int layer)
    {
        int monBlood = _monBasicBlood + 70 * layer;
        //print("this level one monster blood is: "+monBlood);
        return monBlood;
    }

    public int GetMonsterInterval(int layer)
    {
        return _monInterval;
    }
    
    public (float,float) _checkTotalWeaponAttack(int[] weaponDpsRate,int[] weaponRange,int row,int col,int grade,int initDps,int debuff,int weaponType,int weaponProtect)
    {
        int posIndex = row * 18 + col;
        int range = weaponRange[posIndex];
        print("the range of weapon "+weaponType+" is: "+range);
        print("the protect of weapon "+weaponType+" is: "+weaponProtect);
        float time = 0;
        int dps = _calculateDPS(grade, weaponDpsRate, initDps);
        if (weaponProtect >= debuff)
        {
            debuff = 0;
        }
        else
        {
            debuff -= weaponProtect;
        }
        
        
        
        if (dps >= debuff)
        {
            dps -= debuff;
        }
        else
        {
            dps = 0;
        }
        print("the DPS after debuff and protect of weaponType "+weaponType+" is: "+dps);
        
        int monNum = GetMonsterNum(thisNodeData.nodeLayer+1);
        int monInterval = GetMonsterInterval(thisNodeData.nodeLayer + 1); //3
        float attack = 0;
        if (weaponType == 0)
        {
            attack = dps * (range + (monNum - 1) * monInterval);
            time = (range + (monNum - 1) * monInterval);
            print("the total distance of wood is: "+ (range + (monNum - 1) * monInterval));//15=3+(3+2-1)*3
        }
        else if (weaponType == 1)
        {
            attack = dps * 10;
            time = 53;
        }
        if (weaponType == 2)
        {
            attack = dps * (range/2 + (monNum - 1) * monInterval);
            time = (range / 2 + (monNum - 1) * monInterval);
        }
        print("the attacking ideal time of weaponType "+weaponType+" is: "+time);
        print("the total of weaponType "+weaponType+" is: "+attack);
        return (attack,time); //53 time!!
    }
    public void reduceMoney(int moneyAmount)
    {
        moneyLeft += moneyAmount;
    }


    
    //usage£º
    // for (uint256 i=1; i <= 19*9; i++){
    //     bytes memory bytesStr = bytes(map_map[recent_position_index][i]);
    //     if(bytesStr.length >= 5){
    //         bytes1 first_byte_tower = bytesStr[0];
    //      //get type
    //     bytes memory grade_byt = new bytes(bytesStr.length - 4);
    //     for (uint256 j = 4; j < bytesStr.length; j++) {
    //         grade_byt[j - 4] = bytesStr[j];
    //     }        
    //     string memory _grade_str = string(grade_byt);
    //     uint256 grade = stringToUint(_grade_str);
    //
    //     //wood
    //     if(first_byte_tower == bytes1("w")){
    //         _range = range1_string_wood[i];
    //         _dps = calculateDPS(grade,wood_dps_rate,basic_tower["wood"].dps); 
    //         _dps = _compare(_dps,de_wood_attack);
    //         total_attack += _dps * (_range+(basic_monster[lyr_pr].m_number-1)*basic_monster[lyr_pr].m_interval);
    //     }
    //     //Tower "iron"
    //     else if(first_byte_tower == bytes1("i")){
    //         _dps = calculateDPS(grade,iron_dps_rate,basic_tower["iron"].dps);
    //         _dps = _compare(_dps,de_iron_attack);
    //         total_attack += (10)* _dps;
    //     }
    //     //Tower "elec"
    //     else if(first_byte_tower == bytes1("e")&& canNotCal == false){
    //         canNotCal = true;
    //         _dps = calculateDPS(grade,elec_dps_rate,basic_tower["elec"].dps);
    //         _dps = _compare(_dps,de_elec_attack);
    //         if(grade>0&&grade<5){
    //             _range = range1_string_elec[i];
    //         }
    //         else if(grade>=5&&grade<10){ 
    //             _range = range2_string_elec[i];
    //         }
    //         else{ 
    //             _range = range3_string_elec[i];
    //         }
    //         total_attack+=_dps*(_range/2+(basic_monster[lyr_pr].m_number-1)*basic_monster[lyr_pr].m_interval);
    //     }
    // else if(first_byte_tower == bytes1("e")&& canNotCal == true){ 
    //         canNotCal = false; 
    //         }
    //     }
    // }
    // total_monster_blood = basic_monster[lyr_pr].m_blood*basic_monster[lyr_pr].m_number;
    // if(total_monster_blood >total_attack && home_health > total_monster_blood-total_attack ){ 
    //     home_health = home_health + total_attack- total_monster_blood ;
    //     map_money[msg.sender] = map_money[msg.sender] + 500;
    // }
    // else if(total_monster_blood >total_attack && home_health < total_monster_blood-total_attack){
    //     home_health = 0;
    // }else{
    //     map_money[msg.sender] = map_money[msg.sender] + 500;
    // }
    
    //struct:
    //     basic_tower["wood"] = in_tower({
    //         _type: "wood",
    //
    //         dps: 20,
    //
    //         basic_price: 100,
    //         merge_price: 40,
    //         pro_price: 290
    //     });
    // basic_tower["iron"] = in_tower({
    //     _type: "iron",
    //
    //     dps: 30,
    //
    //     basic_price: 300,
    //     merge_price: 60,
    //     pro_price: 290
    // });
    // basic_tower["elec"] = in_tower({
    //     _type: "elec",
    //
    //     dps: 40,
    //
    //     basic_price: 600,
    //     merge_price: 100,
    //     pro_price: 290
    // });
    //     uint256[] range1_string_wood = [2, 0, 2, 1, 2, 3, 3, 2, 1, 1, 2, 3, 3, 2, 1, 2, 0, 2, 3, 0, 3, 2, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 2, 3, 0, 3, 3, 0, 3, 3, 0, 5, 5, 0, 3, 3, 0, 5, 5, 0, 3, 3, 0, 3, 3, 0, 3, 3, 0, 3, 3, 0, 3, 3, 0, 3, 3, 0, 3, 3, 0, 3, 3, 0, 3, 3, 0, 3, 3, 0, 3, 3, 0, 3, 3, 0, 3, 3, 0, 3, 3, 0, 3, 3, 0, 3, 3, 0, 3, 3, 0, 3, 3, 0, 3, 3, 0, 3, 3, 0, 5, 5, 0, 3, 3, 0, 5, 5, 0, 3, 3, 0, 5, 5, 0, 3, 2, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 2, 1, 2, 3, 3, 2, 1, 1, 2, 3, 3, 2, 1, 1, 2, 3, 3, 2, 1];
    //     uint256[] range1_string_elec = [1,0,3,2,3,4,3,2,2,2,3,4,3,2,3,1,0,2,0,2,0,5,2,0,0,0,0,4,2,0,0,0,0,5,2,0,3,0,2,0,6,3,0,8,4,0,6,3,0,8,4,0,6,2,0,3,0,2,0,6,2,0,6,2,0,6,2,0,6,2,0,6,2,0,3,0,2,0,6,2,0,6,2,0,6,2,0,6,2,0,6,2,0,3,0,2,0,6,2,0,6,2,0,6,2,0,6,2,0,6,2,0,3,0,3,0,8,4,0,6,3,0,8,4,0,6,3,0,8,4,0,3,0,2,0,0,0,0,4,2,0,0,0,0,4,2,0,0,0,0,2,0,2,3,4,3,2,2,2,3,4,3,2,2,2,3,4,3,2,1,0];
    //     uint256[] range2_string_elec = [2,0,6,7,6,6,6,6,6,6,6,6,6,7,6,4,0,7,0,3,0,8,8,0,0,0,0,8,7,0,0,0,0,8,6,0,9,0,4,0,10,10,0,10,9,0,10,9,0,10,9,0,10,8,0,10,0,4,0,11,11,0,12,11,0,12,11,0,12,11,0,11,9,0,10,0,4,0,10,9,0,10,9,0,10,9,0,10,9,0,10,9,0,10,0,6,0,12,11,0,12,11,0,12,11,0,12,11,0,12,11,0,10,0,5,0,10,9,0,10,9,0,10,9,0,10,9,0,10,9,0,8,0,4,0,0,0,0,8,7,0,0,0,0,8,7,0,0,0,0,6,0,4,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,4,4,0];
    //     uint256[] range3_string_elec = [9,0,9,12,12,8,11,12,10,12,11,8,12,12,9,7,0,9,0,12,0,11,14,0,0,0,0,12,14,0,0,0,0,11,9,0,11,0,15,0,13,17,0,12,16,0,14,17,0,12,17,0,13,11,0,13,0,18,0,15,20,0,14,19,0,16,20,0,14,20,0,15,13,0,15,0,22,0,18,24,0,20,25,0,20,25,0,20,25,0,18,16,0,16,0,20,0,14,19,0,16,20,0,14,19,0,16,20,0,14,13,0,14,0,17,0,12,16,0,14,17,0,12,16,0,14,17,0,12,11,0,12,0,14,0,0,0,0,12,14,0,0,0,0,12,14,0,0,0,0,10,0,12,8,8,11,12,10,12,11,8,11,12,10,12,11,8,8,10,8,0];
    //     uint256[] wood_dps_rate = [50, 45, 40, 30, 15];
    //     uint256[] iron_dps_rate = [55,55,55];
    //     uint256[] elec_dps_rate =  [25,25,75,75,75,125];

    // basic_monster[c_lyr+1].m_blood = basic_monster[c_lyr].m_blood +70;
    // basic_monster[c_lyr+1].m_number = basic_monster[c_lyr].m_number +1;
    // basic_monster[c_lyr+1].m_interval = 3;

}
