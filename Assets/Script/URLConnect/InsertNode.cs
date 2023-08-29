using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsertNode : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(insert(5,1,1,"test2","test2","123"));
    }


    //insert one node into the database, normally combined with NodesCount and a for loop
    IEnumerator insert(int layer, int idx, int father, string info, string creator, string debuff)
    {
        string url = "119.91.132.241/insert.php";
        url = url + "?layer=" + layer.ToString() + "&idx=" + idx.ToString() + "&father=" + father.ToString() +
              "&info=" + info.ToString() + "&creator=" + creator.ToString() + "&debuff=" + debuff.ToString();
        Debug.Log(url);
        WWW www = new WWW(url);
        yield return www;
        string result = www.text;
        Debug.Log(result);
    }
}