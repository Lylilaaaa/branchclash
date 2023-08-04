using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.Rendering;

public class FieldInit : MonoBehaviour
{
    [Header("--------GlobalVar--------")]
    public GameObject fieldSelectPrefab;
    public string mapStructure;
    private string _previousMapStr;
    public bool mouseEnter;
    
    
    [Header("--------TowerTypeGrade---------")]
    public bool selected =true;
    public string towerType;
    public int woodType=0;
    public int ironType=0;
    public int eleType=0;
    public int eleCType = 0;
    public int wproType = 0;
    public int iproType = 0;
    public int eproType = 0;
    
    
    [Header("--------TowerPrefab---------")]
    public GameObject[] woodPrefab;
    public GameObject[] ironPrefab;
    public GameObject[] elecPrefab;
    public GameObject[] wprPrefab;
    public GameObject[] iprPrefab;
    public GameObject[] eprPrefab;
    
    [Header("--------TowerDataScriptableObj---------")]
    public ProtectData wproSObj;
    public ProtectData iproSObj;
    public ProtectData eproSObj;
    public TowerData woodSObj;
    public TowerData ironSObj;
    public TowerData eleSObj;
    
    [Header("--------ExhibitMode--------")]
    public GameObject[] showingPrefab;
    private bool _viewingInit = false;
    
    
    
    // Update is called once per frame
    private void Start()
    {
        selected = false;
        towerType = "";
        mapStructure = "";
        _previousMapStr = mapStructure;
    }

