using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CreateAssetMenu(fileName = "NodeData",menuName = "ScriptableObjects/Node",order = 3)]
public class NodeData : ScriptableObject
{
    //13
    public string ownerAddr;
    public string setUpTime;
    public int fatherLayer;
    public int fatherIndex;
    public int childCount;

    public int nodeLayer;
    public int nodeIndex;
    public bool isMajor = false;
    
    //basicInfo
    public int curHealth;
    public int fullHealth;
    public int money;
    public string mapStructure;
    public int[] towerDebuffList = new int[3];
}
