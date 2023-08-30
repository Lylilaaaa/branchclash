using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CreateAssetMenu(fileName = "DownNodeData",menuName = "ScriptableObjects/DownNode",order = 3)]
public class DownNodeData : ScriptableObject
{
    public string ownerAddr;
    public string setUpTime;
    public int fatherLayer;
    public int fatherIndex;
    public int childCount;
    public bool isMajor;
    
    public int nodeLayer;
    public int nodeIndex;
    
    public int[] debuffData = new int[3];
    public int[] protectData = new int[3];
}
