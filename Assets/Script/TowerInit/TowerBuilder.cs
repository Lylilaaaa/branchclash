using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerBuilder : MonoBehaviour
{
    private bool _finishViewing=false;
    private string[][] _mapmapList;
    public UIPanalManager _UIPanalManager;
    
    [Header("MERGE INFO")]
    public List<string> targetWeaponPos;
    public List<int> mergeFromPos;
    public List<int> mergeToPos;
    public string targetWeaponType;
    public int targetWeaponGrade;
    public bool mergeChosen = false;
    public bool mergeButtConfirm = false;

    [Header("SCRIPTABLE OBJ")]
    public List<TowerData> towerDataList;
    public List<ProtectData> protectDataList;
    private void Update()
    {
        if (GlobalVar._instance.mapmapList != null && _finishViewing == false )
        {
            _mapmapList = GlobalVar._instance.mapmapList;
            for (int i = 0; i < _mapmapList.Length; i++)
            {
                for (int j = 0; j < _mapmapList[i].Length; j++)
                {
                    if (_mapmapList[i][j].Length >= 5)
                    {
                        String towerType = _mapmapList[i][j].Substring(0, 4);
                        int towerGrade = int.Parse(_mapmapList[i][j].Substring(4, _mapmapList[i][j].Length - 4));
                        if (j <= 9)
                        {
                            GlobalVar._instance.targetField = i.ToString() + "0" + j.ToString();
                        }
                        else
                        {
                            GlobalVar._instance.targetField = i.ToString() + j.ToString();
                        }
                        PreSet(towerType,towerGrade);
                    }
                }
            }
            _finishViewing = true;
        }
        globalDataUpdate();
        
        
        
        //?why
        // if (GlobalVar._instance.finishAdd==true && finish == false)
        // {
        //     finish = true;
        //     Set("wood");
        //     changeStateChoseField();
        //     _UIPanalManager.reduceMoney(-40);
        //
        // }
    }

    private void setField(GameObject thisPos,bool selectedUI, int _woodType, int _ironType, int _eleType)
    {
        FieldInit fieldInit = thisPos.GetComponent<FieldInit>();
        if (fieldInit != null)
        {
            fieldInit.selected = selectedUI;
            fieldInit.woodType = _woodType;
            fieldInit.ironType = _ironType;
            fieldInit.eleType = _eleType;
        }
    }

    public void PreSet(string setTowerType,int _grade)
    {
        string rowColStr = GlobalVar._instance.targetField;
        List<int> row_col;
        row_col = findRowCol(rowColStr);
        GameObject thisField = transform.GetChild(row_col[0]).GetChild(row_col[1]).gameObject;
        //print(thisField);
        if (thisField.tag != "Road")
        {
            FieldInit fieldInit = thisField.GetComponent<FieldInit>();
            fieldInit.towerType = setTowerType;
            if (fieldInit.selected == false && fieldInit.woodType==0 && fieldInit.ironType==0 && fieldInit.eleType==0 && fieldInit.eleCType==0)
            {
                if (setTowerType == "wood")
                {
                    fieldInit.woodType = _grade;
                }
                else if (setTowerType == "iron")
                {
                    fieldInit.ironType = _grade;
                }
                else if (setTowerType == "elec")
                {
                    fieldInit.eleType = _grade;
                }
                else if(setTowerType == "wpro")
                {
                    fieldInit.wproType = _grade;
                }
                else if(setTowerType == "ipro")
                {
                    fieldInit.iproType = _grade;
                }
                else if(setTowerType == "epro")
                {
                    fieldInit.eproType = _grade;
                }
                else if (setTowerType == "eleC")
                {
                    fieldInit.eleCType = _grade;
                }
            }
        }

    }
    
    public void Set(string setTowerType)
    {
        string rowColStr = GlobalVar._instance.targetField;
        List<int> row_col;
        row_col = findRowCol(rowColStr);
        GameObject thisField = transform.GetChild(row_col[0]).GetChild(row_col[1]).gameObject;
        //print(thisField);
        if (thisField.tag != "Road")
        {
            FieldInit fieldInit = thisField.GetComponent<FieldInit>();
            fieldInit.towerType = setTowerType;
            if (fieldInit.selected == false && fieldInit.woodType==0 && fieldInit.ironType==0 && fieldInit.eleType==0&& fieldInit.wproType==0 && fieldInit.iproType==0 && fieldInit.eproType==0)
            {
                if (setTowerType == "wood")
                {
                    fieldInit.woodType = 1;
                    //ContractInteraction._instance.EditAddTower();
                }
                else if (setTowerType == "iron")
                {
                    fieldInit.ironType = 1;
                }
                else if (setTowerType == "elec")
                {
                    try
                    {
                        GameObject thisFieldRight = transform.GetChild(row_col[0]).GetChild(row_col[1]+1).gameObject;
                        FieldInit fieldInitRight = thisFieldRight.GetComponent<FieldInit>();
                        fieldInitRight.eleCType = 1;
                        fieldInit.eleType = 1;
                    }
                    catch
                    {
                        Debug.Log("cannot build elec Tower here!");
                    }
                    
                }
                else if (setTowerType == "wpro")
                {
                    fieldInit.wproType = 1;
                    //ContractInteraction._instance.EditAddTower();
                }
                else if (setTowerType == "ipro")
                {
                    fieldInit.iproType = 1;
                }
                else if (setTowerType == "epro")
                {
                    fieldInit.eproType = 1;
                }
            }
        }
    }
    private List<int> findRowCol(string rowColString)
    {
        int row = int.Parse(rowColString.Substring(0, 1));
        int col = 0;
        if (rowColString.Substring(1, 1) == "0")
        {
            col = int.Parse(rowColString.Substring(2, 1));
        }
        else
        {
            col = int.Parse(rowColString.Substring(1, 2));
        }

        List<int> row_col = new List<int>();
        row_col.Add(row);
        row_col.Add(col);
        return row_col;
    }

    public void CloseSelected()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 18; j++)
            {
                GameObject thisField = transform.GetChild(i).GetChild(j).gameObject;
                //print(thisField);
                if (thisField.tag != "Road")
                {
                    FieldInit fieldInit = thisField.GetComponent<FieldInit>();
                    fieldInit.selected = false;
                }
            }
        }
    }

    public void changeStateChoseField()
    {
        if (GlobalVar._instance.GetState() == GlobalVar.GameState.AddTowerUI)
        {
            GlobalVar._instance.ChangeState("ChooseField");
        }
    }

    public void MergeButt()
    {
        mergeButtConfirm = true;
    }

    public void Merge(List<int> mergeFrom, List<int> mergeTo)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 18; j++)
            {
                //要删除的塔
                if (mergeFrom[0] == i && mergeFrom[1] == j)
                {
                    GameObject thisField = transform.GetChild(i).GetChild(j).gameObject;
                    FieldInit fieldInit = thisField.GetComponent<FieldInit>();
                    _initFI(fieldInit);
                }
                //要升级的塔
                if (mergeTo[0] == i && mergeTo[1] == j)
                {
                    GameObject thisField = transform.GetChild(i).GetChild(j).gameObject;
                    FieldInit fieldInit = thisField.GetComponent<FieldInit>();
                    GameplayCurSorOutline gpCurSorOutline =
                        thisField.transform.GetChild(0).GetComponent<GameplayCurSorOutline>();
                    _levelUpFI(fieldInit);
                    gpCurSorOutline.isMergeToPotential = false;
                }
            }
        }
        GlobalVar._instance.ChangeState("ChooseField");
        mergeButtConfirm = false;
        mergeFrom = new List<int>();
        mergeTo = new List<int>();
    }

    private void _levelUpFI(FieldInit fi)
    {
        if (fi.woodType != 0)
        {
            fi.woodType += 1;
        }
        else if (fi.ironType != 0)
        {
            fi.ironType += 1;
        }
        else if (fi.eleType != 0)
        {
            fi.eleType += 1;
        }
        else if (fi.eleCType != 0)
        {
            print("eleC cannot be merged to!!");
        }
        else if (fi.wproType != 0)
        {
            fi.wproType += 1;
        }
        else if (fi.iproType != 0)
        {
            fi.iproType += 1;
        }
        else if (fi.eproType != 0)
        {
            fi.eproType += 1;
        }
    }

    private void _initFI(FieldInit fi)
    {
        fi.woodType = 0;
        fi.ironType = 0;
        fi.eleType = 0;
        fi.eleCType = 0;
        fi.wproType = 0;
        fi.ironType = 0;
        fi.eproType = 0;
    }

    public void CallAddContract()
    {
        //ContractInteraction._instance.EditAddTower();
    }
    public void BuildWeapon(string weaponType)
    { 
        Set(weaponType);
        changeStateChoseField();
        _UIPanalManager.reduceMoney(-moneyBuild(weaponType));
    }

    private int moneyBuild(string weaponType)
    {
        if (weaponType == "wood")
        {
            return towerDataList[0].basePrice;
        }
        else if (weaponType == "iron")
        {
            return towerDataList[1].basePrice;
        }
        else if (weaponType == "elec")
        {
            return towerDataList[2].basePrice;
        }
        else if (weaponType == "wpro")
        {
            return protectDataList[0].basePrice;
        }
        else if (weaponType == "ipro")
        {
            return protectDataList[1].basePrice;
        }
        else if (weaponType == "epro")
        {
            return protectDataList[2].basePrice;
        }
        else
        {
            return 10000000;
        }
    }

    private void globalDataUpdate()
    {
        if (GlobalVar._instance.GetState() == GlobalVar.GameState.ChooseField ||
            GlobalVar._instance.GetState() == GlobalVar.GameState.AddTowerUI ||
            GlobalVar._instance.GetState() == GlobalVar.GameState.MergeTowerUI)
        {
            string[][] newStringList = new string[9][];
            string[][] originStringList = new string[9][];
            newStringList = GlobalVar._instance.mapmapList;
            originStringList = GlobalVar._instance.mapmapList;
            
            for (int i = 0; i < newStringList.Length; i++)
            {
                for (int j = 0; j < newStringList[i].Length; j++)
                {
                    string rowColStr;
                    if (j <= 9)
                    {
                        rowColStr = i.ToString() + "0" + j.ToString();
                    }
                    else
                    {
                        rowColStr = i.ToString() + j.ToString();
                    }
                    List<int> row_col;
                    row_col = findRowCol(rowColStr);

                    GameObject thisField = transform.GetChild(row_col[0]).GetChild(row_col[1]).gameObject;
                    if (thisField.tag != "Road")
                    {
                        FieldInit fieldInit = thisField.GetComponent<FieldInit>();
                        string towerRead = fieldInit.towerType;
                        if (towerRead.Length >= 5)
                        {
                            newStringList[i][j] = towerRead;
                            if (thisField.transform.childCount!=0)
                            {
                                GameplayCurSorOutline gpCurSorOutline =
                                    thisField.transform.GetChild(0).GetComponent<GameplayCurSorOutline>();
                                if (towerRead.Length >= 5)
                                {
                                    print(gpCurSorOutline.transform.name);
                                    gpCurSorOutline.weaponType = towerRead.Substring(0, 4);
                                    gpCurSorOutline.weaponGrade = int.Parse(towerRead.Substring(4));
                                }
                            }

                        }
                    }
                }
            }
            GlobalVar._instance.mapMapListEdit = newStringList;
            GlobalVar._instance.mapmapList = originStringList;
        }
    }

    public List<string> checkWeaponPos(string weaponType, int weaponGrade)
    {
        List<string> sameTypeGrade = new List<string>();
        if (GlobalVar._instance.GetState() == GlobalVar.GameState.MergeTowerUI)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 18; j++)
                {
                    string rowColStr;
                    if (j <= 9)
                    {
                        rowColStr = i.ToString() + "0" + j.ToString();
                    }
                    else
                    {
                        rowColStr = i.ToString() + j.ToString();
                    }
                    List<int> row_col;
                    row_col = findRowCol(rowColStr);

                    GameObject thisField = transform.GetChild(row_col[0]).GetChild(row_col[1]).gameObject;
                    if (thisField.tag != "Road")
                    {
                        FieldInit fieldInit = thisField.GetComponent<FieldInit>();
                        string towerRead = fieldInit.towerType;
                        if (towerRead.Length >= 5)
                        {
                            if (towerRead == weaponType + (weaponGrade).ToString())
                            {
                                sameTypeGrade.Add(i+"-"+j);
                                _setMergeToEnable(fieldInit);
                            }
                        }
                    }
                }
            }
        }
        return sameTypeGrade;
    }

    private void _setMergeToEnable(FieldInit fi)
    {
        fi.transform.GetChild(0).GetComponent<GameplayCurSorOutline>().isMergeToPotential = true;
    }
    
}
