using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelDownInfoDataViewing : MonoBehaviour
{
    [Header("====Showing====")]
    public DownNodeData thisDownNodeData;
    public DownNodeData majorDownNodeData;
    
    [Header("====UI Setting====")]
    public TextMeshProUGUI layerTMP;
    public TextMeshProUGUI indexTMP;
    public TextMeshProUGUI curOwnerInfo;
    public TextMeshProUGUI majorOwnerInfo;
    

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
        _hasInit = true;
        layerTMP.text = "LAYER: "+thisDownNodeData.nodeLayer;
        indexTMP.text = "INDEX: " + thisDownNodeData.nodeIndex;
        int stringCounted = thisDownNodeData.ownerAddr.Length;
        //print("owner string: "+layerTMP.text+indexTMP.text+thisDownNodeData.ownerAddr);
        curOwnerInfo.text = thisDownNodeData.ownerAddr.Substring(0, 5) + "..." + thisDownNodeData.ownerAddr.Substring(stringCounted - 3, 3);
        stringCounted = majorDownNodeData.ownerAddr.Length;
        majorOwnerInfo.text = majorDownNodeData.ownerAddr.Substring(0, 5) + "..." + majorDownNodeData.ownerAddr.Substring(stringCounted - 3, 3);
        //textViewingParent
    }
    public void ClosePreViewingNode()
    {
        if (GlobalVar._instance.isPreViewing == true)
        {
            transform.parent.parent.GetComponent<CursorOutlinesDown>()._canDisappear = true;
            transform.parent.parent.GetComponent<CursorOutlinesDown>().olWithoutMouse = false;

            transform.GetChild(0).gameObject.SetActive(false);
            GlobalVar._instance.isPreViewing = false;
            CameraController._instance.camLock = false;
            GlobalVar._instance.ChangeState("MainStart");
        }
    }
}
