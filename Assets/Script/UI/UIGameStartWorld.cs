using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIGameStartWorld : MonoBehaviour
{
    private Collider _startCollider;

    public WaveSpawner monsterStart;

    public TowerBuilder closeSelected;
    private bool chosen;
    public bool finish;

    public GameObject _doorTMP;
    public GameObject _doorMat;
    public GameObject _doorKuang;
    public Color _emissionColor;
    public Color _realRed;
    public Material Metal;
    public Material lightMetal;
    public Material redDoor;
    public Material lightRedDoor;
    
    // Start is called before the first frame update
    void Start()
    {
        _startCollider = GetComponent<Collider>();
        _doorTMP = transform.GetChild(0).gameObject;
        _doorMat = transform.GetChild(1).gameObject;
        _doorKuang = transform.GetChild(2).gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        if (chosen == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                print("start!");
                StartCoroutine(checkNowNodeIndex());
            }
        }
        // if (GlobalVar._instance.finishEdit==true && finish == false)
        // {
        //     finish = true;
        //     //closeSelected.CloseSelected();
        //     monsterStart.startGame();
        //     Destroy(gameObject);
        // }
    }

    IEnumerator checkNowNodeIndex()
    {
        ContractInteraction._instance.nowNodeIndex = "";
        ContractInteraction._instance.CheckLevelPr();
        while (ContractInteraction._instance.nowNodeIndex == "")
        {
            yield return null;
        }
        GlobalVar._instance.nowNodeIndex = ContractInteraction._instance.nowNodeIndex;
        StartCoroutine(checkOldNodeIndex());
    }
    IEnumerator checkOldNodeIndex()
    {
        ContractInteraction._instance.oldNodeIndex = "";
        ContractInteraction._instance.CheckLevelOrGetName();
        while (ContractInteraction._instance.oldNodeIndex == "")
        {
            yield return null;
        }
        GlobalVar._instance.oldNodeIndex = ContractInteraction._instance.oldNodeIndex;
        StartCoroutine(checkOldBlood());
    }

    IEnumerator checkOldBlood()
    {
        string pureString = GlobalVar._instance.oldNodeIndex.Substring(1, GlobalVar._instance.oldNodeIndex.Length - 2);
        string[]layerIndex =  pureString.Split(",");
        GlobalVar._instance.oldNodeIndexBlood = String.Empty;
        ContractInteraction._instance.check_blood_old( int.Parse(layerIndex[0]) , int.Parse(layerIndex[1]));
        while (GlobalVar._instance.oldNodeIndexBlood == String.Empty)
        {
            yield return null;
        }
        StartGameStart();
    }


    public void StartGameStart()
    {
        StartCoroutine(_gameStart());
    }

    public IEnumerator _gameStart()
    {
        ContractInteraction._instance.Submit();
        while (!ContractInteraction._instance.finishSubmit)
        {
            yield return null;
        }
        monsterStart.startGame();
        GlobalVar._instance.ChangeState("GamePlay");
        Destroy(gameObject);
        chosen = false;
    }
    
    
    
    
    private void OnMouseEnter()
    {
        //print("mouseEnter!");
        _doorTMP.GetComponent<TextMeshProUGUI>().color = _emissionColor;
        _doorMat.GetComponent<Renderer>().material = lightRedDoor;
        _doorKuang.GetComponent<Renderer>().material = lightMetal;
        chosen = true;

    }
    private void OnMouseExit()
    {
        //print("mouseExit!");
        _doorTMP.GetComponent<TextMeshProUGUI>().color = _realRed;
        _doorMat.GetComponent<Renderer>().material = redDoor;
        _doorKuang.GetComponent<Renderer>().material = Metal;
        chosen = false;
    }
}
