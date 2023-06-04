using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectronicWeapon : MonoBehaviour
{
    private List<Transform> targets;

    public float range = 15f;
    public float damagePerSec = 15f;

    public string enemyTag = "Enemy";

    private void Awake()
    {
        targets = new List<Transform>();
    }

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < range)
            {
                targets.Add(enemy.transform);
            }
        }

        for (int i = targets.Count - 1; i >= 0; i--)
        {
            if(targets[i] != null)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, targets[i].transform.position);
                if (distanceToEnemy > range)
                {
                    targets.Remove(targets[i]);
                }
            }
            else
            {
                targets.Remove(targets[i]);
            }
        }

    }

    void Update()
    {
        for(int i=0;i<targets.Count;i++)
        {
            if(targets[i] == null)
            {
                return;
            }
            else
            {
                GameObject enemyGO = targets[i].gameObject;
                Enemy enemy = enemyGO.GetComponent<Enemy>();
                enemy.GetDamage(damagePerSec * Time.deltaTime);
            }
            
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position,range);
    }
}
