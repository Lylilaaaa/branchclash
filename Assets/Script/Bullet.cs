using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;
    public float speed= 100f;
    public GameObject impactEffect;
    public float attackData;

    private bool hasAttack = false;
    public void seek(Transform _target)
    {
        target = _target;
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        dir.y = 0f;
        float deltaMove = Time.deltaTime * speed;

        if (dir.magnitude <= deltaMove && hasAttack == false)
        {
            HitTarget();
            hasAttack = true;
            return;
        }
        
        transform.Translate(dir.normalized*deltaMove,Space.World);
        transform.rotation = Quaternion.LookRotation(dir);
    }

    private void HitTarget()
    {
        if (target == null)
        {
            return;
        }
        GameObject efffectins = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        efffectins.transform.localScale = new Vector3(5, 5, 5);
        Destroy(efffectins,2f);
        Destroy(gameObject);
        target.GetComponent<EnemyData>().MonsterHealth-= attackData;
    }
    
}
