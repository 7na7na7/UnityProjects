using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public void GameoverFunc()
    {
        StartCoroutine(GameOver());
    }
    public IEnumerator GameOver()
    {
        yield return new WaitForSeconds(0.2f);
        Time.timeScale = 0;
    }
}
