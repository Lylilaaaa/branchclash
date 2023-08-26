using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SearchNodeZoom : MonoBehaviour
{
    public TextMeshProUGUI layerTmp;
    public TextMeshProUGUI indexTmp;
    public int treeType;

    public Button leftBut;
    public Button RightBut;
    
    public Sprite unRightChosenImage;
    public Sprite unLeftChosenImage;
    
    public Sprite rightChosenImage;
    public Sprite leftChosenImage;

    public TextMeshProUGUI rightButTmp;
    public TextMeshProUGUI leftButTmp;
    
    // public Game
    
    public HomePageSelectUI homePageSelectUI;
    
    

    // Update is called once per frame
    public void chooseUpTree()
    {
        treeType = 0;
        leftBut.gameObject.GetComponent<Image>().sprite = leftChosenImage;
        leftButTmp.color = Color.black;
        RightBut.gameObject.GetComponent<Image>().sprite = unRightChosenImage;
        rightButTmp.color = Color.white;
    }

    public void chooseDownTree()
    {
        treeType = 1;
        leftBut.gameObject.GetComponent<Image>().sprite = unLeftChosenImage;
        leftButTmp.color = Color.white;
        RightBut.gameObject.GetComponent<Image>().sprite = rightChosenImage;
        rightButTmp.color = Color.black;
    }

    public void Search()
    {
        if (treeType == 0)
        {
            string _name = layerTmp.text + ',' + indexTmp.text;
            homePageSelectUI.ZoomX_X(_name);
        }
        else if (treeType == 1)
        {
            string _name = layerTmp.text + ',' + indexTmp.text;
            homePageSelectUI.ZoomX_X_down(_name);
        }

    }
}
