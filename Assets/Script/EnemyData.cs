using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyData : MonoBehaviour
{
    public int MonsterHealth=100;
    public Slider healthBar;
    public int totalHealth=100;
    public GameObject BodyHitGameObj;
    private int hitOfBody;
    public Animator thisAnimator;
    public bool isBody;
    public Transform homePos;
    
    
    // Start is called before the first frame update
    void Start()
    {
        //healthBar = transform.GetChild(0).GetChild(0).GetComponent<Slider>();
        //print(transform.GetChild(0).GetChild(0));
        totalHealth = MonsterHealth;
        healthBar.maxValue = totalHealth;
        isBody = false;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = MonsterHealth;
        if (MonsterHealth <= 0 && isBody!=true)
        {
            isBody = true;
            thisAnimator.SetTrigger("isDying");
            hitOfBody = -MonsterHealth;
            gameObject.tag = "Untagged";
            GameObject hitBody = Instantiate(BodyHitGameObj);
            hitBody.transform.parent = transform;
            hitBody.transform.localPosition = Vector3.zero;
            hitBody.transform.name = transform.name + "body";
            healthBar.gameObject.SetActive(false);
            hitBody.GetComponent<EnemyBodyData>().damageBody = hitOfBody;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "home")
        {
            HomeDamage hd = other.gameObject.GetComponent<HomeDamage>();
            hd.homeCurHealth -= MonsterHealth;
        }
    }

    public void desBody()
    {
        Destroy(gameObject);
    }
}
