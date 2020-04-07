using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        Txt.text ="Score : "+ ScoreMgr.instance.score.ToString();
    }
    
    public IEnumerator size()
    {
        while (Txt.fontSize<280)
        {
            Txt.fontSize += 2;
            
            yield return new WaitForSeconds(delay);
        }
        while (Txt.fontSize>250)
        {
            Txt.fontSize -= 2;
            
            yield return new WaitForSeconds(delay);
        }
    }
}
