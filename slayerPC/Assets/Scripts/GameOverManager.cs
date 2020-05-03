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
    public void GameoverFunc(GameObject g)
    {
        StartCoroutine(GameOver(g));
    }
    public IEnumerator GameOver(GameObject g)
    {
        delete.SetActive(false);
        GameObject[] hearts = GameObject.FindGameObjectsWithTag("heart");
        foreach (GameObject heart in hearts)
        {
            heart.SetActive(false);
        }
        Player.instance.StopAllCoroutines();
        //콤보 마무리해줌
        ComboManager.instance.ComboEnd();
        ComboManager.instance.comboCount = 0;
        Time.timeScale = 0.1f;
        CameraManager.instance.gameOver(g);
        yield return new WaitUntil(()=>canGo==true);
        StartCoroutine(Screenfade.fadeInRealTime());
        StartCoroutine(Worldfade.fadeoutRealTime());
        StartCoroutine(panel.GetComponent<ScorePanel>().bonus());
    }
}
