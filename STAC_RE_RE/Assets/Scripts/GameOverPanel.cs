using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    public GameObject high;
    public Text score;
    public Text highScore;
    public Text Combo;
    void OnEnable()
    {
        score.text = ScoreMgr.instance.score.ToString();
        highScore.text = ScoreMgr.instance.highScore.ToString();
        Combo.text = ScoreMgr.instance.maxCombo.ToString();
            
        if(ScoreMgr.instance.isHighScore) //신기록 갱신했다면
            high.SetActive(true);
    }
}
