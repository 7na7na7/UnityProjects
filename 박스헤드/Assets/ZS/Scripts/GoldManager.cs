using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldManager : MonoBehaviour
{
    public int realgold = 0;
    public int gold = 0;
    public static GoldManager instance;

    public string goldstring = "gold";
    public int savedgold = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        savedgold= PlayerPrefs.GetInt(goldstring, 0);
    }
}
