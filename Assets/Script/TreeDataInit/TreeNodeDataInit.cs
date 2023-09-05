using System;
using System.Collections;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TreeNodeDataInit : MonoBehaviour
{
    public static TreeNodeDataInit _instance;
    public TreeData treeData;
    public DownTreeData downTreeData;
    public List<NodeData> previousNodeData;
    public List<DownNodeData> previousDownNodeData;
    public string levelName = "GamePlay";
    private GlobalVar.GameState _previousGameState;
    public bool finish=false;

    private int restartTime;

    private void Awake()
    {
        _instance = this;
        restartTime = 0;
    }

    public void ReStart()
    {
        restartTime++;
        //print("restart time: "+restartTime);
        // treeData = new TreeData();
        // downTreeData = new DownTreeData();
        treeData.nodeDictionary.Clear();
        downTreeData.downNodeDictionary.Clear();
        if (GlobalVar.CurrentGameState == GlobalVar.GameState.MainStart)
        {
            previousNodeData = GlobalVar._instance.nodeDataList;
            previousDownNodeData = GlobalVar._instance.downNodeDataList;
            //
            // treeData.nodeDictionary = new Dictionary<string, NodeData>();
            // treeData.InitNodeData = initNodeData(treeData.InitNodeData);
            // treeData.nodeDictionary.Add("0,1",treeData.InitNodeData);
            //
            // downTreeData.downNodeDictionary = new Dictionary<string, DownNodeData>();
            // downTreeData.initDownNodeData = initDownNodeData(downTreeData.initDownNodeData);
            // downTreeData.downNodeDictionary.Add("0,1",downTreeData.initDownNodeData);
            // treeData.treeNodeCount += 1;
            // downTreeData.downTreeNodeCount += 1;
            //
            foreach (NodeData _nodeData in previousNodeData)
            {
                treeData.nodeDictionary.Add(_nodeData.nodeLayer.ToString()+','+_nodeData.nodeIndex.ToString(),_nodeData);
                treeData.treeNodeCount += 1;
            }
            foreach (DownNodeData _downNodeData in previousDownNodeData)
            {
                downTreeData.downNodeDictionary.Add(_downNodeData.nodeLayer.ToString()+','+_downNodeData.nodeIndex.ToString(),_downNodeData);
                treeData.treeNodeCount += 1;
            }

            GlobalVar._instance._convert2TreeGen(treeData);
            GlobalVar._instance._downConvert2TreeGen(downTreeData);
            //LineGenerator._instance.lineMap = new Dictionary<Vector3, int>();
            TreeGenerator._instance.InitTree();
            RedTreeGenerator._instance.InitDownTree();
            LineGenerator._instance.ReStart();
            RedLineGenerator._instance.ReStart();
        }
    }
    
    // private DownNodeData initDownNodeData(DownNodeData initNode)
    // {
    //     initNode.setUpTime = "";
    //     initNode.fatherLayer = 0;
    //     initNode.fatherIndex = 0;
    //     initNode.childCount = 0;
    //     initNode.nodeLayer = 0;
    //     initNode.nodeIndex = 1;
    //     initNode.debuffData = new int[3];
    //     initNode.debuffData[0] =initNode.debuffData[1] =initNode.debuffData[2]= 0;
    //     return initNode;
    // }
    // private NodeData initNodeData(NodeData initNode)
    // {
    //     DateTime currentDateTime = DateTime.UtcNow;
    //     string nowUTC = currentDateTime.ToString();
    //     initNode.setUpTime = nowUTC;
    //     initNode.fatherLayer = 0;
    //     initNode.fatherIndex = 0;
    //     initNode.childCount = 0;
    //     initNode.nodeLayer = 0;
    //     initNode.nodeIndex = 1;
    //     initNode.fullHealth = 10000;
    //     initNode.curHealth = 10000;
    //     initNode.money = 1500;
    //     initNode.mapStructure = "00,H,00,00,00,00,00,00,00,00,00,00,00,00,00,00,R,00,/n,00,R,00,00,R,R,R,R,00,00,R,R,R,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,R,R,R,00,00,R,R,R,R,00,00,R,R,R,R,00,/n,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,/";
    //     return initNode;
    // }
    //???????????????node???,??????????????
    
    public void AddNodeData()
    {
        DateTime currentDateTime = DateTime.UtcNow;
        string nowUTC = currentDateTime.ToString();
        NodeData previousData = GlobalVar._instance.chosenNodeData;
        NodeData newNodeData = new NodeData();
        newNodeData.ownerAddr = GlobalVar._instance.thisUserAddr;
        newNodeData.setUpTime = nowUTC;
        newNodeData.fatherLayer = previousData.nodeLayer;
        newNodeData.fatherIndex = previousData.nodeIndex;
        newNodeData.childCount = 0;
        
        newNodeData.nodeLayer = previousData.nodeLayer+1;
        newNodeData.nodeIndex = GetMaxSecondNumber(previousData.nodeLayer+1)+1;
        newNodeData.isMajor = false;
        
        //????layer???????????????????
        newNodeData.curHealth = (int)CurNodeDataSummary._instance.homeCurHealth;
        newNodeData.fullHealth =(int)CurNodeDataSummary._instance.homeMaxHealth;
        newNodeData.money = (int)CurNodeDataSummary._instance.moneyLeft+500;
        newNodeData.mapStructure = GlobalVar._instance._getMapmapString(GlobalVar._instance.mapmapList);
        string newNodeName = newNodeData.nodeLayer.ToString() + ',' + newNodeData.nodeIndex.ToString();
        //
        // //???????›Ô?????¡¤????
        // string assetPath = "Assets/Resources/" + newNodeName + ".asset";
        // AssetDatabase.CreateAsset(newNodeData, assetPath);
        // AssetDatabase.SaveAssets();
        // AssetDatabase.Refresh();

        string _info = GlobalVar._instance.thisUserAddr +"-"+ nowUTC+ "-" + newNodeData.fatherLayer+"-" +newNodeData.fatherIndex+"-"+newNodeData.childCount+"-"+newNodeData.nodeLayer+"-"+newNodeData.nodeIndex+"-";
        if (newNodeData.isMajor == false)
        {
            _info += "0-";
        }
        else
        {
            _info += "1-";
        }

        _info += newNodeData.curHealth + "-" + newNodeData.fullHealth + "-" + newNodeData.money + "-" +
                 newNodeData.mapStructure + "-";
        int[] debuff_list = GlobalVar._instance.chosenDownNodeData.debuffData;
        _info += debuff_list[0] + "," + debuff_list[1] + "," + debuff_list[2];
        
        Debug.Log("New node created and saved: " + newNodeName);
        StartCoroutine(_insert(newNodeData.nodeLayer,newNodeData.nodeIndex,newNodeData.nodeLayer-1,_info,newNodeData.ownerAddr,debuff_list[0] + "," + debuff_list[1] + "," + debuff_list[2]));

    }
    IEnumerator _insert(int layer, int idx, int father, string info, string creator, string debuff)
    {
        string insertString;
        insertString = UrLController._instance.url_insert + "?layer=" + layer.ToString() + "&idx=" + idx.ToString() + "&father=" + father.ToString() +
                       "&info=" + info.ToString() + "&creator=" + creator.ToString() + "&debuff=" + debuff.ToString();
        Debug.Log(insertString);
        WWW www = new WWW(insertString);
        yield return www;
        string result = www.text;
        Debug.Log(result);
        
        
        UrLController._instance.upTreeResult = "";
        UrLController._instance.downTreeResult = "";
        
        GlobalVar._instance.ReadData();
        StartCoroutine(ReReadNode());
    }

    IEnumerator ReReadNode()
    {
        while (!GlobalVar._instance.nodePrepared)
        {
            yield return null;
        }
        GlobalVar._instance._loadNextScene("4_End");
    }
    

    public void AddDownNodeData()
    {
        DateTime currentDateTime = DateTime.UtcNow;
        string nowUTC = currentDateTime.ToString();
        DownNodeData previousDownNodeData = GlobalVar._instance.chosenDownNodeData;
        DownNodeData newDownNodeData = new DownNodeData();
        newDownNodeData.ownerAddr = GlobalVar._instance.thisUserAddr;
        newDownNodeData.setUpTime = nowUTC;
        newDownNodeData.fatherLayer = previousDownNodeData.nodeLayer;
        newDownNodeData.fatherIndex = previousDownNodeData.nodeIndex;
        newDownNodeData.childCount = 0;

        newDownNodeData.nodeLayer = previousDownNodeData.nodeLayer + 1;
        newDownNodeData.nodeIndex = GetDownMaxSecondNumber(previousDownNodeData.nodeLayer + 1) + 1;
        newDownNodeData.isMajor = false;

        newDownNodeData.debuffData = CurNodeDataSummary._instance.curDebuffList;
        string newDownNodeName = "("+newDownNodeData.nodeLayer.ToString() + ',' + newDownNodeData.nodeIndex.ToString()+")";
        
        string _info = GlobalVar._instance.thisUserAddr +"-"+ nowUTC+ "-" + newDownNodeData.fatherLayer+"-" +newDownNodeData.fatherIndex+"-"+newDownNodeData.childCount+"-"+newDownNodeData.nodeLayer+"-"+newDownNodeData.nodeIndex+"-";
        if (newDownNodeData.isMajor == false)
        {
            _info += "0-";
        }
        else
        {
            _info += "1-";
        }
        int[] debuff_list = CurNodeDataSummary._instance.debuffList;
        _info += debuff_list[0] + "," + debuff_list[1] + "," + debuff_list[2];
        
        Debug.Log("New down node created and saved: " + newDownNodeName);
        StartCoroutine(_insertSec(newDownNodeData.nodeLayer,newDownNodeData.nodeIndex,newDownNodeData.nodeLayer-1,_info,newDownNodeData.ownerAddr));

        Debug.Log("New down node created and saved: " + newDownNodeName);
        GlobalVar._instance.dataPrepared = false;
    }
    IEnumerator _insertSec(int layer, int idx, int father, string info, string creator)
    {
        string insertString;
        insertString = UrLController._instance.url_insertSec + "?layer=" + layer.ToString() + "&idx=" + idx.ToString() + "&father=" + father.ToString() +
                       "&info=" + info.ToString() + "&creator=" + creator.ToString();
        Debug.Log(insertString);
        WWW www = new WWW(insertString);
        yield return www;
        string result = www.text;
        Debug.Log(result);
        
        UrLController._instance.upTreeResult = "";
        UrLController._instance.downTreeResult = "";
        
        GlobalVar._instance.ReadData();
        StartCoroutine(ReReadNode());
    }
    
    
    private int GetMaxSecondNumber(int firstNumber)
    {
        List<int[]> sequence = new List<int[]>();
        foreach (string key in treeData.nodeDictionary.Keys)
        {
            int[] indexPair = convertStrInt(key);
            sequence.Add(indexPair);
        }
        int maxSecondNumber = 0;
        foreach (int[] pair in sequence)
        {
            if (pair[0] == firstNumber && pair[1] > maxSecondNumber)
            {
                maxSecondNumber = pair[1];
            }
        }

        return maxSecondNumber;
    }
    private int GetDownMaxSecondNumber(int firstNumber)
    {
        List<int[]> sequence = new List<int[]>();
        foreach (string key in downTreeData.downNodeDictionary.Keys)
        {
            int[] indexPair = convertStrInt(key);
            sequence.Add(indexPair);
        }
        int maxSecondNumber = 0;
        foreach (int[] pair in sequence)
        {
            if (pair[0] == firstNumber && pair[1] > maxSecondNumber)
            {
                maxSecondNumber = pair[1];
            }
        }

        return maxSecondNumber;
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
            Debug.LogError("????key???????!");
        }
        return genList.ToArray();
    }
    
}
