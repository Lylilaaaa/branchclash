using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CursorOutlinesDown : MonoBehaviour
{
    [Header("=====Set by runtime=====")]
    public DownNodeData thisDownNodeData;
    public DownNodeData majorDownNodeData;
    public NodeData correspondMajorNodeData;
    public bool mouseEnter;
    public bool _canDisappear = true;
    public bool cursorZoomIn=false;
    public GameObject previewLevelInfoPenal;
    public bool olWithoutMouse = false;
    
    [Header("=====Set by before=====")]
    public Transform cameraPos;
    
    // Start is called before the first frame update
    void Start()
    {
        //print(transform.name + "awake!");
        thisDownNodeData = null;
        majorDownNodeData = null;
        correspondMajorNodeData = null;
        mouseEnter = false;
        _canDisappear = true;
        previewLevelInfoPenal.transform.GetChild(0).gameObject.SetActive(false);
        olWithoutMouse = false;
        cursorZoomIn = false;
    }

    private void _setThisData()
    {
        int length = transform.name.Length;
        string realName = transform.name.Substring(0, length - 4); //去掉“_red”
        string[] layerIndex = realName.Split('-');
        if (layerIndex.Length == 2)
        {
            int layer =  int.Parse(layerIndex[0]);
            int index = int.Parse(layerIndex[1]);
            if (layer == 0 && index == 0)
            {
                thisDownNodeData = GlobalVar._instance.treeData.InitDownNodeData;
                previewLevelInfoPenal.GetComponent<LevelDownInfoDataViewing>().thisDownNodeData = GlobalVar._instance.treeData.InitDownNodeData;
                correspondMajorNodeData = GlobalVar._instance._checkUpNodeMain(layer);
                GlobalVar._instance.chosenNodeData = correspondMajorNodeData;
                majorDownNodeData = _checkDownMajorNodeData(layer);
                previewLevelInfoPenal.GetComponent<LevelDownInfoDataViewing>().majorDownNodeData = majorDownNodeData;
            }
            else
            {
                foreach (DownNodeData downNodeData in GlobalVar._instance.downNodeDataList)
                {
                    if (downNodeData.nodeLayer == layer && downNodeData.nodeIndex == index)
                    {
                        thisDownNodeData = downNodeData;
                        previewLevelInfoPenal.GetComponent<LevelDownInfoDataViewing>().thisDownNodeData = downNodeData;
                        correspondMajorNodeData = GlobalVar._instance._checkUpNodeMain(layer);
                        GlobalVar._instance.chosenNodeData = correspondMajorNodeData;
                        majorDownNodeData = _checkDownMajorNodeData(layer);
                        previewLevelInfoPenal.GetComponent<LevelDownInfoDataViewing>().majorDownNodeData = majorDownNodeData;
                    }
                }
            }
        }
        else
        {
            print("wrong name!!!" + transform.name);
        }
    }

    private void Update()
    {
        if (thisDownNodeData == null)
        {
            _setThisData();
        }
        
        if (mouseEnter == true)
        {
            if (Input.GetMouseButtonDown(0) && _canDisappear == true)
            {
                //print("should be open panel");
                _canDisappear = false;
                cursorZoomIn = true;
                if (GlobalVar._instance.isPreViewing == false) //????????????viewing //load viewing Scene???????? //???Global node
                {
                    CurNodeDataSummary._instance.choseNodeType = 1;
                    previewLevelInfoPenal.transform.GetChild(0).gameObject.SetActive(true);
                    //CameraController._instance.camLock = true;
                    //SceneManager.LoadScene("ExhibExample", LoadSceneMode.Additive);
                    GlobalVar._instance.chosenNodeData = GlobalVar._instance._checkUpNodeMain(thisDownNodeData.nodeLayer);
                    GlobalVar._instance.chosenDownNodeData = previewLevelInfoPenal.GetComponent<LevelDownInfoDataViewing>().thisDownNodeData;
                    
                    GlobalVar._instance.ChangeState("Viewing");
                    GlobalVar._instance.isPreViewing = true;
                }
            }
        }
        if (cursorZoomIn == true)
        {
            Camera.main.gameObject.GetComponent<CameraController>().LookUpNode(transform);
            Camera.main.gameObject.GetComponent<CameraController>().canMove = true;
           // StartCoroutine(ChangeVariableAfterDelay());
            cursorZoomIn = false;
        }
        ZoomCertainNode();
        


    }



    private DownNodeData _checkDownMajorNodeData(int layer)
    {
        foreach (string majorNodeString in GlobalVar._instance.MajorNodeListDown)
        {
            string[] layerIndex = majorNodeString.Split('-');
            if (layerIndex.Length == 2)
            {
                if (int.Parse(layerIndex[0]) == layer)
                {
                    return GlobalVar._instance._findNodeDataDown(majorNodeString);
                }
            }
        }
        return GlobalVar._instance.treeData.InitDownNodeData;
    }
    public void ZoomCertainNode()
    {
        if (transform.name == GlobalVar._instance.zoomingPos)
        {
            CurNodeDataSummary._instance.choseNodeType = 1;
            _canDisappear = false;
            olWithoutMouse = true;
            //print("zoom certain");
            GlobalVar._instance.zoomingPos = "";
            cursorZoomIn = true;
            if (GlobalVar._instance.isPreViewing == false)
            {
                previewLevelInfoPenal.transform.GetChild(0).gameObject.SetActive(true);
                GlobalVar._instance.chosenDownNodeData = previewLevelInfoPenal.GetComponent<LevelDownInfoDataViewing>().thisDownNodeData;
                GlobalVar._instance.chosenNodeData = GlobalVar._instance._checkUpNodeMain(thisDownNodeData.nodeLayer);
                GlobalVar._instance.ChangeState("Viewing");
                GlobalVar._instance.isPreViewing = true;
            }
        }
    }
    
}
