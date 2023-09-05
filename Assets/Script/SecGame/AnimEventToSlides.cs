using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventToSlides : MonoBehaviour
{
    // Start is called before the first frame update
    private SecGamePlayInfoShowing sceDataControl;
    public int debuffIndex;
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
        ContractInteraction._instance.SecSubmit();
        while (!ContractInteraction._instance.finishSecSubmit)
        {
            yield return null;
        }
        sceDataControl = transform.parent.parent.GetComponent<SecGamePlayInfoShowing>();
        sceDataControl.AddDebuffAndRefresh(debuffIndex);
    }

    public void LoadEndScene()
    {
        Debug.Log("end scene preparing!");
        Invoke("_jumpToEndGame", 3f);
        
    }

    private void _jumpToEndGame()
    {
        GlobalVar._instance._loadNextScene("4_End");
    }
}
