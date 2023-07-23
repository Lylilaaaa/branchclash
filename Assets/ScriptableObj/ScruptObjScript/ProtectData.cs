using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Protect",menuName = "ScriptableObjects/ProtectData")]
public class ProtectData : ScriptableObject
{
    public string proType = "wood";
    
    public int baseProtect = 1;
    public int performGrade2 = 5;
    public int performGrade3 = 10;
    
    public int basePrice = 100;
    public int upGradePrice = 50;
}
