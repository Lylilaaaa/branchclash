using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BroadCastURL : MonoBehaviour
{
    public string TwitterURL;

    public string ZeroGasURL;
    public TextMeshProUGUI zeroGasText; 

    public string ConfrimURL;
    public TextMeshProUGUI confirmText; 
    
    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(1).gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNetWorkURL()
    {
        if (string.IsNullOrEmpty(TwitterURL))
        {
            return;
        }
        Application.OpenURL(TwitterURL);
    }

    public void SetZeroGasURL()
    {
        if (string.IsNullOrEmpty(ZeroGasURL))
        {
            return;
        }
        Application.OpenURL(ZeroGasURL);
    }

    public void SetConfirmURL()
    {
        if (string.IsNullOrEmpty(ConfrimURL))
        {
            return;
        }
        Application.OpenURL(ConfrimURL);
    }

    public void QuitBroadCast()
    {
        transform.GetChild(1).gameObject.SetActive(false);
    }

    public void OpenBroadCast()
    {
        transform.GetChild(1).gameObject.SetActive(true);
    }
}
