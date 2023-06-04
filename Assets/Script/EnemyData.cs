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
    
    // Start is called before the first frame update
    void Start()
    {
        //healthBar = transform.GetChild(0).GetChild(0).GetComponent<Slider>();
        //print(transform.GetChild(0).GetChild(0));
        totalHealth = MonsterHealth;
        healthBar.maxValue = totalHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = MonsterHealth;
        if (MonsterHealth <= 0)
        {
            hitOfBody = -MonsterHealth;
            GameObject hitBody = Instantiate(BodyHitGameObj);
            hitBody.transform.parent = transform.parent;
            hitBody.transform.localPosition = transform.localPosition;
            hitBody.transform.name = transform.name + "body";
            hitBody.GetComponent<EnemyBodyData>().damageBody = hitOfBody;
            Destroy(gameObject);
        }
    }
}
