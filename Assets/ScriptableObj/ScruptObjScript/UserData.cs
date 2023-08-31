using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UserData",menuName = "ScriptableObjects/User")]
public class UserData : ScriptableObject
{
    public string address;
    public int role;
    public bool isFirstPlay;
    public int process;
    public Dictionary<string,string> ActionLog; // [Key = function name], block time stamp
}
