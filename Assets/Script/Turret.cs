
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Weapon Date")]
    public int range = 20;
    public float shootingRate = 1;
    public int bulletAttack = 0;
    private float shootingCountDown = 0f;

    [Header("Unity SetUp Field")]
    public Transform partToRotate;
    public Transform target;
    public GameObject bulletPrefab;
    public Transform[] firePoint;
    private string tagString = "Enemy";
    private GameObject[] enemies;
    public float rotateSpeed = 10f;
    
    void Start()
    {
        InvokeRepeating("TargetUpdate",0f,0.5f);
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

        
        if (shootingCountDown <= 0f)
        {
            shoot();
            shootingCountDown = 1f / shootingRate;
        }

        shootingCountDown -= Time.fixedDeltaTime;
    }

    private void shoot()
    {
        
        Debug.Log("SHOOT!"+bulletAttack.ToString());
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
