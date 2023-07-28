using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.Rendering;

public class FieldInit : MonoBehaviour
{
    // Start is called before the first frame update
    public bool selected =true;
    public int woodType=0;
    public int ironType=0;
    public int eleType=0;
    public int eleCType = 0;
    private bool hasInit = false;

    public int wproType = 0;
    public int iproType = 0;
    public int eproType = 0;

    

    public GameObject fieldSelectPrefab;
    public GameObject mergeEnablePrefab;
    public GameObject[] woodPrefab;
    public GameObject[] ironPrefab;
    public GameObject[] elecPrefab;
    public GameObject[] wprPrefab;
    
    public ProtectData wproSObj;
    public ProtectData iproSObj;
    public ProtectData eproObj;
    public TowerData woodSObj;
    public TowerData ironSObj;
    public TowerData eleSObj;
    


    public string towerType;
    public LevelData globalVar;

    public bool mouseEnter;
    
    public string mapStructure;
    private string _previousMapStr;
    private bool _viewingInit = false;
    
    public GameObject[] showingPrefab;
    
    // Update is called once per frame
    private void Start()
    {
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
                    int performIndex = checkPerform(woodSObj, woodType);
                    GameObject fieldSeleted = Instantiate(woodPrefab[performIndex]);
                    fieldSeleted.transform.SetParent(transform);
                    fieldSeleted.transform.localPosition = Vector3.zero;
                    fieldSeleted.transform.localScale = new Vector3(50,50,50);
                    fieldSeleted.transform.name = transform.name+"wod"+woodType;
                    deletExcept("wod"+woodType);    
                    closeSelected();
                    setData(woodSObj);
                    towerType = "wood";
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
                if (checkExsit("iro") == false)
                {
                    int performIndex = checkPerformPro(wproSObj, ironType);
                    GameObject fieldSeleted = Instantiate(ironPrefab[performIndex]);
                    fieldSeleted.transform.SetParent(transform);
                    fieldSeleted.transform.localPosition = Vector3.zero;
                    fieldSeleted.transform.localScale = new Vector3(50,50,50);
                    fieldSeleted.transform.name = transform.name+"iro"+ironType;
                    deletExcept("iro"+ironType);  
                    closeSelected();
                    setData(ironSObj);
                    towerType = "iron";
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
                closeSelected();
            }
            if (eleType != 0)
            {
                if (checkExsit("ele" + eleType) == false)
                {
                    int performIndex = checkPerform(eleSObj, eleType);
                    GameObject fieldSeleted = Instantiate(elecPrefab[performIndex]);
                    fieldSeleted.transform.SetParent(transform);
                    fieldSeleted.transform.localPosition = Vector3.zero;
                    fieldSeleted.transform.localScale = new Vector3(50,50,50);
                    fieldSeleted.transform.name = transform.name+"ele"+eleType;
                    deletExcept("ele"+eleType);  
                    closeSelected();
                    setData(eleSObj);
                    towerType = "elec";
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
                if (checkExsit("wpr" + wproType) == false)
                {
                    GameObject fieldSeleted = Instantiate(wprPrefab[wproType-1]);
                    fieldSeleted.transform.SetParent(transform);
                    fieldSeleted.transform.localPosition = Vector3.zero;
                    fieldSeleted.transform.localScale = new Vector3(25,25,25);
                    fieldSeleted.transform.name = transform.name+"wpro"+wproType;
                    hasInit = true;
                    deletExcept("wpro"+wproType);    
                    closeSelected();
                    towerType = "wpro";
                }
            }
            else
            {
                if (checkExsit("wpr" + 1) == true || checkExsit("wpr" + 2) == true || checkExsit("wpr" + 3) == true)
                {
                    Destroy(findChild("wpr"+woodType));
                }
            }
            
            if (selected == false && woodType == 0 && ironType == 0 && eleType == 0 && wproType == 0)
            {
                deletExcept("hihi");
            }
            if (selected == true && woodType == 0 && ironType == 0 && eleType == 0)
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
            if ( selected == false && woodType == 0 && ironType == 0 && eleType == 0)
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
    private void setData(TowerData towerData)
    {
        int childNum = transform.childCount;
        GameObject child = transform.GetChild(childNum-1).gameObject; //transform.name+"wod"+woodType;003wod12
        string childName = child.transform.name.Substring(3); //wod12
        string childType = childName.Substring(0,3);
        int childGrade = int.Parse(childName.Substring(3));
        if (childType != "fil")
        {
            Turret towerAttack = transform.GetChild(childNum-1).GetComponent<Turret>();
            int towerGrade = childGrade;
            if (towerGrade <= towerData.gradeSpeedToAttack)
            {
                towerAttack.shootingRate = towerData.baseBulletNumberPerSecond + (towerGrade-1) * towerData.upgradeSpeedRate;
                towerAttack.bulletAttack = towerData.baseBulletAttack;
            }
            else if(towerGrade > towerData.gradeSpeedToAttack)
            {
                towerAttack.shootingRate = towerData.baseBulletNumberPerSecond + (towerData.gradeSpeedToAttack-1) * towerData.upgradeSpeedRate;
                towerAttack.bulletAttack = towerData.baseBulletAttack + (towerGrade-towerData.gradeSpeedToAttack)*towerData.upgradeAttackRate;
            }

            if (childType == "wod" || childType == "iro")
            {
                towerAttack.range = towerData.basicRange;
            }
            else
            {
                if (towerGrade < towerData.gradeRange2)
                {
                    towerAttack.range = towerData.basicRange;
                }
                else if (towerGrade >= towerData.gradeRange2 && towerGrade<towerData.gradeRange3)
                {
                    towerAttack.range = towerData.basicRange + 5;
                }
                else if (towerGrade >= towerData.gradeRange3)
                {
                    towerAttack.range = towerData.basicRange + 10;
                }
            }
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

    private List<Transform> findNeighborhood(Transform thisPos)
    {
        List<Transform> neighborhoodField = new List<Transform>();
        Transform posParent = thisPos.parent.parent;
        string fieldName = thisPos.gameObject.name;
        List<int> row_col = findRowCol(fieldName);
        int row = row_col[0];
        int col = row_col[1];

        List<Transform> neighborhoodPos = new List<Transform>();
        if (row >= 1)
        {
            Transform upPos = posParent.GetChild(row - 1).GetChild(col);
            neighborhoodPos.Add(upPos);
        }

        if (row <= 7)
        {
            Transform downPos = posParent.GetChild(row + 1).GetChild(col);
            neighborhoodPos.Add(downPos);
        }
        if (col >= 1)
        {
            Transform leftPos = posParent.GetChild(row).GetChild(col-1);
            neighborhoodPos.Add(leftPos);
        }
        if (col <= 16)
        {
            Transform rightPos = posParent.GetChild(row).GetChild(col+1);
            neighborhoodPos.Add(rightPos);
        }
        
        foreach (Transform pos in neighborhoodPos)
        {
            if (pos.gameObject.tag != "Road")
            {
                FieldInit neighborFI = pos.GetComponent<FieldInit>();
                if (neighborFI.selected == false && neighborFI.woodType==0 && neighborFI.ironType==0 && neighborFI.eleType==0)
                {
                    neighborhoodField.Add(pos);
                }
            }
        }

        return neighborhoodField;

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
                }
            }
        }
    }

    private string checkRelavent(Transform me, Transform posChecked)
    {
        string mePosString = me.gameObject.name;
        string checkPosString = posChecked.gameObject.name;
        int meRow = int.Parse(mePosString.Substring(0, 1));
        int meCol = 0;
        if (mePosString.Substring(1, 1) == "0")
        {
            meCol = int.Parse(mePosString.Substring(2, 1));
        }
        else
        {
            meCol = int.Parse(mePosString.Substring(1, 2));
        }
        
        int checkRow = int.Parse(checkPosString.Substring(0, 1));
        int checkCol = 0;
        if (checkPosString.Substring(1, 1) == "0")
        {
            checkCol = int.Parse(checkPosString.Substring(2, 1));
        }
        else
        {
            checkCol = int.Parse(checkPosString.Substring(1, 2));
        }

        if (meRow == checkRow)
        {
            if (meCol < checkCol)
            {
                return ("right");
            }
            else
            {
                return ("left");
            }
        }

        if (meRow > checkRow)
        {
            return ("up");
        }

        if (meRow < checkRow)
        {
            return ("down");
        }

        return ("null");
    }
}
