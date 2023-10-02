using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    public Button enterNextSceneButton;
    public TextMeshProUGUI major_layerTMP;
    public TextMeshProUGUI major_indexTMP;

    

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
        if (GlobalVar._instance.role == 0)
        {
            enterNextSceneButton.interactable = false;
        }
        else
        {
            enterNextSceneButton.interactable = true;
        }
        _hasInit = true;
        layerTMP.text = "LAYER: "+thisDownNodeData.nodeLayer;
        indexTMP.text = "INDEX: " + thisDownNodeData.nodeIndex;
        int stringCounted = thisDownNodeData.ownerAddr.Length;
        //print("owner string: "+layerTMP.text+indexTMP.text+thisDownNodeData.ownerAddr);
        curOwnerInfo.text = thisDownNodeData.ownerAddr.Substring(0, 5) + "..." + thisDownNodeData.ownerAddr.Substring(stringCounted - 3, 3);
        stringCounted = majorDownNodeData.ownerAddr.Length;
        majorOwnerInfo.text = majorDownNodeData.ownerAddr.Substring(0, 5) + "..." + majorDownNodeData.ownerAddr.Substring(stringCounted - 3, 3);
        //textViewingParent
        major_layerTMP.text = "Layer: " + majorDownNodeData.nodeLayer;
        major_indexTMP.text = "Index: " + majorDownNodeData.nodeIndex;
    }
    public void ClosePreViewingNode()
    {
        if (GlobalVar._instance.isPreViewing == true)
        {
            transform.parent.parent.GetComponent<CursorOutlinesDown>()._canDisappear = true;
            transform.parent.parent.GetComponent<CursorOutlinesDown>().olWithoutMouse = false;

            transform.GetChild(0).gameObject.SetActive(false);
            GlobalVar._instance.isPreViewing = false;
            Camera.main.gameObject.GetComponent<CameraController>().camLock = false;
            GlobalVar._instance.ChangeState("MainStart");
        }
    }

    public void OpenGamePlayScene()
    {
        StartOpenGamePlay();
    }

    public void StartOpenGamePlay()
    {
        StartCoroutine(openGamePlay());
    }

    IEnumerator openGamePlay()
    {
        ContractInteraction._instance.SecInEdit(GlobalVar._instance.chosenDownNodeData.nodeLayer.ToString(),GlobalVar._instance.chosenDownNodeData.nodeIndex.ToString());
        while (!ContractInteraction._instance.finishSecInEdit)
        {
            yield return null;
        }
        GlobalVar._instance.ChangeState("GamePlay");
        GlobalVar._instance._loadNextScene("3_1_SecGamePlay");
    }
}
