using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreMgr : MonoBehaviour
{
    public int goldUpValue = 10;
    public ScoreText goldScript;
    public ScoreText scoreScript;
    public static ScoreMgr instance;
    private string highscoreKey = "highscore";
    
    public int highScore = 0;
    public int score = 0;
    public bool isHighScore = false;

    public int maxCombo = 0;
    
    public GameObject comboText;
    public GameObject Canvas;
    
    void Awake()
    {
        GoldManager.instance.currentGold = 0;
        instance = this;
        highScore = PlayerPrefs.GetInt(highscoreKey, 0); //저장된 값 받아옴
        
    }

    public void GameStart() //게임 시작 시 초기화
    {
        score = 0;
        isHighScore = false;
    }

    public void goldPong()
    {
        goldScript.pong();
    }

    public void scoreUp(int count, int point, bool isCombo, bool isPong = true)
    {
        if (isCombo)
        {
            GameObject go=Instantiate(comboText,Canvas.transform);
            go.GetComponent<Text>().text = count+ "콤보 +" + point;
        }
        score += point;
        if(score>=10000)
            GooglePlayManager.instance.Achievement(1);
        if(score>=50000)
            GooglePlayManager.instance.Achievement(2);
        if(score>=100000)
            GooglePlayManager.instance.Achievement(3);
        if (score > highScore) //최고점수 갱신
        {
            highScore = score;
            PlayerPrefs.SetInt(highscoreKey,highScore);
            isHighScore = true;
            GooglePlayManager.instance.SetScore(score);
        }

        if (isPong)
        {
            scoreScript.pong();   
        }
    }
    
    public void comboInitialize(int v)
    {
        if(v>=10)
            GooglePlayManager.instance.Achievement(4);
        if(v>=20)
            GooglePlayManager.instance.Achievement(5);
        if(v>=30)
            GooglePlayManager.instance.Achievement(6);
        if (v > maxCombo)
            maxCombo = v;
    }
}
