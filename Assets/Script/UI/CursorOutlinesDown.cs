using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CursorOutlinesDown : MonoBehaviour
{
    public DownNodeData thisDownNodeData;
    public DownNodeData majorDownNodeData;
    public NodeData correspondMajorNodeData;
    public bool mouseEnter;
    public bool _canDisappear = true;
    public bool cursorZoomIn=false;
    public GameObject previewLevelInfoPenal;
    public Transform cameraPos;
    public bool olWithoutMouse = false;
    
    // Start is called before the first frame update
    void Awake()
    {
        mouseEnter = false;
        _canDisappear = true;
        previewLevelInfoPenal.transform.GetChild(0).gameObject.SetActive(false);
        
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
                correspondMajorNodeData = _checkUpNodeMain(layer);
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
                        correspondMajorNodeData = _checkUpNodeMain(layer);
                        majorDownNodeData = _checkDownMajorNodeData(layer);
                        previewLevelInfoPenal.GetComponent<LevelDownInfoDataViewing>().majorDownNodeData = majorDownNodeData;
                    }
                }
            }
        }
        else
        {
            print("wrong name!!!");
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
                _canDisappear = false;
                cursorZoomIn = true;
                if (GlobalVar._instance.isPreViewing == false) //????????????viewing //load viewing Scene???????? //???Global node
                {
                    previewLevelInfoPenal.transform.GetChild(0).gameObject.SetActive(true);
                    //CameraController._instance.camLock = true;
                    //SceneManager.LoadScene("ExhibExample", LoadSceneMode.Additive);
                    GlobalVar._instance.downChosenNodeData = previewLevelInfoPenal.GetComponent<LevelDownInfoDataViewing>().thisDownNodeData;
                    GlobalVar._instance.chosenNodeData = _checkUpNodeMain(thisDownNodeData.nodeLayer);
                    
                    GlobalVar._instance.ChangeState("Viewing");
                    GlobalVar._instance.isPreViewing = true;
                }
            }
        }
        if (cursorZoomIn == true)
        {
            CameraController._instance.LookUpNode(transform);
            CameraController._instance.canMove = true;
           // StartCoroutine(ChangeVariableAfterDelay());
            cursorZoomIn = false;
        }
        ZoomCertainNode();



    }

    private NodeData _checkUpNodeMain(int layer)
    {
        if (layer <= GlobalVar._instance.maxLevelTree) //如果这个downNode的layer小于当前的最大正树layer
        {
            foreach (string majorNodeString in GlobalVar._instance.MajorNodeList)
            {
                string[] layerIndex = majorNodeString.Split('-');
                if (layerIndex.Length == 2)
                {
                    if (int.Parse(layerIndex[0]) == layer)
                    {
                        return GlobalVar._instance._findNodeData(majorNodeString);
                    }
                }
            }
        }
        else //如果这个downNode的layer大于已有的正树layer
        {
            foreach (string majorNodeString in GlobalVar._instance.MajorNodeList)
            {
                string[] layerIndex = majorNodeString.Split('-');
                if (layerIndex.Length == 2)
                {
                    if (int.Parse(layerIndex[0]) == GlobalVar._instance.maxLevelTree)
                    {
                        return GlobalVar._instance._findNodeData(majorNodeString);
                    }
                }
            }
        }
        return GlobalVar._instance.treeData.InitNodeData;
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
            _canDisappear = false;
            olWithoutMouse = true;
            //print("zoom certain");
            GlobalVar._instance.zoomingPos = "";
            cursorZoomIn = true;
            if (GlobalVar._instance.isPreViewing == false)
            {
                previewLevelInfoPenal.transform.GetChild(0).gameObject.SetActive(true);
                GlobalVar._instance.downChosenNodeData = previewLevelInfoPenal.GetComponent<LevelDownInfoDataViewing>().thisDownNodeData;
                GlobalVar._instance.chosenNodeData = _checkUpNodeMain(thisDownNodeData.nodeLayer);
                GlobalVar._instance.ChangeState("Viewing");
                GlobalVar._instance.isPreViewing = true;
            }
            
        }
    }
    
}
