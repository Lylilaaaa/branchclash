using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReadHomeBlood : MonoBehaviour
{
    private TextMeshProUGUI home;
    // Start is called before the first frame update
    void Start()
    {
        home = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        home.text = CurNodeDataSummary._instance.thisNodeData.curHealth.ToString()+"\\"+CurNodeDataSummary._instance.thisNodeData.fullHealth.ToString();
    }
}
