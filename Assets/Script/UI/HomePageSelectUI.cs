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
    public bool _nodeFinish=false;
    private bool hasCheckYourNode = false;
    private bool _messageFinish = false;
    private bool _messageFix = false;
    private bool _treeNumFinish = false;
    
    public Sprite yellowImage;
    public Sprite blueImage;
    public Sprite pinkImage;
    public Sprite redImage;
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
        Debug.Log("=============RESET HOMEPAGESELCTUI================");
        _nodeFinish=false;
        _messageFinish = false;
        _messageFix = false;
        _treeNumFinish = false;
        hasCheckYourNode = false;
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

            if (GlobalVar._instance.nodeDataList.Count != 0 && _nodeFinish == false && GlobalVar._instance.role == 0 && GlobalVar._instance.finalNodePrepared &&!hasCheckYourNode )
            {
                GlobalVar._instance.t.text+= "\n init Nodes";
                Debug.Log("=====================Re Init Your Nodes=======================");
                yourNode.Clear();
                _checkYourNode();
                hasCheckYourNode = true;
            }
            else if (GlobalVar._instance.downNodeDataList.Count != 0 && _nodeFinish == false &&
                     GlobalVar._instance.role == 1 && GlobalVar._instance.finalNodePrepared && !hasCheckYourNode)
            {
                GlobalVar._instance.t.text+= "\n init down Nodes";
                Debug.Log("=====================Re Init Your DOWN Nodes=======================");
                yourDownNode.Clear();
                _checkYourDownNode();
                hasCheckYourNode = true;
            }
        
        
            if (GlobalVar._instance.nodeDataList.Count != 0 && _messageFinish == false && GlobalVar._instance.finalNodePrepared)
            {
                GlobalVar._instance.t.text+= "\n init message";
                _InformationMessage.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Day: "+System.DateTime.Now.Day+"  Month: "+System.DateTime.Now.Month+"  Year: "+System.DateTime.Now.Year;
                _InformationMessage.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = messagesShown();
                _showMessageFix();
                _messageFinish = true;
            }
            if (GlobalVar._instance.nodeDataList.Count != 0 && _treeNumFinish == false && GlobalVar._instance.finalNodePrepared)
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
        if (GlobalVar._instance.role == 0)
        {
            int endIndex = Math.Max(GlobalVar._instance.nodeDataList.Count - 5, 0);
            for (int i = GlobalVar._instance.nodeDataList.Count - 1; i >= endIndex; i--)
            {
                NodeData node = GlobalVar._instance.nodeDataList[i];
                int stringCounted = node.ownerAddr.Length;
                finalString = finalString + node.ownerAddr.Substring(0, 5) + "..." +
                              node.ownerAddr.Substring(stringCounted - 3, 3)+" created the "+node.nodeIndex+stndrdth(node.nodeIndex)+ " node on the "+node.nodeLayer+stndrdth(node.nodeLayer)+" layer of tree trunk.\n";
            }
            endIndex = Math.Max( GlobalVar._instance.downNodeDataList.Count - 5, 0);
            for (int i = GlobalVar._instance.downNodeDataList.Count - 1; i >= endIndex; i--)
            {
                DownNodeData node = GlobalVar._instance.downNodeDataList[i];
                int stringCounted = node.ownerAddr.Length;
                finalString = finalString + node.ownerAddr.Substring(0, 5) + "..." +
                              node.ownerAddr.Substring(stringCounted - 3, 3)+" created the "+node.nodeIndex+stndrdth(node.nodeIndex)+ " node on the "+node.nodeLayer+stndrdth(node.nodeLayer)+" layer of tree root.\n";
            }
        }
        else
        {
            int endIndex = Math.Max(GlobalVar._instance.nodeDataList.Count - 5, 0);
            for (int i = GlobalVar._instance.nodeDataList.Count - 1; i >= endIndex; i--)
            {
                NodeData node = GlobalVar._instance.nodeDataList[i];
                int stringCounted = node.ownerAddr.Length;
                finalString = finalString + "Worm "+ node.ownerAddr.Substring(0, 5) + "..." +
                              node.ownerAddr.Substring(stringCounted - 3, 3)+" created the "+node.nodeIndex+stndrdth(node.nodeIndex)+ " node on the "+node.nodeLayer+stndrdth(node.nodeLayer)+" layer of tree trunk.\n";
            }
            endIndex = Math.Max( GlobalVar._instance.downNodeDataList.Count - 5, 0);
            for (int i = GlobalVar._instance.downNodeDataList.Count - 1; i >= endIndex; i--)
            {
                DownNodeData node = GlobalVar._instance.downNodeDataList[i];
                int stringCounted = node.ownerAddr.Length;
                finalString = finalString + node.ownerAddr.Substring(0, 5) + "..." +
                              node.ownerAddr.Substring(stringCounted - 3, 3)+" created the "+node.nodeIndex+stndrdth(node.nodeIndex)+ " node on the "+node.nodeLayer+stndrdth(node.nodeLayer)+" layer of tree root.\n";
            }
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
        foreach (NodeData node in GlobalVar._instance.nodeDataList)
        {
            GlobalVar._instance.t.text += "(" + node.nodeLayer.ToString()+"," + node.nodeIndex + "):" + node.ownerAddr +"\n And my addr is:"+GlobalVar._instance.thisUserAddr;
            if (node.ownerAddr.ToUpper() == GlobalVar._instance.thisUserAddr.ToUpper())
            {
                yourNode.Add(node);
            }
        }
        Debug.Log(yourNode.Count+"______your Node Count!" );
        _initYourNodeUI();
    }
    private void _checkYourDownNode()
    {
        foreach (DownNodeData node in GlobalVar._instance.downNodeDataList)
        {
            if (node.ownerAddr.ToUpper() == GlobalVar._instance.thisUserAddr.ToUpper())
            {
                yourDownNode.Add(node);
            }
        }
        Debug.Log(yourDownNode.Count+"______your downNode" );
        _initYourDownNodeUI();
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
            print("init your node UI: "+node.nodeLayer+","+node.nodeIndex);
            if (node.isMajor == true)
            {
                thisNode.transform.GetChild(0).GetComponent<Image>().sprite = yellowImage;
            }
            else
            {
                thisNode.transform.GetChild(0).GetComponent<Image>().sprite = blueImage;
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
        _nodeFinish = true;
    }
    private void _initYourDownNodeUI()
    {
        float yPos = 0;
        foreach (DownNodeData node in yourDownNode)
        {
            GameObject thisNode = Instantiate(yourDownNodePrefab, yourNodeInitPos);
            print("init your down node UI: "+node.nodeLayer+","+node.nodeIndex);
            if (node.isMajor == true)
            {
                thisNode.transform.GetChild(0).GetComponent<Image>().sprite = pinkImage;
            }
            else
            {
                thisNode.transform.GetChild(0).GetComponent<Image>().sprite = redImage;
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
        _nodeFinish = true;
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
