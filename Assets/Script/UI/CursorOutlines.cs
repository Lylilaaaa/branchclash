using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorOutlines : MonoBehaviour
{
    private GameObject outlineGbj;

    public bool mouseEnter;

    public bool cursorZoomIn=false;
    // Start is called before the first frame update
    void Start()
    {
        mouseEnter = false;
        outlineGbj = FindChildWithTag(transform, "outline").gameObject;
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
            if (Input.GetMouseButtonDown(0))
            {
                cursorZoomIn = true;
            }
        }
        if (cursorZoomIn == true)
        {
            CameraController._instance.LookUpNode(transform);
            CameraController._instance.canMove = true;
           // StartCoroutine(ChangeVariableAfterDelay());
            cursorZoomIn = false;
        }
    }
    private IEnumerator ChangeVariableAfterDelay()
    {
        yield return new WaitForSeconds(1f);

        CameraController._instance.canMove = false; 
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
