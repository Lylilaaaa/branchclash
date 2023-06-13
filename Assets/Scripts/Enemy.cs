using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 10f;

    private Transform target;
    private int wavepointIndex = 0;

    public float hitPoint = 500f;

    public void GetDamage(float damage)
    {
        hitPoint -= damage;
    }

    void Start()
    {
        //target = WayPoints.points[0];
    }

    void Update()
    {
        if (hitPoint <= 0)
        {
            Destroy(gameObject);
            return;
        }

        //Vector3 dir = target.position - transform.position;
        //transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        //if (Vector3.Distance(transform.position, target.position) <= 0.2f)
        //{
        //    GetNextWaypoint();
        //}
    }

    //void GetNextWaypoint()
    //{
    //    if (wavepointIndex >= WayPoints.points.Length - 1)
    //    {
    //        Destroy(gameObject);
    //        return;
    //    }
    //    wavepointIndex++;
    //    target = WayPoints.points[wavepointIndex];
    //}

}
