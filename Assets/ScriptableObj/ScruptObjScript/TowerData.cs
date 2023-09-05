using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Tower",menuName = "ScriptableObjects/TowerData",order = 1)]
public class TowerData : ScriptableObject
{
    public string towerType = "wood";

    public int baseBulletAttack = 4;

    public float baseBulletNumberPerSecond = 1;
    
    public int basicRange = 5;
    public int gradeRange2 = 5;
    public int gradeRange3 = 10;

    public int performGrade2 = 5;
    public int performGrade3 = 10;

    public int basePrice = 600;
    public int merge_price = 40;
    /*struct in_tower{
        string tower_type;

        uint256 base_bullet_attack; 
        uint256 base_bullet_num;
        uint256 basic_price;

        uint256 grade_range2; 
        uint256 grade_range3; 

        uint256 grade_speed_to_attack; 

        uint256 upgrade_price1_5; 
        uint256 upgrade_price_inf; 
    }*/
}
