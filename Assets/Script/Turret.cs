
using System;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("========Weapon Date========")]
    public int range = 20;
    public float shootingRate = 1;
    public float bulletAttack = 0;
    private float shootingCountDown = 0f;

    [Header("========Unity SetUp Field========")]
    public Transform partToRotate;
    public GameObject bulletPrefab;
    public Transform[] firePoint;
    private string tagString = "Enemy";
    private GameObject[] enemies;
    public float rotateSpeed = 10f;
    public AudioClip effectSound;

    [Header("=======Enemy Check======")]
    public Transform target;
    public List<Transform> targetList;

    [Header("========ReadData========")] 
    public GameplayCurSorOutline gpCurSorOutline;
    public bool dataInit;
    public float totalAttack;

    private string _weaponType;
    private int _weaponGrade;
    private float _range;
    private float _attack;
    private float _bulletPerSecond;
    
    
    
    void Start()
    {
        totalAttack = 0f;
        dataInit = false;
        targetList = new List<Transform>();
        gpCurSorOutline = transform.GetComponent<GameplayCurSorOutline>();
        InvokeRepeating("TargetUpdate",0f,0.1f);
        gpCurSorOutline.attackColliderCondition.SetActive(false);
    }

    private void Update()
    {
        //Debug.Log("weapon type: "+ gpCurSorOutline.weaponType);
        //Debug.Log("init data: "+ CurNodeDataSummary._instance._initData);
        if (GlobalVar.CurrentGameState == GlobalVar.GameState.GamePlay && dataInit == false &&
            gpCurSorOutline.weaponType != "" && CurNodeDataSummary._instance._initData)
        {
            
            Debug.Log("Start to fire enmey");
            if (gpCurSorOutline.weaponType == "wood" || gpCurSorOutline.weaponType == "iron")
            {
                if (!gpCurSorOutline.attackColliderCondition.activeSelf)
                {
                    gpCurSorOutline.attackColliderCondition.SetActive(true);
                }
            }
            else
            {
                if (gpCurSorOutline.weaponGrade < GlobalVar._instance.elecTowerData.gradeRange2)
                {
                    gpCurSorOutline.attackColliderCondition.SetActive(true);
                    gpCurSorOutline.attackColliderCondition.transform.GetChild(0).gameObject.SetActive(true);
                    gpCurSorOutline.attackColliderCondition.transform.GetChild(1).gameObject.SetActive(false);
                    gpCurSorOutline.attackColliderCondition.transform.GetChild(2).gameObject.SetActive(false);
                }
                else if (gpCurSorOutline.weaponGrade >= GlobalVar._instance.elecTowerData.gradeRange2 &&
                         gpCurSorOutline.weaponGrade < GlobalVar._instance.elecTowerData.gradeRange3)
                {
                    gpCurSorOutline.attackColliderCondition.SetActive(true);
                    gpCurSorOutline.attackColliderCondition.transform.GetChild(0).gameObject.SetActive(false);
                    gpCurSorOutline.attackColliderCondition.transform.GetChild(1).gameObject.SetActive(true);
                    gpCurSorOutline.attackColliderCondition.transform.GetChild(2).gameObject.SetActive(false);
                }
                else if (gpCurSorOutline.weaponGrade >= GlobalVar._instance.elecTowerData.gradeRange3)
                {
                    gpCurSorOutline.attackColliderCondition.SetActive(true);
                    gpCurSorOutline.attackColliderCondition.transform.GetChild(0).gameObject.SetActive(false);
                    gpCurSorOutline.attackColliderCondition.transform.GetChild(1).gameObject.SetActive(false);
                    gpCurSorOutline.attackColliderCondition.transform.GetChild(2).gameObject.SetActive(true);
                }
            }

        }
        
        if (GlobalVar.CurrentGameState == GlobalVar.GameState.GamePlay && !dataInit && gpCurSorOutline.weaponType!="" && CurNodeDataSummary._instance.gamePlayInitData && gpCurSorOutline.attackColliderCondition.activeSelf)
        {
            dataInit = true;
            _weaponType = gpCurSorOutline.weaponType;
            _weaponGrade = gpCurSorOutline.weaponGrade;
            if (_weaponType.Substring(1, 3) != "pro")
            {
                (_attack,_bulletPerSecond,_range) =
                    CurNodeDataSummary._instance.CheckAttackSpeedRangeFloat(_weaponType, _weaponGrade);
                if (_weaponType == "wood")
                {
                    bulletAttack =  CurNodeDataSummary._instance.CheckAttackAfterDebuff(_weaponType,_checkDebuff(0),(int)_weaponGrade,(float)_bulletPerSecond);
                }
                else if (_weaponType == "iron")
                {
                    bulletAttack =  CurNodeDataSummary._instance.CheckAttackAfterDebuff(_weaponType,_checkDebuff(1),(int)_weaponGrade,(float)_bulletPerSecond);
                }
                else if (_weaponType == "elec")
                {
                    bulletAttack =  CurNodeDataSummary._instance.CheckAttackAfterDebuff(_weaponType,_checkDebuff(2),(int)_weaponGrade,(float)_bulletPerSecond);
                }
                shootingRate = _bulletPerSecond;
            }
        }
    }

    private int _checkDebuff(int weaponIndex)
    {
        float totalDebuff = CurNodeDataSummary._instance.debuffList[weaponIndex] -
                            CurNodeDataSummary._instance.protectList[weaponIndex];
        print("the debuff of weapon index "+weaponIndex+"is: "+CurNodeDataSummary._instance.debuffList[weaponIndex]);
        print("the protect of weapon index "+weaponIndex+"is: "+CurNodeDataSummary._instance.protectList[weaponIndex]);
        if (totalDebuff > 0)
        {
            return ((int)totalDebuff);
        }
        else
        {
            return 0;
        }
    }
    
    public void _onTriggerEnter(Transform addedTransform)
    {
        Debug.Log("trigger enter!"+ addedTransform.name);
        if (!targetList.Contains(addedTransform))
        {
            targetList.Add(addedTransform);
        }
    }
    public void _onTriggerExit(Transform removeTransform)
    {
        Debug.Log("trigger Exit!"+ removeTransform.name);
        if (targetList.Contains(removeTransform))
        {
            targetList.Remove(removeTransform);
        }
    }
    
    void TargetUpdate()
    {
        // 创建一个新的列表，用于存储要保留的目标
        List<Transform> validTargets = new List<Transform>();
        if (targetList.Count > 0)
        {
            foreach (Transform targetTransform in targetList)
            {
                if (targetTransform != null)
                {
                    // 检查每个目标是否有效（假设有效的条件是 isBody 为 false）
                    if (!targetTransform.GetComponent<EnemyData>().isBody)
                    {
                        validTargets.Add(targetTransform);
                    }
                }
            }
        }
        
        // 更新 targetList 为有效的目标列表
        targetList = validTargets;

        if (targetList.Count > 0)
        {
            target = targetList[0];
        }
        else
        {
            target = null;
        }
    }
    

    // void TargetUpdate()
    // {
    //     GameObject nearestEnemy = null;
    //     float shorestDistence = Mathf.Infinity;
    //     enemies = GameObject.FindGameObjectsWithTag(tagString);
    //     foreach (GameObject enemy in enemies)
    //     {
    //         float distance = Vector3.Distance(enemy.transform.position, transform.position);
    //         if (distance <= shorestDistence)
    //         {
    //             shorestDistence = distance;
    //             nearestEnemy = enemy;
    //             //Debug.Log(shorestDistence);
    //         }
    //     }
    //
    //     if (shorestDistence <= range && nearestEnemy != null)
    //     {
    //         target = nearestEnemy.transform;
    //     }
    //     else
    //     {
    //         target = null;
    //     }
    // }

    private void FixedUpdate()
    {
        if (target == null)
            return;

        Vector3 direction =target.position - partToRotate.transform.position;
        direction = new Vector3(direction.x, 0, direction.z);

        float angle = Vector3.SignedAngle(partToRotate.transform.forward, direction, Vector3.up);

        Quaternion targetRotation = Quaternion.Euler(0f, partToRotate.transform.rotation.eulerAngles.y+angle-90f, 0f);
        partToRotate.transform.rotation = Quaternion.Slerp(partToRotate.transform.rotation, targetRotation, 10*rotateSpeed * Time.fixedDeltaTime);
        //partToRotate.transform.rotation = targetRotation;
        
        if (shootingCountDown <= 0f)
        {
            shoot();
            shootingCountDown = 1f / shootingRate;
        }

        shootingCountDown -= Time.fixedDeltaTime;
    }

    private void shoot()
    {
        SoundManager._instance.PlayEffectSound(effectSound);
        Debug.Log(_weaponType.ToString() +_weaponGrade.ToString() +" SHOOT!"+bulletAttack.ToString());
        foreach (Transform fpt in firePoint)
        {
            GameObject BulletGo = (GameObject)Instantiate(bulletPrefab, fpt.position, fpt.rotation);
            Bullet bullet = BulletGo.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.attackData = bulletAttack;
                bullet.seek(target);
                totalAttack += bullet.attackData;
            }
        }
    }
    // private void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position,range);
    // }
}
