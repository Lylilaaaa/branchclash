using UnityEngine;

public class WoodenBullet : MonoBehaviour
{
    private Transform target;

    public float speed = 70f;

    private float damage;

    public void Seek(Transform _target, float _damage)
    {
        target = _target;
        damage = _damage;
    }



    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;
        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        GameObject enemyGO = target.gameObject;
        Enemy enemy = enemyGO.GetComponent<Enemy>();
        enemy.GetDamage(damage);
        Destroy(gameObject);
    }
}
