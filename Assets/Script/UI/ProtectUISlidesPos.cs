using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProtectUISlidesPos : MonoBehaviour
{
    private Vector3 _debuffStartLocalPos;
    private Vector3 _debuffMidPtLocalPos;
    private TextMeshProUGUI debuffData;
    private TextMeshProUGUI protectData;
    void Start()
    {
        debuffData = transform.GetChild(0).GetChild(1).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        protectData = transform.GetChild(0).GetChild(1).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        _debuffStartLocalPos = transform.parent.GetChild(1).localPosition;
        _debuffMidPtLocalPos = transform.parent.GetChild(0).localPosition;
        transform.localPosition = _debuffMidPtLocalPos - _debuffStartLocalPos + _debuffMidPtLocalPos;
        if (transform.parent.parent.GetComponent<Slider>().maxValue == 0)
        {
            debuffData.text = "-0%";
        }
        else
        {
            debuffData.text =
                        "-"+((transform.parent.parent.GetComponent<Slider>().value /
                         transform.parent.parent.GetComponent<Slider>().maxValue) * 100).ToString() + "%";
        }

        if (transform.GetChild(0).GetComponent<Slider>().maxValue == 0)
        {
            protectData.text = "+0%";
        }
        else
        {
            protectData.text =
                        "+"+((transform.GetChild(0).GetComponent<Slider>().value /
                          transform.GetChild(0).GetComponent<Slider>().maxValue) * 100).ToString() + "%";
        }
    }
}
