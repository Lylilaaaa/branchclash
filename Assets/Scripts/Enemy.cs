using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 10f;

    private Transform target;
    private int wavepointIndex = 0;

    public float hitPoint = 500f;
    public bool isRotate;
    private float angleSpeed = 0.1f;

    public void GetDamage(float damage)
    {
        hitPoint -= damage;
    }

    void Start()
    {
        target = WayPoints.points[0];
    }

    void Update()
    {
        if (hitPoint <= 0)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 target_plane = new Vector3(target.position.x, transform.position.y, target.position.z);
        Vector3 dir = target_plane - transform.position;
        
        Quaternion rotate = Quaternion.LookRotation(dir);
        if (Vector3.Angle(dir, transform.forward) < 0.1f)
        {
            isRotate = false;
        }
        else
        {
            isRotate = true;
        }
        if (isRotate)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, rotate, angleSpeed);
        }
        
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target_plane) <= 1f)
        {
            GetNextWaypoint();
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
