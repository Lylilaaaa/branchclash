using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIPanalManager : MonoBehaviour
{
    public GameObject AddPanal;
    public GameObject mergePanel1;
    public GameObject mergePanel2;
    public TextMeshProUGUI moneyTMP;
    public GameObject quitMerge;

    private void Start()
    {
        AddPanal.SetActive(false);
        mergePanel1.SetActive(false);
        mergePanel2.SetActive(false);
        quitMerge.SetActive(false);
    }

    private void Update()
    {
        GlobalVar.GameState _currentState = GlobalVar._instance.GetState();
        switch (_currentState)
        {
            case GlobalVar.GameState.MainStart:
                //AddPanal.SetActive(false);
                break;
            case GlobalVar.GameState.ChooseField:
                AddPanal.SetActive(false);
                mergePanel1.SetActive(false);
                mergePanel2.SetActive(false);
                break;
            case GlobalVar.GameState.AddTowerUI:
                AddPanal.SetActive(true);
                mergePanel1.SetActive(false);
                mergePanel2.SetActive(false);
                break;
            case GlobalVar.GameState.MergeTowerUI:
                AddPanal.SetActive(false);
                if (GlobalVar._instance.tempMerge1)
                {
                    mergePanel1.SetActive(true);
                }
                else if (!GlobalVar._instance.tempMerge1)
                {
                    mergePanel1.SetActive(false);
                }
                if (GlobalVar._instance.tempMerge2)
                {
                    mergePanel2.SetActive(true);
                }
                else if (!GlobalVar._instance.tempMerge2)
                {
                    mergePanel2.SetActive(false);
                }
                break;
            case GlobalVar.GameState.Viewing:
                //AddPanal.SetActive(false);
                break;
            case GlobalVar.GameState.GameOver:
                AddPanal.SetActive(false);
                break;
            default:
                AddPanal.SetActive(false);
                break;
        }

        if (GlobalVar._instance.showMergable == true)
        {
            quitMerge.SetActive(true);
        }
        else
        {
            quitMerge.SetActive(false);
        }
    }

    public void CloseMerge1()
    {
        GlobalVar._instance.tempMerge1 = false;
        GlobalVar._instance.showMergable = true;
        GlobalVar._instance.ChangeState("ChooseField");
        //ContractInteraction._instance.EditMergeTower();
    }
    public void CloseMerge2()
    {
        GlobalVar._instance.tempMerge2 = false;
        GlobalVar._instance.showMergable = false;
        GlobalVar._instance.ChangeState("ChooseField");
    }
    public void reduceMoney(int moneyAmount)
    {
        moneyTMP.text = (int.Parse(moneyTMP.text) + moneyAmount).ToString();
    }
}
