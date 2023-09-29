using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class GlobalVar : MonoBehaviour
{
    public static GlobalVar _instance;
    
    [Header("--------User--------")]
    public string thisUserAddr = "";
    public int isNew;
    public int role;

    [Header("--------Tree--------")]
    public int maxLevelTree;
    public int maxLevelTreeDown;
    public List<string> MajorNodeList = new List<string>();
    public List<string> MajorNodeListDown = new List<string>();
    public List<string> deadNodeList = new List<string>();
    public List<NodeData> nodeDataList;
    public List<DownNodeData> downNodeDataList;
    public List<int> upTreeNodeLayerIndex;
    public List<int> downTreeNodeLayerIndex;
    public List<string> upStringResults = new List<string>();
    public List<string> downStringResults = new List<string>();
    public int[] TreeGen;
    public int[] redTreeGen;
    public int totalNumOnServeUp;
    public int totalNumOnServeDown;
    public int[] needToCheckList;
    public int[] needToCheckListSec;
    
    
    [Header("--------Process--------")]
    public NodeData chosenNodeData;
    public DownNodeData chosenDownNodeData;
    public GameState initialGameState;
    public static GameState CurrentGameState;
    public string gameStateShown="";
    public bool dataPrepared;
    public bool upServePrepare;
    public bool downServePrepare;
    public bool nodePrepared;
    public bool finalNodePrepared;
    public bool isPreViewing = false;
    public bool global_OL;
    public int gameResult;  //0: 家没有受伤 ；1：家受伤了 ；2：失败了
    //public bool finishEdit;
    private int finishUp = 0;
    private int finishDown= 0;
    private int _curCheckingLayer;
    private int _targetCheckingLayer;
    private int _secCurCheckingLayer;
    private int _secTargetCheckingLayer;
    private int curUpdateIndex;
    private int curUpdateIndexSec;
    public string nowNodeIndex;
    public string nowNodeIndexBlood;
    public string oldNodeIndex;
    public string oldNodeIndexBlood;
    public bool gamePlaySelect;
    

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
    public TextMeshProUGUI t;

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
        finalNodePrepared = false;
        gamePlaySelect = false;
        upServePrepare = downServePrepare = false;
        thisUserAddr = "";
        UrLController._instance.CheckAllDownNode();
    }

    // void Start()
    // {
    //     ReStart();
    // }

    public void debufStartReStart()
    {
        t.text = "debugRestart!";
        StartCoroutine(ReStart());
    }

    public IEnumerator  ReStart()
    {
        //
        t.text = "";
        print("GlobalVar Restart!");

        dataPrepared = false;
        nodePrepared = false;
        upServePrepare = downServePrepare = false;
        //TreeGenerator._instance.InitTree();
        //RedTreeGenerator._instance.InitDownTree();
        ContractInteraction._instance.ReSetMain();
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
        finalNodePrepared = false;
        ReadData();
        while (!nodePrepared)
        {
            yield return null; // 等待一帧
        }
        
        UpLoadDataFromContract();
        
    }

    private void reStartTree()
    {
        _getMainNode();
        _getMainNodeDown();
        _getDeadNode();
        TreeNodeDataInit._instance.ReStart();
        CurNodeDataSummary._instance.ReStart();
        
        dataPrepared = true;
        loadingGameObj.SetActive(false);
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
                //print(stringList[i]);
            }
        }

        for (int i = 0; i < stringList.Length; i++)
        {
            for (int j = 0; j < stringList[i].Length; j++)
            {
                //print(stringList[i][j]);
                if (stringList[i][j].Length >= 5)
                {
                    //print("ij: "+stringList[i][j]);
                    string mapType = stringList[i][j].Substring(0, 4);
                    int towerGrade = int.Parse(stringList[i][j].Substring(4, stringList[i][j].Length - 4));
                    if (mapType == "elec")
                    {
                        stringList[i][j + 1] = "eleC" + towerGrade;
                    }
                    else if (mapType == "prow")
                    {
                        //Debug.Log("prow HERE!!!!!!!");
                        stringList[i][j] = "wpro"+towerGrade;
                    }
                    else if (mapType == "proi")
                    {
                        //Debug.Log("proi HERE!!!!!!!");
                        stringList[i][j] = "ipro"+towerGrade;
                    }
                    else if (mapType == "proe")
                    {
                        //Debug.Log("proe HERE!!!!!!!");
                        stringList[i][j] ="epro"+towerGrade;
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

    private void _getDeadNode()
    {
        deadNodeList = new List<string>();
        foreach (NodeData VARIABLE in nodeDataList)
        {
            if (VARIABLE.curHealth <= 0)
            {
                string name = VARIABLE.name.Substring(1, VARIABLE.name.Length - 2);
                string[] layerIndex = name.Split(",");
                deadNodeList.Add(layerIndex[0]+"-"+layerIndex[1]);
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
    public void UpLoadDataFromContract()
    {
        t.text += "\n checking new nodes from up and down contracts!";
        nodePrepared = false;
        upServePrepare = downServePrepare = false;
        /////remember to re read the data!!!!!!
        StartCoroutine(checkServeNum());
        StartCoroutine(waitToReStartTree());
    }
    IEnumerator checkServeNum()
    {
        t.text += "\n checking num...";
        ContractInteraction._instance.numOnContract = 0;
        ContractInteraction._instance.finishNumOnServeUp =ContractInteraction._instance.finishNumOnServeDown = false;
        ContractInteraction._instance.numOnContractSec = 0;
        ContractInteraction._instance.CheckServe();
        //ContractInteraction._instance.CheckServeSec();
        while (!ContractInteraction._instance.finishNumOnServeUp||!ContractInteraction._instance.finishNumOnServeDown)
        {
            yield return null;
        }

        if (ContractInteraction._instance.numOnContract-(totalNumOnServeUp) > 0 && ContractInteraction._instance.numOnContractSec-(totalNumOnServeDown) == 0)
        {
            t.text += "\n" + "only need to update the up nodes in serve!";
            upServePrepare = false;
            downServePrepare = true;
            StartCoroutine(confirmUpdate());
           loadingGameObj.transform.GetChild(1).GetComponent<Slider>().maxValue =1f;
        }
        else if (ContractInteraction._instance.numOnContract-(totalNumOnServeUp) == 0 && ContractInteraction._instance.numOnContractSec-(totalNumOnServeDown) > 0)
        {
            t.text += "\n" + "only need to update the down nodes in serve!";
            upServePrepare = true;
            downServePrepare = false;
            StartCoroutine(confirmDowndate());
            loadingGameObj.transform.GetChild(1).GetComponent<Slider>().maxValue =1f;
        }
        else if (ContractInteraction._instance.numOnContract - (totalNumOnServeUp) > 0 &&
                 ContractInteraction._instance.numOnContractSec - (totalNumOnServeDown) > 0)
        {
            t.text += "\n" + "need to update both down and up nodes in serve!";
            upServePrepare = false;
            downServePrepare = false;
            StartCoroutine(confirmUpdate());
            loadingGameObj.transform.GetChild(1).GetComponent<Slider>().maxValue =2f;
        }
        else if (ContractInteraction._instance.numOnContract==(totalNumOnServeUp))
        {
            upServePrepare = true;
            downServePrepare = true;
            t.text += "\n" + "do not need to update the down and up nodes in serve!";
        }
    }
    IEnumerator confirmDowndate()
    {
        print("total num on serve down is: "+totalNumOnServeDown);
        needToCheckListSec = new int[ContractInteraction._instance.numOnContractSec - totalNumOnServeDown];
        int needtoupdate = ContractInteraction._instance.numOnContractSec - (totalNumOnServeDown);
        print("new node needed to be updated on serve down is: "+ needtoupdate);
        curUpdateIndexSec = 0;
        for (int i = 0; i < ContractInteraction._instance.numOnContractSec - (totalNumOnServeDown); i++)
        {
            needToCheckListSec[i] = i+(totalNumOnServeDown + 1 - 1-1);
            //t.text += "\n" + "start check downdate node index"+needToCheckListSec[i];
            yield return StartCoroutine(checkDowndateNodeIndex(needToCheckListSec[i]));
            loadingGameObj.transform.GetChild(1).GetComponent<Slider>().value =curUpdateIndexSec/(ContractInteraction._instance.numOnContractSec - (totalNumOnServeDown));
        }
        // while (curUpdateIndexSec != ContractInteraction._instance.numOnContractSec - (totalNumOnServeDown))
        // {
        //     yield return null;
        // }
        t.text += "\n" + "finish down check!";
        StartCoroutine(insertToWebFromContractSec());

    }
    IEnumerator confirmUpdate()
    {   
        print("total num on serve up is: "+totalNumOnServeUp);
        needToCheckList = new int[ContractInteraction._instance.numOnContract-(totalNumOnServeUp)];
        int needtoupdate = ContractInteraction._instance.numOnContract - (totalNumOnServeUp);
        print("new node needed to be updated on serve up is: "+ needtoupdate);
        curUpdateIndex = 0;
        for (int i = 0; i < ContractInteraction._instance.numOnContract - (totalNumOnServeUp); i++)
        {
            needToCheckList[i] = i+(totalNumOnServeUp + 1 - 1-1);
            //t.text += "\n" + "start check update node index"+needToCheckList[i];
            yield return StartCoroutine(checkUpdateNodeIndex(needToCheckList[i]));
            loadingGameObj.transform.GetChild(1).GetComponent<Slider>().value =curUpdateIndex/(ContractInteraction._instance.numOnContract - (totalNumOnServeUp));
        }

        // while (curUpdateIndex != ContractInteraction._instance.numOnContract - (totalNumOnServeUp))
        // {
        //     yield return null;
        // }
        t.text += "\n" + "finish check update up node!";
        StartCoroutine(insertToWebFromContract());

        //ReadData();
    }

    IEnumerator insertToWebFromContract()
    {
        TreeNodeDataInit._instance.isInserting = true;
        TreeNodeDataInit._instance.insertCount = new List<int>();
        ContractInteraction._instance.newNodeInfoContract.Clear();
        int currentIndex = 0;
        loadingGameObj.transform.GetChild(1).GetComponent<Slider>().value =0;
        while (currentIndex < needToCheckList.Length)
        {
            yield return StartCoroutine(readLostNodeFromContract(needToCheckList[currentIndex]));
            currentIndex++;
            loadingGameObj.transform.GetChild(1).GetComponent<Slider>().value +=(1/needToCheckList.Length);
        }

        StartCoroutine(waitTillInsertFinish());
    }

    IEnumerator waitTillInsertFinish()
    {
        while (TreeNodeDataInit._instance.insertCount.Count < needToCheckList.Length)
        {
            yield return null;
        }

        t.text += "finishi insert up!";
        upServePrepare = true;
        TreeNodeDataInit._instance.isInserting = false;
        if (!downServePrepare)
        {
            StartCoroutine(confirmDowndate());
        }
    }
    
    
    IEnumerator insertToWebFromContractSec()
    {
        TreeNodeDataInit._instance.isInsertingSec = true;
        TreeNodeDataInit._instance.insertCountSec = new List<int>();
        ContractInteraction._instance.newNodeInfoContractSec.Clear();
        int currentIndex = 0;
        loadingGameObj.transform.GetChild(1).GetComponent<Slider>().value =0;
        t.text += "\n need to check list sec length is: " + needToCheckListSec.Length; //should be 2
        while (currentIndex < needToCheckListSec.Length)
        {
            yield return StartCoroutine(readLostNodeFromContractSec(needToCheckListSec[currentIndex]));
            currentIndex++;
            loadingGameObj.transform.GetChild(1).GetComponent<Slider>().value +=(1/needToCheckListSec.Length);
        }

        StartCoroutine(waitTillInsertDownFinish());
    }
    IEnumerator waitTillInsertDownFinish()
    {
        while (TreeNodeDataInit._instance.insertCountSec.Count < needToCheckListSec.Length)
        {
            yield return null;
        }

        t.text += "finish insert down!";
        TreeNodeDataInit._instance.isInsertingSec = false;
        downServePrepare = true;
    }

    IEnumerator reReadDataAndStartTree()
    {
        t.text += "finish and restart tree!! wula!! Time to re read data and re start tree!";
        nodePrepared = false;
        ReadData();
        while (!nodePrepared)
        {
            yield return null;
        }

        finalNodePrepared = true;
        reStartTree();
    }

    IEnumerator waitToReStartTree()
    {
        t.text += "\n wait until up and down prepared!: "+upServePrepare+","+downServePrepare;
        while (!upServePrepare || !downServePrepare)
        {
            yield return null;
        }
        Debug.Log("\n finish up and down prepared!: "+upServePrepare +","+downServePrepare);
        t.text += "\n finish up and down prepared!: "+upServePrepare +","+downServePrepare;
        UrLController._instance.upTreeResult = "";
        UrLController._instance.downTreeResult = "";
        StartCoroutine(reReadDataAndStartTree());
        
    }
    

    IEnumerator readLostNodeFromContract(int newListIndex)
    {
        //t.text += "readLostNode: " + newListIndex;
        string nodeindexFromContract = ContractInteraction._instance.newListContract[newListIndex];
        //t.text += "nodeIndex is: " + nodeindexFromContract;
        string nodeindex = nodeindexFromContract.Substring(1, nodeindexFromContract.Length - 2);
        //t.text += "\n nodeindex string is: " + nodeindex;
        int[] layerIndex = new[] { int.Parse(nodeindex.Split(",")[0]), int.Parse(nodeindex.Split(",")[1]) };
        //t.text += "\n nodeindex int is: " + layerIndex[0]+","+layerIndex[1];

        StartCoroutine(ContractInteraction._instance.Check(layerIndex[0], layerIndex[1]));
        
        //修改！

        while (!ContractInteraction._instance.newListFinish.ContainsKey(nodeindexFromContract))
        {
            yield return null;
        }

        //t.text += "\n the contract info is: " + ContractInteraction._instance.newNodeInfoContract[nodeindexFromContract];
        
        string _layer, _ind, _father,info, creator, debuff;
        (_layer, _ind, _father, info, creator, debuff) =
            converToWebInfoSecVer(layerIndex[0],layerIndex[1] ,ContractInteraction._instance.newNodeInfoContract[nodeindexFromContract]);
        //address,blockTime,blood,money,map,father
        
        StartCoroutine(TreeNodeDataInit._instance._insert(int.Parse(_layer),int.Parse(_ind),int.Parse(_father),info,creator,debuff));
        //string: 1693714576,0x433e7287FEAA21fBb2D06Dd6525c98835D49C276,9440,2000,,xx,H,xx,xx,xx,xx,xx,xx,xx,xx,xx,xx,xx,xx,xx,xx,R,xx,/nn,xx,R,xx,xx,R,R,R,R,xx,xx,R,R,R,R,xx,xx,R,xx,/nn,xx,R,xx,xx,R,xx,xx,R,xx,xx,R,xx,xx,R,xx,xx,R,xx,/nn,xx,R,xx,xx,R,xx,xx,R,xx,xx,R,xx,xx,R,xx,xx,R,xx,/nn,xx,R,xx,xx,R,xx,xx,R,xx,xx,R,xx,xx,R,xx,xx,R,xx,/nn,xx,R,xx,xx,R,xx,xx,R,xx,xx,R,xx,xx,R,xx,xx,R,xx,/nn,xx,R,xx,xx,R,xx,xx,R,xx,xx,R,xx,xx,R,xx,xx,R,xx,/nn,xx,R,R,R,R,xx,xx,R,R,R,R,xx,xx,R,R,R,R,xx,/nn,xx,xx,xx,xx,xx,xx,xx,xx,xx,xx,xx,xx,xx,xx,xx,xx,xx,xx,/nn,0,0,0,(1,2),(0,1)
        //        base_struct[0]["(0,1)"] = inlevel({
        //        timestamp: block.timestamp,
        //        owner: address(this),  
        //        blood: 10000, 
        //        money: 1500,
        //        map: initial_map,
        //        wood_protect: 0,
        //        iron_protect: 0,
        //        elec_protect: 0,
        //        block_position: "(0,1)",
        //        original_position: "null"}
        // );
        //"0xfd376a919b9a1280518e9a5e29e3c3637c9faa12-20230905-0-0-0-0-1-1-10000-10000-1500-"+_map+"-0,0,0";
        //_map = "00,H,00,00,00,00,00,00,00,00,00,00,00,00,00,00,R,00,/n,00,R,00,00,R,R,R,R,00,00,R,R,R,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,R,R,R,00,00,R,R,R,R,00,00,R,R,R,R,00,/n,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,/";

    }
    IEnumerator readLostNodeFromContractSec(int newListIndex)
    {
        t.text += "readLostDownNode: " + newListIndex;
        string nodeindexFromContract = ContractInteraction._instance.newListContractSec[newListIndex];
        string nodeindex = nodeindexFromContract.Substring(1, nodeindexFromContract.Length - 2);
        t.text += "\n downnodeindex string is: " + nodeindex;
        int[] layerIndex = new[] { int.Parse(nodeindex.Split(",")[0]), int.Parse(nodeindex.Split(",")[1]) };
        t.text += "\n downnodeindex int is: " + layerIndex[0]+","+layerIndex[1];

        StartCoroutine(ContractInteraction._instance.CheckSec(layerIndex[0], layerIndex[1]));
        
        //修改！

        while (!ContractInteraction._instance.newListFinishSec.ContainsKey(nodeindexFromContract))
        {
            yield return null;
        }

        t.text += "\n" + ContractInteraction._instance.newNodeInfoContractSec[nodeindexFromContract];
        
        string _layer, _ind, _father,info, creator;
        (_layer, _ind, _father, info, creator) =
            converToWebInfoSecVerSec(layerIndex[0],layerIndex[1] ,ContractInteraction._instance.newNodeInfoContractSec[nodeindexFromContract],newListIndex);
        //address,debuffWood,debuffIron,debuffElec,father
        
        StartCoroutine(TreeNodeDataInit._instance. _insertSec(int.Parse( _layer), int.Parse(_ind), int.Parse(_father), info, creator));

    }

    private (string, string, string, string, string) converToWebInfoSecVerSec(int _layer, int _index, string _baseStrucString,int indecAsTime)
    {
        string[] sub = _baseStrucString.Split("-");
        string layer = _layer.ToString();
        string index = _index.ToString();
        string father;
        if (sub[4] == "null" || sub[4] == "")
        {
            father = "0-0";
        }
        else
        {
            string[] subFatherIndex = sub[4].Split(",");
            father = subFatherIndex[0].Substring(1, subFatherIndex[0].Length - 1)+"-"+subFatherIndex[1].Substring(0, subFatherIndex[1].Length - 1);
        }
        string debuff = sub[1] + "," + sub[2] + "," + sub[3];
        string info;
        info = sub[0] + "-" + indecAsTime.ToString() + "-" + father + "-0-" + layer + "-" + index + "-0-" + debuff;


        return (layer, index, father.Split("-")[0], info, sub[0]);
    }
       
    private (string, string, string, string, string, string) converToWebInfoSecVer(int _layer,int _index, string _baseStrucString)
    {
        //owner,time,blood,money,map,father
        string[] sub = _baseStrucString.Split("-");
        //t.text += "\n sub length" + sub.Length;
        string layer = _layer.ToString();
        string ind = _index.ToString();
        string readingMap = sub[4];
        string[] mapIndex = readingMap.Split(",");
        //t.text +="\n"+"mapCount: "+ mapIndex.Length;
        string map = "";
        for (int i = 1; i < 173; i++)
        {
            string insert = mapIndex[i];
            if (mapIndex[i] == "/nn")
            {
                insert = "/n";
            }
            else if (mapIndex[i] == "xx")
            {
                insert = "00";
            }
            if (i < 172)
            {
                map += insert + ",";
            }
            else
            {
                map += insert;
            }
        }

        //t.text += "\n this map: " + map;
        string createAddr = sub[0];
        string timeBlock = sub[1];
        string father;
        string fatherlayerindex = sub[5];
        if (fatherlayerindex == "null" || fatherlayerindex == "")
        {
            father = "0-0";
        }
        else
        {
            string[] subFatherIndex = fatherlayerindex.Split(",");
            father = subFatherIndex[0].Substring(1, subFatherIndex[0].Length - 1)+"-"+subFatherIndex[1].Substring(0, subFatherIndex[1].Length - 1);
        }
        string child = "0";
        string ismajor = "0";
        string curHealth = sub[2];
        string fullHealth = "10000";
        string money = sub[3];
        string debuff = "0,0,0";
        map = map.Substring(0, map.Length - 1);
        string info = createAddr + "-" + timeBlock + "-" + father + "-" + child + "-" + layer + "-" + ind +
                      "-" + ismajor + "-" + curHealth + "-" + fullHealth + "-" + money + "-" + map + "-" + debuff;
        
        return (layer, ind, father.Split("-")[0], info, createAddr, debuff);
        
    }
    IEnumerator checkUpdateNodeIndex(int lostNodeIndex)
    {
        ContractInteraction._instance.ServeLevel(lostNodeIndex);
        while (!ContractInteraction._instance.newListContract.ContainsKey(lostNodeIndex))
        {
            yield return null;
        }
        //Ct.text += "\n" + "curUpdateIndex: "+curUpdateIndex;
        curUpdateIndex += 1;
        
    }
    IEnumerator checkDowndateNodeIndex(int lostNodeIndex)
    {
        ContractInteraction._instance.ServeLevelSec(lostNodeIndex);
        while (!ContractInteraction._instance.newListContractSec.ContainsKey(lostNodeIndex))
        {
            yield return null;
        }
        t.text += "\n" + "curDowndateIndex: "+curUpdateIndexSec;
        curUpdateIndexSec += 1;
    }
    
    
    public void ReadData()
    {
        loadingGameObj.SetActive(true);
        t.text += "start read data!";
        ReadFromWeb();

    }
    
    //"layer0 idx1-"
    private void ReadFromWeb()
    {
        UrLController._instance.upTreeResult = "";
        UrLController._instance.downTreeResult = "";
        
        UrLController._instance.CheckAllUpNode();
        
        t.text += "\n"+"checking all nodes!";
        StartCoroutine(readFromWeb());
    }
    
    IEnumerator readFromWeb()
    {
        t.text += "\n"+"wait for up tree and down tree...!";
        while (UrLController._instance.upTreeResult.Length == 0 || UrLController._instance.downTreeResult.Length == 0)
        {
            yield return null; // 等待一帧
        }

        t.text += "\n"+"finish read from web!";
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
            //t.text+="\n"+_infoString;
            string[] nodeInfo = _infoString.Split("-");
            print( "reading nodes result: "+nodeInfo[0]+", "+ nodeInfo[5]+","+nodeInfo[6]+" map: "+nodeInfo[11]);
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
            newNodeData.towerDebuffList = _debuff;
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
            newNodeData.debuffData = _debuff;
            newNodeData.name = "("+newNodeData.nodeLayer.ToString() + "," + newNodeData.nodeIndex.ToString()+")";
            downNodeDataList.Add(newNodeData);
        }
        //sort node list by time!

        // foreach (var VARIABLE in nodeDataList)
        // {
        //     Debug.Log(VARIABLE.setUpTime);
        // }
        // foreach (var VARIABLE in downNodeDataList)
        // {
        //     Debug.Log(VARIABLE.setUpTime);
        // }
        nodeDataList.Sort((node1, node2) => int.Parse(node1.setUpTime).CompareTo(int.Parse(node2.setUpTime)));
        downNodeDataList.Sort((node1, node2) => int.Parse(node1.setUpTime).CompareTo(int.Parse(node2.setUpTime)));
        
        nodePrepared = true;
        totalNumOnServeUp = nodeDataList.Count;
        totalNumOnServeDown = downNodeDataList.Count;
        t.text +="\n nodeDataList has: "+nodeDataList.Count;
        t.text += "\n downNodeDataList has: " + downNodeDataList.Count;
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
        StartCoroutine(ReadUpNodeFromWeb());
        
    }
    
    private IEnumerator ReadUpNodeFromWeb()
    {
        upStringResults = new List<string>();
        int currentIndex = 0;
        while (currentIndex < upTreeNodeLayerIndex.Count/2)
        {
            yield return StartCoroutine(checkUpNode(upTreeNodeLayerIndex[currentIndex*2], upTreeNodeLayerIndex[currentIndex*2+1]));
            loadingGameObj.transform.GetChild(1).GetComponent<Slider>().value = (currentIndex/upTreeNodeLayerIndex.Count/2)/2;
            currentIndex++;
        }
        StartCoroutine(ReadDownNodeFromWeb()) ;
    }
    private IEnumerator checkUpNode(int layer, int idx)
    {
        string checkString;
        checkString = UrLController._instance.url_search + "?layer=" + layer.ToString() + "&idx=" + idx.ToString();
        Debug.Log("checking node info of Reading Data: "+checkString);
        WWW www = new WWW(checkString);
        yield return www;
        string result = www.text;
        Debug.Log("reading result: "+result);
        upStringResults.Add(result);
        finishUp++;
    }
    private IEnumerator ReadDownNodeFromWeb()
    {
        downStringResults = new List<string>();
        int currentIndex = 0;
        while (currentIndex < downTreeNodeLayerIndex.Count/2)
        {
            yield return (StartCoroutine(checkDownNode(downTreeNodeLayerIndex[currentIndex*2], downTreeNodeLayerIndex[currentIndex*2+1]) ));
            loadingGameObj.transform.GetChild(1).GetComponent<Slider>().value = 0.5f+(currentIndex/downTreeNodeLayerIndex.Count/2)/2;
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
        foreach (int[] VARIABLE in inputList) //int[] = 0,0
        {
            if (VARIABLE.Length == 2)
            {
                //层数，层内序号，父节点层内序号，子节点数量
                genList.Add(VARIABLE[0]);
                genList.Add(VARIABLE[1]);
                NodeData curNodeData = new NodeData();
                if (VARIABLE[0] == 0 && VARIABLE[1] == 0)
                {
                    curNodeData = inputTreeData.nodeDictionary["0,1"];
                }
                else
                {
                    curNodeData = inputTreeData.nodeDictionary[VARIABLE[0].ToString()+','+VARIABLE[1]];
                }
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
                DownNodeData curNodeData = new DownNodeData();
                if (VARIABLE[0] == 0 && VARIABLE[1] == 0)
                {
                    curNodeData = inputDownTreeData.downNodeDictionary["0,1"];
                }
                else
                {
                    curNodeData = inputDownTreeData.downNodeDictionary[VARIABLE[0].ToString()+','+VARIABLE[1]];
                }
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
        t.text += "\n+ " + sequence;
        return sequence;
    }
    private List<int[]> SortSequence(TreeData curTreeData)
    {
        List<int[]> sequence = new List<int[]>();
        foreach (string key in treeData.nodeDictionary.Keys) //"0,1"
        {
            Debug.Log(key);
            int[] indexPair = convertStrInt(key); //int[] = 0,0
            sequence.Add(indexPair);
        }
        sequence.Sort((x, y) =>
        {
            if (x[0] != y[0])
                return x[0].CompareTo(y[0]);
            else
                return x[1].CompareTo(y[1]);
        });
        foreach (var VARIABLE in sequence)
        {
            foreach (var _VARIABLE in VARIABLE)
            {
                GlobalVar._instance.t.text += "-" +_VARIABLE;
            }
        }
        
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
                if (layer == 0 && index == 1)
                {
                    layer = 0;
                    index = 0;
                }
                genList.Add(layer);
                genList.Add(index);
            }
        }
        else
        {
            Debug.LogError("key of tree is not right!");
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
        if (sceneName == "1_0_HomePage" || sceneName == "1_1_SecHomePage")
        {
            finalNodePrepared = false;
        }
        else if(sceneName == "3_1_SecGamePlay"|| sceneName == "3_0_GamePlay")
        {
            //CurNodeDataSummary._instance._initData = false;
            CurNodeDataSummary._instance.gamePlayInitData = false;
        }
        
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
