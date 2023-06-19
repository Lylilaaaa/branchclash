using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class ViewingDebuffProtect : MonoBehaviour
{
    public Slider debuffWood;
    public Slider debuffIron;
    public Slider debuffElec;
    public Slider proWood;
    public Slider proIron;
    public Slider proElec;

    private int[] _debufflist;

    private bool _counted = false;
    private string _at, _sp, _ra;
    
    public int woodTotalBlood=0;
    public int ironTotalBlood=0;
    public int elecTotalBlood=0;
    
    public int woodTotalProtect=0;
    public int ironTotalProtect=0;
    public int elecTotalProtect=0;
    
    void Start()
    {
        _debufflist = CurNodeDataSummary._instance.debuffList;
        //_wood[0].text = _debufflist[0]
    }

    // Update is called once per frame
    void Update()
    {
        if (CurNodeDataSummary._instance.dictionaryFinish == true && _counted ==false)
        {
            if (CurNodeDataSummary._instance.woodCount != null)
            {
                foreach (int grade in CurNodeDataSummary._instance.woodCount.Keys)
                {
                    (_at,_sp,_ra) = CurNodeDataSummary._instance.CheckAttackSpeedRange("wood", grade);
                    woodTotalBlood += CurNodeDataSummary._instance.woodCount[grade]*int.Parse(_at);
                }
            }
            if (CurNodeDataSummary._instance.ironCount != null)
            {
                foreach (int grade in CurNodeDataSummary._instance.ironCount.Keys)
                {
                    (_at,_sp,_ra) = CurNodeDataSummary._instance.CheckAttackSpeedRange("iron", CurNodeDataSummary._instance.ironCount[grade]);
                    ironTotalBlood += int.Parse(_at);
                }
            }
            if (CurNodeDataSummary._instance.elecCount != null)
            {
                foreach (int grade in CurNodeDataSummary._instance.elecCount.Keys)
                {
                    (_at,_sp,_ra) = CurNodeDataSummary._instance.CheckAttackSpeedRange("iron", CurNodeDataSummary._instance.elecCount[grade]);
                    elecTotalBlood += int.Parse(_at);
                }
            }
            if (CurNodeDataSummary._instance.wproCount != null)
            {
                foreach (int grade in CurNodeDataSummary._instance.wproCount.Keys)
                {
                    woodTotalProtect += CurNodeDataSummary._instance.wproCount[grade];
                }
            }
            if (CurNodeDataSummary._instance.iproCount != null)
            {
                foreach (int grade in CurNodeDataSummary._instance.iproCount.Keys)
                {
                    ironTotalProtect += CurNodeDataSummary._instance.iproCount[grade];
                }
            }
            if (CurNodeDataSummary._instance.eproCount != null)
            {
                foreach (int grade in CurNodeDataSummary._instance.eproCount.Keys)
                {
                    elecTotalProtect += CurNodeDataSummary._instance.eproCount[grade];
                }
            }
            
            debuffWood.maxValue = woodTotalBlood;
            debuffIron.maxValue = ironTotalBlood;
            debuffElec.maxValue = elecTotalBlood;
            debuffWood.minValue = debuffIron.minValue = debuffElec.minValue = 0;
            proWood.minValue = proIron.minValue = proElec.minValue = 0;
            debuffWood.value = _debufflist[0];
            debuffIron.value = _debufflist[1];
            debuffElec.value = _debufflist[2];
            
            proWood.maxValue = _debufflist[0];
            proIron.maxValue = _debufflist[1];
            proElec.maxValue = _debufflist[2];
            proWood.value = woodTotalProtect;
            proIron.value = ironTotalProtect;
            proElec.value = elecTotalProtect;
            _counted = true;
        }

    }

    public void ExitThisNode()
    {
        ContractInteraction._instance.InEdit();
        GlobalVar._instance.ChangeState("ChooseField");
        SceneManager.LoadScene("GamePlay");
    }
}
