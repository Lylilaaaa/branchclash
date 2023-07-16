using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HomePageSelectUI : MonoBehaviour
{
    private Transform _myNode;
    private Transform _InformationMessage;
    private Transform _upTreeNum;
    private Transform _downTreeNum;
    private bool _nodeFinish=false;

    public Sprite  yellowImage;
    public List<NodeData> yourNode;
    public TextMeshProUGUI textMeshPro;
    public GameObject yourNodePrefab;
    public Transform yourNodeInitPos;
    // Start is called before the first frame update
    void Start()
    {
        _myNode = transform.GetChild(0).GetChild(1);
        _InformationMessage = transform.GetChild(0).GetChild(2);
        _upTreeNum = transform.GetChild(0).GetChild(3);
        _downTreeNum = transform.GetChild(0).GetChild(4);
        yourNode = new List<NodeData>();
    }

    private void Update()
    {
        if (GlobalVar._instance.nodeDataList.Count != 0 && _nodeFinish == false)
        {
            _checkYourNode();
            _initYourNodeUI();
            _nodeFinish = true;
        }
    }

    private void _checkYourNode()
    {
        foreach (NodeData node in GlobalVar._instance.nodeDataList)
        {
            if (node.ownerAddr == GlobalVar._instance.userAddr)
            {
                yourNode.Add(node);
            }
        }
    }

    public void ZoomX_X(string name)
    {
        //print("button pressed!");
        string[] layerNode = name.Split(',');
        //print(layerNode);
        GlobalVar._instance.zoomingPos = layerNode[0]+"-"+layerNode[1];
    }

    private void _initYourNodeUI()
    {
        float yPos = 0;
        foreach (NodeData node in yourNode)
        {
            GameObject thisNode = Instantiate(yourNodePrefab, yourNodeInitPos);
            if (node.isMajor == true)
            {
                thisNode.transform.GetChild(0).GetComponent<Image>().sprite = yellowImage;
            }
            thisNode.transform.localPosition = new Vector3(0, yPos, 0);
            thisNode.transform.localRotation = Quaternion.identity;
            thisNode.name = node.name;
            string[] layerNode = node.name.Split(',');
            thisNode.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Layer: "+layerNode[0];
            thisNode.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Node: "+layerNode[1];
            yPos -= 20f;
        }
    }
    
    public void ChangeName()
    {
        int stringLenth = GlobalVar._instance.userAddr.Length;
        if (stringLenth != 0)
        {
            textMeshPro.text = GlobalVar._instance.userAddr.Substring(0, 5) + "..." +
                               GlobalVar._instance.userAddr.Substring(stringLenth-3);
        }
        else
        {
            Debug.Log("address login failed!!!");
        }
    }
}
