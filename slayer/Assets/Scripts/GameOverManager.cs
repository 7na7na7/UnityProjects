using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameOverManager : MonoBehaviour
{
    public GameObject delete;
    public GameObject panel;
    public bool canGo = false;
    public fade Screenfade;
    public fade Worldfade;
    public void GameoverFunc()
    {
        StartCoroutine(GameOver());
    }
    public IEnumerator GameOver()
    {
        delete.SetActive(false);
        //콤보 마무리해줌
        if (ComboManager.instance.comboCount >= 2)
        {
            ScoreMgr.instance.comboInitialize(ComboManager.instance.comboCount);
            ScoreMgr.instance.scoreUp(100*ComboManager.instance.comboCount,true);
        }
        ComboManager.instance.comboCount = 0;
        Time.timeScale = 0.1f;
        CameraManager.instance.gameOver();
        yield return new WaitUntil(()=>canGo==true);
        StartCoroutine(Screenfade.fadeInRealTime());
        StartCoroutine(Worldfade.fadeoutRealTime());
        StartCoroutine(panel.GetComponent<ScorePanel>().bonus());
    }

    
}
