using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSetActive : MonoBehaviour
{
    private Button _setWeapon;
    public int initMoney;
    
    // Start is called before the first frame update
    void Start()
    {
        _setWeapon = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CurNodeDataSummary._instance.moneyLeft < initMoney)
        {
            _setWeapon.interactable = false;
        }
        else
        {
            _setWeapon.interactable = true;
        }

    }
}
