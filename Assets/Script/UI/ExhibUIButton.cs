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
    // Start is called before the first frame update
    void Start()
    {
        thisNodeData = CurNodeDataSummary._instance.thisNodeData;
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
        //���Һ�Լ��Ӧ�¼�������������
        GlobalVar._instance.ChangeState("ChooseField");
        SceneManager.LoadScene("GamePlay");
    }

    public void QuitThisNode()
    {
        GlobalVar._instance.ChangeState("MainStart");
        SceneManager.LoadScene("HomePage");
    }
}