using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ViewTowerCurSor : MonoBehaviour
{
    public bool mouseEnter;
    public bool _canDisappear = true;
    public GameObject previewLevelInfoPenal;
    private bool _finish=false;
    public GameObject[] rangeList;

    private int _grade;

    private TextMeshProUGUI _range;
    private TextMeshProUGUI _damage;
    private TextMeshProUGUI _speed;
    private TextMeshProUGUI _towerName;
    private TextMeshProUGUI _discription;
    private Transform _rangeParent;
    public TowerType ThisTowerType;
    public enum TowerType
    {
        wood,
        iron,
        elec,
        wpro,
        ipro,
        epro
    }
    void Start()
    {
        _canDisappear = true;
        mouseEnter = false;
        //outlineGbj = transform.GetChild(2).gameObject;
        int childCount = transform.childCount;
        //print(outlineGbj);
        previewLevelInfoPenal.gameObject.SetActive(false);
        if (ThisTowerType == TowerType.wood || ThisTowerType == TowerType.iron || ThisTowerType == TowerType.elec)
        {
            _range = previewLevelInfoPenal.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
            _damage = previewLevelInfoPenal.transform.GetChild(2).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
            _speed = previewLevelInfoPenal.transform.GetChild(2).GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>();
            _towerName = previewLevelInfoPenal.transform.GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>();
            _rangeParent = previewLevelInfoPenal.transform.GetChild(2).GetChild(0);
        }
        else
        {
            _towerName = previewLevelInfoPenal.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
            _discription = previewLevelInfoPenal.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();
        }
        

    }

    private void Update()
    {
        //print((outlineGbj));
        if (CurNodeDataSummary._instance.dictionaryFinish == true && _finish == false && GlobalVar.CurrentGameState == GlobalVar.GameState.Viewing)
        {
            _finish = true;
            //print(transform.parent);
            _grade = transform.parent.GetComponent<TowerDataInit>().towerGrade;
            if (ThisTowerType == TowerType.wood)
            {
                GameObject woodRange = Instantiate(rangeList[0], _rangeParent.GetChild(0));
            }
            else if(ThisTowerType == TowerType.iron)
            {
                GameObject ironRange = Instantiate(rangeList[1], _rangeParent.GetChild(1));
            }

            string _a, _s, _r;
            if (ThisTowerType == TowerType.wood)
            {
                (_a, _s, _r) = CurNodeDataSummary._instance.CheckAttackSpeedRange("wood", _grade);
                _towerName.text = "Wood Tower Level " + _grade.ToString();
                _damage.text = _a;
                _speed.text = _s;
                _range.text = "3x3";
            }
            else if (ThisTowerType == TowerType.iron)
            {
                (_a, _s, _r) = CurNodeDataSummary._instance.CheckAttackSpeedRange("iron", _grade);
                _towerName.text = "Iron Tower Level " + _grade.ToString();
                _damage.text = _a;
                _speed.text = _s;
                _range.text = "full map";
            }
            else if (ThisTowerType == TowerType.elec)
            {
                (_a, _s, _r) = CurNodeDataSummary._instance.CheckAttackSpeedRange("elec", _grade);
                _towerName.text = "Electric Tower Level " + _grade.ToString();
                _damage.text = _a;
                _speed.text = _s;
                if (_r == "10")
                {
                    _range.text = "3x4";
                    Instantiate(rangeList[2], _rangeParent.GetChild(2));
                }
                else if (_r == "28")
                {
                    _range.text = "5x6";
                    Instantiate(rangeList[3], _rangeParent.GetChild(2));
                }
                else
                {
                    _range.text = "7x8";
                    Instantiate(rangeList[4], _rangeParent.GetChild(2));
                }
            }
            else if (ThisTowerType == TowerType.wpro)
            {
                _towerName.text = "Wood Protector Level "+_grade.ToString();
                _discription.text = "Increase wood tower's resistance to debuffs by " + _grade.ToString() + "%";
            }
            else if (ThisTowerType == TowerType.ipro)
            {
                _towerName.text = "Iron Protector Level "+_grade.ToString();
                _discription.text = "Increase iron tower's resistance to debuffs by " + _grade.ToString() + "%";
            }
            else if (ThisTowerType == TowerType.epro)
            {
                _towerName.text = "Electric Protector Level "+_grade.ToString();
                _discription.text = "Increase electric tower's resistance to debuffs by " + _grade.ToString() + "%";
            }
        }

        if (Input.GetMouseButtonDown(0)&&mouseEnter ==true)
        {
            previewLevelInfoPenal.gameObject.SetActive(true);
            _canDisappear = false;
        }
        

    }

    private Transform FindChildWithTag(Transform parent, string tagString)
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
    private void OnMouseEnter()
    {
        mouseEnter = true;
    }
    private void OnMouseExit()
    {
        mouseEnter = false;
    }

    public void ClosePenal()
    {
        _canDisappear = true;
        previewLevelInfoPenal.gameObject.SetActive(false);
    }
}
