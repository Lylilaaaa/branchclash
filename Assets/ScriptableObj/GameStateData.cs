using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CreateAssetMenu(fileName = "GameState",menuName = "ScriptableObjects/GameStateData",order = 3)]
public class GameStateData : ScriptableObject
{
    public GlobalVar.GameState currentGameState;
}
