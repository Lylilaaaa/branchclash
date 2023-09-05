using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GlobalVar : MonoBehaviour
{
    public static GlobalVar _instance;

    [Header("--------User--------")]
    public string thisUserAddr = "0xfd376a919b9a1280518e9a5e29e3c3637c9faa12";
    public int isNew;
    public int role;

    [Header("--------Tree--------")]
    public int maxLevelTree;
    public int maxLevelTreeDown;
    public List<string> MajorNodeList = new List<string>();
    public List<string> MajorNodeListDown = new List<string>();
    public List<NodeData> nodeDataList;
    public List<DownNodeData> downNodeDataList;
    public List<int> upTreeNodeLayerIndex;
    public List<int> downTreeNodeLayerIndex;
    public List<string> upStringResults = new List<string>();
    public List<string> downStringResults = new List<string>();
    public int[] TreeGen;
    public int[] redTreeGen;
    
    
    [Header("--------Process--------")]
    public NodeData chosenNodeData;
    public DownNodeData chosenDownNodeData;
    public GameState initialGameState;
    public static GameState CurrentGameState;
    public string gameStateShown="";
    public bool dataPrepared;
    public bool nodePrepared;
    public bool isPreViewing = false;
    public bool global_OL;
    public int gameResult;  //0: 家没有受伤 ；1：家受伤了 ；2：失败了
    //public bool finishEdit;
    private int finishUp = 0;
    private int finishDown= 0;

    [Header("--------Map--------")]
    public string[][] mapmapList;
    public int mapmapRow;
    public string[] mapmapCol;
    public int indexMapMapCol;
    
    [Header("--------Camera--------")]
    public string zoomingPos = "";
    public string targetField="";
    
    [Header("--------DataSetting--------")]
    public DownTreeData downTreeData;
    public TreeData treeData;
    public TowerData woodTowerData;
    public TowerData ironTowerData;
    public TowerData elecTowerData;
    public ProtectData ProWood;
    public ProtectData ProIron;
    public ProtectData ProElec;

    [Header("--------GameObjSetting--------")]
    public GameObject loadingGameObj;

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
        loadingGameObj.SetActive(false);
        dataPrepared = false;
        nodePrepared = false;
        thisUserAddr = "";
    }

    // void Start()
    // {
    //     ReStart();
    // }

    public IEnumerator  ReStart()
    {
        //
        print("GlobalVar Restart!");
        ContractInteraction._instance.ReSetMain();
        dataPrepared = false;
        nodePrepared = false;
        //TreeGenerator._instance.InitTree();
        //RedTreeGenerator._instance.InitDownTree();
        isNew = _checkUserGreen(thisUserAddr);
        role = _checkUserRole(thisUserAddr);
        SoundManager._instance.PlayMusicSound(SoundManager._instance.homePageBackSound,true,0.8f);
        // 初始化游戏状态
        
        CurrentGameState = initialGameState;

        isPreViewing = false;
        _getMapmapList();
        upTreeNodeLayerIndex= new List<int>();
        downTreeNodeLayerIndex= new List<int>();
        upStringResults = new List<string>();
        downStringResults = new List<string>();
        // nodeDataList = new List<NodeData>();
        // downNodeDataList = new List<DownNodeData>();
        MajorNodeList = new List<string>();
        MajorNodeListDown = new List<string>();
        MajorNodeList = new List<string>();
        MajorNodeListDown = new List<string>();
        ReadData();
        while (!nodePrepared)
        {
            yield return null; // 等待一帧
        }
        _getMainNode();
        _getMainNodeDown();
        TreeNodeDataInit._instance.ReStart();
        CurNodeDataSummary._instance.ReStart();
        dataPrepared = true;
    }

    private void Update()
    {
        if (dataPrepared == true)
        {
            //for shown purpose
            mapmapRow = mapmapList.Length;
            mapmapCol = mapmapList[indexMapMapCol];
            gameStateShown = GetState().ToString();
            if (mapmapList.Length != 9)
            {
                mapmapList = _recheckMapmapList();
            }

            foreach (var VARIABLE in mapmapList)
            {
                if (VARIABLE.Length != 18)
                {
                    mapmapList = _recheckMapmapList();
                }
            }
        }
    }

    private string[][] _recheckMapmapList()
    {
        string[][] newMapMapList =new string[9][];
        for (int i = 0; i < 9; i++)
        {
            newMapMapList[i] = new string[18];
        }

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 18; j++)
            {
                newMapMapList[i][j] = mapmapList[i][j];
            }
        }

        return newMapMapList;
    }
    

    public void _getMapmapList()
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
        int curLayer = 0;

        foreach (NodeData _node in nodeDataList)
        {
            if (_node.nodeLayer > maxLayer)
            {
                maxLayer = _node.nodeLayer;
            }
        }
        print("maxUpLayer: "+maxLayer);

        curLayer = maxLayer;
        maxLevelTree = maxLayer;
        maxIndex = 1;
        while (curLayer >= 0)
        {
            //print(curLayer);
            MajorNodeList.Add(maxLayer.ToString() + '-' + maxIndex.ToString());
            //print(maxLayer.ToString() + '-' + maxIndex.ToString());

            NodeData majorNode = _findNodeData(maxLayer.ToString() + '-' + maxIndex.ToString());
            if (majorNode != null)
            {
                majorNode.isMajor = true;
                if (curLayer != 0)
                {
                    maxLayer = majorNode.fatherLayer;
                    maxIndex = majorNode.fatherIndex;
            
                    curLayer = maxLayer;
                }
                else
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }
        
    }
    private void _getMainNodeDown()
    {
        int maxLayer = 0;
        int maxIndex = 0;
        //int maxChildCount = 0;
        int curLayer = 0;

        foreach (DownNodeData _node in downNodeDataList)
        {
            _node.isMajor = false;
            if (_node.nodeLayer > maxLayer)
            {
                maxLayer = _node.nodeLayer;
            }
        }
        //print("maxDownLayer: "+maxLayer);

        curLayer = maxLayer;
        maxLevelTreeDown = maxLayer;
        maxIndex = 1;
        
        while (curLayer >= 0)
        {
            //print(curLayer);
            MajorNodeListDown.Add(maxLayer.ToString() + '-' + maxIndex.ToString());
            //print(maxLayer.ToString() + '-' + maxIndex.ToString());
            DownNodeData majorNode = _findNodeDataDown(maxLayer.ToString() + '-' + maxIndex.ToString());
            if (majorNode != null)
            {
                majorNode.isMajor = true;

                if (curLayer != 0)
                {
                    maxLayer = majorNode.fatherLayer;
                    maxIndex = majorNode.fatherIndex;

                    curLayer = maxLayer;
                }
                else
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }
        
    }
    public NodeData _findNodeData(string nodeName) //"1-2"
    {
        if (nodeName == "0-0")
        {
            nodeName = "0-1";
        }
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
    public DownNodeData _findNodeDataDown(string nodeName) //"1-2"
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
    public void ReadData()
    {
        ReadFromWeb();
    }
    
    //"layer0 idx1-"
    private void ReadFromWeb()
    {
        UrLController._instance.CheckAllUpNode();
        UrLController._instance.CheckAllDownNode();
        StartCoroutine(readFromWeb());
        
    }
    
    IEnumerator readFromWeb()
    {
        while (UrLController._instance.upTreeResult.Length == 0 || UrLController._instance.downTreeResult.Length == 0)
        {
            yield return null; // 等待一帧
        }
        _getTreeList();
    }
    IEnumerator setNodeFromWeb()
    {
        while (finishUp < upTreeNodeLayerIndex.Count/2 || finishDown < downTreeNodeLayerIndex.Count/2 )
        {
            yield return null; // 等待一帧
        }

        finishUp = finishDown = 0;
        print("start get node");
        _getAllNodeList();
    }

    private void _getAllNodeList()
    {
        nodeDataList = new List<NodeData>();
        downNodeDataList = new List<DownNodeData>();
        foreach (string _infoString in upStringResults)
        {

            string[] nodeInfo = _infoString.Split("-");
            foreach (string _string in nodeInfo)
            {
                print(_string);
            }
            NodeData newNodeData = new NodeData();
            newNodeData.ownerAddr = nodeInfo[0];
            newNodeData.setUpTime = nodeInfo[1];
            newNodeData.fatherLayer = int.Parse(nodeInfo[2]);
            newNodeData.fatherIndex = int.Parse(nodeInfo[3]);
            newNodeData.childCount = int.Parse(nodeInfo[4]);
            newNodeData.nodeLayer = int.Parse(nodeInfo[5]);
            newNodeData.nodeIndex = int.Parse(nodeInfo[6]);
            if (int.Parse(nodeInfo[7]) == 0)
            {
                newNodeData.isMajor = false;
            }
            else
            {
                newNodeData.isMajor = true;
            }
            newNodeData.curHealth = int.Parse(nodeInfo[8]);
            newNodeData.fullHealth = int.Parse(nodeInfo[9]);
            newNodeData.money = int.Parse(nodeInfo[10]);
            newNodeData.mapStructure = nodeInfo[11];
            int[] _debuff = new int[3];
            string[] debuffString = nodeInfo[12].Split(",");
            _debuff[0] = int.Parse(debuffString[0]);
            _debuff[1] = int.Parse(debuffString[1]);
            _debuff[2] = int.Parse(debuffString[2]);
            newNodeData.name = newNodeData.nodeLayer.ToString() + "," + newNodeData.nodeIndex.ToString();
            nodeDataList.Add(newNodeData);
        }
        foreach (string _infoString in downStringResults)
        {
            string[] nodeInfo = _infoString.Split("-");
            DownNodeData newNodeData = new DownNodeData();
            newNodeData.ownerAddr = nodeInfo[0];
            newNodeData.setUpTime = nodeInfo[1];
            newNodeData.fatherLayer = int.Parse(nodeInfo[2]);
            newNodeData.fatherIndex = int.Parse(nodeInfo[3]);
            newNodeData.childCount = int.Parse(nodeInfo[4]);
            newNodeData.nodeLayer = int.Parse(nodeInfo[5]);
            newNodeData.nodeIndex = int.Parse(nodeInfo[6]);
            if (int.Parse(nodeInfo[7]) == 0)
            {
                newNodeData.isMajor = false;
            }
            else
            {
                newNodeData.isMajor = true;
            }
            int[] _debuff = new int[3];
            string[] debuffString = nodeInfo[8].Split(",");
            _debuff[0] = int.Parse(debuffString[0]);
            _debuff[1] = int.Parse(debuffString[1]);
            _debuff[2] = int.Parse(debuffString[2]);
            newNodeData.name = "("+newNodeData.nodeLayer.ToString() + "," + newNodeData.nodeIndex.ToString()+")";
            downNodeDataList.Add(newNodeData);
        }
        nodePrepared = true;
    }

    private void _getTreeList()
    {
        upTreeNodeLayerIndex = new List<int>();
        downTreeNodeLayerIndex = new List<int>();
        string upNodes = UrLController._instance.upTreeResult;
        string downNodes = UrLController._instance.downTreeResult;
        upNodes = upNodes.Substring(0, upNodes.Length - 1);
        downNodes = downNodes.Substring(0, downNodes.Length - 1);
        string[] upLayerIndex = upNodes.Split("-");
        foreach (var VARIABLE in upLayerIndex)
        {
            string[] layer_index = VARIABLE.Split(" ");
            upTreeNodeLayerIndex.Add(int.Parse(layer_index[0].Substring(5,layer_index[0].Length-5)) );    //"layer0 idx1-"
            upTreeNodeLayerIndex.Add(int.Parse(layer_index[1].Substring(3,layer_index[1].Length-3)) );
        }
        
        string[] downLayerIndex = downNodes.Split("-");
        foreach (var VARIABLE in downLayerIndex)
        {
            string[] layer_index = VARIABLE.Split(" ");
            downTreeNodeLayerIndex.Add(int.Parse(layer_index[0].Substring(5,layer_index[0].Length-5)) );
            downTreeNodeLayerIndex.Add(int.Parse(layer_index[1].Substring(3,layer_index[1].Length-3)) );
        }

        finishUp = finishDown = 0;
        StartCoroutine(setNodeFromWeb());
        ReadUpNodeFromWeb();
        ReadDownNodeFromWeb();
    }
    
    private void ReadUpNodeFromWeb()
    {
        upStringResults = new List<string>();
        int currentIndex = 0;
        while (currentIndex < upTreeNodeLayerIndex.Count/2)
        {
            StartCoroutine(checkUpNode(upTreeNodeLayerIndex[currentIndex*2], upTreeNodeLayerIndex[currentIndex*2+1]));
            currentIndex++;
        }
    }
    private IEnumerator checkUpNode(int layer, int idx)
    {
        string checkString;
        checkString = UrLController._instance.url_search + "?layer=" + layer.ToString() + "&idx=" + idx.ToString();
        Debug.Log(checkString);
        WWW www = new WWW(checkString);
        yield return www;
        string result = www.text;
        Debug.Log(result);
        upStringResults.Add(result);
        finishUp++;
    }
    private void ReadDownNodeFromWeb()
    {
        downStringResults = new List<string>();
        int currentIndex = 0;
        while (currentIndex < downTreeNodeLayerIndex.Count/2)
        {
            StartCoroutine(checkDownNode(downTreeNodeLayerIndex[currentIndex*2], downTreeNodeLayerIndex[currentIndex*2+1]));
            currentIndex++;
        }
    }
    private IEnumerator checkDownNode(int layer, int idx)
    {
        string checkString;
        checkString = UrLController._instance.url_searchSec + "?layer=" + layer.ToString() + "&idx=" + idx.ToString();
        Debug.Log(checkString);
        WWW www = new WWW(checkString);
        yield return www;
        string result = www.text;
        downStringResults.Add(result);
        finishDown++;
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
        foreach (string key in downTreeData.downNodeDictionary.Keys)
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
    
    
    public int _checkUserGreen(string address)
    {
        // if (GlobalVar._instance.userDict.Keys.Contains(address))
        // {
        //     GlobalVar._instance.userDict[address] = 1;
        // }
        // else
        // {
        //     GlobalVar._instance.userDict.Add(address,0);
        // }
        return 0;
    }

    public int _checkUserRole(string address)
    {
        return 1;
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
        //UploadData();
        if (role == 0)
        {
            _loadNextScene("1_0_HomePage");
        }
        else if (role == 1)
        {
            _loadNextScene("1_1_SecHomePage");
        }
        ChangeState("MainMenu");
        //ReStart();
    }
    
    
    public void _loadNextScene(string sceneName)
    {
        loadingGameObj.SetActive(true);
        StartCoroutine(LoadLeaver(sceneName));
    }
    IEnumerator LoadLeaver(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            loadingGameObj.transform.GetChild(1).GetComponent<Slider>().value = operation.progress;
            yield return null;
        }
        loadingGameObj.SetActive(false);
        if (sceneName == "1_0_HomePage" || sceneName == "1_1_SecHomePage")
        {
            StartCoroutine(ReStart());
        }
        else if (sceneName == "3_1_SecGamePlay"|| sceneName == "3_0_GamePlay")
        {
            CurNodeDataSummary._instance._initData = false;
        }
    }
    
    public NodeData _checkUpNodeMain(int layer)
    {
        if (layer <=maxLevelTree) //如果这个downNode的layer小于当前的最大正树layer
        {
            foreach (string majorNodeString in MajorNodeList)
            {
                string[] layerIndex = majorNodeString.Split('-');
                if (layerIndex.Length == 2)
                {
                    if (int.Parse(layerIndex[0]) == layer)
                    {
                        return _findNodeData(majorNodeString);
                    }
                }
            }
        }
        else //如果这个downNode的layer大于已有的正树layer
        {
            foreach (string majorNodeString in MajorNodeList)
            {
                string[] layerIndex = majorNodeString.Split('-');
                if (layerIndex.Length == 2)
                {
                    if (int.Parse(layerIndex[0]) == maxLevelTree)
                    {
                        return _findNodeData(majorNodeString);
                    }
                }
            }
        }
        return treeData.InitNodeData;
    }
    public DownNodeData _checkDownNodeMain(int layer)
    {
        if (layer <=maxLevelTree) //如果这个Node的layer小于当前的最大倒树layer
        {
            foreach (string majorNodeString in MajorNodeListDown)
            {
                string[] layerIndex = majorNodeString.Split('-');
                if (layerIndex.Length == 2)
                {
                    if (int.Parse(layerIndex[0]) == layer)
                    {
                        return _findNodeDataDown(majorNodeString);
                    }
                }
            }
        }
        else //如果这个downNode的layer大于已有的正树layer
        {
            foreach (string majorNodeString in MajorNodeListDown)
            {
                string[] layerIndex = majorNodeString.Split('-');
                if (layerIndex.Length == 2)
                {
                    if (int.Parse(layerIndex[0]) == maxLevelTree)
                    {
                        return _findNodeDataDown(majorNodeString);
                    }
                }
            }
        }
        return treeData.InitDownNodeData;
    }
}
