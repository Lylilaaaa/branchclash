using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadInfo : MonoBehaviour
{
    string url = "119.91.132.241/search.php";
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(rInfo(1, 1));
    }

   

    IEnumerator rInfo(int layer, int idx)
    {
        url = url + "?layer=" + layer.ToString() + "&idx=" + idx.ToString();
        Debug.Log(url);
        WWW www = new WWW(url);
        yield return www;
        string result = www.text;
        Debug.Log(result);
    }
}
