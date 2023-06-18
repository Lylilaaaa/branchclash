using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewingSceneGlobal : MonoBehaviour
{
    public Camera viewingCameraMain;

    public GameObject viewingEditUI;

    private bool _hasSet=false;
    // Start is called before the first frame update
    void Start()
    {
        //viewingCameraMain = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalVar._instance.GetState() == GlobalVar.GameState.Viewing && _hasSet == false)
        {
            viewingCameraMain.gameObject.SetActive(false);
            viewingEditUI.SetActive(false);
            _hasSet = true;
        }
    }
}
