using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
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

    public void GetGold(int value)
    {
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
