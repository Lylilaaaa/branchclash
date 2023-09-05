using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 10f;

    private Transform target;
    private int wavepointIndex = 0;
    public float disPerUnit = 5f;
    

    public float hitPoint = 500f;
    

    public void GetDamage(float damage)
    {
        hitPoint -= damage;
    }

    void Start()
    {
        target = WayPoints.points[0];
        speed = disPerUnit;
    }

    void Update()
    {
        if (hitPoint <= 0)
        {
            Destroy(gameObject);
            return;
        }

        if (transform.GetComponent<EnemyData>().isBody == false)
        {
            Vector3 dir = target.position - transform.position;
            dir.y = 0f;
            transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
            transform.rotation = Quaternion.LookRotation(dir);
            if (Vector3.Distance(new Vector3(transform.position.x,transform.position.y,transform.position.z), new Vector3(target.position.x,transform.position.y,target.position.z)) <= 0.2f)
            {
                GetNextWaypoint();
            }
        }
    }

    void GetNextWaypoint()
    {
        if (wavepointIndex >= WayPoints.points.Length - 1)
        {
            Destroy(gameObject);
            return;
        }
        wavepointIndex++;
        target = WayPoints.points[wavepointIndex];
    }

}
