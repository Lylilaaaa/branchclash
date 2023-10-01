using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;

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

    public GameObject loadingGameObj;
    private void Awake()
    {
        // thisNetWorkChain = "Polygon";
        // url_checkAll = "8.219.216.37/" + thisNetWorkChain + "/checkAll.php";
        // url_insert = "8.219.216.37/" + thisNetWorkChain + "/insert.php";
        // url_search ="8.219.216.37/" + thisNetWorkChain + "/search.php";
        // url_clear = "8.219.216.37/" + thisNetWorkChain + "/clear.php";
        // url_checkAllSec = "8.219.216.37/" + thisNetWorkChain + "/checkAllSec.php";
        // url_insertSec =  "8.219.216.37/" + thisNetWorkChain + "/insertSec.php";
        // url_searchSec = "8.219.216.37/" + thisNetWorkChain + "/searchSec.php";
        // url_clearSec = "8.219.216.37/" + thisNetWorkChain + "/clearSec.php";
        
        _instance = this; 
        _info = "0xfd376a919b9a1280518e9a5e29e3c3637c9faa12-0-0-0-0-0-1-1-10000-10000-1500-"+_map+"-0,0,0";
        _infoDown = "0xfd376a919b9a1280518e9a5e29e3c3637c9faa12-0-0-0-0-0-1-1-0,0,0";
        upTreeResult = "";
        downTreeResult = "";
        t.text = "";
    }

    public void ChangeServeDataSet(string chainNet)
    {
        // if (chainNet == "Sepolia")
        // {
        //     chainNet = "Polygon";
        // }
        url_checkAll = "8.219.216.37/" + chainNet + "/checkAll.php";
        url_insert = "8.219.216.37/" + chainNet + "/insert.php";
        url_search ="8.219.216.37/" + chainNet + "/search.php";
        url_clear = "8.219.216.37/" + chainNet + "/clear.php";
        url_checkAllSec = "8.219.216.37/" + chainNet + "/checkAllSec.php";
        url_insertSec =  "8.219.216.37/" + chainNet + "/insertSec.php";
        url_searchSec = "8.219.216.37/" + chainNet + "/searchSec.php";
        url_clearSec = "8.219.216.37/" + chainNet + "/clearSec.php";
        Debug.Log("change URL to: "+chainNet);
        Debug.Log("For instance, the checkAll link is: "+url_checkAll);
    }

    private void Start()
    {
        //default
        thisNetWorkChain = "Sepolia";
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
        _infoDown = "0xfd376a919b9a1280518e9a5e29e3c3637c9faa12-0-0-0-0-0-1-1-0,0,0";
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
    public void ClearUpData()
    {
        StartCoroutine(clearUpData());
    }
    public void ClearDownData()
    {
        StartCoroutine(clearDownData());
    }
    public void InsertUpNode(int layer, int idx, int father, string info, string creator, string debuff)
    {
        //loadingGameObj.SetActive(true);
        StartCoroutine(insert(layer,idx,father,info,creator,debuff));
    }
    
    public void InsertDownNode(int layer, int idx, int father, string info, string creator)
    {
        StartCoroutine(insertSec(layer,idx,father,info,creator));
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
        loadingGameObj.SetActive(true);
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
        while (!www.isDone)
        {
            loadingGameObj.transform.GetChild(1).GetComponent<Slider>().value = www.uploadProgress/2;
            yield return null;
        }
        //yield return www;
        loadingGameObj.transform.GetChild(1).GetComponent<Slider>().value = 0.5f;
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
        while (!www.isDone)
        {
            loadingGameObj.transform.GetChild(1).GetComponent<Slider>().value = www.uploadProgress/2+0.5f;
            //Debug.Log("Upload Progress(checking all down nodes): " + www.uploadProgress/2+0.5f);
            yield return null;
        }
        //yield return www;
        loadingGameObj.transform.GetChild(1).GetComponent<Slider>().value = 1;
        //loadingGameObj.SetActive(false);
        string result = www.text;
        Debug.Log(result);
        t.text +="\n"+ result;
        downTreeResult = result;
       
        GlobalVar._instance.t.text += "\n finish check all down index from serve!";
    }
    
}
