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
        GetComponent<RectTransform>().anchoredPosition=new Vector2(  GetComponent<RectTransform>().anchoredPosition.x-300,  GetComponent<RectTransform>().anchoredPosition.y);
        Txt = GetComponent<Text>();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Main3" || SceneManager.GetActiveScene().name == "Main3_EZ" ||
            SceneManager.GetActiveScene().name == "Main3_H")
        {
            Txt.text = "Time : " +GameManager.instance.trainTime / 60 +"분 "+ GameManager.instance.trainTime % 60+"초";
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
