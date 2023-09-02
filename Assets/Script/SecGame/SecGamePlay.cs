using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecGamePlay : MonoBehaviour
{
    [Header("=====AudioSetting=====")] 
    public AudioClip loadingSound;

    [Header("=====ObjectSetting=====")] 
    public Collider[] pushColliderList;
    public Animator[] pushAnimator;
    public Animator[] tubeAnimator;
    
    // Start is called before the first frame update
    void Awake()
    {
        SoundManager._instance.PlayMusicSound(SoundManager._instance.secGamePlayBackSound,true,0.8f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            float maxRaycastDistance = 100f;
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray, maxRaycastDistance);
            foreach (RaycastHit hit in hits)
            {
                Collider collider = hit.collider;
                for(int i = 0; i<3;i++)
                {
                    Collider _collider = pushColliderList[i];
                    if (_collider.gameObject == collider.gameObject)
                    {
                        _pushDebuff(i);
                        break;
                    }
                }
            }
        }
    }

    private void _pushDebuff(int index)
    {
        SoundManager._instance.PlayEffectSound(loadingSound);
        TreeNodeDataInit._instance.AddDownNodeData();
        pushAnimator[index].SetTrigger("isPushed");
        tubeAnimator[index].SetTrigger("push");
    }
}
