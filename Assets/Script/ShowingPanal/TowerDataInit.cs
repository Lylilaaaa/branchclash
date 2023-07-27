using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDataInit : MonoBehaviour
{
    public TowerData towerTypeData;

    private FieldInit parentData;
    public int towerGrade=1;
    private int _previousGrade;
    
    public GameObject[] performList;

    private void Start()
    {
        towerGrade = 1;
        _previousGrade = towerGrade;
        _initPerform(0);
        parentData = transform.parent.GetComponent<FieldInit>();
    }

    // Update is called once per frame
    void Update()
    {
        
        string stringWeapon = parentData.mapStructure;
        towerGrade = int.Parse(stringWeapon.Substring(4));
        if (_previousGrade != towerGrade)
        {
            if (0 < towerGrade && towerGrade < towerTypeData.performGrade2)
            {
                Destroy(transform.GetChild(0).gameObject);
                _initPerform(0);
            }
            else if(towerGrade >= towerTypeData.performGrade2 && towerGrade < towerTypeData.performGrade3)
            {
                Destroy(transform.GetChild(0).gameObject);
                _initPerform(1);
            }
            else
            {
                Destroy(transform.GetChild(0).gameObject);
                _initPerform(2);
            }
        }
        _previousGrade = towerGrade;
    }

    private void _initPerform(int index)
    {
        GameObject performGObj = Instantiate(performList[index]);
        performGObj.transform.SetParent(transform);
        performGObj.transform.localPosition = Vector3.zero;
        performGObj.transform.localScale = new Vector3(1,1,1);
    }
}
