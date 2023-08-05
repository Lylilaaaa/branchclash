using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameReloader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReStartTree()
    {
        UploadData();
        SceneManager.LoadScene("HomePage");
    }

    private void UploadData()
    {
        TreeNodeDataInit._instance.AddNodeData();
    }
}
