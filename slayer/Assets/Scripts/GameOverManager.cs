using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameOverManager : MonoBehaviour
{
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
        //콤보 마무리해줌
        if (ComboManager.instance.comboCount >= 2)
        {
            ScoreMgr.instance.comboInitialize(ComboManager.instance.comboCount);
            ScoreMgr.instance.scoreUp(100*ComboManager.instance.comboCount,true);
        }
        ComboManager.instance.comboCount = 0;
        
        Camera.main.transform.position = new Vector3(Player.instance.transform.position.x,Player.instance.transform.position.y,Camera.main.transform.position.z);
        Time.timeScale = 0.1f;
        FindObjectOfType<CameraManager>().gameOver();
        yield return new WaitUntil(()=>canGo==true);
        StartCoroutine(Screenfade.fadeInRealTime());
        StartCoroutine(Worldfade.fadeoutRealTime());
        StartCoroutine(panel.GetComponent<ScorePanel>().bonus());
    }
}
