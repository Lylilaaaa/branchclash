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
    public DownNodeData corrDownNodeData;
    
    [Header("======Set by runtime!======")]
    public string[][] _mapStruct;
    
    public GameObject _slidesParent;
    public GameObject _textViewingParent;

    public GameObject _bloodSlider;
    public GameObject[] _debuffSlider;

    public GameObject[] _basicInfo;
    public GameObject[] _debuffTower;
    public GameObject[] _towerNum;
    public GameObject[] _protectNum;
    
   public Dictionary<int,int> woodCount; //grade, count
    public Dictionary<int,int> ironCount;
    public Dictionary<int,int> elecCount;
    public Dictionary<int,int> wproCount;
    public Dictionary<int,int> iproCount;
    public Dictionary<int,int> eproCount; 


    public Button _enterNextBut;

    public bool _hasInit = false;
    
    private 
    // Start is called before the first frame update
    void Start()
    {
        _hasInit = false;
        thisNodeData = null;
        _slidesParent = transform.GetChild(0).GetChild(2).gameObject;
        _textViewingParent = transform.GetChild(0).GetChild(3).GetChild(0).gameObject;
        _enterNextBut = transform.GetChild(0).GetChild(4).GetChild(0).GetComponent<Button>();

        _bloodSlider = _slidesParent.transform.GetChild(0).gameObject;
        _debuffSlider = new GameObject[3];
        _debuffSlider[0] = _slidesParent.transform.GetChild(1).gameObject;
        _debuffSlider[1] = _slidesParent.transform.GetChild(2).gameObject;
        _debuffSlider[2] = _slidesParent.transform.GetChild(3).gameObject;
        _basicInfo = new GameObject[4];
        _basicInfo[0] = _textViewingParent.transform.GetChild(0).GetChild(0).gameObject; //layer
        _basicInfo[1] = _textViewingParent.transform.GetChild(0).GetChild(1).gameObject; //node
        _basicInfo[2] = _textViewingParent.transform.GetChild(0).GetChild(2).gameObject; //blood
        _basicInfo[3] = _textViewingParent.transform.GetChild(0).GetChild(3).gameObject; //money
        _basicInfo[4] = _textViewingParent.transform.GetChild(0).GetChild(4).gameObject; //ownerAddr
        
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
        if (thisNodeData != null && corrDownNodeData != null && _hasInit == false)
        {
            InitNeverChange();
        }
    }

    void InitNeverChange()
    {
        if (GlobalVar._instance.role == 1)
        {
            _enterNextBut.interactable = false;
        }
        else
        {
            _enterNextBut.interactable = true;
        }
        _getMapmapList(thisNodeData.mapStructure);
        Debug.Log(thisNodeData.mapStructure);
        (woodCount,ironCount,elecCount,wproCount,iproCount,eproCount) =  CurNodeDataSummary._instance._checkTypeIndex(_mapStruct);
        int[] towerCount = new int[3];
        int[] protectCount = new int[3];
        foreach (var VARIABLE in woodCount.Keys)
        {
            towerCount[0] += woodCount[VARIABLE];
        }
        foreach (var VARIABLE in ironCount.Keys)
        {
            towerCount[1] += ironCount[VARIABLE];
        }
        foreach (var VARIABLE in elecCount.Keys)
        {
            towerCount[2] += elecCount[VARIABLE];
        }
        foreach (var VARIABLE in wproCount.Keys)
        {
            protectCount[0] +=wproCount[VARIABLE];
        }
        foreach (var VARIABLE in iproCount.Keys)
        {
            protectCount[1] += iproCount[VARIABLE];
        }
        foreach (var VARIABLE in eproCount.Keys)
        {
            protectCount[2] += eproCount[VARIABLE];
        }
        //(int[] towerCount, int[] protectCount) = _checkTypeIndex();
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
        int stringCounted = thisNodeData.ownerAddr.Length;
        _basicInfo[4].GetComponent<TextMeshProUGUI>().text = thisNodeData.ownerAddr.Substring(0, 5) + "..." +
            thisNodeData.ownerAddr.Substring(stringCounted - 3, 3);
        Slider healthSlider = _bloodSlider.GetComponent<Slider>();
        healthSlider.maxValue = thisNodeData.fullHealth;
        healthSlider.value = thisNodeData.curHealth;
        healthSlider.minValue = 0;

        int[] weaponBlood = new int[3];
        weaponBlood = CurNodeDataSummary._instance.GetMainMaxWeaponLevelBlood(woodCount, ironCount, elecCount);
        Debug.Log(weaponBlood[0]+", "+weaponBlood[1]+", "+weaponBlood[2]);
        int[] debuff = new int[3];
        debuff = corrDownNodeData.debuffData;
        Debug.Log("debuff: "+debuff[0]+debuff[1]+debuff[2]);
        float[] debuffPresentage = new float[3];
        for (int i = 0; i < 3; i++)
        {
            if (weaponBlood[i] != 0)
            {
                debuffPresentage[i] = (float)debuff[i] / (float)weaponBlood[i];
            }
            else
            {
                debuffPresentage[i] = 1;
            } 
        }

        Slider WdebuffSlider = _debuffSlider[0].GetComponent<Slider>();
        WdebuffSlider.maxValue = 1;
        WdebuffSlider.value = debuffPresentage[0];
        WdebuffSlider.minValue = 0;
        Slider IdebuffSlider = _debuffSlider[1].GetComponent<Slider>();
        IdebuffSlider.maxValue = 1;
        IdebuffSlider.value =  debuffPresentage[1];;
        IdebuffSlider.minValue = 0;
        Slider EdebuffSlider = _debuffSlider[2].GetComponent<Slider>();
        EdebuffSlider.maxValue = 1;
        EdebuffSlider.value =  debuffPresentage[2];;
        EdebuffSlider.minValue = 0;
        _debuffTower[0].GetComponent<TextMeshProUGUI>().text = (WdebuffSlider.value*100).ToString()+"%";
        _debuffTower[1].GetComponent<TextMeshProUGUI>().text = (IdebuffSlider.value*100).ToString()+"%";
        _debuffTower[2].GetComponent<TextMeshProUGUI>().text = (EdebuffSlider.value*100).ToString()+"%";
        
        _hasInit = true;
    }

    
    

    // private (int[],int[]) _checkTypeIndex()
    // {
    //     int[] towerCount = new int[3];
    //     int[] protectCount = new int[3];
    //     for (int i = 0; i < _mapStruct.Length; i++)
    //     {
    //         for (int j = 0; j < _mapStruct[i].Length; j++)
    //         {
    //             if (_mapStruct[i][j].Length >= 5)
    //             {
    //                 string mapType = _mapStruct[i][j].Substring(0, 4);
    //                 if (mapType == "wood")
    //                 {
    //                     towerCount[0] += 1;
    //                 }
    //                 else if(mapType == "iron")
    //                 {
    //                     towerCount[1] += 1;
    //                 }
    //                 else if (mapType == "elec")
    //                 {
    //                     towerCount[2] += 1;
    //                 }
    //                 else if (mapType == "wpro")
    //                 {
    //                     protectCount[0] += 1;
    //                 }
    //                 else if (mapType == "ipro")
    //                 {
    //                     protectCount[1] += 1;
    //                 }
    //                 else if (mapType == "epro")
    //                 {
    //                     protectCount[2] += 1;
    //                 }
    //                 else if (mapType == "eleC")
    //                 {
    //                 }
    //                 else
    //                 {
    //                     Debug.LogError(thisNodeData.nodeLayer+","+thisNodeData.nodeIndex+": incorrect Map String!!!");
    //                 }
    //             }
    //         }
    //     }
    //     return (towerCount,protectCount);
    // }
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
                // ????????
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
            Camera.main.gameObject.GetComponent<CameraController>().camLock = false;
            GlobalVar._instance.ChangeState("MainStart");
        }
    }
    public void OpenViewingScene()
    {
        LineGenerator._instance.Clear();
        RedLineGenerator._instance.Clear();
        GlobalVar._instance.ChangeState("Viewing");
        GlobalVar._instance._loadNextScene("2_0_ExhibExample");
        CurNodeDataSummary._instance.GetChosenNodeInfo();
    }
}
