using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class AddPanalManager : MonoBehaviour
{
    public static AddPanalManager _instance;
    public GameObject displayTower;
    public GameObject displayProtector;
    
    private GameObject _towerPanal;
    private GameObject _displayTexture;
    private GameObject _protectorPanal;
    private GameObject _towBG;
    private GameObject _proBG;
    private TextMeshProUGUI _towerTMP;
    private TextMeshProUGUI _proteTMP;
    // private ColorBlock _towerBgColo;
    // private ColorBlock _proteBgColo;
    private Color transparent;
    private Vector3 displayRect;
    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        _towerTMP = transform.GetChild(0).GetChild(4).GetComponentInChildren<TextMeshProUGUI>();
        _proteTMP = transform.GetChild(0).GetChild(5).GetComponentInChildren<TextMeshProUGUI>();
        // _towerBgColo = transform.GetChild(0).GetChild(2).GetComponentInChildren<Button>().colors;
        // _proteBgColo = transform.GetChild(0).GetChild(3).GetComponentInChildren<Button>().colors;
        _towBG = transform.GetChild(0).GetChild(0).gameObject;
        _proBG = transform.GetChild(0).GetChild(1).gameObject;
        _displayTexture = transform.GetChild(0).GetChild(6).gameObject;
        _towerPanal = transform.GetChild(1).gameObject;
        _protectorPanal = transform.GetChild(2).gameObject;
        transparent = new Color(255, 255, 255, 0);
        displayTower.SetActive(true);
        displayProtector.SetActive(false);
        _towerPanal.SetActive(true);
        _protectorPanal.SetActive(false);
        _towBG.SetActive(true);
        _proBG.SetActive(false);
        _towerTMP.color = Color.white;
        _proteTMP.color = Color.grey;
        transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
        transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
        // _towerBgColo.normalColor = Color.white;
        // _proteBgColo.normalColor = transparent;
        displayRect = _displayTexture.GetComponent<RectTransform>().localPosition;
    }

    public void changeTowPro(string towPro)
    {
        //print("button pressed");
        if (towPro == "tower")
        {
            displayTower.SetActive(true);
            displayProtector.SetActive(false);
            _towerPanal.SetActive(true);
            _protectorPanal.SetActive(false);
            _towBG.SetActive(true);
            _proBG.SetActive(false);
            // _towerBgColo.normalColor = Color.white;
            // _proteBgColo.normalColor = transparent;
            _towerTMP.color = Color.white;
            _proteTMP.color = Color.grey;
            if (_displayTexture.GetComponent<RectTransform>().localPosition != displayRect)
            {
                _displayTexture.GetComponent<RectTransform>().localPosition -= new Vector3(55f, 0, 0);
            }

            transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
            transform.GetChild(0).GetChild(3).gameObject.SetActive(true);

        }
        else if(towPro == "protect")
        {
            displayTower.SetActive(false);
            displayProtector.SetActive(true);
            _towerPanal.SetActive(false);
            _protectorPanal.SetActive(true);
            _towBG.SetActive(false);
            _proBG.SetActive(true);
            // _proteBgColo.normalColor = Color.white;
            // _towerBgColo.normalColor = transparent;
            _proteTMP.color = Color.white;
            _towerTMP.color = Color.grey;
            _displayTexture.GetComponent<RectTransform>().localPosition += new Vector3(55f, 0, 0);
            transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
            transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
        }
        else
        {  
            Debug.Log("Error in changeTowPro!");
        }
    }

    // Update is called once per frame  
    public void closeAddPanal()
    {
        if (GlobalVar._instance.GetState() == GlobalVar.GameState.AddTowerUI)
        {
            //transform.gameObject.SetActive(false);
            GlobalVar._instance.ChangeState("ChooseField");
        }
    }
}
