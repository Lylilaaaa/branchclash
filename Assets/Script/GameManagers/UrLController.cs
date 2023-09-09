using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class UrLController : MonoBehaviour
{
    public static UrLController _instance;
    public string thisNetWorkChain;
    public string url_checkAll;
    public string url_insert ;
    public string url_search;
    public string url_clear;
    
    public string url_checkAllSec;
    public string url_insertSec;
    public string url_searchSec ;
    public string url_clearSec;

    //Remember to Clear!!!!!!!
    public string upTreeResult;
    public string downTreeResult;

    public TextMeshProUGUI t;
    
    
    private string _map = "00,H,00,00,00,00,00,00,00,00,00,00,00,00,00,00,R,00,/n,00,R,00,00,R,R,R,R,00,00,R,R,R,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,00,R,00,/n,00,R,R,R,R,00,00,R,R,R,R,00,00,R,R,R,R,00,/n,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,/";
    private string _info;
    private string _infoDown;
    private void Awake()
    {
        thisNetWorkChain = "opBNB";
        url_checkAll = "8.219.216.37/" + thisNetWorkChain + "/checkAll.php";
        url_insert = "8.219.216.37/" + thisNetWorkChain + "/insert.php";
        url_search ="8.219.216.37/" + thisNetWorkChain + "/search.php";
        url_clear = "8.219.216.37/" + thisNetWorkChain + "/clear.php";
        url_checkAllSec = "8.219.216.37/" + thisNetWorkChain + "/checkAllSec.php";
        url_insertSec =  "8.219.216.37/" + thisNetWorkChain + "/insertSec.php";
        url_searchSec = "8.219.216.37/" + thisNetWorkChain + "/searchSec.php";
        url_clearSec = "8.219.216.37/" + thisNetWorkChain + "/clearSec.php";
        
        _instance = this; 
        _info = "0xfd376a919b9a1280518e9a5e29e3c3637c9faa12-0-0-0-0-0-1-1-10000-10000-1500-"+_map+"-0,0,0";
        _infoDown = "0xfd376a919b9a1280518e9a5e29e3c3637c9faa12-0-0-0-0-0-1-1-1,0,0";
        upTreeResult = "";
        downTreeResult = "";
        t.text = "";
    }

    private void Start()
    {
        thisNetWorkChain = "opBNB";
        url_checkAll = "8.219.216.37/opBNB/checkAll.php";
        url_insert = "8.219.216.37/opBNB/insert.php";
        url_search ="8.219.216.37/opBNB/search.php";
        url_clear = "8.219.216.37/opBNB/clear.php";
        url_checkAllSec = "8.219.216.37/opBNB/checkAllSec.php";
        url_insertSec =  "8.219.216.37/opBNB/insertSec.php";
        url_searchSec = "8.219.216.37/opBNB/searchSec.php";
        url_clearSec = "8.219.216.37/opBNB/clearSec.php";
        
        _instance = this; 
        _info = "0xfd376a919b9a1280518e9a5e29e3c3637c9faa12-0-0-0-0-0-1-1-10000-10000-1500-"+_map+"-0,0,0";
        _infoDown = "0xfd376a919b9a1280518e9a5e29e3c3637c9faa12-0-0-0-0-0-1-1-1,0,0";
        upTreeResult = "";
        downTreeResult = "";
        t.text = "";
    }

    public void initDataDown()
    {
        StartCoroutine(initAfterClearDown());
        //InsertDownNode(0, 1, 0, _infoDown, "0xfd376a919b9a1280518e9a5e29e3c3637c9faa12");
    }

    public void initDataUp()
    {
        StartCoroutine(initAfterClearUp());
    }

    IEnumerator initAfterClearUp()
    {   
        //t.text+="\n"+url_clear;
        Debug.Log(url_clear);
        WWW www = new WWW(url_clear);
        yield return www;
        string result = www.text;
        InsertUpNode(0, 1, 0, _info, "0xfd376a919b9a1280518e9a5e29e3c3637c9faa12", "000");
    }
    IEnumerator initAfterClearDown()
    {
        Debug.Log(url_clearSec);
        //t.text+="\n"+url_clearSec;
        WWW www = new WWW(url_clearSec);
        yield return www;
        string result = www.text;
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
        Debug.Log(insertString);
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
        Debug.Log(insertString);
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
        GlobalVar._instance.t.text += "\n checking all: "+url_checkAll;
        Debug.Log(url_checkAll);
        WWW www = new WWW(url_checkAll);
        yield return www;
        string result = www.text;
        Debug.Log(result);
        t.text += "\n"+result;
        upTreeResult = result;
        if (upTreeResult.Length == 0)
        {
            Debug.Log("recheck!");
            StartCoroutine(checkAllUpLayerIdx());
        }
        else
        {
            StartCoroutine(waitToCheckDownNode());
        }
    }

    IEnumerator waitToCheckDownNode()
    {
        while (upTreeResult.Length==0)
        {
            yield return null;
        }
        CheckAllDownNode();
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
       
        GlobalVar._instance.t.text += "\n finish check all down index from serve!";
    }
    IEnumerator Post(string url, Dictionary<string, string> postData)
    {
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        string data = "";
        foreach (KeyValuePair<string, string> pair in postData)
        {
            data += string.Format("{0}={1}&", pair.Key, pair.Value);
        }
        byte[] bytes = Encoding.UTF8.GetBytes(data);
        request.uploadHandler = new UploadHandlerRaw(bytes);
        request.downloadHandler = new DownloadHandlerBuffer();
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            Debug.Log(response);
        }
        else
        {
            Debug.LogError(request.error);
        }
    }
    
    IEnumerator Get(string url)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                string response = request.downloadHandler.text;
                Debug.Log(response);
            }
            else
            {
                Debug.LogError(request.error);
            }
        }
    }
}
