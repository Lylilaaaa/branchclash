using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class HintPoolScrollToPos : MonoBehaviour
{
    public int curHintNum;
    public Button crossBut;
    public bool finishHint;

    private int _maxHintNum;
    private int _previousHintNum;
    
    private void Awake()
    {
        curHintNum = 0;
        _maxHintNum = transform.childCount-1;
        _previousHintNum = curHintNum;
        _setcertainHint(0);
        crossBut.gameObject.SetActive(false);
        finishHint = false;
    }

    private void _setcertainHint(int hintNum)
    {
        for (int i = 0; i < _maxHintNum+1; i++)
        {
            if (i == hintNum)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (curHintNum != _previousHintNum)
        {
            //Destroy(transform.GetChild(_previousHintNum));
            _setcertainHint(curHintNum);
            _previousHintNum = curHintNum;
        }
        
        if (curHintNum == _maxHintNum && finishHint == false)
        {
            crossBut.gameObject.SetActive(true);
            finishHint = true;
        }
    }


    public void ScrollRight()
    {
        if (_previousHintNum < _maxHintNum)
        {
            curHintNum += 1;
        }
    }
    public void ScrollLeft()
    {
        if (_previousHintNum > 0)
        {
            curHintNum -= 1;
        }
        
    }

}
