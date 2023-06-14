using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GlobalVar : MonoBehaviour
{
    public static GlobalVar _instance;
    public string targetField="";
    public bool chooseSedElec;
    public string gameStateShown="";
    public GameState initialGameState;
    public static GameState CurrentGameState;

    public TreeData treeData;
    public int[] TreeGen;
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

    
    private void Start()
    {
        // 初始化游戏状态
        CurrentGameState = initialGameState;
        _convert2TreeGen(treeData);
    }

    public void UpdateTreeGen(TreeData newTreeDate)
    {
        _convert2TreeGen(newTreeDate);
    }

    private void _convert2TreeGen(TreeData inputTreeData)
    {
        List<int[]> inputList = new List<int[]>();
        inputList = SortSequence(inputTreeData);
        List<int> genList = new List<int>();
        foreach (int[] VARIABLE in inputList)
        {
            if (VARIABLE.Length == 2)
            {
                //层数，层内序号，父节点层内序号，子节点数量
                genList.Add(VARIABLE[0]);
                genList.Add(VARIABLE[1]);
                NodeData curNodeData = inputTreeData.nodeDictionary[VARIABLE[0].ToString()+','+VARIABLE[1]];
                genList.Add(curNodeData.fatherIndex);
                genList.Add(curNodeData.childCount);
            }
            else
            {
                Debug.LogError("树的key结构不正确!");
            }
        }
        TreeGen = genList.ToArray();
    }
    private List<int[]> SortSequence(TreeData curTreeData)
    {
        List<int[]> sequence = new List<int[]>();
        foreach (string key in treeData.nodeDictionary.Keys)
        {
            int[] indexPair = convertStrInt(key);
            sequence.Add(indexPair);
        }
        sequence.Sort((x, y) =>
        {
            if (x[0] != y[0])
                return x[0].CompareTo(y[0]);
            else
                return x[1].CompareTo(y[1]);
        });
        return sequence;
    }
    private int[] convertStrInt(string layerIndex)
    {
        List<int> genList = new List<int>();
        string[] parts = layerIndex.Split(',');
        if (parts.Length == 2)
        {
            int layer;
            int index;
            if (int.TryParse(parts[0], out layer) && int.TryParse(parts[1], out index))
            {
                genList.Add(layer);
                genList.Add(index);
                
            }
        }
        else
        {
            Debug.LogError("树的key结构不正确!");
        }
        return genList.ToArray();
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
                newState = CurrentGameState;
                break;
        }
        
        CurrentGameState = newState;
    }

    public void changeEleMode(bool choseEle)
    {
        chooseSedElec = choseEle;
    }

    // Update is called once per frame
    public GameState GetState()
    {
        return (CurrentGameState);
    }

    public void chooseField(string FieldPos)
    {
        targetField = FieldPos;
    }
}
