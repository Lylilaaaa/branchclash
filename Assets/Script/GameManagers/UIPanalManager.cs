using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIPanalManager : MonoBehaviour
{
    public GameObject AddPanal;
    public GameObject MergePanel;
    public TextMeshProUGUI moneyTMP;
    public int money;
    public TowerBuilder tp;
    public GameObject hintHint;
    public GameObject hintPanal;
    public bool isNew;

    public void ReStart()
    {
        hintPanal.SetActive(false);
        AddPanal.SetActive(false);
        MergePanel.SetActive(false);
        money = GlobalVar._instance.chosenNodeData.money;
        CurNodeDataSummary._instance.moneyLeft = money;
        CurNodeDataSummary._instance.homeDestroyData = 0;
        moneyTMP.text = money.ToString();
    }
    private void Start()
    {
        isNew = true;
        ReStart();
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
                GlobalVar._instance.gamePlaySelect = true;
                AddPanal.SetActive(false);
                MergePanel.SetActive(false);
                break;
            case GlobalVar.GameState.AddTowerUI:
                GlobalVar._instance.gamePlaySelect = false;
                AddPanal.SetActive(true);
                MergePanel.SetActive(false);
                break;
            case GlobalVar.GameState.MergeTowerUI:
                // AddPanal.SetActive(false);
                // MergePanel.SetActive(true);
                break;
            case GlobalVar.GameState.Viewing:
                //AddPanal.SetActive(false);
                break;
            case GlobalVar.GameState.GameOver:
                AddPanal.SetActive(false);
                break;
            case GlobalVar.GameState.GamePlay:
                AddPanal.SetActive(false);
                break;
            default:
                AddPanal.SetActive(false);
                break;
        }

        // if (GlobalVar._instance.showMergable == true)
        // {
        //     quitMerge.SetActive(true);
        // }
        // else
        // {
        //     quitMerge.SetActive(false);
        // }
        moneyTMP.text = (CurNodeDataSummary._instance.moneyLeft).ToString();
    }

    public void CloseMerge()
    {
        GlobalVar._instance.ChangeState("ChooseField");
        //ContractInteraction._instance.EditMergeTower();
        tp.mergeButtConfirm = false;
        tp.targetWeaponPos = new List<string>();
        tp.mergeFromPos = new List<int>();
        tp.mergeToPos = new List<int>();
        tp.mergeChosen = false;
    }
    public void hintHintFlicker()
    {
        hintHint.SetActive(false);
    }
    public void OpenHintPanal()
    {
        hintPanal.SetActive(true);
        hintHint.SetActive(false);
    }
    public void CloseHintPanal()
    {
        hintPanal.SetActive(false);
        //print(GlobalVar._instance.thisUserAddr);
        isNew = false;
    }
    

}
