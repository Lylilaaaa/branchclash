using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeDamage : MonoBehaviour
{
    public float homeMaxHealth;
    public float homeCurHealth;
    
    // Start is called before the first frame update
    void Start()
    {
        ReStart();
    }

    void ReStart()
    {
        homeMaxHealth = GlobalVar._instance.chosenNodeData.fullHealth;
        homeCurHealth = GlobalVar._instance.chosenNodeData.curHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalVar.CurrentGameState == GlobalVar.GameState.GamePlay)
        {
            CurNodeDataSummary._instance.homeMaxHealth = homeMaxHealth;
            CurNodeDataSummary._instance.homeCurHealth = homeCurHealth;
        }
    }
}
