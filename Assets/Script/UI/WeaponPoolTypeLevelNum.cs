using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WeaponPoolTypeLevelNum : MonoBehaviour
{
    private TowerData wData;
    private TowerData iData;
    private TowerData eData;

    private ProtectData wPro;
    private ProtectData iPro;
    private ProtectData ePro;

    private string thisName;
    private string _thisName;
    
    private TextMeshProUGUI level;
    private TextMeshProUGUI num;
    private RawImage rawImageDisplay;
    
    
    public List<Texture> WeaponTexture;
    public List<Texture> ProTexture;

    private int _poolTypeIndex;
    private int _weaponTypeIndex;
    private int _performTypeIndex;

    private bool _finishi=false;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        if (_finishi == false)
        {
            _finishi = true;
            wData = GlobalVar._instance.woodTowerData;
            iData = GlobalVar._instance.ironTowerData;
            eData = GlobalVar._instance.elecTowerData;
            wPro = GlobalVar._instance.ProWood;
            iPro = GlobalVar._instance.ProIron;
            ePro = GlobalVar._instance.ProElec;
            
            _thisName = transform.name; //type-level-num
            string[] parts = _thisName.Split('-');
            thisName = parts[0];
            //print(parts[0]+parts[1]+parts[2]);
            level = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            rawImageDisplay = transform.GetChild(1).GetComponent<RawImage>();
            num = transform.GetChild(2).GetComponent<TextMeshProUGUI>();

            
            num.text = "x "+parts[2];
            if (thisName == "wood")
            {
                level.text = "Wood Level "+parts[1];//Wood Level 2
                _poolTypeIndex = 0;
                _weaponTypeIndex = 0;
                _performTypeIndex=outputPerformIndex(wData, int.Parse(parts[1]));
            }
            else if(thisName == "iron")
            {
                level.text = "Iron Level "+parts[1];//Wood Level 2
                _poolTypeIndex = 0;
                _weaponTypeIndex = 1;
                _performTypeIndex=outputPerformIndex(iData, int.Parse(parts[1]));
            }
            else if(thisName == "elec")
            {
                level.text = "Electric Level "+parts[1];//Wood Level 2
                _poolTypeIndex = 0;
                _weaponTypeIndex = 2;
                _performTypeIndex=outputPerformIndex(eData, int.Parse(parts[1]));
            }
            else if(thisName == "wpro")
            {
                level.text = "Wood Protector \n Level "+parts[1];//Wood Protector Level 1
                _poolTypeIndex = 1;
                _weaponTypeIndex = 0;
                _performTypeIndex=outputPerformIndexPro(wPro, int.Parse(parts[1]));
            }
            else if(thisName == "epro")
            {
                level.text = "Electric Protector \n Level "+parts[1];
                _poolTypeIndex = 1;
                _weaponTypeIndex = 1;
                _performTypeIndex=outputPerformIndexPro(iPro, int.Parse(parts[1]));
            }
            else if(thisName == "ipro")
            {
                level.text = "Iron Protector \n Level "+parts[1];
                _poolTypeIndex = 1;
                _weaponTypeIndex = 2;
                _performTypeIndex=outputPerformIndexPro(ePro, int.Parse(parts[1]));
            }
            rawImageDisplay.texture = changeTexture(_poolTypeIndex,_weaponTypeIndex,_performTypeIndex) ;
        }
    }

    private int outputPerformIndex(TowerData thisWeaponData,int curLevel)
    {
        if (0 < curLevel && curLevel< thisWeaponData.performGrade2)
        {
            return 0;
        }
        else if (thisWeaponData.performGrade2 <= curLevel && curLevel < thisWeaponData.performGrade3)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }
    private int outputPerformIndexPro(ProtectData thisWeaponData,int curLevel)
    {
        if (0 < curLevel && curLevel< thisWeaponData.performGrade2)
        {
            return 0;
        }
        else if (thisWeaponData.performGrade2 <= curLevel && curLevel < thisWeaponData.performGrade3)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }

    private Texture changeTexture(int pool,int type,int perform) //0,1,2
    {
        if (pool == 0)
        {
            return (WeaponTexture[perform+3*type]);
        }
        else
        {
            return (ProTexture[perform+3*type]);
        }
    }
    

}
