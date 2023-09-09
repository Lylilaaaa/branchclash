using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowUserMoney : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;
    // Start is called before the first frame update
    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        textMeshPro.text = "Money: "+ GlobalVar._instance.chosenNodeData.money;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
