using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DownTreeData",menuName = "ScriptableObjects/DownTree")]
public class DownTreeData : ScriptableObject
{
    public Dictionary<string, DownNodeData> downNodeDictionary = new Dictionary<string, DownNodeData>();
    public DownNodeData initDownNodeData;
    public int downTreeNodeCount;
}
