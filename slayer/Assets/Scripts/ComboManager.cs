using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ComboManager : MonoBehaviour
{
    public static ComboManager instance;
    public int comboCount = 0; //콤보횟수
    public bool canCombo = false; //콤보중인지 판단
    public float comboDelay; //콤보 간 지속될 수 있는 시간

    private void Start()
    {
        instance = this;
        comboDelay = 1;
    }

    public void comboIniitailize()
    {
      
            StopAllCoroutines();
            StartCoroutine(comboInitialization());
    }
    IEnumerator comboInitialization()
    {
        int value = 25;
        canCombo = true;
        comboCount++;
        yield return new WaitForSeconds(0.2f);
        yield return new WaitForSeconds(comboDelay);
        if (GameManager.instance.canChangeTimeScale)
            Time.timeScale = 1;
        canCombo = false;
        if (comboCount >= 2)
        {
            if (comboCount >= 6)
                GooglePlayManager.instance.Achievement1();
            if (comboCount >= 12)
                GooglePlayManager.instance.Achievement2();
            if (comboCount >= 24)
                GooglePlayManager.instance.Achievement3();

            ScoreMgr.instance.comboInitialize(comboCount);
            value += comboCount / 5 * 25;

            if (Player.instance.playerIndex == 1) //플레이어가 탄지로면 히노카미 카구라 사용
                kagura.instance.valueUp(comboCount);
            else if(Player.instance.playerIndex==3) //플레이어가 기유면 11형 잔잔한물결 사용
                kagura.instance.ValueUp2(comboCount);
            
            if (SceneManager.GetActiveScene().name == "Main3" || SceneManager.GetActiveScene().name == "Main3_EZ" ||
                SceneManager.GetActiveScene().name == "Main3_H")
            {
            }
            else
            {
                ScoreMgr.instance.scoreUp(comboCount, value * comboCount, true);
            }
        }
        comboCount = 0;
    }

    public void ComboEnd()
    {
        if (SceneManager.GetActiveScene().name == "Main3" || SceneManager.GetActiveScene().name == "Main3_EZ" ||
            SceneManager.GetActiveScene().name == "Main3_H")
        {
        }
        else
        {
            if (comboCount >= 2)
            {
                ScoreMgr.instance.comboInitialize(comboCount);
                ScoreMgr.instance.comboInitialize(comboCount);
                int value = 25;
                value += comboCount / 5 * 25;
                ScoreMgr.instance.scoreUp(comboCount,value*comboCount,true);
            }
            comboCount = 0;   
        }
    }
}
