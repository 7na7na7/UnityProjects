using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
    public int maxFontSize = 300;
    public int minFontSize = 250;
    public int currentGold = 0;
    public bool isScore = true;
    private Text Txt;
    public int Upvalue = 3;
    public float delay;
    private void Start()
    {
        Txt = GetComponent<Text>();
    }

    void Update()
    {
        if (isScore)
            Txt.text = ScoreMgr.instance.score.ToString();
        else
            Txt.text = currentGold.ToString();
    }

    public void pong()
    {
        StopAllCoroutines();
        StartCoroutine(size());
    }
    public IEnumerator size()
    {
        while (Txt.fontSize < maxFontSize)
            {
                Txt.fontSize += Upvalue;

                yield return new WaitForSeconds(Time.deltaTime);
            }

            while (Txt.fontSize > minFontSize)
            {
                Txt.fontSize -= Upvalue;

                yield return new WaitForSeconds(Time.deltaTime);
            }
    }
}
