using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ExhibUIButton : MonoBehaviour
{
    public TextMeshProUGUI layInfo;
    public TextMeshProUGUI inidexInfo;
    

    private NodeData thisNodeData;
    public GameObject hintPanal;
    public GameObject hintHint;
    // Start is called before the first frame update
    void Awake()
    {
        thisNodeData = CurNodeDataSummary._instance.thisNodeData;
        hintPanal.SetActive(false);
        hintHint.SetActive(true);
    }

    private void Update()
    {
        layInfo.text = thisNodeData.nodeLayer + " Layer";
        //2nd Node
        if (thisNodeData.nodeIndex % 10 == 1)
        {
            inidexInfo.text = thisNodeData.nodeIndex + "st Node";
        }
        else if (thisNodeData.nodeIndex % 10 == 2)
        {
            inidexInfo.text = thisNodeData.nodeIndex + "nd Node";
        }
        else if (thisNodeData.nodeIndex % 10 == 3)
        {
            inidexInfo.text = thisNodeData.nodeIndex + "rd Node";
        }
        else
        {
            inidexInfo.text = thisNodeData.nodeIndex + "th Node";
        }
        
    }

    // Update is called once per frame
    public void ExitThisNode()
    {
        //ContractInteraction._instance.InEdit();
        
        //comment build!!!
        //查找合约响应事件！！！！！！
        GlobalVar._instance.ChangeState("ChooseField");
        SceneManager.LoadScene("GamePlay");
    }

    public void QuitThisNode()
    {
        GlobalVar._instance.ChangeState("MainStart");
        SceneManager.LoadScene("HomePage");
    }
    public void hintHintFlicker()
    {
        hintHint.SetActive(false);
    }
    public void OpenHintPanal()
    {
        hintPanal.SetActive(true);
    }
    public void CloseHintPanal()
    {
        hintPanal.SetActive(false);
    }
}
