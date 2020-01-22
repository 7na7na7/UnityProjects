﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool isGameOver = false;
    public GameObject GameOverpanel;
    public GameObject p1, p2;
    public bool p1Dead = false;
    public bool p2Dead = false;
    public float spawnDelay;
    
    public Text wave_left;
    public int[] zombiecount;

    public int currentzombie = 0;
    public int i = 0; //for문용 변수
    public int wave = 1;
    private bool waveClear = false;
    private void Start()
    {
        StartCoroutine(spawn());
    }

    private void Update()
    {
        if(p1Dead&&p2Dead)
        { 
            if (!isGameOver) 
            { 
                StopAllCoroutines(); 
                isGameOver = true; 
                Time.timeScale = 0; 
                GameOverpanel.SetActive(true); 
            } 
        }

        if (isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                Time.timeScale = 1;
                SceneManager.LoadScene("Title");
            }
        }
        else
        {
            if (!waveClear)
                wave_left.text = wave + " Wave - " + currentzombie + " Left";
            else
                wave_left.text = "  Wave Clear!";
        }
    }

    public void Dead1()
    {
        StartCoroutine(spawnP1());
    }

    public void Dead2()
    {
        StartCoroutine(spawnP2());
    }
    IEnumerator spawnP1()
    {
        yield return new WaitForSeconds(spawnDelay);
        Instantiate(p1, transform.position,Quaternion.identity);
        p1Dead = false;
    }
    IEnumerator spawnP2()
    {
        yield return new WaitForSeconds(spawnDelay);
        Instantiate(p2, transform.position,Quaternion.identity);
        p2Dead = false;
    }
    IEnumerator spawn()
    {
        for (i = 0; i < zombiecount.Length; i++) //끝나고 i++
        {
            yield return new WaitUntil(() => zombiecount[i] <= 0); //zombiecount[i]가 0이 될 때까지 기다림(현재 웨이브의 좀비가 다 죽을 때까지)
            waveClear = true;
            yield return new WaitForSeconds(5f); //기다림...
            waveClear = false;
            currentzombie = 0; //현재좀비수 초기화
            wave++;
        }
    }
    
    public void zombieDead()
    {
        currentzombie--;
        zombiecount[i]--;
    }
    public void zombieCreate()
    {
        currentzombie++;
    }
}
