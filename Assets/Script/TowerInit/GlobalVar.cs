using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVar : MonoBehaviour
{
    public static GlobalVar _instance;
    public enum GameState
    {
        MainStart,
        Viewing,
        ChooseFiled,
        AddTowerUI,
        GameOver
    }
    public static GameState currentGameState;
    private void Start()
    {
        // 初始化游戏状态
        currentGameState = GameState.MainStart;
    }
    private void Awake()
    {
        _instance = this;
    }
    // Start is called before the first frame update
    public void ChangeState()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
