using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMajor : MonoBehaviour
{
    string url = "119.91.132.241/checkMajor.php";
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(rInfo(2));
    }



    IEnumerator rInfo(int layer)
    {
        url = url + "?layer=" + layer.ToString();
        Debug.Log(url);
        WWW www = new WWW(url);
        yield return www;
        string result = www.text;
        Debug.Log(result);
    }
}
