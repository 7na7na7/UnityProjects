using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
  public Sprite pauseSprite, goSprite;
  public Button pauseBtn;
  public bool isGameOver=false;
  public void pause()
  {
    if (!isGameOver)
    {
      if (Time.timeScale == 0) //재개
      {
        if (ComboManager.instance.comboCount >= 2)
          Time.timeScale = 0.7f;
        else
          Time.timeScale = 1;
        pauseBtn.GetComponent<Image>().sprite = goSprite;
      }
      else //일시정지
      {
        Time.timeScale = 0;
        pauseBtn.GetComponent<Image>().sprite = pauseSprite;
      }
    }
  }

  public void RESTART()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }

  public void TITLE()
  {
    SceneManager.LoadScene("Title");
  }
}
