using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyData : MonoBehaviour
{
    public int damageBody;

    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log("hit!");
        //Debug.Log(other.gameObject);
        if (other.gameObject.tag == "Enemy")
        {
            //Debug.Log("body hit enemy!");
            other.gameObject.GetComponent<EnemyData>().MonsterHealth-=damageBody;
            transform.parent.GetComponent<EnemyData>().desBody();
            Destroy(gameObject);
            
        }
    }
}
