
using System;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("========Weapon Date========")]
    public int range = 20;
    public float shootingRate = 1;
    public int bulletAttack = 0;
    private float shootingCountDown = 0f;

    [Header("========Unity SetUp Field========")]
    public Transform partToRotate;
    public Transform target;
    public GameObject bulletPrefab;
    public Transform[] firePoint;
    private string tagString = "Enemy";
    private GameObject[] enemies;
    public float rotateSpeed = 10f;

    [Header("========ReadData========")] 
    public GameplayCurSorOutline gpCurSorOutline;
    public bool dataInit;

    private string _weaponType;
    private int _weaponGrade;
    private string _range;
    private string _attack;
    private string _bulletPerSecond;
    
    void Start()
    {
        gpCurSorOutline = transform.GetComponent<GameplayCurSorOutline>();
        InvokeRepeating("TargetUpdate",0f,0.5f);
    }

    private void Update()
    {
        if (GlobalVar.CurrentGameState == GlobalVar.GameState.GamePlay && dataInit == false && gpCurSorOutline.weaponType!="" && CurNodeDataSummary._instance.gamePlayInitData == true)
        {
            dataInit = true;
            _weaponType = gpCurSorOutline.weaponType;
            _weaponGrade = gpCurSorOutline.weaponGrade;
            if (_weaponType.Substring(1, 3) != "pro")
            {
                (_attack,_bulletPerSecond,_range) =
                    CurNodeDataSummary._instance.CheckAttackSpeedRange(_weaponType, _weaponGrade);
                if (_weaponType == "wood")
                {
                    bulletAttack = int.Parse(_attack)-_checkDebuff(0);
                }
                else if (_weaponType == "iron")
                {
                    bulletAttack = int.Parse(_attack)-_checkDebuff(1);
                }
                else if (_weaponType == "elec")
                {
                    bulletAttack = int.Parse(_attack)-_checkDebuff(2);
                }
                //bulletAttack = int.Parse(_attack);
                if (_range == "full map")
                {
                    range = 20 * 100;
                }
                else
                {
                    range = 20 * int.Parse(_range);
                }
                shootingRate = float.Parse(_bulletPerSecond);
            }
        }

    }

    private int _checkDebuff(int weaponIndex)
    {
        float totalDebuff = CurNodeDataSummary._instance.debuffList[weaponIndex] -
                            CurNodeDataSummary._instance.protectList[weaponIndex];
        if (totalDebuff > 0)
        {
            return ((int)totalDebuff);
        }
        else
        {
            return 0;
        }
    }

    void TargetUpdate()
    {
        GameObject nearestEnemy = null;
        float shorestDistence = Mathf.Infinity;
        enemies = GameObject.FindGameObjectsWithTag(tagString);
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(enemy.transform.position, transform.position);
            if (distance <= shorestDistence)
            {
                shorestDistence = distance;
                nearestEnemy = enemy;
                //Debug.Log(shorestDistence);
            }
        }

        if (shorestDistence <= range && nearestEnemy != null)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    private void FixedUpdate()
    {
        if (target == null)
            return;
        // Calculate the direction from this object to the target
        Vector3 direction = target.position - partToRotate.transform.position;

        // Calculate the angle between the current forward direction and the target direction in the Y axis
        float angle = Vector3.SignedAngle(partToRotate.transform.forward, direction, Vector3.up);

        // Create a new rotation that only rotates in the Y axis
        Quaternion targetRotation = Quaternion.Euler(0f, angle, 0f);

        // Use Lerp to smoothly interpolate between the current rotation and the target rotation
        partToRotate.transform.rotation = Quaternion.Lerp(partToRotate.transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
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
        
        //Debug.Log("SHOOT!"+bulletAttack.ToString());
        foreach (Transform fpt in firePoint)
        {
            GameObject BulletGo = (GameObject)Instantiate(bulletPrefab, fpt.position, fpt.rotation);
            Bullet bullet = BulletGo.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.attackData = bulletAttack;
                bullet.seek(target);
            }
        }

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,range);
    }
}
