using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;


public class GlobalVar : MonoBehaviour
{
    public string userAddr="0xfd376a919b9a1280518e9a5e29e3c3637c9faa12";
    public static GlobalVar _instance;
    public string targetField="";
    public bool chooseSedElec;
    public string gameStateShown="";
    public string zoomingPos = "";
    public bool tempMerge1;
    public bool tempMerge2;
    public bool showMergable;
    public bool canDeleteWoodMerge = false;

    public bool finishiEditIn = false;
    public bool finishAdd= false;
    public bool finishMerge= false;
    public bool finishSubmit= false;
    
    public GameState initialGameState;
    public static GameState CurrentGameState;

    public TreeData treeData;
    public DownTreeData downTreeData;
    public NodeData chosenNodeData;
    public DownNodeData downChosenNodeData;
    public TowerData woodTowerData;
    public TowerData ironTowerData;
    public TowerData elecTowerData;
    public string[][] mapmapList;
    public int mapmapRow;
    public int indexMapMapCol;
    public string[] mapmapCol;
    private NodeData _previousNodeData;
    private DownNodeData _previousDownNodeData;
    
    public List<NodeData> nodeDataList;
    public List<DownNodeData> downNodeDataList;
    public int[] TreeGen;
    public int[] redTreeGen;

    public bool isPreViewing = false;
    public enum GameState
    {
        MainStart,
        Viewing,
        ChooseField,
        AddTowerUI,
        MergeTowerUI,
        GameOver
    }
    private void Awake()
    {
        _instance = this;
        
    }
    private void Start()
    {
        // 初始化游戏状态
        userAddr = "0xfd376a919b9a1280518e9a5e29e3c3637c9faa12";
        finishiEditIn = false;
        finishAdd= false;
        finishMerge= false;
        finishSubmit= false;

        CurrentGameState = initialGameState;
        _previousNodeData = chosenNodeData;
        _previousDownNodeData = downChosenNodeData;
        _getMapmapList();

        nodeDataList = new List<NodeData>();
        downNodeDataList = new List<DownNodeData>();
        ReadData();
    }

    private void Update()
    {
        mapmapRow = mapmapList.Length;
        mapmapCol = mapmapList[indexMapMapCol];
        gameStateShown = GetState().ToString();
        if (_previousNodeData.nodeIndex!= chosenNodeData.nodeIndex || _previousNodeData.nodeLayer!= chosenNodeData.nodeLayer )
        {
            _getMapmapList();
        }
    }
    

    private void _getMapmapList()
    {
        string totalString = chosenNodeData.mapStructure;
        string[] rows = totalString.Split("/n");
        string[][] stringList = new string[rows.Length][];
        for (int i = 0; i < rows.Length; i++)
        {
            stringList[i] = rows[i].Split(',');
        }

        for (int i = 0; i < stringList.Length; i++)
        {
            if (stringList[i] != null)
            {
                string[] row = stringList[i];
                List<string> newRow = new List<string>();

                for (int j = 0; j < row.Length; j++)
                {
                    if (row[j] != null&&row[j] != "")
                    {
                        newRow.Add(row[j]);
                    }
                }
                // 覆盖原始行
                stringList[i] = newRow.ToArray();
            }
        }
        mapmapList = stringList;
    }
    
    private void ReadData()
    {
        NodeData[] allMyDataObjects = Resources.LoadAll<NodeData>("");
        foreach (NodeData VARIABLE in allMyDataObjects)
        {
            nodeDataList.Add(VARIABLE);
        }
        DownNodeData[] downAllMyDataObjects = Resources.LoadAll<DownNodeData>("");
        foreach (DownNodeData _VARIABLE in downAllMyDataObjects)
        {
            downNodeDataList.Add(_VARIABLE);
        }
        // nodeDataList.Add(Resources.Load<NodeData>("1,1"));
        // nodeDataList.Add(Resources.Load<NodeData>("1,2"));
        // nodeDataList.Add(Resources.Load<NodeData>("1,3"));
        // string[] assetPaths = UnityEditor.AssetDatabase.FindAssets("t:NodeData", new[] { "Assets/ScriptableObj/NodeDataObj/" });
        //
        // foreach (string assetPath in assetPaths)
        // {
        //     NodeData nodeData = UnityEditor.AssetDatabase.LoadAssetAtPath<NodeData>(UnityEditor.AssetDatabase.GUIDToAssetPath(assetPath));
        //     
        //     nodeDataList.Add(nodeData);
        // }
    }

    public void UpdateTreeGen(TreeData newTreeDate,DownTreeData newDowntreeData)
    {
        _convert2TreeGen(newTreeDate);
        _downConvert2TreeGen(newDowntreeData);
    }

    public void _convert2TreeGen(TreeData inputTreeData)
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
    public void _downConvert2TreeGen(DownTreeData inputDownTreeData)
    {
        List<int[]> inputList = new List<int[]>();
        inputList = downSortSequence(inputDownTreeData);
        List<int> genList = new List<int>();
        foreach (int[] VARIABLE in inputList)
        {
            if (VARIABLE.Length == 2)
            {
                //层数，层内序号，父节点层内序号，子节点数量
                genList.Add(VARIABLE[0]);
                genList.Add(VARIABLE[1]);
                DownNodeData curNodeData = inputDownTreeData.downNodeDictionary[VARIABLE[0].ToString()+','+VARIABLE[1]];
                genList.Add(curNodeData.fatherIndex);
                genList.Add(curNodeData.childCount);
            }
            else
            {
                Debug.LogError("树的key结构不正确!");
            }
        }
        redTreeGen = genList.ToArray();
    }
    private List<int[]> downSortSequence(DownTreeData curTreeData)
    {
        List<int[]> sequence = new List<int[]>();
        foreach (string key in curTreeData.downNodeDictionary.Keys)
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
        if (layerIndex.Substring(0, 1) == "(")
        {
            layerIndex = layerIndex.Substring(1, layerIndex.Length - 2);
        }
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
            case "MergeTowerUI":
                newState = GameState.MergeTowerUI;
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
