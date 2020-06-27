using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public GameObject gameover;
    public Text score;
   public int scoreValue = 0;
   private bool isGameOver = false;
    void Update()
    {
        score.text = "Score : "+scoreValue;
        if (scoreValue >= 100)
        {
            if (!isGameOver)
            {
                isGameOver = true;
                StartCoroutine(delayExit());
            }
        }
    }

    IEnumerator delayExit()
    {
        yield return new WaitForSeconds(0.25f);
        gameover.SetActive(true);   
        yield return new WaitForSeconds(1f);
        Application.Quit();
    }
}
