using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClikField : MonoBehaviour
{
    private Collider selectedCollider;
    private GameObject posParent;
    private FieldInit _fieldInit;
    public bool isElecTwice;

    public string towerType;
    
    // Start is called before the first frame update
    void Start()
    {
        selectedCollider = transform.GetComponent<BoxCollider>();
        posParent = transform.parent.gameObject;
        _fieldInit = posParent.GetComponent<FieldInit>();
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider == selectedCollider)
                {
                    towerType = _fieldInit.towerType;
                    if (towerType == "wood")
                    {
                        _fieldInit.woodType = 1;
                        _fieldInit.ironType = 0;
                        _fieldInit.eleType = 0;
                        _fieldInit.selected = false;
                    }
                    else if (towerType == "iron")
                    {
                        _fieldInit.woodType = 0;
                        _fieldInit.ironType = 1;
                        _fieldInit.eleType = 0;
                        _fieldInit.selected = false;
                    }
                    else if (towerType == "elec")
                    {
                        if (isElecTwice == false)
                        {
                            _fieldInit.woodType = 0;
                            _fieldInit.ironType = 0;
                            _fieldInit.eleType = 1;
                            _fieldInit.eleType2 = 0;
                            _fieldInit.selected = false;
                        }
                        else
                        {
                            _fieldInit.woodType = 0;
                            _fieldInit.ironType = 0;
                            _fieldInit.eleType = 0;
                            _fieldInit.eleType2 = 1;
                            _fieldInit.selected = false;
                        }
                    }
                }
            }
        }
    }
}
