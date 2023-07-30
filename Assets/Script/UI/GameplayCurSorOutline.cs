using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GameplayCurSorOutline : MonoBehaviour
{
    
    [Header("BASIC INFO")]
    public string weaponType;
    public int weaponGrade;

    [Header("MERGE INFO")]
    public List<string> sameWeaponPos;
    public bool isMergeToPotential;
    public bool mouseEnter;
    
    [Header("GOBJ ACTIVE")]
    public GameObject tempMergePenal;
    public GameObject outlineGbj;

    private TowerBuilder tb;
    
    // Start is called before the first frame update
    void Start()
    {
        weaponType = "";
        weaponGrade = 1;
        sameWeaponPos = new List<string>();
        tb = transform.parent.parent.parent.GetComponent<TowerBuilder>();
        
        mouseEnter = false;
        isMergeToPotential = false;
        tempMergePenal.SetActive(false);
        outlineGbj.SetActive(false);
    }

    private void Update()
    {

            if (mouseEnter == true && tb.mergeChosen == false)
            {
                //选中第一个merge from的塔
                if (Input.GetMouseButtonDown(0))
                {
                    tb.mergeChosen = true;
                    GlobalVar._instance.ChangeState("MergeTowerUI");
                    sameWeaponPos=tb.checkWeaponPos(weaponType, weaponGrade);
                    tb.targetWeaponPos = sameWeaponPos;
                    tb.targetWeaponType = weaponType;
                    tb.targetWeaponGrade = weaponGrade;
                    tb.mergeFromPos = findRowCol( transform.name.Substring(0,3)); 
                }
            }
            if (GlobalVar._instance.GetState() == GlobalVar.GameState.MergeTowerUI)
            {
                if (tb.mergeChosen == true && isMergeToPotential == true)
                {
                    if (mouseEnter == true && Input.GetMouseButtonDown(0))
                    {
                        tb.mergeToPos = findRowCol( transform.name.Substring(0,3)); 
                        tb.Merge(tb.mergeFromPos,tb.mergeToPos);
                    }
                }
            
                //merge按钮按下之后才可以显示“可merge”的塔
                if (isMergeToPotential == true && tb.mergeButtConfirm == true)
                {
                    tempMergePenal.SetActive(true);
                }
            }
    }
    
    private void OnMouseEnter()
    {
        mouseEnter = true;
        if (tb.mergeChosen == false)
        {
            outlineGbj.SetActive(true);
        }
    }
    private void OnMouseExit()
    {
        mouseEnter = false;
        outlineGbj.SetActive(false);
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
