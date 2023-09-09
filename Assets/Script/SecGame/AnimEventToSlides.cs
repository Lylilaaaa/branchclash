using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventToSlides : MonoBehaviour
{
    // Start is called before the first frame update
    private SecGamePlayInfoShowing sceDataControl;
    public int debuffIndex;
    public bool finishSubmitConfirm = false;

    public void TubeFinish()
    {
        Debug.Log("tube Anim finish!");
        StartOpenGamePlay();
    }
    public void StartOpenGamePlay()
    {
        StartCoroutine( secSubmint());
    }
    
    IEnumerator secSubmint()
    {
        ContractInteraction._instance.finishSecSubmit = false;
        ContractInteraction._instance.SecSubmit((debuffIndex+1).ToString());
        while (!ContractInteraction._instance.finishSecSubmit)
        {
            yield return null;
        }
        sceDataControl = transform.parent.parent.GetComponent<SecGamePlayInfoShowing>();
        sceDataControl.AddDebuffAndRefresh(debuffIndex);
        
        StartCoroutine( checkNewNode());
    }

    IEnumerator checkNewNode()
    {
        ContractInteraction._instance.endProcessSec = "";
        ContractInteraction._instance.SecCheckLevelOr();
        while (ContractInteraction._instance.endProcessSec.Length == 0)
        {
            yield return null;
        }
        finishSubmitConfirm = checkEqual();
        Debug.Log("endProcessSec is: "+ ContractInteraction._instance.endProcessSec);
        if (finishSubmitConfirm)
        {
            GlobalVar._instance._loadNextScene("4_End");
        }
        else
        {
            StartCoroutine( checkNewNode());
            Debug.Log("not finish the confirm!");
        }
    }

    private bool checkEqual()
    {
        if (ContractInteraction._instance.endProcessSec == "finish")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    
    
    
    
    // public void LoadEndScene()
    // {
    //     Debug.Log("end scene preparing!");
    //     Invoke("_jumpToEndGame", 3f);
    //     
    // }
    //
    // private void _jumpToEndGame()
    // {
    //     GlobalVar._instance._loadNextScene("4_End");
    // }
}
