using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CreateAssetMenu(fileName = "NodeData",menuName = "ScriptableObjects/Node",order = 3)]
public class NodeData : ScriptableObject
{
    public int fatherLayer;
    public int fatherIndex;
    public int childCount;

    public int nodeLayer;
    public int nodeIndex;
    
    public int health;
    public int monsterCount;
    public int money;
    public string mapStructure;
}
