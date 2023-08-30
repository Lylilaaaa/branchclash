using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TreeData",menuName = "ScriptableObjects/Tree")]
public class TreeData : ScriptableObject
{
    public Dictionary<string, NodeData> nodeDictionary = new Dictionary<string, NodeData>();
    public NodeData InitNodeData;
    public DownNodeData InitDownNodeData;
    public int treeNodeCount;
}
