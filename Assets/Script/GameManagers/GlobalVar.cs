using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class GlobalVar : MonoBehaviour
{
    public static GlobalVar _instance;
    
    [Header("--------User--------")]
    public string userAddr="0xfd376a919b9a1280518e9a5e29e3c3637c9faa12";
    
    [Header("--------Tree--------")]
    public int maxLevelTree;
    public int maxLevelTreeDown;
    public List<string> MajorNodeList = new List<string>();
    public List<string> MajorNodeListDown = new List<string>();
    public List<NodeData> nodeDataList;
    public List<DownNodeData> downNodeDataList;
    public int[] TreeGen;
    public int[] redTreeGen;
    
    
    [Header("--------Process--------")]
    private NodeData _previousNodeData;
    private DownNodeData _previousDownNodeData;
    public NodeData chosenNodeData;
    public DownNodeData downChosenNodeData;
    public GameState initialGameState;
    public static GameState CurrentGameState;
    public string gameStateShown="";
    public bool isPreViewing = false;
    //public bool finishEdit;

    [Header("--------Map--------")]
    public string[][] mapmapList;
    public int mapmapRow;
    public string[] mapmapCol;
    public int indexMapMapCol;
    
    [Header("--------Camera--------")]
    public string zoomingPos = "";
    public string targetField="";
    
    [Header("--------Setting--------")]
    public DownTreeData downTreeData;
    public TreeData treeData;
    public TowerData woodTowerData;
    public TowerData ironTowerData;
    public TowerData elecTowerData;
    public ProtectData ProWood;
    public ProtectData ProIron;
    public ProtectData ProElec;
    

    public enum GameState
    {
        MainStart,
        Viewing,
        ChooseField,
        AddTowerUI,
        MergeTowerUI,
        GamePlay,
        GameOver
    }
    private void Awake()
    {
        _instance = this;
        
    }

    void Start()
    {
        ReStart();
    }

    public void ReStart()
    {
        //
        
        //TreeGenerator._instance.InitTree();
        //RedTreeGenerator._instance.InitDownTree();

        
        // 初始化游戏状态
        userAddr = "0xfd376a919b9a1280518e9a5e29e3c3637c9faa12";

        CurrentGameState = initialGameState;
        _previousNodeData = chosenNodeData;
        _previousDownNodeData = downChosenNodeData;
        
        _getMapmapList();

        nodeDataList = new List<NodeData>();
        downNodeDataList = new List<DownNodeData>();
        ReadData();
        _getMainNode();
        _getMainNodeDown();
        TreeNodeDataInit._instance.ReStart();
    }

    private void Update()
    {
        mapmapRow = mapmapList.Length;
        mapmapCol = mapmapList[indexMapMapCol];
        gameStateShown = GetState().ToString();

        if (_previousNodeData.nodeIndex!= chosenNodeData.nodeIndex || _previousNodeData.nodeLayer!= chosenNodeData.nodeLayer )
        {
            _getMapmapList();
            _previousNodeData = chosenNodeData;
        }
        //print("zommingPos:    "+zoomingPos);
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

        for (int i = 0; i < stringList.Length; i++)
        {
            for (int j = 0; j < stringList[i].Length; j++)
            {
                //print(stringList[i][j]);
                if (stringList[i][j].Length >= 5)
                {
                    string mapType = stringList[i][j].Substring(0, 4);
                    int towerGrade = int.Parse(stringList[i][j].Substring(4, stringList[i][j].Length - 4));
                    if (mapType == "elec")
                    {
                        stringList[i][j + 1] = "eleC" + towerGrade;
                    }
                }
            }
        }

        mapmapList = stringList;
    }
    public string _getMapmapString(string[][] stringList)
    {
        List<string> rows = new List<string>();

        for (int i = 0; i < stringList.Length; i++)
        {
            if (stringList[i] != null)
            {
                string[] row = stringList[i];
                List<string> newRow = new List<string>();

                for (int j = 0; j < row.Length; j++)
                {
                    newRow.Add(row[j]);
                }

                string combinedRow = string.Join(",", newRow);
                rows.Add(combinedRow);
            }
        }
        string temp = string.Join(",/n,", rows);
        if (temp.EndsWith(",/n,"))
        {
            temp = temp.Substring(0, temp.Length - 1);
        }
        return temp;
    }
    private void _getMainNode()
    {
        int maxLayer = 0;
        int maxIndex = 0;
        int maxChildCount = 0;
        int curLayer = 0;

        foreach (NodeData _node in nodeDataList)
        {
            if (_node.nodeLayer > maxLayer)
            {
                maxLayer = _node.nodeLayer;
            }
        }
        //print("maxUpLayer: "+maxLayer);

        curLayer = maxLayer;
        maxLevelTree = maxLayer;

        foreach (NodeData _maxnode in nodeDataList)
        {
            if (_maxnode.nodeLayer == maxLayer)
            {
                if (_maxnode.childCount >= maxChildCount)
                {
                    maxChildCount = _maxnode.childCount;
                    maxIndex = _maxnode.nodeIndex;
                }
            }
        }
        
        while (curLayer > 0)
        {
            //print(curLayer);
            MajorNodeList.Add(maxLayer.ToString() + '-' + maxIndex.ToString());
            //print(maxLayer.ToString() + '-' + maxIndex.ToString());
            NodeData majorNode = _findNodeData(maxLayer.ToString() + '-' + maxIndex.ToString());
            majorNode.isMajor = true;

            maxLayer = majorNode.fatherLayer;
            maxIndex = majorNode.fatherIndex;
            
            curLayer = maxLayer;
        }
        
    }
    private void _getMainNodeDown()
    {
        int maxLayer = 0;
        int maxIndex = 0;
        int maxChildCount = 0;
        int curLayer = 0;

        foreach (DownNodeData _node in downNodeDataList)
        {
            if (_node.nodeLayer > maxLayer)
            {
                maxLayer = _node.nodeLayer;
            }
        }
        //print("maxDownLayer: "+maxLayer);

        curLayer = maxLayer;
        maxLevelTreeDown = maxLayer;

        foreach (DownNodeData _maxnode in downNodeDataList)
        {
            if (_maxnode.nodeLayer == maxLayer)
            {
                if (_maxnode.childCount >= maxChildCount)
                {
                    maxChildCount = _maxnode.childCount;
                    maxIndex = _maxnode.nodeIndex;
                }
            }
        }
        
        while (curLayer > 0)
        {
            //print(curLayer);
            MajorNodeListDown.Add(maxLayer.ToString() + '-' + maxIndex.ToString());
            //print(maxLayer.ToString() + '-' + maxIndex.ToString());
            DownNodeData majorNode = _findNodeDataDown(maxLayer.ToString() + '-' + maxIndex.ToString());
            majorNode.isMajor = true;

            maxLayer = majorNode.fatherLayer;
            maxIndex = majorNode.fatherIndex;
            
            curLayer = maxLayer;
        }
        
    }
    private NodeData _findNodeData(string nodeName) //"1-2"
    {
        string[] layerIndex = nodeName.Split('-');
        foreach (NodeData _maxnode in nodeDataList)
        {
            if (_maxnode.nodeLayer == int.Parse(layerIndex[0]) &&_maxnode.nodeIndex == int.Parse(layerIndex[1]))
            {
                return (_maxnode);
            }
        }
        return null;
    }
    private DownNodeData _findNodeDataDown(string nodeName) //"1-2"
    {
        string[] layerIndex = nodeName.Split('-');
        foreach (DownNodeData _maxnode in downNodeDataList)
        {
            if (_maxnode.nodeLayer == int.Parse(layerIndex[0]) &&_maxnode.nodeIndex == int.Parse(layerIndex[1]))
            {
                return (_maxnode);
            }
        }
        return null;
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
            case "GamePlay":
                newState = GameState.GamePlay;
                break;
            default:
                newState = CurrentGameState;
                break;
        }
        
        CurrentGameState = newState;
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
    public void ReStartTree()
    {
        UploadData();
        SceneManager.LoadScene("HomePage");
        
        ReStart();

    }

    private void UploadData()
    {
        TreeNodeDataInit._instance.AddNodeData();
    }
}
