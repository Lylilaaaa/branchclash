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
    private bool _messageFinish = false;
    private bool _messageFix = false;
    private bool _treeNumFinish = false;
    
    public Sprite yellowImage;
    public Sprite pinkImage;
    public List<NodeData> yourNode;
    public List<DownNodeData> yourDownNode;
    public TextMeshProUGUI textMeshPro;
    public GameObject yourNodePrefab;
    public GameObject yourDownNodePrefab;
    public Transform yourNodeInitPos;

    public GameObject hintPanal;
    public GameObject hintHint;
    // Start is called before the first frame update
    void Awake()
    {
        _nodeFinish=false;
        _messageFinish = false;
        _messageFix = false;
        _treeNumFinish = false;
        _myNode = transform.GetChild(0).GetChild(1);
        _InformationMessage = transform.GetChild(0).GetChild(2);
        _upTreeNum = transform.GetChild(0).GetChild(3);
        _downTreeNum = transform.GetChild(0).GetChild(4);
        yourNode = new List<NodeData>();
        yourDownNode = new List<DownNodeData>();
        hintPanal.SetActive(false);
    }

    private void Update()
    {
        if (GlobalVar._instance.dataPrepared == true)
        {
            if (GlobalVar._instance.thisUserAddr != null)
            {
                if (GlobalVar._instance.isNew == 0 && hintHint.activeSelf == false)
                {
                    hintHint.SetActive(true);
                }
            }

            if (GlobalVar._instance.nodeDataList.Count != 0 && _nodeFinish == false && GlobalVar._instance.role == 0)
            {
                print("init Nodes");
                _checkYourNode();
                _initYourNodeUI();
                _nodeFinish = true;
            }
            else if (GlobalVar._instance.downNodeDataList.Count != 0 && _nodeFinish == false &&
                     GlobalVar._instance.role == 1)
            {
                print("init downNodes");
                _checkYourDownNode();
                _initYourDownNodeUI();
                _nodeFinish = true;
            }
        
        
            if (GlobalVar._instance.nodeDataList.Count != 0 && _messageFinish == false)
            {
                _InformationMessage.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Day: "+System.DateTime.Now.Day+"  Month: "+System.DateTime.Now.Month+"  Year: "+System.DateTime.Now.Year;
                _InformationMessage.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = messagesShown();
                _showMessageFix();
                _messageFinish = true;
            }
            if (GlobalVar._instance.nodeDataList.Count != 0 && _treeNumFinish == false)
            {
                int upTreeNum = GlobalVar._instance.maxLevelTree;
                int downTreeNum = GlobalVar._instance.maxLevelTreeDown;
                if (upTreeNum > downTreeNum)
                {
                    _upTreeNum.GetChild(1).GetComponent<Slider>().maxValue = upTreeNum+downTreeNum;
                    _upTreeNum.GetChild(1).GetComponent<Slider>().value = upTreeNum;
                    _downTreeNum.GetChild(1).GetComponent<Slider>().maxValue = upTreeNum+downTreeNum;
                    _downTreeNum.GetChild(1).GetComponent<Slider>().value = downTreeNum;
                }
                else
                {
                    _upTreeNum.GetChild(1).GetComponent<Slider>().maxValue = upTreeNum+downTreeNum;
                    _upTreeNum.GetChild(1).GetComponent<Slider>().value = upTreeNum;
                    _downTreeNum.GetChild(1).GetComponent<Slider>().maxValue = downTreeNum+upTreeNum;
                    _downTreeNum.GetChild(1).GetComponent<Slider>().value = downTreeNum;
                }

                _upTreeNum.GetChild(3).GetComponent<TextMeshProUGUI>().text = "LAYER: " + upTreeNum;
                _downTreeNum.GetChild(3).GetComponent<TextMeshProUGUI>().text = "LAYER: " + downTreeNum;
                _treeNumFinish = true;
            }
        }
        
        
    }

    private string messagesShown()
    {
        string finalString = "";
        foreach (NodeData node in GlobalVar._instance.nodeDataList)
        {
            int stringCounted = node.ownerAddr.Length;
            finalString = finalString + node.ownerAddr.Substring(0, 5) + "..." +
                          node.ownerAddr.Substring(stringCounted - 3, 3)+" created the "+node.nodeIndex+stndrdth(node.nodeIndex)+ " node on the "+node.nodeLayer+stndrdth(node.nodeLayer)+" layer of the upright tree.\n";
        }
        //0xb3a...890 created the 4th node on the 4th layer of the upright tree.
        return finalString;
    }

    private void _showMessageFix()
    {
        RectTransform _rectTransform = _InformationMessage.GetChild(2).GetChild(0).GetComponent<RectTransform>();
        int childCount = GlobalVar._instance.nodeDataList.Count;
        if (_messageFix == false && childCount!=0)
        {
            
            Vector2 sizeDelta = _rectTransform.sizeDelta;
    
            float newHeight = 10f*childCount; 
            float newWidth = sizeDelta.x; 
            sizeDelta.y = newHeight;
            sizeDelta.x = newWidth;
            
            _rectTransform.sizeDelta = sizeDelta;
            _messageFix = true;
        }
        
    }


    private string stndrdth(int num)
    {
        if (num%10 == 1)
        {
            return "st";
        }
        else if (num%10 == 2)
        {
            return "nd";
        }
        else if (num%10 == 3)
        {
            return "rd";
        }
        else
        {
            return "th";
        }
    }
    private void _checkYourNode()
    {
        yourNode.Clear();
        foreach (NodeData node in GlobalVar._instance.nodeDataList)
        {
            if (node.ownerAddr == GlobalVar._instance.thisUserAddr)
            {
                yourNode.Add(node);
            }
        }
    }
    private void _checkYourDownNode()
    {
        yourDownNode.Clear();
        foreach (DownNodeData node in GlobalVar._instance.downNodeDataList)
        {
            if (node.ownerAddr == GlobalVar._instance.thisUserAddr)
            {
                yourDownNode.Add(node);
            }
        }
    }

    public void ZoomX_X(string name)
    {
        //print(name);
        //print("button pressed!");
        string[] layerNode = name.Split(',');
        //print(layerNode);
        GlobalVar._instance.zoomingPos = layerNode[0]+"-"+layerNode[1];
    }
    public void ZoomX_X_down(string name)
    {
        //print("button pressed!");
        string[] layerNode = name.Split(',');
        //print(layerNode);
        GlobalVar._instance.zoomingPos = layerNode[0]+"-"+layerNode[1]+"_red";
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
            thisNode.transform.GetChild(0).GetComponent<YourNodeButtonClickHandler>().ReStart();
            string[] layerNode = node.name.Split(',');
            thisNode.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Layer: "+layerNode[0];
            thisNode.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Node: "+layerNode[1];
            yPos -= 20f;
        }
    }
    private void _initYourDownNodeUI()
    {
        float yPos = 0;
        foreach (DownNodeData node in yourDownNode)
        {
            GameObject thisNode = Instantiate(yourDownNodePrefab, yourNodeInitPos);
            if (node.isMajor == true)
            {
                thisNode.transform.GetChild(0).GetComponent<Image>().sprite = pinkImage;
            }
            thisNode.transform.localPosition = new Vector3(0, yPos, 0);
            thisNode.transform.localRotation = Quaternion.identity;
            int nameLength = node.name.Length;
            thisNode.name = node.name.Substring(1,nameLength-2);
            thisNode.transform.GetChild(0).GetComponent<YourNodeButtonClickHandler>().ReStart();
            string[] layerNode = thisNode.name.Split(',');
            thisNode.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Layer: "+layerNode[0];
            thisNode.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Node: "+layerNode[1];
            yPos -= 20f;
        }
    }
    
    public void ChangeName()
    {
        int stringLenth = GlobalVar._instance.thisUserAddr.Length;
        if (stringLenth != 0)
        {
            textMeshPro.text = GlobalVar._instance.thisUserAddr.Substring(0, 5) + "..." +
                               GlobalVar._instance.thisUserAddr.Substring(stringLenth-3);
        }
        else
        {
            Debug.Log("address login failed!!!");
        }
    }

    public void hintHintFlicker()
    {
        hintHint.SetActive(false);
    }
    public void OpenHintPanal()
    {
        hintPanal.SetActive(true);
        hintHint.SetActive(false);
    }
    public void CloseHintPanal()
    {
        hintPanal.SetActive(false);
        print(GlobalVar._instance.thisUserAddr);
        GlobalVar._instance.isNew = 1;
    }
}
