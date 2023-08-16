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
        // ����ť��ӵ���¼��ļ�����
        endButton.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        GlobalVar._instance.ReStartTree();
    }
}

