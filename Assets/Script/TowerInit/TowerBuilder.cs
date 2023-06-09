using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuilder : MonoBehaviour
{
    
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
                        fieldInit.checkState = 1;
                    }
                }
            }
        }
    }
    
}
