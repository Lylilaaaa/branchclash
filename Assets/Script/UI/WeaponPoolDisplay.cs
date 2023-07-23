using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPoolDisplay : MonoBehaviour
{
    private int childCount;
    private RectTransform _rectTransform;
    private bool has;

    public GameObject Type_level_num_Prefab;

    private bool _weapenPoolFinish=false;
    // Start is called before the first frame update
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        has = false;
    }

    private void Update()
    {
        if (CurNodeDataSummary._instance._initData == true && _weapenPoolFinish == false)
        {
            _clearWeaponPool();
            _initWeaponPoolUI();
            _weapenPoolFinish = true;
        }
        if (CurNodeDataSummary._instance.changeData == true)
        {
            _clearWeaponPool();
            _initWeaponPoolUI();
            CurNodeDataSummary._instance.changeData = false;
        }
        //print(childCount);
        childCount = transform.childCount;
        if (has == false && childCount!=0)
        {
            
            Vector2 sizeDelta = _rectTransform.sizeDelta;
    
            float newHeight = sizeDelta.y; 
            float newWidth = 60f*childCount; 
            sizeDelta.y = newHeight;
            sizeDelta.x = newWidth;
            
            _rectTransform.sizeDelta = sizeDelta;
            has = true;
        }

        positionFix();
    }
    

    private void _clearWeaponPool()
    {
        int childCount = transform.childCount;
        if (childCount > 0)
        {
            for (int i = 0; i < childCount; i++)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
        }
    }

    private void _initWeaponPoolUI()
    {
        float xPos = 0;
        if (CurNodeDataSummary._instance.woodCount.Count != 0)
        {
            foreach (int grade in CurNodeDataSummary._instance.woodCount.Keys)
            {
                GameObject thisNode = Instantiate(Type_level_num_Prefab, transform);
                thisNode.transform.localPosition = new Vector3(xPos, 0, 0);
                thisNode.transform.localRotation = Quaternion.identity;
                thisNode.name = "wood-"+grade+"-"+CurNodeDataSummary._instance.woodCount[grade];
                xPos += 60f;
            }
        }

        if (CurNodeDataSummary._instance.ironCount.Count != 0)
        {
            foreach (int grade in CurNodeDataSummary._instance.ironCount.Keys)
            {
                GameObject thisNode = Instantiate(Type_level_num_Prefab, transform);
                thisNode.transform.localPosition = new Vector3(xPos, 0, 0);
                thisNode.transform.localRotation = Quaternion.identity;
                thisNode.name = "iron-"+grade+"-"+CurNodeDataSummary._instance.ironCount[grade];
                xPos += 60f;
            }
        }

        if (CurNodeDataSummary._instance.elecCount.Count != 0)
        {
            foreach (int grade in CurNodeDataSummary._instance.elecCount.Keys)
            {
                GameObject thisNode = Instantiate(Type_level_num_Prefab, transform);
                thisNode.transform.localPosition = new Vector3(xPos, 0, 0);
                thisNode.transform.localRotation = Quaternion.identity;
                thisNode.name = "elec-"+grade+"-"+CurNodeDataSummary._instance.elecCount[grade];
                xPos += 60f;
            }
        }

        //print("inited weapon pool UI!");
    }
    private void positionFix()
    {
        int cur_child = 0;
        Transform cur_childTrans;
        while (cur_child < childCount)
        {
            cur_childTrans = transform.GetChild(cur_child);
            cur_childTrans.transform.localPosition = new Vector3(60f*cur_child, 0, 0);
            cur_child += 1;
            //print("UI local position: "+cur_childTrans.transform.localPosition);
        }
    }
}
