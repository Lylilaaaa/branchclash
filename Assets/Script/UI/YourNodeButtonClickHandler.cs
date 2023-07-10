using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YourNodeButtonClickHandler : MonoBehaviour
{
    private Button button;
    private string parameter;
    public HomePageSelectUI _HomePageSelectUI;

    private void Start()
    {
        _HomePageSelectUI = transform.parent.parent.parent.parent.parent.parent.GetComponent<HomePageSelectUI>();
        parameter = transform.parent.name;
        button = GetComponent<Button>();
        button.onClick.AddListener(() => _HomePageSelectUI.ZoomX_X(parameter));
    }
}
