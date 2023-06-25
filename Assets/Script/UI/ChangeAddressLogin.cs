using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ChangeAddressLogin : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    
    void Start()
    {
    }

    // Update is called once per frame
    public void changeName()
    {
        print("hshs");
        textMeshPro.text = "0xb3a...890";
    }
}
