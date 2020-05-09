using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
    private Text Txt;
    public float delay;
    private void Start()
    {
        Txt = GetComponent<Text>();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Main3" || SceneManager.GetActiveScene().name == "Main3_EZ" ||
            SceneManager.GetActiveScene().name == "Main3_H")
        {
            if (TextManager.instance.isKor == 1) //한국어
                Txt.text = "Time : " +GameManager.instance.trainTime / 60 +"분 "+ GameManager.instance.trainTime % 60+"초";
            else //영어
                Txt.text = "Time : " +GameManager.instance.trainTime / 60 +"min "+ GameManager.instance.trainTime % 60+"sec";
        }
        else
        {
            Txt.text ="Score : "+ ScoreMgr.instance.score.ToString();
        }
    }
    
    public IEnumerator size()
    { if (SceneManager.GetActiveScene().name == "Main3" || SceneManager.GetActiveScene().name == "Main3_EZ" ||
          SceneManager.GetActiveScene().name == "Main3_H")
        {
        }
        else
        {
            while (Txt.fontSize < 280)
            {
                Txt.fontSize += 2;

                yield return new WaitForSeconds(delay);
            }

            while (Txt.fontSize > 250)
            {
                Txt.fontSize -= 2;

                yield return new WaitForSeconds(delay);
            }
        }
    }
}
