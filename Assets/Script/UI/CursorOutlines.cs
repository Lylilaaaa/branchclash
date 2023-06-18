using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CursorOutlines : MonoBehaviour
{
    private GameObject outlineGbj;

    public bool mouseEnter;
    public bool cursorZoomIn=false;
    public GameObject previewLevelInfoPenal;
    
    // Start is called before the first frame update
    void Start()
    {
        mouseEnter = false;
        outlineGbj = FindChildWithTag(transform, "outline").gameObject;
        previewLevelInfoPenal.transform.GetChild(0).gameObject.SetActive(false);
    }
    private Transform FindChildWithTag(Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
            {
                return child;
            }

            Transform result = FindChildWithTag(child, tag);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }

    private void Update()
    {
        if (mouseEnter == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                cursorZoomIn = true;
                if (GlobalVar._instance.isPreViewing == false) //不能同时打开两个viewing //load viewing Scene传入数据 //改变Global node
                {
                    previewLevelInfoPenal.transform.GetChild(0).gameObject.SetActive(true);
                    GlobalVar._instance.chosenNodeData =
                        previewLevelInfoPenal.GetComponent<LevelInfoDataViewing>().thisNodeData;
                    //CameraController._instance.camLock = true;
                    //SceneManager.LoadScene("ExhibExample", LoadSceneMode.Additive);
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

        string[] layerIndex = transform.name.Split('-');
        if (layerIndex.Length == 2)
        {
            int layer =  int.Parse(layerIndex[0]);
            int index = int.Parse(layerIndex[1]);
            if (layer == 0 && index == 0)
            {
                previewLevelInfoPenal.GetComponent<LevelInfoDataViewing>().thisNodeData =
                    GlobalVar._instance.treeData.InitNodeData;
            }
            else
            {
                foreach (NodeData nodeDataGlobal in GlobalVar._instance.nodeDataList)
                {
                    if (nodeDataGlobal.nodeLayer == layer && nodeDataGlobal.nodeIndex == index)
                    {
                        previewLevelInfoPenal.GetComponent<LevelInfoDataViewing>().thisNodeData = nodeDataGlobal;
                    }
                }
            }
        }
    }
    private IEnumerator ChangeVariableAfterDelay()
    {
        yield return new WaitForSeconds(1f);

        CameraController._instance.canMove = false; 
    }

    // Update is called once per frame
    private void OnMouseEnter()
    {
        mouseEnter = true;
        if (GlobalVar._instance.GetState() == GlobalVar.GameState.MainStart)
        {
            outlineGbj.SetActive(true);
        }
    }
    private void OnMouseExit()
    {
        mouseEnter = false;
        if (GlobalVar._instance.GetState() == GlobalVar.GameState.MainStart)
        {
            outlineGbj.SetActive(false);
        }
    }
}
