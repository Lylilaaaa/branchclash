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
    public TowerBuilder tp;


    private void Start()
    {
        AddPanal.SetActive(false);
        MergePanel.SetActive(false);
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
                MergePanel.SetActive(false);
                break;
            case GlobalVar.GameState.AddTowerUI:
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

    public void reduceMoney(int moneyAmount)
    {
        moneyTMP.text = (int.Parse(moneyTMP.text) + moneyAmount).ToString();
    }
}
