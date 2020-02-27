using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    public Text[] scores;
    private MoveByClick[] players;
    public int[] score =new int[5];
    public string[] playerName=new string[5];

    void Update()
    {
        players = FindObjectsOfType<MoveByClick>();
        for (int a = 0; a < players.Length; a++)
        {
            score[a] = players[a].score;
            playerName[a] = players[a].nickname.text;
        }

        for (int j = 0; j < players.Length; j++)
        {
            for (int i = 0; i < players.Length - 1 - j; i++)
            {
                if (score[i] < score[i + 1])
                {
                    int temp = score[i + 1];
                    score[i + 1] = score[i];
                    score[i] = temp;

                    string temp2 = playerName[i + 1];
                    playerName[i + 1] = playerName[i];
                    playerName[i] = temp2;
                }
            }
        }

        for (int i = 0; i <5; i++)
        {
            scores[i].text = playerName[i] + " - " + score[i];
            if (i >= players.Length)
                scores[i].text = "플레이어 - 점수";
        }
    }
}
