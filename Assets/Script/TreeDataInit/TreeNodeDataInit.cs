using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeNodeDataInit : MonoBehaviour
{
    public TreeData treeData;

    private void Start()
    {
        treeData.nodeDictionary.Add("0,1",treeData.InitNodeData);
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
            newNodeData.nodeIndex = GetMaxSecondNumber(layer_index[0] + 1);
            
            //处理layer升级之后的数据，待处理
            newNodeData.health = baseNodeData.health;
            newNodeData.monsterCount = baseNodeData.monsterCount;
            newNodeData.money = baseNodeData.money;
            newNodeData.mapStructure = baseNodeData.mapStructure;
        }
    }
    private void SortSequence(TreeData curTreeData)
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
    }
    private int GetMaxSecondNumber(int firstNumber)
    {
        List<int[]> sequence = new List<int[]>();
        foreach (string key in treeData.nodeDictionary.Keys)
        {
            int[] indexPair = convertStrInt(key);
            sequence.Add(indexPair);
        }
        int maxSecondNumber = int.MinValue;
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
