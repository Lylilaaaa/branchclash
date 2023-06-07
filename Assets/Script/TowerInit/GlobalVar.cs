using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVar : MonoBehaviour
{
    public static GlobalVar _instance;
    public string targetField="";
    public string gameStateShown="";
    public enum GameState
    {
        MainStart,
        Viewing,
        ChooseField,
        AddTowerUI,
        GameOver
    }
    private void Awake()
    {
        _instance = this;
    }

    private void Update()
    {
        gameStateShown = GetState().ToString();
    }

    public static GameState currentGameState;
    private void Start()
    {
        // 初始化游戏状态
        currentGameState = GameState.ChooseField;
    }

    // Start is called before the first frame update
    public void ChangeState(string stateStr)
    {
        GameState newState;
        
        switch (stateStr)
        {
            case "MainMenu":
                newState = GameState.MainStart;
                break;
            case "Viewing":
                newState = GameState.Viewing;
                break;
            case "ChooseField":
                newState = GameState.ChooseField;
                break;
            case "AddTowerUI":
                newState = GameState.AddTowerUI;
                break;
            case "GameOver":
                newState = GameState.GameOver;
                break;
            default:
                newState = currentGameState;
                break;
        }
        
        currentGameState = newState;
    }

    // Update is called once per frame
    public GameState GetState()
    {
        return (currentGameState);
    }

    public void chooseField(string FieldPos)
    {
        targetField = FieldPos;
    }
}
