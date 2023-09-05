using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MergeButtonInter : MonoBehaviour
{
    public TowerBuilder tb;
    public int price;
    private Button bt;
    // Start is called before the first frame update
    void Start()
    {
        bt = transform.GetComponent<Button>();
        price = 0;
    }

    // Update is called once per frame
    void Update()
    {
                
        if (tb.targetWeaponType == "wood")
        {
            price = GlobalVar._instance.woodTowerData.merge_price;
        }
        else if (tb.targetWeaponType == "iron")
        {
            price = GlobalVar._instance.ironTowerData.merge_price;
        }
        else if (tb.targetWeaponType == "elec")
        {
            price = GlobalVar._instance.elecTowerData.merge_price;
        }
        else
        {
            price = 0;
        }
        
        if (tb.targetWeaponPos.Count == 0 || CurNodeDataSummary._instance.moneyLeft < price)
        {
            bt.interactable = false;
        }
        else
        {
            bt.interactable = true;
        }

    }
}
