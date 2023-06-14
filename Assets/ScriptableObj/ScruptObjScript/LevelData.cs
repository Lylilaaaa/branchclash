using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Level",menuName = "ScriptableObjects/LevelData",order = 2)]
public class LevelData : ScriptableObject
{
    public int levelNum = 0;

    public int monsterInit = 0;
    
    public int deltaMonster = 4;

    public int homeHealth = 100;
    
    public List<string> MergeMem;
    
}
