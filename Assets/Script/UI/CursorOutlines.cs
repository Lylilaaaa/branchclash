using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CursorOutlines : MonoBehaviour
{
    // private GameObject outlineGbj;
    // private Renderer outline_render;
    //
    // public Material outlineMat_0;
    // public Material outlineMat_1;

    public NodeData thisNodeData;
    public DownNodeData correspondMajorDownNodeData;
    public bool mouseEnter;
    public bool _canDisappear = true;
    public bool cursorZoomIn=false;
    public GameObject previewLevelInfoPenal;
    public Transform cameraPos;
    public bool olWithoutMouse = false;
    
    // Start is called before the first frame update
    void Awake()
    {
        thisNodeData = null;
        correspondMajorDownNodeData = null;
        cursorZoomIn=false;
        _canDisappear = true;
        olWithoutMouse = false;
        // outlineGbj = FindChildWithTag(transform, "outline").gameObject;
        // outline_render = outlineGbj.GetComponent<Renderer>();
        // outline_render.material = outlineMat_0;
        previewLevelInfoPenal.transform.GetChild(0).gameObject.SetActive(false);
    }

    private void _setThisData()
    {
        //print("setdata!" + transform.name);
        string[] layerIndex = transform.name.Split('-');
        if (layerIndex.Length == 2)
        {
            int layer =  int.Parse(layerIndex[0]);
            int index = int.Parse(layerIndex[1]);
            if (layer == 0 && index == 0)
            {
                previewLevelInfoPenal.GetComponent<LevelInfoDataViewing>().thisNodeData =
                    GlobalVar._instance.treeData.InitNodeData;
                thisNodeData = GlobalVar._instance.treeData.InitNodeData;
                correspondMajorDownNodeData = _checkUpNodeMain(layer);
            }
            else
            {
                foreach (NodeData nodeDataGlobal in GlobalVar._instance.nodeDataList)
                {
                    if (nodeDataGlobal.nodeLayer == layer && nodeDataGlobal.nodeIndex == index)
                    {
                        previewLevelInfoPenal.GetComponent<LevelInfoDataViewing>().thisNodeData = nodeDataGlobal;
                        thisNodeData = nodeDataGlobal;
                        correspondMajorDownNodeData = _checkUpNodeMain(layer);
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
        if (thisNodeData == null)
        {
            _setThisData();
        }
        
        ZoomCertainNode();
        if (mouseEnter == true)
        {
            if (Input.GetMouseButtonDown(0) && _canDisappear == true)
            {
                _canDisappear = false;
                cursorZoomIn = true;
                if (GlobalVar._instance.isPreViewing == false)
                {
                    previewLevelInfoPenal.transform.GetChild(0).gameObject.SetActive(true);
                    GlobalVar._instance.chosenNodeData = previewLevelInfoPenal.GetComponent<LevelInfoDataViewing>().thisNodeData;
                    GlobalVar._instance.downChosenNodeData = correspondMajorDownNodeData;
                    GlobalVar._instance.ChangeState("Viewing");
                    GlobalVar._instance.isPreViewing = true;
                }
            }
        }
        if (cursorZoomIn == true)
        {
            cursorZoomIn = false;
            CameraController._instance.LookUpNode(transform);
            CameraController._instance.canMove = true;
           // StartCoroutine(ChangeVariableAfterDelay());
            
        }


        if (mouseEnter == false && _canDisappear == true)
        {
            // outline_render.material = outlineMat_0;
            //outlineGbj.SetActive(false);
        }
    }

    public void ZoomCertainNode()
    {
        //print("test zooming!");
        if (transform.name == GlobalVar._instance.zoomingPos)
        {
            _canDisappear = false;
            olWithoutMouse = true;
            // print("zoom certain");
            GlobalVar._instance.zoomingPos = "";
            cursorZoomIn = true;
            if (GlobalVar._instance.isPreViewing == false)
            {
                previewLevelInfoPenal.transform.GetChild(0).gameObject.SetActive(true);
                GlobalVar._instance.chosenNodeData = previewLevelInfoPenal.GetComponent<LevelInfoDataViewing>().thisNodeData;
                GlobalVar._instance.downChosenNodeData = correspondMajorDownNodeData;
                GlobalVar._instance.ChangeState("Viewing");
                GlobalVar._instance.isPreViewing = true;
            }
        }
    }
    private DownNodeData _checkUpNodeMain(int layer)
    {
        if (layer <= GlobalVar._instance.maxLevelTreeDown) //如果这个downNode的layer小于当前的最大正树layer
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
                        return GlobalVar._instance._findNodeDataDown(majorNodeString);
                    }
                }
            }
        }
        return GlobalVar._instance.treeData.InitDownNodeData;
    }
    private IEnumerator ChangeVariableAfterDelay()
    {
        yield return new WaitForSeconds(1f);

        CameraController._instance.canMove = false; 
    }

    // Update is called once per frame
    // private void OnMouseEnter()
    // {
    //     mouseEnter = true;
    //
    //     // outline_render.material = outlineMat_1;
    //
    // }
    // private void OnMouseExit()
    // {
    //     mouseEnter = false;
    // }
}
