using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrLController : MonoBehaviour
{
    public static UrLController _instance;
    
    string url_insert =  "8.219.216.37/insert.php";
    string url_checkMajor = "8.219.216.37/checkMajor.php";
    string url_nodesCount = "8.219.216.37/nodesCount.php";
    string url_search = "8.219.216.37/search.php";
    string url_clear = "8.219.216.37/clear.php";
    
    private void Awake()
    {
        _instance = this;
    }
    
    private void Start()
    {

    }

    public void InsertUpNode(int layer, int idx, int father, string info, string creator, string debuff)
    {
        StartCoroutine(insert(layer,idx,father,info,creator,debuff));
    }
    public void InsertDownNode(int layer, int idx, int father, string info, string creator, string debuff)
    {
        StartCoroutine(insert(layer,idx,father,info,creator,debuff));
    }

    public void ClearData()
    {
        StartCoroutine(clearData());
    }

    public void CheckUpNode(int layer, int index)
    {
        StartCoroutine(checkLayerIdx(layer,index));
    }
    public void CheckDownNode(int layer, int index)
    {
        StartCoroutine(checkLayerIdx(layer,index));
    }

    public void CheckMajor(int layer)
    {
        StartCoroutine(checkMajor(layer));
    }

    public void CountUpNode()
    {
        StartCoroutine(Count());
    }
    

    IEnumerator insert(int layer, int idx, int father, string info, string creator, string debuff)
    {
        string insertString;
        insertString = url_insert + "?layer=" + layer.ToString() + "&idx=" + idx.ToString() + "&father=" + father.ToString() +
                       "&info=" + info.ToString() + "&creator=" + creator.ToString() + "&debuff=" + debuff.ToString();
        Debug.Log(insertString);
        WWW www = new WWW(insertString);
        yield return www;
        string result = www.text;
        Debug.Log(result);
    }
    
    IEnumerator checkMajor(int layer)
    {
        string checkString;
        checkString = url_checkMajor + "?layer=" + layer.ToString();
        Debug.Log(checkString);
        WWW www = new WWW(checkString);
        yield return www;
        string result = www.text;
        Debug.Log(result);
    }
    
    IEnumerator Count()
    {
        Debug.Log(url_nodesCount);
        WWW www = new WWW(url_nodesCount);
        yield return www;
        string result = www.text;
        Debug.Log(result);
    }
    
    IEnumerator checkLayerIdx(int layer, int idx)
    {
        string checkString;
        checkString = url_search + "?layer=" + layer.ToString() + "&idx=" + idx.ToString();
        Debug.Log(checkString);
        WWW www = new WWW(checkString);
        yield return www;
        string result = www.text;
        Debug.Log(result);
    }
    
    IEnumerator clearData()
    {
        Debug.Log(url_clear);
        WWW www = new WWW(url_clear);
        yield return www;
        string result = www.text;
        Debug.Log(result);
    }
}
