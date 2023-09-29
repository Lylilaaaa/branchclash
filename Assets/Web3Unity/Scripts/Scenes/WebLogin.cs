
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#if UNITY_WEBGL
public class WebLogin : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void Web3Connect();

    [DllImport("__Internal")]
    private static extern string ConnectAccount();

    [DllImport("__Internal")]
    private static extern void SetConnectAccount(string value);
    
    [DllImport("__Internal")]
    private static extern void ChangeNetwork(int value);

    private int expirationTime;
    private string account;
    ProjectConfigScriptableObject projectConfigSO = null;
    
    public int chain = 11155111;
    
    void Start()
    {
        // loads the data saved from the editor config
        ChangeNetwork(4000003);
        PlayerPrefs.SetString("ProjectID", projectConfigSO.ProjectID);
        PlayerPrefs.SetString("ChainID", "4000003");
        PlayerPrefs.SetString("Chain", "zero");
        PlayerPrefs.SetString("Network", "mainnet");
        PlayerPrefs.SetString("RPC","https://zero.alt.technology/");
        Debug.Log("change to AltLayer");
        ContractInteraction._instance.changeNetWork("AltLayer");
        
    }

    public void ChooseToChangeLine(string netWorkName)
    {
        if (netWorkName == "opBNB")
        {
            ChangeNetwork(5611);
            PlayerPrefs.SetString("ProjectID", projectConfigSO.ProjectID);
            PlayerPrefs.SetString("ChainID", "5611");
            PlayerPrefs.SetString("Chain", "opBNB");
            PlayerPrefs.SetString("Network", "Testnet");
            PlayerPrefs.SetString("RPC", "https://opbnb-testnet-rpc.bnbchain.org");
           
            Debug.Log("change to opBNB");
        }
        else if (netWorkName == "Sepolia")
        {
            ChangeNetwork(11155111);
            PlayerPrefs.SetString("ProjectID", projectConfigSO.ProjectID);
            PlayerPrefs.SetString("ChainID", "11155111");
            PlayerPrefs.SetString("Chain", "ethereum");
            PlayerPrefs.SetString("Network", "sepolia");
            PlayerPrefs.SetString("RPC","https://endpoints.omniatech.io/v1/eth/sepolia/public");
            
            Debug.Log("change to Sepolia");
        }
        else if (netWorkName == "Polygon")
        {
            ChangeNetwork(137);
            PlayerPrefs.SetString("ProjectID", projectConfigSO.ProjectID);
            PlayerPrefs.SetString("ChainID", "137");
            PlayerPrefs.SetString("Chain", "polygon");
            PlayerPrefs.SetString("Network", "mainnet");
            PlayerPrefs.SetString("RPC","https://polygon-rpc.com/");
            Debug.Log("change to Polygon");
        }
        else if (netWorkName == "AltLayer")
        {
            ChangeNetwork(4000003);
            PlayerPrefs.SetString("ProjectID", projectConfigSO.ProjectID);
            PlayerPrefs.SetString("ChainID", "4000003");
            PlayerPrefs.SetString("Chain", "zero");
            PlayerPrefs.SetString("Network", "mainnet");
            PlayerPrefs.SetString("RPC","https://zero.alt.technology/");
            Debug.Log("change to AltLayer");
        }
        
        ContractInteraction._instance.changeNetWork(netWorkName);
    }

    public void OnLogin()
    {
        
        Debug.Log("Start login");
        Web3Connect();
        OnConnected();
    }

    async private void OnConnected()
    {
        account = ConnectAccount();
        while (account == "")
        {
            await new WaitForSeconds(1f);
            account = ConnectAccount();
        };
        // save account for next scene
        
        PlayerPrefs.SetString("Account", account);
        print(account);
        // reset login message
        SetConnectAccount("");
        // load next scene
        ContractInteraction._instance.getAccount();
        GlobalVar._instance.thisUserAddr = PlayerPrefs.GetString("Account");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnSkip()
    {
        // burner account for skipped sign in screen
        PlayerPrefs.SetString("Account", "");
        // move to next scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
#endif
