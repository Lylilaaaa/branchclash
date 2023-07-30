using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MergeButtonInter : MonoBehaviour
{
    public TowerBuilder tb;

    private Button bt;
    // Start is called before the first frame update
    void Start()
    {
        bt = transform.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tb.targetWeaponPos.Count == 0)
        {
            bt.interactable = false;
        }
        else
        {
            bt.interactable = true;
        }
    }
}