    void Update()
    {
        if (GlobalVar._instance.GetState() == GlobalVar.GameState.Viewing)
        {
            _initViewing();
            if (_viewingInit == false)
            {
                _setViewing();
                _viewingInit = true;
            }
            if (_previousMapStr != mapStructure)
            {
                _setViewing();
            }
            _previousMapStr = mapStructure;
        }
        if (GlobalVar._instance.GetState() == GlobalVar.GameState.ChooseField ||
            GlobalVar._instance.GetState() == GlobalVar.GameState.AddTowerUI)
        {
            //print(GlobalVar._instance.GetState());
            if (selected == true)
            {
                //可以被选择
                if (checkExsit("fil") == false)
                {
                    GameObject fieldSeleted = Instantiate(fieldSelectPrefab);
                    fieldSeleted.transform.SetParent(transform);
                    fieldSeleted.transform.localPosition = Vector3.zero;
                    fieldSeleted.transform.localScale = Vector3.one;
                    fieldSeleted.transform.name = transform.name+"fild";
                }
            }
            else
            {   //不可以被选择
                if (checkExsit("fil") == true)
                {
                    deletType("fil");
                }
            }
            
            if (woodType != 0)
            {
                if (checkExsitExact("wod"+woodType) == false) //没有wood的武器
                {
                    //print("does exist: "+"wod"+woodType);
                    int performIndex = checkPerform(woodSObj, woodType);
                    GameObject fieldSeleted = Instantiate(woodPrefab[performIndex]);
                    fieldSeleted.transform.SetParent(transform);
                    fieldSeleted.transform.localPosition = Vector3.zero;
                    fieldSeleted.transform.localScale = new Vector3(50,50,50);
                    fieldSeleted.transform.name = transform.name+"wod"+woodType;
                    deletExcept("wod"+woodType);
                    selected = false;
                    towerType = "wood"+woodType;
                }
                    
            }
            else //(woodType == 0)
            {
                if (checkExsit("wod" ) == true )
                {
                    deletType("wod");
                }
            }
            if (ironType != 0)
            {
                if (checkExsitExact("iro"+ironType) == false)
                {
                    int performIndex = checkPerformPro(wproSObj, ironType);
                    GameObject fieldSeleted = Instantiate(ironPrefab[performIndex]);
                    fieldSeleted.transform.SetParent(transform);
                    fieldSeleted.transform.localPosition = Vector3.zero;
                    fieldSeleted.transform.localScale = new Vector3(50,50,50);
                    fieldSeleted.transform.name = transform.name+"iro"+ironType;
                    deletExcept("iro"+ironType);  
                    selected = false;
                    towerType = "iron"+ironType;
                }
            }
            else
            {
                if (checkExsit("iro" ) == true )
                {
                    deletType("iro");
                }
            }

            if (eleCType != 0)
            {
                selected = false;
                towerType = "eleC"+eleCType;
            }
            if (eleType != 0)
            {
                if (checkExsitExact("ele"+eleType) == false)
                {
                    int performIndex = checkPerform(eleSObj, eleType);
                    GameObject fieldSeleted = Instantiate(elecPrefab[performIndex]);
                    fieldSeleted.transform.SetParent(transform);
                    fieldSeleted.transform.localPosition = Vector3.zero;
                    fieldSeleted.transform.localScale = new Vector3(50,50,50);
                    fieldSeleted.transform.name = transform.name+"ele"+eleType;
                    deletExcept("ele"+eleType);  
                    selected = false;
                    towerType = "elec"+eleType;
                }
            }
            else
            {
                if (checkExsit("ele") == true)
                {
                    deletType("ele");
                }
            }
            if (wproType != 0)
            {
                if (checkExsitExact("wpr"+wproType) == false)
                {
                    int performIndex = checkPerformPro(wproSObj, wproType);
                    GameObject fieldSeleted = Instantiate(wprPrefab[performIndex]);
                    fieldSeleted.transform.SetParent(transform);
                    fieldSeleted.transform.localPosition = Vector3.zero;
                    fieldSeleted.transform.localScale = new Vector3(25,25,25);
                    fieldSeleted.transform.name = transform.name+"wpr"+wproType;
                    deletExcept("wpr"+wproType);    
                    selected = false;
                    towerType = "wpro"+wproType;
                }
            }
            else
            {
                if (checkExsit("wpr") == true )
                {
                    deletType("wpr");
                }
            }
            if (iproType != 0)
            {
                if (checkExsitExact("ipr"+iproType) == false)
                {
                    int performIndex = checkPerformPro(iproSObj, iproType);
                    GameObject fieldSeleted = Instantiate(iprPrefab[performIndex]);
                    fieldSeleted.transform.SetParent(transform);
                    fieldSeleted.transform.localPosition = Vector3.zero;
                    fieldSeleted.transform.localScale = new Vector3(25,25,25);
                    fieldSeleted.transform.name = transform.name+"ipr"+iproType;
                    deletExcept("ipr"+iproType);    
                    selected = false;
                    towerType = "ipro"+iproType;
                }
            }
            else
            {
                if (checkExsit("ipr") == true )
                {
                    deletType("ipr");
                }
            }
            if (eproType != 0)
            {
                if (checkExsitExact("epr"+eproType) == false)
                {
                    int performIndex = checkPerformPro(eproSObj, eproType);
                    GameObject fieldSeleted = Instantiate(eprPrefab[performIndex]);
                    fieldSeleted.transform.SetParent(transform);
                    fieldSeleted.transform.localPosition = Vector3.zero;
                    fieldSeleted.transform.localScale = new Vector3(25,25,25);
                    fieldSeleted.transform.name = transform.name+"epr"+eproType;
                    deletExcept("epr"+eproType);    
                    selected = false;
                    towerType = "epro"+eproType;
                }
            }
            else
            {
                if (checkExsit("epr") == true )
                {
                    deletType("epr");
                }
            }
            
            if (selected == false && woodType == 0 && ironType == 0 && eleType == 0 && wproType == 0&& iproType == 0&& eproType == 0)
            {
                deletExcept("hihi");
            }
            if (selected == true && woodType == 0 && ironType == 0 && eleType == 0 && wproType == 0&& iproType == 0&& eproType == 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (GlobalVar._instance.GetState() == GlobalVar.GameState.ChooseField )
                    {
                        GlobalVar._instance.ChangeState("AddTowerUI");
                        GlobalVar._instance.chooseField(transform.name);
                        selected = false;
                    }
                }
            }
        }
    }
    private void OnMouseEnter()
    {
        mouseEnter = true;
        if (GlobalVar._instance.GetState() == GlobalVar.GameState.ChooseField)
        {
            if ( selected == false && woodType == 0 && ironType == 0 && eleType == 0&& wproType == 0 && iproType == 0 && eproType == 0)
            {
                selected = true;
            }
        }
    }

    private void _setViewing()
    {
        deletExcept("haha");
        if (mapStructure == "00" || mapStructure == "R")
        {
            return;
        }
        string typeViewingTower = mapStructure.Substring(0, 4);
        int towerGrade = int.Parse(mapStructure.Substring(4));
        //print(typeViewingTower+": "+towerGrade);
        if (typeViewingTower == "wood")
        {
            _initPerform(0,towerGrade);
        }
        else if (typeViewingTower == "iron")
        {
            _initPerform(1,towerGrade);
        }
        else if (typeViewingTower == "elec")
        {
            _initPerform(2,towerGrade);
        }
        else if (typeViewingTower == "wpro")
        {
            _initPerform(3,towerGrade);
        }
        else if (typeViewingTower == "ipro")
        {
            _initPerform(4,towerGrade);
        }
        else if (typeViewingTower == "epro")
        {
            _initPerform(5,towerGrade);
        }
    }
    private void _initPerform(int index,int grade)
    {
        if (index <= 2)
        {
            GameObject performGObj = Instantiate(showingPrefab[index]);
            performGObj.transform.SetParent(transform);
            performGObj.transform.localPosition = Vector3.zero;
            performGObj.transform.localScale = new Vector3(50,50,50);
            TowerDataInit dataViewing = performGObj.GetComponent<TowerDataInit>();
            dataViewing.towerGrade = grade;
        }
        else
        {
            GameObject performGObj = Instantiate(showingPrefab[index]);
            performGObj.transform.SetParent(transform);
            performGObj.transform.localPosition = Vector3.zero;
            performGObj.transform.localScale = new Vector3(25,25,25);
            TowerDataInit dataViewing = performGObj.GetComponent<TowerDataInit>();
            dataViewing.towerGrade = grade;
        }
    }

    private void _initViewing()
    {
        string fieldName = transform.gameObject.name;
        List<int> row_col = findRowCol(fieldName);
        int row = row_col[0];
        int col = row_col[1];
        mapStructure = GlobalVar._instance.mapmapList[row][col];
    }

    private void OnMouseExit()
    {
        mouseEnter = false;
        //print("yes, miao!");
        selected = false;
    }
    private bool checkExsit(string checkObj) //输入三个char
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i).gameObject;
            string childName = child.transform.name.Substring(3,3);
            if (childName == checkObj)
            {
                return true;
            }
        }
        return false;
    }
    private bool checkExsitExact(string checkObj) //输入三个char+grade
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i).gameObject;
            string childName = child.transform.name.Substring(3);
            if (childName == checkObj)
            {
                return true;
            }
        }
        return false;
    }

    private int checkPerform(TowerData towerdata,int grade)
    {
        if (grade>=0 && grade < towerdata.performGrade2)
        {
            return 0;
        }
        else if (grade >= towerdata.performGrade2 && grade < towerdata.performGrade3)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }
    private int checkPerformPro(ProtectData towerdata,int grade)
    {
        if (grade>=0 && grade < towerdata.performGrade2)
        {
            return 0;
        }
        else if (grade >= towerdata.performGrade2 && grade < towerdata.performGrade3)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }

    private GameObject findChild(string findName)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i).gameObject;
            string childName = child.transform.name.Substring(3);
            if (childName == findName)
            {
                return child;
            }
        }
        return null;
    }
    
    public void deletExcept(string deleObj)
    {
        for (int i=0;i<transform.childCount;i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            string childName = child.transform.name.Substring(3);
            if (childName != deleObj)
            {
                //print("destroy"+child.transform.name);
                Destroy(child);
            }
        }
    }
    public void deletType(string deleObj) //3 char type 
    {
        for (int i=0;i<transform.childCount;i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            string childName = child.transform.name.Substring(3,3);
            if (childName == deleObj)
            {
                //print("destroy"+child.transform.name);
                Destroy(child);
            }
        }
    }

    private List<int> findRowCol(string rowColString)
    {
        int row = int.Parse(rowColString.Substring(0, 1));
        int col = 0;
        if (rowColString.Substring(1, 1) == "0")
        {
            col = int.Parse(rowColString.Substring(2, 1));
        }
        else
        {
            col = int.Parse(rowColString.Substring(1, 2));
        }

        List<int> row_col = new List<int>();
        row_col.Add(row);
        row_col.Add(col);
        return row_col;
    }
    
    private void closeSelected()
    {
        Transform posParent = transform.parent.parent;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 18; j++)
            {
                GameObject thisField = posParent.GetChild(i).GetChild(j).gameObject;
                //print(thisField);
                if (thisField.tag != "Road")
                {
                    FieldInit fieldInit = thisField.GetComponent<FieldInit>();
                    fieldInit.selected = false;
                    print("close select!!");
                }
            }
        }
    }
    
    
}
