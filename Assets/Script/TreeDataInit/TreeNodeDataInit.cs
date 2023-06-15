using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;

public class TreeNodeDataInit : MonoBehaviour
{
    public TreeData treeData;

    private void Start()
    {
        treeData.nodeDictionary = new Dictionary<string, NodeData>();
        treeData.InitNodeData = initNodeData(treeData.InitNodeData);
        treeData.nodeDictionary.Add("0,1",treeData.InitNodeData);
        treeData.treeNodeCount += 1;
        fake_preAdd("0,1");
        fake_preAdd("0,1");
        fake_preAdd("0,1");
        
        GlobalVar._instance._convert2TreeGen(treeData);
        TreeGenerator._instance.InitTree();
    }

    private void Update()
    {
        treeData.treeNodeCount = treeData.nodeDictionary.Count;
    }

    private NodeData initNodeData(NodeData initNode)
    {
        initNode.fatherLayer = 0;
        initNode.fatherIndex = 0;
        initNode.childCount = 0;
        initNode.nodeLayer = 0;
        initNode.nodeIndex = 1;
        initNode.health = 100;
        initNode.monsterCount = 2;
        initNode.money = 50;
        initNode.mapStructure = "00,H,00,00,00,00,00,00,00,00,00,00,00,00,00,00,R,00,/n,00,R,00,00,R,R,R,R,00,00,R,R,R,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,R,R,R,00,00,R,R,R,R,00,00,R,R,R,R,00,/n,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,/";
        return initNode;
    }
    //先假设预先生成几个node再说,真的加节点需要刷新树
    private void fake_preAdd(string father)
    {
        if (!treeData.nodeDictionary.ContainsKey(father))
        {
            Debug.Log("Base node not found.");
        }
        else
        {
            int[] layer_index = convertStrInt(father);
            NodeData baseNodeData = treeData.nodeDictionary[father];
            treeData.nodeDictionary[father].childCount += 1;
            //init
            NodeData newNodeData = new NodeData();
            newNodeData.fatherLayer = layer_index[0];
            newNodeData.fatherIndex = layer_index[1];
            newNodeData.childCount = 0;
            
            newNodeData.nodeLayer = layer_index[0]+1;
            newNodeData.nodeIndex = GetMaxSecondNumber(layer_index[0] + 1)+1;
            
            //处理layer升级之后的数据，待处理
            newNodeData.health = baseNodeData.health;
            newNodeData.monsterCount = baseNodeData.monsterCount;
            newNodeData.money = baseNodeData.money;
            newNodeData.mapStructure = baseNodeData.mapStructure;
            string newNodeName = newNodeData.nodeLayer.ToString() + ',' + newNodeData.nodeIndex.ToString();
            
            treeData.nodeDictionary.Add(newNodeName, newNodeData);
            
            // 将新节点保存到文件夹路径中
            string assetPath = "Assets/ScriptableObj/NodeDataObj/" + newNodeName + ".asset";
            AssetDatabase.CreateAsset(newNodeData, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("New node created and saved: " + newNodeName);
            treeData.treeNodeCount += 1;
        }
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
    
}
