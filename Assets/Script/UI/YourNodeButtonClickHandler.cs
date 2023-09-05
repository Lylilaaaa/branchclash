using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YourNodeButtonClickHandler : MonoBehaviour
{
    private Button button;
    private string parameter;
    public HomePageSelectUI _HomePageSelectUI;

    public void ReStart()
    {
        _HomePageSelectUI = transform.parent.parent.parent.parent.parent.parent.parent.GetComponent<HomePageSelectUI>();
        parameter = transform.parent.name;
        button = GetComponent<Button>();
        if (GlobalVar._instance.role == 0)
        {
            button.onClick.AddListener(() => _HomePageSelectUI.ZoomX_X(parameter));
        }
        else if (GlobalVar._instance.role == 1)
        {
            button.onClick.AddListener(() => _HomePageSelectUI.ZoomX_X_down(parameter));
        }
        
    }
}
