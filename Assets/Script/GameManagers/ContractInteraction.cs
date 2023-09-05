using Web3Unity.Scripts.Library.Ethers.Contracts;
using Newtonsoft.Json;
using Web3Unity.Scripts.Library.Ethers.Providers;
using UnityEngine;
using System;
using TMPro;

/*
 Description:       Unity-smart contract interaction
 Attention:         Before writing to smart contract, you MUST login first (Assets/Web3Unity/Scripts/Scenes/WebLogin.cs)  
 Unity Version:     2021.3.13f1
 Web3Unity Version: 2.1.0
 Author:            ZHUANG Yan
 Date:              09/01/2023
 Last Modified:     09/03/2023
 */

public class ContractInteraction : MonoBehaviour
{
    public static ContractInteraction _instance;
    // abi in json format
    private string abi = "[ { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"d_array\", \"type\": \"uint256\" } ], \"name\": \"choose_duty\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"map_tower\", \"type\": \"uint256\" }, { \"internalType\": \"string\", \"name\": \"tower\", \"type\": \"string\" } ], \"name\": \"edit_addtower\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"map_tower0\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"map_tower1\", \"type\": \"uint256\" } ], \"name\": \"edit_mergetower\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"c_lyr\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"c_idx\", \"type\": \"uint256\" } ], \"name\": \"in_edit\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"choose_layer\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"choose_order\", \"type\": \"uint256\" } ], \"name\": \"sec_in_edit\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"weapontype\", \"type\": \"uint256\" } ], \"name\": \"sec_submitt\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"submit\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"constructor\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" }, { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"name\": \"base_struct\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"timestamp\", \"type\": \"uint256\" }, { \"internalType\": \"address\", \"name\": \"owner\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"blood\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"money\", \"type\": \"uint256\" }, { \"internalType\": \"string\", \"name\": \"map\", \"type\": \"string\" }, { \"internalType\": \"uint256\", \"name\": \"wood_protect\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"iron_protect\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"elec_protect\", \"type\": \"uint256\" }, { \"internalType\": \"string\", \"name\": \"block_position\", \"type\": \"string\" }, { \"internalType\": \"string\", \"name\": \"original_position\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"choose_layer\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"choose_order\", \"type\": \"uint256\" } ], \"name\": \"check\", \"outputs\": [ { \"components\": [ { \"internalType\": \"uint256\", \"name\": \"timestamp\", \"type\": \"uint256\" }, { \"internalType\": \"address\", \"name\": \"owner\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"blood\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"money\", \"type\": \"uint256\" }, { \"internalType\": \"string\", \"name\": \"map\", \"type\": \"string\" }, { \"internalType\": \"uint256\", \"name\": \"wood_protect\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"iron_protect\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"elec_protect\", \"type\": \"uint256\" }, { \"internalType\": \"string\", \"name\": \"block_position\", \"type\": \"string\" }, { \"internalType\": \"string\", \"name\": \"original_position\", \"type\": \"string\" } ], \"internalType\": \"struct BranchClash.inlevel\", \"name\": \"thisnode\", \"type\": \"tuple\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"wallet\", \"type\": \"address\" } ], \"name\": \"check_duty\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"wallet\", \"type\": \"address\" } ], \"name\": \"check_level_or\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"wallet\", \"type\": \"address\" } ], \"name\": \"check_level_pr\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"wallet\", \"type\": \"address\" } ], \"name\": \"check_map\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"thisnode\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"wallet\", \"type\": \"address\" } ], \"name\": \"check_money\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"check_num_layer\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"layer_num\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"choose_layer\", \"type\": \"uint256\" } ], \"name\": \"check_num_nodes\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"node_num\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"choose_layer\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"choose_order\", \"type\": \"uint256\" } ], \"name\": \"check_sec\", \"outputs\": [ { \"components\": [ { \"internalType\": \"address\", \"name\": \"owner\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"wood_debuff\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"iron_debuff\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"elec_debuff\", \"type\": \"uint256\" }, { \"internalType\": \"string\", \"name\": \"block_position\", \"type\": \"string\" }, { \"internalType\": \"string\", \"name\": \"original_position\", \"type\": \"string\" } ], \"internalType\": \"struct BranchClash.sec_inlevel\", \"name\": \"thisnode\", \"type\": \"tuple\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"check_serve\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"check_serve_sec\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"check_time\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"choose_layer\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"choose_order\", \"type\": \"uint256\" } ], \"name\": \"checkowner\", \"outputs\": [ { \"internalType\": \"address\", \"name\": \"\", \"type\": \"address\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"choose_layer\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"choose_order\", \"type\": \"uint256\" } ], \"name\": \"checkowner_sec\", \"outputs\": [ { \"internalType\": \"address\", \"name\": \"\", \"type\": \"address\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"home_health\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"lyr_pr\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"\", \"type\": \"address\" } ], \"name\": \"map_edit_pr\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" }, { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"name\": \"map_map\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"\", \"type\": \"address\" } ], \"name\": \"player\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"sec_check_num_layer\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"layer_num\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"choose_layer\", \"type\": \"uint256\" } ], \"name\": \"sec_check_num_nodes\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"node_num\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"choose_layer\", \"type\": \"uint256\" } ], \"name\": \"sec_judg_mainnode\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"_main_node\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"sec_sum_layer\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"wallet\", \"type\": \"address\" } ], \"name\": \"seccheck_level_or\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"wallet\", \"type\": \"address\" } ], \"name\": \"seccheck_level_pr\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"name\": \"serve_check\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"name\": \"serve_check_sec\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"num\", \"type\": \"uint256\" } ], \"name\": \"serve_level\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"num\", \"type\": \"uint256\" } ], \"name\": \"serve_level_sec\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"total_attack\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"total_monster_blood\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" } ]";
    // address of contract
    private string address = "0x1AeAb40e9eED5F6d0dFC1b915Fa4A075a4e67f69";
    private string account;
    private string rpc = "https://opbnb-testnet-rpc.bnbchain.org";

    public int role;
    public bool finishChoseDuty;
    
    public bool finshiInEidt;
    public bool finishiAdd;
    public bool finishMerge;
    public bool finishSubmit;

    public bool finishSecInEdit;
    public bool finishSecSubmit;

    public void ReSetMain()
    {
        finshiInEidt = false;
        finishiAdd = false;
        finishMerge = false;
        finishSubmit = false;
        
        finishSecInEdit = false;
        finishSecSubmit = false;
    }

    private void Awake()
    {
        _instance = this;
        role = 100;
        finishChoseDuty = false;
        
        finshiInEidt = false;
        finishMerge = false;
        finishiAdd = false;
        finishSubmit = false;
        
        finishSecInEdit = false;
        finishSecSubmit = false;
    }

    public void getAccount()
    {
        account = PlayerPrefs.GetString("Account");
    }


    /////////////////////////////////////Write Function/////////////////////////////////////
    async public void ChooseDuty(string duty)
    {
        // smart contract method to call
        string method = "choose_duty";
        // value in wei
        string value = "0";
        // gas limit OPTIONAL
        string gasLimit = "";
        // gas price OPTIONAL
        string gasPrice = "";
        // args to be serialized
        string d_array = duty;
        // serialize process
        string[] obj = { d_array };
        string args = JsonConvert.SerializeObject(obj);
        // call contract
        string response = await Web3GL.SendContract(method, abi, address, args, value, gasLimit, gasPrice);
        Debug.Log(response);
        finishChoseDuty = true;
    }
    
    async public void InEdit(string _layer,string _index)
    {
        string method = "in_edit";
        string value = "0";
        string gasLimit = "";
        string gasPrice = "";
        string layer = _layer;
        string idx = _index;
        string[] obj = { layer, idx };
        string args = JsonConvert.SerializeObject(obj);
        string response = await Web3GL.SendContract(method, abi, address, args, value, gasLimit, gasPrice);
        Debug.Log(response);
        finshiInEidt = true;
    }

    async public void EditAddTower(string _map_tower,string _tower)
    {
        string method = "edit_addtower";
        string value = "0";
        string gasLimit = "";
        string gasPrice = "";
        string map_tower = _map_tower;
        string tower = _tower;
        string[] obj = { map_tower, tower };
        string args = JsonConvert.SerializeObject(obj);
        string response = await Web3GL.SendContract(method, abi, address, args, value, gasLimit, gasPrice);
        Debug.Log(response);
        finishiAdd = true;
    }

    async public void EditMergeTower(string from_map,string to_map)
    {
        string method = "edit_mergetower";
        string value = "0";
        string gasLimit = "";
        string gasPrice = "";
        string map_tower0 = from_map;
        string map_tower1 = to_map;
        string[] obj = { map_tower0, map_tower1 };
        string args = JsonConvert.SerializeObject(obj);
        string response = await Web3GL.SendContract(method, abi, address, args, value, gasLimit, gasPrice);
        Debug.Log(response);
        finishMerge = true;
    }

    async public void SecInEdit(string _layer,string _index )
    {
        string method = "sec_in_edit";
        string value = "0";
        string gasLimit = "";
        string gasPrice = "";
        string layer = _layer;
        string idx = _index;
        string[] obj = { layer, idx };
        string args = JsonConvert.SerializeObject(obj);
        string response = await Web3GL.SendContract(method, abi, address, args, value, gasLimit, gasPrice);
        Debug.Log(response);
        finishSecInEdit = true;
    }

    async public void SecSubmit()
    {
        string method = "sec_submitt";
        string value = "0";
        string gasLimit = "";
        string gasPrice = "";
        string weapontype = "1";
        string[] obj = { weapontype };
        string args = JsonConvert.SerializeObject(obj);
        string response = await Web3GL.SendContract(method, abi, address, args, value, gasLimit, gasPrice);
        Debug.Log(response);
        finishSecSubmit = true;
    }

    async public void Submit()
    {
        string method = "submit";
        string value = "0";
        string gasLimit = "";
        string gasPrice = "";
        string[] obj = {  };
        string args = JsonConvert.SerializeObject(obj);
        string response = await Web3GL.SendContract(method, abi, address, args, value, gasLimit, gasPrice);
        Debug.Log(response);
        finishSubmit = true;
    }

    /////////////////////////////////////Read Function/////////////////////////////////////
    async public void Check()
    {
        int layer = 1;
        int idx = 1;
        string method = "check";
        var provider = new JsonRpcProvider(rpc);
        var contract = new Contract(abi, address, provider);

        var calldata = await contract.Call(method, new object[]
        {
            layer,
            idx
        });

        //t.text = calldata[0].ToString();
    }

    async public void CheckNumLayer()
    {
        string method = "check_num_layer";
        var provider = new JsonRpcProvider(rpc);
        var contract = new Contract(abi, address, provider);

        var calldata = await contract.Call(method, new object[]
        {

        });

        //t.text = calldata[0].ToString();
    }

    async public void CheckDuty()
    {
        string method = "check_duty";
        var provider = new JsonRpcProvider(rpc);
        var contract = new Contract(abi, address, provider);

        var calldata = await contract.Call(method, new object[]
        {
            account
        });
        role = int.Parse(calldata[0].ToString()) ;
        //t.text = calldata[0].ToString();
    }

    async public void SecJudgMainnode()
    {
        int layer = 1;
        string method = "sec_judg_mainnode";
        var provider = new JsonRpcProvider(rpc);
        var contract = new Contract(abi, address, provider);

        var calldata = await contract.Call(method, new object[]
        {
            layer
        });

        //t.text = calldata[0].ToString();
    }

    async public void CheckMap()
    {
        string method = "check_map";
        var provider = new JsonRpcProvider(rpc);
        var contract = new Contract(abi, address, provider);

        var calldata = await contract.Call(method, new object[]
        {
            account
        });

        //t.text = calldata[0].ToString();
    }

    async public void CheckTime()
    {
        string method = "check_time";
        var provider = new JsonRpcProvider(rpc);
        var contract = new Contract(abi, address, provider);

        var calldata = await contract.Call(method, new object[]
        {
            
        });

        //t.text = calldata[0].ToString();
    }

    async public void CheckSec()
    {
        int layer = 1;
        int idx = 1;
        string method = "check_sec";
        var provider = new JsonRpcProvider(rpc);
        var contract = new Contract(abi, address, provider);

        var calldata = await contract.Call(method, new object[]
        {
            layer,
            idx
        });

        //t.text = calldata[0].ToString();
    }

    async public void CheckNumNodes()
    {
        int layer = 1;
        string method = "check_num_nodes";
        var provider = new JsonRpcProvider(rpc);
        var contract = new Contract(abi, address, provider);

        var calldata = await contract.Call(method, new object[]
        {
            layer
        });

        //t.text = calldata[0].ToString();
    }

    async public void SecCheckNumLayer()
    {
        string method = "sec_check_num_layer";
        var provider = new JsonRpcProvider(rpc);
        var contract = new Contract(abi, address, provider);

        var calldata = await contract.Call(method, new object[]
        {
            
        });

        //t.text = calldata[0].ToString();
    }

    async public void SecCheckNumNodes()
    {
        int layer = 1;
        string method = "sec_check_num_nodes";
        var provider = new JsonRpcProvider(rpc);
        var contract = new Contract(abi, address, provider);

        var calldata = await contract.Call(method, new object[]
        {
            layer
        });

        //t.text = calldata[0].ToString();
    }

    async public void CheckServe()
    {
        string method = "check_serve";
        var provider = new JsonRpcProvider(rpc);
        var contract = new Contract(abi, address, provider);

        var calldata = await contract.Call(method, new object[]
        {
            
        });

        //t.text = calldata[0].ToString();
    }

    async public void ServeLevel()
    {
        int num = 1;
        string method = "serve_level";
        var provider = new JsonRpcProvider(rpc);
        var contract = new Contract(abi, address, provider);

        var calldata = await contract.Call(method, new object[]
        {
            num
        });

        //t.text = calldata[0].ToString();
    }

    async public void CheckServeSec()
    {
        string method = "check_serve_sec";
        var provider = new JsonRpcProvider(rpc);
        var contract = new Contract(abi, address, provider);

        var calldata = await contract.Call(method, new object[]
        {
            
        });

        //t.text = calldata[0].ToString();
    }

    async public void ServeLevelSec()
    {
        int num = 1;
        string method = "serve_level_sec";
        var provider = new JsonRpcProvider(rpc);
        var contract = new Contract(abi, address, provider);

        var calldata = await contract.Call(method, new object[]
        {
            num
        });

       // t.text = calldata[0].ToString();
    }

    async public void CheckMoney()
    {
        string method = "check_money";
        var provider = new JsonRpcProvider(rpc);
        var contract = new Contract(abi, address, provider);

        var calldata = await contract.Call(method, new object[]
        {
            account
        });

        //t.text = calldata[0].ToString();
    }

    async public void CheckLevelOr()
    {
        string method = "check_level_or";
        var provider = new JsonRpcProvider(rpc);
        var contract = new Contract(abi, address, provider);

        var calldata = await contract.Call(method, new object[]
        {
            account
        });

        //t.text = calldata[0].ToString();
    }

    async public void CheckLevelPr()
    {
        string method = "check_level_pr";
        var provider = new JsonRpcProvider(rpc);
        var contract = new Contract(abi, address, provider);

        var calldata = await contract.Call(method, new object[]
        {
            account
        });

        //t.text = calldata[0].ToString();
    }

    async public void SecCheckLevelOr()
    {
        string method = "seccheck_level_or";
        var provider = new JsonRpcProvider(rpc);
        var contract = new Contract(abi, address, provider);

        var calldata = await contract.Call(method, new object[]
        {
            account
        });

        //t.text = calldata[0].ToString();
    }

    async public void SecCheckLevelPr()
    {
        string method = "seccheck_level_pr";
        var provider = new JsonRpcProvider(rpc);
        var contract = new Contract(abi, address, provider);

        var calldata = await contract.Call(method, new object[]
        {
            account
        });

        //t.text = calldata[0].ToString();
    }

    async public void CheckOwner()
    {
        int layer = 1;
        int idx = 1;
        string method = "checkowner";
        var provider = new JsonRpcProvider(rpc);
        var contract = new Contract(abi, address, provider);

        var calldata = await contract.Call(method, new object[]
        {
            layer,
            idx
        });

        //t.text = calldata[0].ToString();
    }

    async public void CheckOwnerSec()
    {
        int layer = 1;
        int idx = 1;
        string method = "checkowner_sec";
        var provider = new JsonRpcProvider(rpc);
        var contract = new Contract(abi, address, provider);

        var calldata = await contract.Call(method, new object[]
        {
            layer,
            idx
        });

        //t.text = calldata[0].ToString();
    }
}
