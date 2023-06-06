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
    public int eleType2 = 0;

    public int checkState = 0;

    public GameObject fieldSelectPrefab;
    public GameObject mergeEnablePrefab;
    public GameObject[] woodPrefab;
    public GameObject[] ironPrefab;
    public GameObject[] elecPrefab;

    public TowerData woodSObj;
    public TowerData ironSObj;
    public TowerData eleSObj;

    public string towerType;
    public LevelData globalVar;
    
    private Collider selectedCollider;
    
    // Update is called once per frame
    private void Start()
    {
        selectedCollider = transform.GetComponent<BoxCollider>();
        towerType = "";
    }

    void Update()
    {
        if (checkState == 0)
        {
            Destroy(findChild("able"));
            checkState = 100;
        }
        if (checkState == 1)
        {
            if (woodType >= 1 || ironType>=1 || eleType>=1)
            {
                GameObject mergeSeleted = Instantiate(mergeEnablePrefab);
                mergeSeleted.transform.SetParent(transform);
                mergeSeleted.transform.localPosition = new Vector3(0f,150f,0f);
                mergeSeleted.transform.localScale = new Vector3(1.2f,1.2f,1.2f);
                mergeSeleted.transform.name = transform.name+"mergable";
                checkState = 2;
            }
        }

        if (checkState == 3)
        {
            if (globalVar.MergeMem.Count == 2)
            {
                if (globalVar.MergeMem[1] == transform.name.Substring(0, 3))
                {
                    int mergeFromRow = findRowCol(globalVar.MergeMem[0])[0];
                    int mergeFromCol = findRowCol(globalVar.MergeMem[0])[1];
                    Transform mergeFromTrans = transform.parent.parent.GetChild(mergeFromRow).GetChild(mergeFromCol);
                    print(mergeFromTrans);
                    FieldInit mergeFromTransFI = mergeFromTrans.GetComponent<FieldInit>();
                    if (towerType == "wood" && (woodType==mergeFromTransFI.woodType))
                    {
                        woodType += 1;
                        mergeFromTransFI.woodType = 0;
                        mergeFromTransFI.deletExcept("wod"+woodType);
                    }
                    else if (towerType == "iron"&& (ironType==mergeFromTransFI.ironType))
                    {
                        ironType += 1;
                        mergeFromTransFI.ironType = 0;
                        mergeFromTransFI.deletExcept("iro"+woodType);
                    }
                    else if (towerType == "elec"&& (eleType==mergeFromTransFI.eleType))
                    {
                        eleType += 1;
                        mergeFromTransFI.eleType = 0;
                        mergeFromTransFI.deletExcept("ele"+eleType);
                    }
                    else
                    {
                        print("wrong merging!");
                    }
                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = 0; j < 18; j++)
                        {
                            GameObject thisField = transform.parent.parent.GetChild(i).GetChild(j).gameObject;
                            print(thisField);
                            if (thisField.tag != "Road")
                            {
                                FieldInit fieldInit = thisField.GetComponent<FieldInit>();
                                //print(fieldInit.name+ fieldInit.selected);
                                fieldInit.checkState = 0;
                            }
                        }
                    }
                }
            }
           
        }
        if (selected == true)
        {
            if (checkExsit("fild") == false)
            {
                GameObject fieldSeleted = Instantiate(fieldSelectPrefab);
                fieldSeleted.transform.SetParent(transform);
                fieldSeleted.transform.localPosition = Vector3.zero;
                fieldSeleted.transform.localScale = Vector3.one;
                fieldSeleted.transform.name = transform.name+"fild";
            }
        }
        else
        {
            if (checkExsit("fild") == true)
            {
                Destroy(findChild("fild"));
            }
        }

        if (woodType != 0)
        {
            if (checkExsit("wod" + woodType) == false)
            {
                GameObject fieldSeleted = Instantiate(woodPrefab[woodType-1]);
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
        else
        {
            if (checkExsit("wod" + 1) == true || checkExsit("wod" + 2) == true || checkExsit("wod" + 3) == true)
            {
                Destroy(findChild("wod"+woodType));
            }
        }
        if (ironType != 0)
        {
            if (checkExsit("iro" + ironType) == false)
            {
                GameObject fieldSeleted = Instantiate(ironPrefab[ironType-1]);
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
            if (checkExsit("iro" + 1) == true || checkExsit("iro" + 2) == true || checkExsit("iro" + 3) == true)
            {
                Destroy(findChild("iro"+ironType));
            }
        }
        if (eleType != 0)
        {
            if (checkExsit("ele" + eleType) == false)
            {
                closeSelected();
                List<Transform> neighborhood = findNeighborhood(transform);
                foreach (Transform pos in neighborhood)
                {
                    FieldInit neighborhoodFI = pos.GetComponent<FieldInit>();
                    neighborhoodFI.selected = true;
                    ClikField neighborhoodCF = pos.GetChild(0).GetComponent<ClikField>();
                    neighborhoodCF.isElecTwice = true;
                    //继续检测直到点击了第二次
                    if (neighborhoodFI.eleType2 == 1)
                    {
                        string relaventPos = checkRelavent(transform, pos);
                        if (relaventPos == "up")
                        {
                            GameObject fieldSeleted = Instantiate(elecPrefab[eleType-1]);
                            fieldSeleted.transform.SetParent(transform);
                            fieldSeleted.transform.localPosition = Vector3.zero;
                            fieldSeleted.transform.localScale = new Vector3(50,50,50);
                            fieldSeleted.transform.localRotation = Quaternion.Euler(90f,0f,0f);
                            fieldSeleted.transform.name = transform.name+"ele"+eleType;
                            deletExcept("ele"+eleType);   
                        }
                        else if (relaventPos == "down")
                        {
                            GameObject fieldSeleted = Instantiate(elecPrefab[eleType-1]);
                            fieldSeleted.transform.SetParent(transform);
                            fieldSeleted.transform.localPosition = Vector3.zero;
                            fieldSeleted.transform.localScale = new Vector3(50,50,50);
                            fieldSeleted.transform.localRotation = Quaternion.Euler(90f,0f,180f);
                            fieldSeleted.transform.name = transform.name+"ele"+eleType;
                            deletExcept("ele"+eleType);
                        }
                        else if (relaventPos == "right")
                        {
                            GameObject fieldSeleted = Instantiate(elecPrefab[eleType-1]);
                            fieldSeleted.transform.SetParent(transform);
                            fieldSeleted.transform.localPosition = Vector3.zero;
                            fieldSeleted.transform.localScale = new Vector3(50,50,50);
                            fieldSeleted.transform.localRotation = Quaternion.Euler(90f,0f,270f);
                            fieldSeleted.transform.name = transform.name+"ele"+eleType;
                            deletExcept("ele"+eleType);
                        }
                        else if (relaventPos == "left")
                        {
                            GameObject fieldSeleted = Instantiate(elecPrefab[eleType-1]);
                            fieldSeleted.transform.SetParent(transform);
                            fieldSeleted.transform.localPosition = Vector3.zero;
                            fieldSeleted.transform.localScale = new Vector3(50,50,50);
                            fieldSeleted.transform.localRotation = Quaternion.Euler(90f,0f,90f);
                            fieldSeleted.transform.name = transform.name+"ele"+eleType;
                            deletExcept("ele"+eleType);
                        }

                        foreach (Transform posend in neighborhood)
                        {
                            FieldInit neighborhoodendFI = posend.GetComponent<FieldInit>();
                            //print(posend.gameObject.name);
                            neighborhoodendFI.selected = false;
                        }
                        setData(eleSObj);
                        towerType = "elec";
                        break;
                    }
                }
 
            }
        }
        else
        {
            if (checkExsit("ele" + 1) == true || checkExsit("ele" + 2) == true || checkExsit("ele" + 3) == true)
            {
                Destroy(findChild("ele"+eleType));
            }
        }

        if (checkState == 0 && selected == false && woodType == 0 && ironType == 0 && eleType == 0)
        {
            deletExcept("hihi");
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            
            if (hit.collider == selectedCollider)
            {
                print("yes, miao!");
                if (checkState == 0 && selected == false && woodType == 0 && ironType == 0 && eleType == 0)
                {
                    selected = true;
                }
            }
            else
            {
                selected = false;
            }
        }
    }

    private bool checkExsit(string checkObj)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i).gameObject;
            string childName = child.transform.name.Substring(child.transform.name.Length-4,4);
            if (childName == checkObj)
            {
                return true;
            }
        }
        return false;
    }

    private void setData(TowerData towerData)
    {
        int childNum = transform.childCount;
        GameObject child = transform.GetChild(childNum-1).gameObject;
        string childType = child.transform.name.Substring(child.transform.name.Length-4,4);
        if (childType != "fild")
        {
            Turret towerAttack = transform.GetChild(childNum-1).GetComponent<Turret>();
            int towerGrade = int.Parse(childType.Substring(childType.Length - 1, 1));
            if (towerGrade < 5)
            { 
                towerAttack.shootingRate =
                    towerData.baseBulletNumberPerSecond + (towerGrade - 1) * towerData.upgradeSpeedRate;
                towerAttack.bulletAttack = towerData.baseBulletAttack;
            }
            else
            {
                towerAttack.shootingRate = towerData.baseBulletNumberPerSecond + 4 * towerData.upgradeSpeedRate;
                towerAttack.bulletAttack = towerData.baseBulletAttack + (towerGrade - 5) * towerData.upgradeAttackRate;
            }

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
    private GameObject findChild(string findName)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i).gameObject;
            string childName = child.transform.name.Substring(child.transform.name.Length-4,4);
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
            string childName = child.transform.name.Substring(child.transform.name.Length-4,4);
            if (childName != deleObj)
            {
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
