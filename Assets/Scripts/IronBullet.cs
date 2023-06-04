using UnityEngine;

public class IronBullet : MonoBehaviour
{
    private Transform target;
    private Vector3 origin;

    public float speed = 70f;
    public float damageRange = 30f;

    private float damage;
    private Vector3 dir;

    public void Seek(Transform _target, float _damage, Vector3 _origin)
    {
        target = _target;
        damage = _damage;
        origin = _origin;
        dir = target.position - origin;
    }



    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(origin,transform.position) >= damageRange)
        {
            Destroy(gameObject);
            return;
        }


        float distanceThisFrame = speed * Time.deltaTime;
            //if (dir.magnitude <= distanceThisFrame)
            //{
            //    HitTarget();
            //    return;
            //}

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);

        

    }

    //void HitTarget()
    //{
    //    GameObject enemyGO = target.gameObject;
    //    Enemy enemy = enemyGO.GetComponent<Enemy>();
    //    enemy.GetDamage(damage);
    //    Destroy(gameObject);
    //}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.GetDamage(damage);
        }
    }
}
