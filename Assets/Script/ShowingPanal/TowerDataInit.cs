using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDataInit : MonoBehaviour
{
    public TowerData towerTypeData;

    public int towerGrade=1;
    private int _previousGrade;
    
    public GameObject[] performList;

    private void Start()
    {
        towerGrade = 1;
        _previousGrade = towerGrade;
        _initPerform(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (_previousGrade != towerGrade)
        {
            if (0 < towerGrade && towerGrade < towerTypeData.performGrade2)
            {
                _initPerform(0);
            }
            else if(towerGrade >= towerTypeData.performGrade2 && towerGrade < towerTypeData.performGrade3)
            {
                _initPerform(1);
            }
            else
            {
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
