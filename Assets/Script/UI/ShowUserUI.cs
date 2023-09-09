using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowUserUI : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;
    // Start is called before the first frame update
    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        int stringCounted = GlobalVar._instance.thisUserAddr.Length;
        textMeshPro.text = GlobalVar._instance.thisUserAddr.Substring(0, 5) + "..." +
                           GlobalVar._instance.thisUserAddr.Substring(stringCounted - 3, 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
