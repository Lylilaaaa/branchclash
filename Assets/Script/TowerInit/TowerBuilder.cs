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
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 18; j++)
            {
                GameObject thisField = transform.GetChild(i).GetChild(j).gameObject;
                //print(thisField);
                if (thisField.tag != "Road")
                {
                    FieldInit fieldInit = thisField.GetComponent<FieldInit>();
                    fieldInit.towerType = setTowerType;
                    if (fieldInit.selected == false && fieldInit.woodType==0 && fieldInit.ironType==0 && fieldInit.eleType==0)
                    {
                        fieldInit.selected = true;
                    }
                }
            }
        }
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
