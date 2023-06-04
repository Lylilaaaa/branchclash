using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;


public class WaveSpawner : MonoBehaviour
{
    public Transform startPosition;

    public GameObject enemyPrefab;
    public GameObject selectPanal;
    public int enemyNum;

    public LevelData levelDate;

    public bool start;
    public bool _hasStart=false;
    public int checkChildNum;

    private int hasSpawn = 0;
    private bool hasEnamy = false;

    

    private void Update()
    {
        checkChildNum = transform.childCount;
        enemyNum = levelDate.deltaMonster + levelDate.levelNum * levelDate.deltaMonster;

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
            hasEnamy = false;
            foreach (Transform child in transform)
            {
                if (child.tag == "Enemy")
                {
                    hasEnamy = true;
                }
            }

            if (hasEnamy == false)
            {
                endGame();
                _hasStart = false;
            }
        }

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
        GameObject enemySpawn = Instantiate(enemyPrefab,startPosition.position,startPosition.rotation);
        enemySpawn.name = j.ToString() + "enemy";
        enemySpawn.transform.SetParent(transform);
        enemySpawn.transform.localScale = new Vector3(1f,1f,1f);
    }

    public void startGame()
    {
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
    }
}
