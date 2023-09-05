using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonElecActive : MonoBehaviour
{
    private Button _setElec;
    public GameObject initGObj;
    public GameObject textHint;
    public List<GameObject> moneyHint;
    public int initMoney;

    // Start is called before the first frame update
    void Start()
    {
        _setElec = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        string rowColStr = GlobalVar._instance.targetField;
        List<int> row_col = new List<int>();
        row_col = findRowCol(rowColStr);
        try
        {
            GameObject nextField = initGObj.transform.GetChild(row_col[0]).GetChild(row_col[1]+1).gameObject;
            FieldInit fieldInitRight = nextField.GetComponent<FieldInit>();
            if (fieldInitRight.selected == false && fieldInitRight.woodType == 0 && fieldInitRight.ironType == 0 &&
                fieldInitRight.eleType == 0 && fieldInitRight.wproType == 0 && fieldInitRight.iproType == 0 &&
                fieldInitRight.eproType == 0)
            {
                if (CurNodeDataSummary._instance.moneyLeft >= initMoney)
                {
                   _setElec.interactable = true;
                    textHint.SetActive(false);
                    foreach (var VARIABLE in moneyHint)
                    {
                        VARIABLE.SetActive(true);
                    }
                }
                else
                {
                    _setElec.interactable = false;
                    textHint.SetActive( true);
                    foreach (var VARIABLE in moneyHint)
                    {
                        VARIABLE.SetActive(false);
                    }
                }
            }
            else
            {
                _setElec.interactable = false;
                textHint.SetActive( true);
                foreach (var VARIABLE in moneyHint)
                {
                    VARIABLE.SetActive(false);
                }
                //other explain
            }
        }
        catch 
        {
            _setElec.interactable = false;
            textHint.SetActive( true);
            foreach (var VARIABLE in moneyHint)
            {
                VARIABLE.SetActive(false);
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
}
