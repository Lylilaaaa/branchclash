using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropdownChainChangeName : MonoBehaviour
{
    public TMP_Dropdown dropD;

    //private bool setOnce;
    // Start is called before the first frame update
    void Start()
    {
        //setOnce = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.name == "Dropdown List")
        {
            dropD.options[0].text = "Polygon";
            // if (setOnce == false)
            // {
            //     setOnce = true;
            //     dropD.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Polygon";
            // }
            transform.GetChild(0).GetChild(0).GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().text = "Polygon";
        }
    }
    
}
