using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelInfoDataViewing : MonoBehaviour
{
    public NodeData thisNodeData;
    private string[][] _mapStruct;
    
    private GameObject _slidesParent;
    private GameObject _textViewingParent;

    private GameObject _bloodSlider;
    private GameObject[] _debuffSlider;

    private GameObject[] _basicInfo;
    private GameObject[] _debuffTower;
    private GameObject[] _towerNum;
    private GameObject[] _protectNum;
    

    private bool _hasInit = false;
    
    private 
    // Start is called before the first frame update
    void Awake()
    {
        _hasInit = false;
        thisNodeData = null;
        _slidesParent = transform.GetChild(0).GetChild(2).gameObject;
        _textViewingParent = transform.GetChild(0).GetChild(3).GetChild(0).gameObject;

        _bloodSlider = _slidesParent.transform.GetChild(0).gameObject;
        _debuffSlider = new GameObject[3];
        _debuffSlider[0] = _slidesParent.transform.GetChild(1).gameObject;
        _debuffSlider[1] = _slidesParent.transform.GetChild(2).gameObject;
        _debuffSlider[2] = _slidesParent.transform.GetChild(3).gameObject;
        _basicInfo = new GameObject[5];
        _basicInfo[0] = _textViewingParent.transform.GetChild(0).GetChild(0).gameObject; //layer
        _basicInfo[1] = _textViewingParent.transform.GetChild(0).GetChild(1).gameObject; //node
        _basicInfo[2] = _textViewingParent.transform.GetChild(0).GetChild(2).gameObject; //blood
        _basicInfo[3] = _textViewingParent.transform.GetChild(0).GetChild(3).gameObject; //money
        //_basicInfo[4] = _textViewingParent.transform.GetChild(0).GetChild(4).gameObject; //ownerAddr
        _debuffTower = new GameObject[3];
        _debuffTower[0] = _textViewingParent.transform.GetChild(1).GetChild(0).gameObject; //wood
        _debuffTower[1] = _textViewingParent.transform.GetChild(1).GetChild(1).gameObject; //iron
        _debuffTower[2] = _textViewingParent.transform.GetChild(1).GetChild(2).gameObject; //elec
        
        _towerNum = new GameObject[3];
        _towerNum[0] = _textViewingParent.transform.GetChild(2).GetChild(0).gameObject; //wood
        _towerNum[1] = _textViewingParent.transform.GetChild(2).GetChild(1).gameObject; //iron
        _towerNum[2] = _textViewingParent.transform.GetChild(2).GetChild(2).gameObject; //elec
        _protectNum = new GameObject[3];
        _protectNum[0] = _textViewingParent.transform.GetChild(3).GetChild(0).gameObject; //wood
        _protectNum[1] = _textViewingParent.transform.GetChild(3).GetChild(1).gameObject; //iron
        _protectNum[2] = _textViewingParent.transform.GetChild(3).GetChild(2).gameObject; //elec
    }

    private void Update()
    {
        if (thisNodeData != null && _hasInit == false)
        {
            InitNeverChange();
        }
    }

    void InitNeverChange()
    {
        _getMapmapList(thisNodeData.mapStructure);
        (int[] towerCount, int[] protectCount) = _checkTypeIndex();
        for (int i=0;i<3;i++)
        {
            TextMeshProUGUI countedTowerTMP = _towerNum[i].GetComponent<TextMeshProUGUI>();
            countedTowerTMP.text = towerCount[i].ToString();
            TextMeshProUGUI countedProtectTMP = _protectNum[i].GetComponent<TextMeshProUGUI>();
            countedProtectTMP.text = protectCount[i].ToString();
        }

        _basicInfo[0].GetComponent<TextMeshProUGUI>().text = "LAYER: "+thisNodeData.nodeLayer.ToString();
        _basicInfo[1].GetComponent<TextMeshProUGUI>().text = "INDEX: "+thisNodeData.nodeIndex.ToString();
        _basicInfo[2].GetComponent<TextMeshProUGUI>().text = thisNodeData.curHealth.ToString()+"/"+thisNodeData.fullHealth.ToString();
        _basicInfo[3].GetComponent<TextMeshProUGUI>().text = thisNodeData.money.ToString();
        //_basicInfo[4].GetComponent<TextMeshProUGUI>().text = thisNodeData.ownerAddr;
        Slider healthSlider = _bloodSlider.GetComponent<Slider>();
        healthSlider.maxValue = thisNodeData.fullHealth;
        healthSlider.value = thisNodeData.curHealth;
        healthSlider.minValue = 0;
        
        //暂时先这样写
        

        Slider WdebuffSlider = _debuffSlider[0].GetComponent<Slider>();
        WdebuffSlider.maxValue = 5;
        WdebuffSlider.value = 4;
        WdebuffSlider.minValue = 0;
        Slider IdebuffSlider = _debuffSlider[1].GetComponent<Slider>();
        IdebuffSlider.maxValue = 4;
        IdebuffSlider.value = 0;
        IdebuffSlider.minValue = 0;
        Slider EdebuffSlider = _debuffSlider[2].GetComponent<Slider>();
        EdebuffSlider.maxValue = 3;
        EdebuffSlider.value = 1;
        EdebuffSlider.minValue = 0;
        _debuffTower[0].GetComponent<TextMeshProUGUI>().text = "80%";
        _debuffTower[1].GetComponent<TextMeshProUGUI>().text = "0%";
        _debuffTower[2].GetComponent<TextMeshProUGUI>().text = "10%";
            
        _hasInit = true;
    }

    private (int[],int[]) _checkTypeIndex()
    {
        int[] towerCount = new int[3];
        int[] protectCount = new int[3];
        for (int i = 0; i < _mapStruct.Length; i++)
        {
            for (int j = 0; j < _mapStruct[i].Length; j++)
            {
                if (_mapStruct[i][j].Length >= 5)
                {
                    string mapType = _mapStruct[i][j].Substring(0, 4);
                    if (mapType == "wood")
                    {
                        towerCount[0] += 1;
                    }
                    else if(mapType == "iron")
                    {
                        towerCount[1] += 1;
                    }
                    else if (mapType == "elec")
                    {
                        towerCount[2] += 1;
                    }
                    else if (mapType == "wpro")
                    {
                        protectCount[0] += 1;
                    }
                    else if (mapType == "ipro")
                    {
                        protectCount[1] += 1;
                    }
                    else if (mapType == "epro")
                    {
                        protectCount[2] += 1;
                    }
                    else if (mapType == "eleC")
                    {
                    }
                    else
                    {
                        Debug.LogError(thisNodeData.nodeLayer+","+thisNodeData.nodeIndex+": incorrect Map String!!!");
                    }
                }
            }
        }
        return (towerCount,protectCount);
    }
    private void _getMapmapList(string mapmapString)
    {
        string totalString = mapmapString;
        string[] rows = totalString.Split("/n");
        string[][] stringList = new string[rows.Length][];
        for (int i = 0; i < rows.Length; i++)
        {
            stringList[i] = rows[i].Split(',');
        }
        for (int i = 0; i < stringList.Length / 2; i++)
        {
            string[] temp = stringList[i];
            stringList[i] = stringList[stringList.Length - 1 - i];
            stringList[stringList.Length - 1 - i] = temp;
        }
        
        for (int i = 0; i < stringList.Length; i++)
        {
            if (stringList[i] != null)
            {
                string[] row = stringList[i];
                List<string> newRow = new List<string>();

                for (int j = 0; j < row.Length; j++)
                {
                    if (row[j] != null&&row[j] != "")
                    {
                        newRow.Add(row[j]);
                    }
                }
                // 覆盖原始行
                stringList[i] = newRow.ToArray();
            }
        }
        _mapStruct = stringList;
    }

    public void ClosePreViewingNode()
    {
        if (GlobalVar._instance.isPreViewing == true)
        {
            // if (transform.parent.parent.name.Length > 3)
            // {
            //     transform.parent.parent.GetComponent<CursorOutlinesPure>()._canDisappear = true;
            // }
            // else
            // {
                transform.parent.parent.GetComponent<CursorOutlines>()._canDisappear = true;
                transform.parent.parent.GetComponent<CursorOutlines>().olWithoutMouse = false;
            //  }
            //transform.parent.parent.GetComponent<CursorOutlines>()._canDisappear = true;
            transform.GetChild(0).gameObject.SetActive(false);
            GlobalVar._instance.isPreViewing = false;
            CameraController._instance.camLock = false;
            GlobalVar._instance.ChangeState("MainStart");
        }
    }
    public void OpenViewingScene()
    {
        GlobalVar._instance.ChangeState("Viewing");
        SceneManager.LoadScene("ExhibExample");
    }
}
