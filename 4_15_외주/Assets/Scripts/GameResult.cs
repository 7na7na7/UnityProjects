﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameResult : MonoBehaviour
{
    // Start is called before the first frame update
    private int highScore;
    public Text resultTime;
    public Text bestTime;
    public GameObject parts;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("HighScore"))
        {
            highScore = PlayerPrefs.GetInt("HighScore");
        }
        else
        {
            highScore = 999;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (GoalArea.goal)
        {
            parts.SetActive(true);
            int result = Mathf.FloorToInt(Timer.time);
            resultTime.text = "ResultTime " + result;
            bestTime.text = "BestTime " + highScore;
            if (highScore > result)
            {
                PlayerPrefs.SetInt("HighScore", result);
            }
        }
    }
    public void OnRetry()
    {
        Application.LoadLevel("Main");
    }
}