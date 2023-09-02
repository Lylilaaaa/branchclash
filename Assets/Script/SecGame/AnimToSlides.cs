using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimToSlides : StateMachineBehaviour
{
    private SecGamePlayInfoShowing sceDataControl;
    public int debuffIndex;
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("tube Anim finish!");
        sceDataControl = animator.transform.parent.parent.GetComponent<SecGamePlayInfoShowing>();
        sceDataControl.AddDebuffAndRefresh(debuffIndex);
    }
}
