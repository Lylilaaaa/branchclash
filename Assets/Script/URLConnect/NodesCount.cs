using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodesCount : MonoBehaviour
{
    string url = "119.91.132.241/nodesCount.php";
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Count());
    }



    IEnumerator Count()
    {
        Debug.Log(url);
        WWW www = new WWW(url);
        yield return www;
        string result = www.text;
        Debug.Log(result);
    }
}
