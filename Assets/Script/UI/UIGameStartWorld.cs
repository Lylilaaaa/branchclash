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
                closeSelected.CloseSelected();
                monsterStart.startGame();
            }
        }

    }
    private void OnMouseEnter()
    {
        print("mouseEnter!");
        _doorTMP.GetComponent<TextMeshProUGUI>().color = _emissionColor;
        _doorMat.GetComponent<Renderer>().material = lightRedDoor;
        _doorKuang.GetComponent<Renderer>().material = lightMetal;
        chosen = true;

    }
    private void OnMouseExit()
    {
        print("mouseExit!");
        _doorTMP.GetComponent<TextMeshProUGUI>().color = _realRed;
        _doorMat.GetComponent<Renderer>().material = redDoor;
        _doorKuang.GetComponent<Renderer>().material = Metal;
        chosen = false;
    }
}
