using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameReloader : MonoBehaviour
{
    public Button endButton;

    private void Start()
    {
        // 给按钮添加点击事件的监听器
        endButton.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        GlobalVar._instance.ReStartTree();
    }
}

