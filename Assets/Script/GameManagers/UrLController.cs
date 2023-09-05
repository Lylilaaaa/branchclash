using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UrLController : MonoBehaviour
{
    public static UrLController _instance;
    
    public string url_checkAll = "8.219.216.37/checkAll.php";
    public string url_insert =  "8.219.216.37/insert.php";
    public string url_search = "8.219.216.37/search.php";
    public string url_clear = "8.219.216.37/clear.php";
    
    public string url_checkAllSec = "8.219.216.37/checkAllSec.php";
    public string url_insertSec =  "8.219.216.37/insertSec.php";
    public string url_searchSec = "8.219.216.37/searchSec.php";
    public string url_clearSec = "8.219.216.37/clearSec.php";

    //Remember to Clear!!!!!!!
    public string upTreeResult;
    public string downTreeResult;

    public TextMeshProUGUI t;
    
    
    private string _map = "00,H,00,00,00,00,00,00,00,00,00,00,00,00,00,00,R,00,/n,00,R,00,00,R,R,R,R,00,00,R,R,R,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,R,R,R,00,00,R,R,R,R,00,00,R,R,R,R,00,/n,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,/";
    private string _info;
    private string _infoDown;
    private void Awake()
    {
        _instance = this; 
        _info = "0xfd376a919b9a1280518e9a5e29e3c3637c9faa12-20230905-0-0-0-0-1-1-10000-10000-1500-"+_map+"-0,0,0";
        _infoDown = "0xfd376a919b9a1280518e9a5e29e3c3637c9faa12-20230905-0-0-0-0-1-1-1,0,0";
        upTreeResult = "";
        downTreeResult = "";
        t.text = "";
    }

    public void initData()
    {
        StartCoroutine(initAfterClearUp());
        StartCoroutine(initAfterClearDown());
    }
    IEnumerator initAfterClearUp()
    {
        t.text+="\n"+url_clear;
        WWW www = new WWW(url_clear);
        yield return www;
        string result = www.text;
        t.text+="\n"+result;
        InsertUpNode(0, 1, 0, _info, "0xfd376a919b9a1280518e9a5e29e3c3637c9faa12", "000");
    }
    IEnumerator initAfterClearDown()
    {
        t.text+="\n"+url_clearSec;
        WWW www = new WWW(url_clearSec);
        yield return www;
        string result = www.text;
        t.text+="\n"+result;
        InsertDownNode(0, 1, 0, _infoDown, "0xfd376a919b9a1280518e9a5e29e3c3637c9faa12");
    }

    public void InsertUpNode(int layer, int idx, int father, string info, string creator, string debuff)
    {

        StartCoroutine(insert(layer,idx,father,info,creator,debuff));
    }
    
    public void InsertDownNode(int layer, int idx, int father, string info, string creator)
    {
        StartCoroutine(insertSec(layer,idx,father,info,creator));
    }
    public void InsertUpNodeTest()
    {
        StartCoroutine(insert(1,3,0,"text","lyy","123"));
    }
    public void InsertDownNodeTest()
    {
        StartCoroutine(insertSec(0,1,0,"text","lyy"));
    }

    public void ClearUpData()
    {
        StartCoroutine(clearUpData());
    }
    
    public void ClearDownData()
    {
        StartCoroutine(clearDownData());
    }
    public void CheckUpNode11()
    {
        StartCoroutine(checkUpNode(0,1));
    }
    public void CheckDownNode11()
    {
        StartCoroutine(checkDownNode(0,1));
    }

    public void CheckUpNode(int layer, int index)
    {
        StartCoroutine(checkUpNode(layer,index));
    }
    public void CheckDownNode(int layer, int index)
    {
        StartCoroutine(checkDownNode(layer,index));
    }
    
    public void CheckAllUpNode()
    {
        StartCoroutine(checkAllUpLayerIdx());
    }
    public void CheckAllDownNode()
    {
        StartCoroutine(checkAllDownLayerIdx());
    }

    
    IEnumerator insertSec(int layer, int idx, int father, string info, string creator)
    {
        string insertString;
        insertString = url_insertSec + "?layer=" + layer.ToString() + "&idx=" + idx.ToString() + "&father=" + father.ToString() +
                       "&info=" + info.ToString() + "&creator=" + creator.ToString();
       //t.text+="\n"+insertString;
        WWW www = new WWW(insertString);
        yield return www;
        string result = www.text;
        t.text+="\n"+result;
    }

    IEnumerator insert(int layer, int idx, int father, string info, string creator, string debuff)
    {
        string insertString;
        insertString = url_insert + "?layer=" + layer.ToString() + "&idx=" + idx.ToString() + "&father=" + father.ToString() +
                       "&info=" + info.ToString() + "&creator=" + creator.ToString() + "&debuff=" + debuff.ToString();
        //t.text+="\n"+insertString;
        WWW www = new WWW(insertString);
        yield return www;
        string result = www.text;
        t.text+="\n"+result;
    }
    IEnumerator clearUpData()
    {
        Debug.Log(url_clear);
        WWW www = new WWW(url_clear);
        yield return www;
        string result = www.text;
        Debug.Log(result);
    }
    IEnumerator clearDownData()
    {
        Debug.Log(url_clearSec);
        WWW www = new WWW(url_clearSec);
        yield return www;
        string result = www.text;
        Debug.Log(result);
    }
    
    public IEnumerator checkUpNode(int layer, int idx)
    {
        string checkString;
        checkString = url_search + "?layer=" + layer.ToString() + "&idx=" + idx.ToString();
        Debug.Log(checkString);
        t.text+="\n"+checkString;
        WWW www = new WWW(checkString);
        yield return www;
        string result = www.text;
        Debug.Log(result);
        t.text+="\n"+result;
    }
    public IEnumerator checkDownNode(int layer, int idx)
    {
        string checkString;
        checkString = url_searchSec + "?layer=" + layer.ToString() + "&idx=" + idx.ToString();
        t.text+="\n"+checkString;
        WWW www = new WWW(checkString);
        yield return www;
        string result = www.text;
        t.text+="\n"+result;
        Debug.Log(result);
    }
    
    IEnumerator checkAllUpLayerIdx()
    {
        t.text += url_checkAll;
        WWW www = new WWW(url_checkAll);
        yield return www;
        string result = www.text;
        Debug.Log(result);
        t.text += "\n"+result;
        upTreeResult = result;
    }
    
    IEnumerator checkAllDownLayerIdx()
    {
        t.text +="\n"+ url_checkAllSec;
        Debug.Log(url_checkAllSec);
        WWW www = new WWW(url_checkAllSec);
        yield return www;
        string result = www.text;
        Debug.Log(result);
        t.text +="\n"+ result;
        downTreeResult = result;
    }
}
