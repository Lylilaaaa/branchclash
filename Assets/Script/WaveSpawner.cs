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
    public float enemySpeed;

    public int level;

    public bool start;
    public bool _hasStart=false;
    public float disPerUnit = 5f;
    public int checkChildNum;

    private int hasSpawn = 0;
    private bool hasEnamy = false;

    public Transform monsterContainer;
    public bool finishSubmitConfirm;


    private void Start()
    {
        _hasStart = false;
        start = false;
        hasEnamy = false;
        hasSpawn = 0;
        level = CurNodeDataSummary._instance.thisNodeData.nodeLayer;
    }

    private void Update()
    {
        if (GlobalVar.CurrentGameState == GlobalVar.GameState.GamePlay)
        {
            checkChildNum = monsterContainer.childCount;
            enemyNum = CurNodeDataSummary._instance.GetMonsterNum(level);

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
            yield return new WaitForSeconds(_getSpawnTime());
        }
    }

    private float _getSpawnTime()
    {
        float intervalAbs;
        intervalAbs = disPerUnit*CurNodeDataSummary._instance.GetMonsterInterval(level);
        enemySpeed = disPerUnit;
        return intervalAbs / enemySpeed;
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
        
        if (CurNodeDataSummary._instance.homeDestroyData == 0)
        {
            GlobalVar._instance.gameResult = 0;
        }
        else if (CurNodeDataSummary._instance.homeDestroyData > 0 && CurNodeDataSummary._instance.homeCurHealth >0)
        {
            GlobalVar._instance.gameResult = 1;
        }
        else if(CurNodeDataSummary._instance.homeDestroyData > 0  && CurNodeDataSummary._instance.homeCurHealth <= 0)
        {
            GlobalVar._instance.gameResult = 2;
        }
        selectPanal.SetActive(true);
        selectPanal.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        selectPanal.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        selectPanal.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
        selectPanal.transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
        GlobalVar._instance.ChangeState("GameOver");
        // TreeNodeDataInit._instance.finish = false;
        // GlobalVar._instance.nodePrepared = false;
        //TreeNodeDataInit._instance.AddNodeData();
        //TreeNodeDataInit._instance.restartUp();
        StartOpenGamePlay();
    }
    
    public void StartOpenGamePlay()
    {
        StartCoroutine( checkNewNode());
    }
    IEnumerator checkNewNode()
    {
        ContractInteraction._instance.endProcessSec = "";
        ContractInteraction._instance.CheckLevelOr();
        while (ContractInteraction._instance.endProcessSec.Length == 0)
        {
            yield return null;
        }
        finishSubmitConfirm = checkEqual();
        Debug.Log("endProcessSec is: "+ ContractInteraction._instance.endProcessSec);
        if (finishSubmitConfirm)
        {
            StartCoroutine(checkLostBlood());
        }
        else
        {
            StartCoroutine( checkNewNode());
            Debug.Log("not finish the confirm!");
        }
    }
    private bool checkEqual()
    {
        if (ContractInteraction._instance.endProcessSec == "finish")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator checkLostBlood()
    {
        GlobalVar._instance.nowNodeIndexBlood = "0";
        ContractInteraction._instance.finshiCheckNewBlood = false;
        string layIndexString =
            GlobalVar._instance.nowNodeIndex.Substring(1, GlobalVar._instance.nowNodeIndex.Length - 2);
        string[] layIndex = layIndexString.Split(",");
        Debug.Log("new node is " +layIndex[0] +layIndex[1]);
        ContractInteraction._instance.check_blood_new( int.Parse(layIndex[0]) , int.Parse(layIndex[1]));
        while (!ContractInteraction._instance.finshiCheckNewBlood)
        {
            yield return null;
        }
        Debug.Log("the new blood is: "+GlobalVar._instance.nowNodeIndexBlood);
        Debug.Log("the old blood is: "+GlobalVar._instance.oldNodeIndexBlood);
        int lostBlood = int.Parse(GlobalVar._instance.oldNodeIndexBlood) - int.Parse(GlobalVar._instance.nowNodeIndexBlood);
        
        if (lostBlood > 0&& lostBlood< GlobalVar._instance.chosenNodeData.curHealth)
        {
            GlobalVar._instance.gameResult = 1;
            CurNodeDataSummary._instance.homeDestroyData = lostBlood;
        }
        else if (lostBlood <= 0)
        {
            GlobalVar._instance.gameResult = 0;
        }
        else
        {
            CurNodeDataSummary._instance.homeDestroyData = GlobalVar._instance.chosenNodeData.curHealth;
            GlobalVar._instance.gameResult = 2;
        }
        GlobalVar._instance._loadNextScene("4_End");
    }
}
