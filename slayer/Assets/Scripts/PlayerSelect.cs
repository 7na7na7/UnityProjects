using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.Experimental.UIElements.Image;

public class PlayerSelect : MonoBehaviour
{
    public GameObject[] Players;
    public UnityEngine.UI.Image[] playerSprites;
    public GameObject[] contexts;
    private int currentIndex = 0;

    private void Update()
    {
        for (int i = 0; i < contexts.Length; i++)
        {
            if (i == currentIndex)
            {
                contexts[i].SetActive(true);
                Color color = playerSprites[i].color;
                color.a = 1;
                playerSprites[i].color = color;
            }
            else
            {
                contexts[i].SetActive(false);   
                Color color = playerSprites[i].color;
                color.a = 0.3f;
                playerSprites[i].color = color;
            }
        }
    }

    private void Awake()
    {
        Time.timeScale = 0;
    }
    

    public void Zenichu()
    {
        if(currentIndex==0)
            Go();
        currentIndex = 0;
    }
    public void Tanjiro()
    {
        if(currentIndex==1)
            Go();
        currentIndex = 1;
    }
    public void Inoskae()
    {
        if(currentIndex==2)
            Go();
        currentIndex = 2;
    }
    public void Giyu()
    {
        if(currentIndex==3)
            Go();
        currentIndex = 3;
    }

    public void Go()
    {
        Instantiate(Players[currentIndex]);
        Time.timeScale = 1;
        CameraManager.instance.GameStart();
        Destroy(gameObject);
    }
}
