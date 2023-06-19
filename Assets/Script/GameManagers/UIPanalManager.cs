using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanalManager : MonoBehaviour
{
    public GameObject AddPanal;

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
                break;
            case GlobalVar.GameState.AddTowerUI:
                AddPanal.SetActive(true);
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
    }
    
}
