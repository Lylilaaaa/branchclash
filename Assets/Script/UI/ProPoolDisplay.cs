using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProPoolDisplay : MonoBehaviour
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
            _weapenPoolFinish = true;
            _clearWeaponPool();
            _initWeaponPoolUI();
        }
        if (CurNodeDataSummary._instance.changeData == true)
        {
            CurNodeDataSummary._instance.changeData = false;
            _clearWeaponPool();
            _initWeaponPoolUI();
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
        if (CurNodeDataSummary._instance.wproCount.Count != 0)
        {
            foreach (int grade in CurNodeDataSummary._instance.wproCount.Keys)
            {
                GameObject thisNode = Instantiate(Type_level_num_Prefab, transform);
                thisNode.transform.localPosition = new Vector3(xPos, 0, 0);
                thisNode.transform.localRotation = Quaternion.identity;
                thisNode.name = "wpro-"+grade+"-"+CurNodeDataSummary._instance.wproCount[grade];
                xPos += 60f;
            }
        }

        if (CurNodeDataSummary._instance.iproCount.Count != 0)
        {
            foreach (int grade in CurNodeDataSummary._instance.iproCount.Keys)
            {
                GameObject thisNode = Instantiate(Type_level_num_Prefab, transform);
                thisNode.transform.localPosition = new Vector3(xPos, 0, 0);
                thisNode.transform.localRotation = Quaternion.identity;
                thisNode.name = "ipro-"+grade+"-"+CurNodeDataSummary._instance.iproCount[grade];
                xPos += 60f;
            }
        }

        if (CurNodeDataSummary._instance.eproCount.Count != 0)
        {
            foreach (int grade in CurNodeDataSummary._instance.eproCount.Keys)
            {
                GameObject thisNode = Instantiate(Type_level_num_Prefab, transform);
                thisNode.transform.localPosition = new Vector3(xPos, 0, 0);
                thisNode.transform.localRotation = Quaternion.identity;
                thisNode.name = "epro-"+grade+"-"+CurNodeDataSummary._instance.eproCount[grade];
                xPos += 60f;
            }
        }
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
        }
    }
}
