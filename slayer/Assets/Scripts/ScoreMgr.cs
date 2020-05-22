using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreMgr : MonoBehaviour
{
    public int headshot = 0; //급소 공격당 추가점수
    public int killedOni = 0; //죽인 오니수 표시
    public int maxCombo = 0; //최대 콤보에 비례하여 추가점수
    public int score = 0;
    
    public GameObject comboText;
    public GameObject Canvas;
    public static ScoreMgr instance;

    private void Start()
    {
        instance = this;
    }

    public void scoreUp(int count,int point, bool isCombo)
    {
        if (isCombo)
        {
           GameObject go=Instantiate(comboText,Canvas.transform);
           if (TextManager.instance.isKor == 1) //한국어
               go.GetComponent<Text>().text = count+ "콤보 +" + point;
           else if (TextManager.instance.isKor == 0) //한국어//영어
               go.GetComponent<Text>().text = count+ "combo +" + point;
           else if (TextManager.instance.isKor == 2) //일본어
               go.GetComponent<Text>().text = count+ "コンボ +" + point;
           
         
        }
        score += point;
        if(FindObjectOfType<ScoreText>()!=null) 
            StartCoroutine(FindObjectOfType<ScoreText>().size());
    }

    public void scoreDown(int point)
    {
        score -= point;
    }

    public void comboInitialize(int v)
    {
        if (v > maxCombo)
            maxCombo = v;
    }
}
