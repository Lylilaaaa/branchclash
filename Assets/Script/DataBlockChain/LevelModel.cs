using UnityEngine;
using UnityEngine.UI;

public class LevelModel : MonoBehaviour
{
    [Header("===== Weapon Property =====")]
    public float weaponRange;
    public WeapenTower weaponScriptableObj;
    
    [Header("===== Monster Property =====")]
    public Monster monsterScriptableObj;

    public float CurMonsterHealth;
    public float totalWeaponEffort;


    [Header("===== Level Data =====")]
    
    public float levelNum = 0;

    public float homeHealth = 100;

    public float totalMoney;

    public float monsterNum = 2;

    public float weaponNum = 0;
    public float weaponNum2 = 0;
    public float weaponNum3 = 0;

    public bool hasCalculate = false;
    
    [Header("===== UI Settings =====")]
    public Text levelUI;
    public Text homeHealthUI;
    public Text moneyNumUI;
    
    public Text monsterNumUI;
    public Text monsterHealth;
    public Text monsterAttackPerWeapon;
    
    public Text weaponNumUI;
    public Text WeaponTotalAttack;
    public Text weaponRangeUI;
    
    
    // Start is called before the first frame update
    void Start()
    {
        CurMonsterHealth = monsterScriptableObj.basicHealth;
        weaponRange = Counter._instance.AVERAGERANK;
    }

    // Update is called once per frame
    void Update()
    {
        weaponRange = Counter._instance.AVERAGERANK;
        levelUI.text = levelNum.ToString();
        homeHealthUI.text = homeHealth.ToString();
        moneyNumUI.text = totalMoney.ToString();
        monsterNumUI.text = monsterNum.ToString();
        monsterHealth.text = (monsterNum * CurMonsterHealth).ToString();
        weaponNumUI.text = weaponNum.ToString();
        weaponRangeUI.text = weaponRange.ToString();
        
        WeaponTotalAttack.text = (((weaponRange / monsterScriptableObj.movingSpeed) *    //one monster been attacked time
                                   (weaponScriptableObj.bulletAttack * weaponScriptableObj.bulletNum)*(weaponNum-weaponNum2-weaponNum3) + 
                                   (weaponScriptableObj.bulletAttack2 * weaponScriptableObj.bulletNum2)*(weaponNum2)+
                                   (weaponScriptableObj.bulletAttack3 * weaponScriptableObj.bulletNum3)*(weaponNum3)) *  monsterNum).ToString();
        monsterAttackPerWeapon.text = ((weaponRange / monsterScriptableObj.movingSpeed) *
                                       (weaponScriptableObj.bulletAttack * weaponScriptableObj.bulletNum)).ToString(); //每一个武器能够给每一个怪物打击的伤害
    }

    public void NextLevel()
    {
        hasCalculate = false;
        levelNum += 1;
        totalMoney += 4000;
    }
    
    public void MonsterHealthUpGrade()
    {
        CurMonsterHealth += 6f;
    }
    public void WeaponPlusOne()
    {
        weaponNum += 1;
        totalMoney -= weaponScriptableObj.basicMoney;
    }

    public void Calculating()
    {
        float totalMonsterHealth;
        totalMonsterHealth = monsterNum * CurMonsterHealth;
        
        totalWeaponEffort =
            ((weaponRange / monsterScriptableObj.movingSpeed) *    //one monster been attacked time
             (weaponScriptableObj.bulletAttack * weaponScriptableObj.bulletNum)*(weaponNum-weaponNum2-weaponNum3) + 
             (weaponScriptableObj.bulletAttack2 * weaponScriptableObj.bulletNum2)*(weaponNum2)+
             (weaponScriptableObj.bulletAttack3 * weaponScriptableObj.bulletNum3)*(weaponNum3)) *  monsterNum;

        float homeHealthDetection;
        homeHealthDetection = totalMonsterHealth - totalWeaponEffort;

        if (homeHealthDetection > 0) //&& !hasCalculate)
        {
            homeHealth -= homeHealthDetection;
            hasCalculate = true;
        }
    }

    public void upGradeOneWeapon2()
    {
        if (weaponNum >= 1)
        {
            weaponNum2 += 1;
            totalMoney -= weaponScriptableObj.upgrade2Money;
        }
    }
    public void upGradeOneWeapon3()
    {
        if (weaponNum2 >= 1)
        {
            weaponNum3 += 1;
            totalMoney -= weaponScriptableObj.upgrade3Money;
        }
    }
}
