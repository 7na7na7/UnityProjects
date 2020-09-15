using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoldManager : MonoBehaviour
{
    public int currentGold=0;
    public static GoldManager instance;
    private string goldKey = "goldKey";
    public int gold;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            
            gold = PlayerPrefs.GetInt(goldKey, 0);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
            GetGold(100);
    }

    public void GetGold(int value)
    {
        if (SceneManager.GetActiveScene().name == "Play")
        {
            currentGold += value;
            if(currentGold>=100)
                GooglePlayManager.instance.Achievement(7);
        }
        gold += value;
        PlayerPrefs.SetInt(goldKey,gold);
    }

    public void LoseGold(int value)
    {
        gold -= value;
        PlayerPrefs.SetInt(goldKey,gold);
    }

    public bool isGold(int value)
    {
        if (gold >= value)
            return true;
        else
            return false;
    }
}
