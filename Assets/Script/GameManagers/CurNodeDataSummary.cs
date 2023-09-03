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

    private int _calculateDPS(int grade, int[] dpsRate, int initDps)
    {
        if(grade == 1){
            return initDps;
        }
        for (int i = 1; i < grade; i++) {
            int index = (i - 1) % dpsRate.Length;
            initDps = (initDps * 2 + dpsRate[index])  ;
        }
        return initDps;
    }

    private int _getMonsterNum(int layer)
    {
        return 0;
    }

    private int _getMonsterInterval(int layer)
    {
        return 0;
    }
    
    public int _checkWeaponAttack(int[] weaponDpsRate,int[] weaponRange,int row,int col,int grade,int initDps,int debuff)
    {
        int posIndex = row * 18 + col;
        int range = weaponRange[posIndex];
        int dps = _calculateDPS(grade, weaponDpsRate, initDps);
        if (dps >= debuff)
        {
            dps -= debuff;
        }
        else
        {
            dps = 0;
        }

        int monNum = _getMonsterNum(thisNodeData.nodeLayer+1);
        int monInterval = _getMonsterInterval(thisNodeData.nodeLayer + 1);
        int attack = dps * (range + (monNum - 1) * monInterval);
        return attack;
    }
    
    //usage��
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
