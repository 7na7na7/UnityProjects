using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreMgr : MonoBehaviour
{
    public GameObject comboText;
    public GameObject Canvas;
    public static ScoreMgr instance;
    public int score = 0;

    private void Start()
    {
        instance = this;
    }

    public void scoreUp(int point, bool isCombo)
    {
        if (isCombo)
        {
           GameObject go=Instantiate(comboText,Canvas.transform);
           go.GetComponent<Text>().text = (point * 0.01) + "콤보 +" + point;
        }
        score += point;
        StartCoroutine(FindObjectOfType<ScoreText>().size());
    }

    public void scoreDown(int point)
    {
        score -= point;
    }
}
