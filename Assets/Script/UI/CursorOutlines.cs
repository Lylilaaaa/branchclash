using System;
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
        outlineGbj = FindChildWithTag(transform, "outline").gameObject;
        int childNum = transform.childCount;
        outlineGbj = transform.GetChild(childNum - 1).gameObject;
        outlineGbj.SetActive(false);
    }
    private Transform FindChildWithTag(Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
            {
                return child;
            }

            Transform result = FindChildWithTag(child, tag);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }

    private void Update()
    {
        if (mouseEnter == true)
        {
            CameraController._instance.LookUpNode(transform);
        }
        else
        {
            CameraController._instance.isMoving = false;
        }
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
