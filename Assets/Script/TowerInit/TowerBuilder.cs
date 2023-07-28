using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuilder : MonoBehaviour
{
    private bool _finish=false;
    private string[][] _mapmapList;
    public bool finish = false;
    public UIPanalManager _UIPanalManager;
    private void Update()
    {
        if (GlobalVar._instance.mapmapList != null && _finish == false)
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
            _finish = true;
        }
        if (GlobalVar._instance.finishAdd==true && finish == false)
        {
            finish = true;
            Set("wood");
            changeStateChoseField();
            _UIPanalManager.reduceMoney(-40);

        }
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
            if (fieldInit.selected == false && fieldInit.woodType==0 && fieldInit.ironType==0 && fieldInit.eleType==0)
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
                    fieldInit.eleType = 1;
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

    public void Merge(string mergeTowerType)
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
                    //print(fieldInit.name+ fieldInit.selected);
                    if (fieldInit.towerType == mergeTowerType)
                    {
                        //fieldInit.checkState = 1;
                    }
                }
            }
        }
    }

    public void CallAddContract()
    {
        ContractInteraction._instance.EditAddTower();
        // Set("wood");
        // changeStateChoseField();
        // _UIPanalManager.reduceMoney(-40);
    }
    
}
