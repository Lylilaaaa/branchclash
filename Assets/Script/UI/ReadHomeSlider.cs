using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadHomeSlider : MonoBehaviour
{
    private Slider home;
    // Start is called before the first frame update
    void Start()
    {
        home = transform.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        home.minValue = 0;
        home.maxValue = CurNodeDataSummary._instance.thisNodeData.fullHealth;
        home.value = CurNodeDataSummary._instance.thisNodeData.curHealth;
    }
}
