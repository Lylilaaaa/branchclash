using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorOutlines : MonoBehaviour
{
    private GameObject outlineGbj;

    public bool mouseEnter;
    // Start is called before the first frame update
    void Start()
    {
        mouseEnter = false;
        int childNum = transform.childCount;
        outlineGbj = transform.GetChild(childNum - 1).gameObject;
        outlineGbj.SetActive(false);
    }

    // Update is called once per frame
    private void OnMouseEnter()
    {
        mouseEnter = true;
        if (GlobalVar._instance.GetState() == GlobalVar.GameState.MainStart)
        {
            outlineGbj.SetActive(true);
        }

    }
    private void OnMouseExit()
    {
        mouseEnter = false;
        if (GlobalVar._instance.GetState() == GlobalVar.GameState.MainStart)
        {
            outlineGbj.SetActive(false);
        }
    }
}
