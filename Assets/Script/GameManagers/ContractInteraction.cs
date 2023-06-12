using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
 Description:       Unity-smart contract interaction
 Attention:         Before writing to smart contract, you MUST login first (Assets/Web3Unity/Scripts/WebLogin.cs)  
 Unity Version:     2021.3.13f1
 Web3Unity Version: 1.2.9
 Author:            ZHUANG Yan
 Date:              06/11/2023
 Last Modified:     06/11/2023
 */

public class ContractInteraction : MonoBehaviour
{
    // Show return info
    public TMP_Text t;
    // Set chain: ethereum, moonbeam, polygon etc
    string chain = "ethereum";
    // Set network mainnet, testnet
    string network = "sepolia";
    // ABI in json format
    string abi = "[ { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"index_tower\", \"type\": \"uint256\" }, { \"internalType\": \"string\", \"name\": \"tower\", \"type\": \"string\" } ], \"name\": \"edit_addtower\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"index_fromtower\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"index_totower\", \"type\": \"uint256\" } ], \"name\": \"edit_mergetower\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"choose_layer\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"choose_order\", \"type\": \"uint256\" } ], \"name\": \"in_edit\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"choose_layer\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"choose_order\", \"type\": \"uint256\" } ], \"name\": \"sec_in_edit\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"weapontype\", \"type\": \"uint256\" } ], \"name\": \"sec_submitt\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"choose_layer\", \"type\": \"uint256\" } ], \"name\": \"submit\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"constructor\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"choose_layer\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"choose_order\", \"type\": \"uint256\" } ], \"name\": \"check\", \"outputs\": [ { \"components\": [ { \"internalType\": \"address\", \"name\": \"owner\", \"type\": \"address\" }, { \"internalType\": \"string\", \"name\": \"weapon\", \"type\": \"string\" }, { \"internalType\": \"uint256\", \"name\": \"blood\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"money\", \"type\": \"uint256\" }, { \"internalType\": \"string\", \"name\": \"map\", \"type\": \"string\" }, { \"internalType\": \"string\", \"name\": \"block_position\", \"type\": \"string\" }, { \"internalType\": \"string\", \"name\": \"original_position\", \"type\": \"string\" } ], \"internalType\": \"struct insect_war.inlevel\", \"name\": \"thisnode\", \"type\": \"tuple\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"choose_layer\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"choose_order\", \"type\": \"uint256\" } ], \"name\": \"check_father\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"father_layer_order\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"index\", \"type\": \"uint256\" } ], \"name\": \"check_list\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"node_num\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"check_list_length\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"list_length\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"choose_layer\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"choose_order\", \"type\": \"uint256\" } ], \"name\": \"check_map\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"thisnode\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"choose_layer\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"choose_order\", \"type\": \"uint256\" } ], \"name\": \"check_nodes_owner\", \"outputs\": [ { \"internalType\": \"address\", \"name\": \"owner_address\", \"type\": \"address\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"check_num_layer\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"layer_num\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"choose_layer\", \"type\": \"uint256\" } ], \"name\": \"check_num_nodes\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"node_num\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"name\": \"node_list\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"choose_layer\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"choose_order\", \"type\": \"uint256\" } ], \"name\": \"sec_check_father\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"father_layer_order\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"index\", \"type\": \"uint256\" } ], \"name\": \"sec_check_list\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"node_num\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"sec_check_list_length\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"list_length\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"choose_layer\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"choose_order\", \"type\": \"uint256\" } ], \"name\": \"sec_check_nodes_owner\", \"outputs\": [ { \"internalType\": \"address\", \"name\": \"owner_address\", \"type\": \"address\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"sec_check_num_layer\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"layer_num\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"choose_layer\", \"type\": \"uint256\" } ], \"name\": \"sec_check_num_nodes\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"node_num\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"name\": \"sec_node_list\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" } ]";
    // Address of contract
    string contract = "0x71e8e4436c85f0a41aa671DE7fB433c04Ff90728";

    /*========================Write Functions=============================*/

    async public void InEdit()
    {
        // Smart contract method to call
        string method = "in_edit";
        // Array of arguments for contract
        // Change arguments here
        // Follow the format below, change numbers to you arguments only
        string args = "[\"2\",\"1\"]";
        // Value in wei
        string value = "0";
        // Gas limit OPTIONAL
        string gasLimit = "";
        // Gas price OPTIONAL
        string gasPrice = "";
        // Connects to user's browser wallet (metamask) to update contract state
        try
        {
            string response = await Web3GL.SendContract(method, abi, contract, args, value, gasLimit, gasPrice);
            Debug.Log(response);
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
        }
    }

    async public void EditAddTower()
    {
        string method = "edit_addtower";

        string args = "[\"23\",\"wood\"]";

        string value = "0";

        string gasLimit = "";

        string gasPrice = "";
        try
        {
            string response = await Web3GL.SendContract(method, abi, contract, args, value, gasLimit, gasPrice);
            Debug.Log(response);
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
        }
    }

    async public void EditMergeTower()
    {
        
        string method = "edit_mergetower";
        
        string args = "[\"23\",\"22\"]";
        
        string value = "0";
        
        string gasLimit = "";
        
        string gasPrice = "";
        
        try
        {
            string response = await Web3GL.SendContract(method, abi, contract, args, value, gasLimit, gasPrice);
            Debug.Log(response);
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
        }
    }

    async public void Submit()
    {
        
        string method = "submit";
        
        string args = "[\"2\"]";
        
        string value = "0";
        
        string gasLimit = "";
        
        string gasPrice = "";
        
        try
        {
            string response = await Web3GL.SendContract(method, abi, contract, args, value, gasLimit, gasPrice);
            Debug.Log(response);
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
        }
    }

    async public void SecInEdit()
    {
        
        string method = "sec_in_edit";
        
        string args = "[\"1\",\"1\"]";
        
        string value = "0";
        
        string gasLimit = "";
        
        string gasPrice = "";
        
        try
        {
            string response = await Web3GL.SendContract(method, abi, contract, args, value, gasLimit, gasPrice);
            Debug.Log(response);
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
        }
    }

    async public void SecSubmit()
    {
        
        string method = "sec_submitt";
        
        string args = "[\"1\"]";
        
        string value = "0";
        
        string gasLimit = "";
        
        string gasPrice = "";
        
        try
        {
            string response = await Web3GL.SendContract(method, abi, contract, args, value, gasLimit, gasPrice);
            Debug.Log(response);
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
        }
    }

    /*========================Read Functions=============================*/

    async public void CheckListLength()
    {
        // Smart contract method to call
        string method = "check_list_length";
        // Array of arguments for contract
        // Same as write function arguments
        string args = "[]";
        // Connects to user's browser wallet to call a transaction
        string response = await EVM.Call(chain, network, contract, abi, method, args);
        // Display response in game
        t.text = response;
    }

    async public void SecCheckListLength()
    {
        string method = "sec_check_list_length";
        
        string args = "[]";
        
        string response = await EVM.Call(chain, network, contract, abi, method, args);

        t.text = response;
    }

    async public void CheckList()
    {
        string method = "check_list";
        
        string args = "[\"1\"]";
        
        string response = await EVM.Call(chain, network, contract, abi, method, args);

        t.text = response;
    }

    async public void SecCheckList()
    {
        string method = "sec_check_list";
        
        string args = "[\"1\"]";
        
        string response = await EVM.Call(chain, network, contract, abi, method, args);

        t.text = response;
    }

    async public void CheckFather()
    {
        string method = "check_father";
        
        string args = "[\"2\",\"1\"]";
        
        string response = await EVM.Call(chain, network, contract, abi, method, args);

        t.text = response;
    }

    async public void SecCheckFather()
    {
        string method = "sec_check_father";
        
        string args = "[\"1\",\"1\"]";
        
        string response = await EVM.Call(chain, network, contract, abi, method, args);

        t.text = response;
    }

    async public void CheckNodesOwner()
    {
        string method = "check_nodes_owner";
        
        string args = "[\"2\",\"1\"]";
        
        string response = await EVM.Call(chain, network, contract, abi, method, args);

        t.text = response;
    }

    async public void SecCheckNodesOwner()
    {
        string method = "sec_check_nodes_owner";
        
        string args = "[\"1\",\"1\"]";
        
        string response = await EVM.Call(chain, network, contract, abi, method, args);

        t.text = response;
    }

    async public void Check()
    {
        string method = "check";
        
        string args = "[\"2\",\"1\"]";
        
        string response = await EVM.Call(chain, network, contract, abi, method, args);

        t.text = response;
    }
}

