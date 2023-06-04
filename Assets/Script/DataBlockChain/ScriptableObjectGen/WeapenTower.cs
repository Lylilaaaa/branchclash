
using UnityEngine;

[CreateAssetMenu(menuName = "TowerWeapon")]
public class WeapenTower : ScriptableObject
{
    public float bulletAttack = 1f;
    public float bulletNum = 2f;  //1s bullet number
    public float basicMoney = 600f;

    public float bulletAttack2;
    public float bulletNum2;
    public float upgrade2Money = 400f;
    
    public float bulletAttack3;
    public float bulletNum3;
    public float upgrade3Money = 500f;
}
