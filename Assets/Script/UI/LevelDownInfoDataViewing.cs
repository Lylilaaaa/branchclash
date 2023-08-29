using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDownInfoDataViewing : MonoBehaviour
{
    public DownNodeData thisDownNodeData;
    public GameObject textViewingParent;

    public GameObject curSlidesParent;
    public GameObject curOwnerInfo;
    
    public GameObject mainSlidesParent;
    public GameObject mainOwnerInfo;

    private bool _hasInit;
    // Start is called before the first frame update
    void Awake()
    {
        _hasInit = false;
        thisDownNodeData = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (thisDownNodeData != null && _hasInit == false)
        {
            InitNeverChange();
        }
    }

    void InitNeverChange()
    {
        
    }
}
