using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WaveSpawner : MonoBehaviour
{
    public  List<GameObject> EnemeList;
    public List<Transform> SpawnPos;
    //1.Cake 2.JellyFish
    
    private List<int> myList;
    public GameObject selectPanal;
    public int enemyNum;

    public LevelData levelDate;

    public bool start;
    public bool _hasStart=false;
    public int checkChildNum;

    private int hasSpawn = 0;
    private bool hasEnamy = false;

    public Transform monsterContainer;


    private void Start()
    {
        _hasStart = false;
        start = false;
        hasEnamy = false;
        hasSpawn = 0;
    }

    private void Update()
    {
        if (GlobalVar.CurrentGameState == GlobalVar.GameState.GamePlay)
        {
            checkChildNum = monsterContainer.childCount;
            enemyNum = levelDate.deltaMonster + levelDate.levelNum * levelDate.deltaMonster;
            CurNodeDataSummary._instance.monsterCount = enemyNum;
    
            if (start == true)
            {
                StartCoroutine(spawnWave());
                start = false;
            }
    
            if (hasSpawn == enemyNum)
            {
                _hasStart = true;
                hasSpawn = 0;
            }
    
            if (_hasStart == true)
            {
                if (_hasEnemy(monsterContainer) == false)
                {
                    endGame();
                    _hasStart = false;
                }
            }
        }
    }

    private bool _hasEnemy(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.tag == "Enemy")
            {
                return true;
            }
        }
        return false;
    }
    

    IEnumerator spawnWave()
    {
        for (int i = 0; i < enemyNum; i++)
        {
            spawnEnemy(i);
            hasSpawn += 1;
            yield return new WaitForSeconds(2f);
        }
    }

    void spawnEnemy(int j)
    {
        //print("spawn enemy");
        int randomIndex = UnityEngine.Random.Range(0, EnemeList.Count);
        GameObject enemySpawn = Instantiate(EnemeList[randomIndex],SpawnPos[randomIndex].position,SpawnPos[randomIndex].rotation);
        enemySpawn.name = j.ToString() + "enemy";
        enemySpawn.transform.rotation = Quaternion.Euler(Vector3.zero);
        enemySpawn.transform.SetParent(monsterContainer);
        //enemySpawn.transform.localScale = new Vector3(1f,1f,1f);
    }

    public void startGame()
    {
        StartCoroutine(WaitAndExecute());
    }
    private IEnumerator WaitAndExecute()
    {
        yield return new WaitForSeconds(1f);

        start = true;
        selectPanal.SetActive(false);
        
    }

    public void endGame()
    {
        print("game is end!");
        selectPanal.SetActive(true);
        selectPanal.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        selectPanal.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        selectPanal.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
        selectPanal.transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
        GlobalVar._instance.ChangeState("GameOver");
        TreeNodeDataInit._instance.finish = false;
        SceneManager.LoadScene("End");
    }
}
