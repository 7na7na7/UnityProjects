using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
    private Text text;
    public float delay;
    private void Start()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
       text.text ="Score : "+ ScoreMgr.instance.score.ToString();
    }

    public IEnumerator size()
    {
        while (text.fontSize<=175)
        {
            text.fontSize += 3;
            yield return new WaitForSeconds(delay);
        }
        while (text.fontSize>=150)
        {
            text.fontSize -= 3;
            yield return new WaitForSeconds(delay);
        }
    }
}
