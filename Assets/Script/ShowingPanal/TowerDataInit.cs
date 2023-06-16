using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDataInit : MonoBehaviour
{
    public TowerData towerTypeData;

    public int towerGrade;
    
    public GameObject[] performList;

    // Update is called once per frame
    void Update()
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

    private void _initPerform(int index)
    {
        GameObject performGObj = Instantiate(performList[index]);
        performGObj.transform.SetParent(transform);
        performGObj.transform.localPosition = Vector3.zero;
        performGObj.transform.localScale = new Vector3(50,50,50);
    }
}
