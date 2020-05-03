using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadePanel : MonoBehaviour
{
    public bool isFade = false;
    public static FadePanel instance;
    private Color color;
    public Image img;
    public float value;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        color.r = 0;
        color.g = 0;
        color.b = 0;
        color.a = 1;
    }

    public void Fade()
    {
        isFade = true;
    }

    public void UnFade()
    {
        isFade = false;
    }

    public void rightFade()
    {
        color.a = 1;
        img.color = color;
    }

    public void rightUnFade()
    {
        color.a = 0;
        img.color = color;
    }
    private void Update()
    {
        if (isFade)
        {
            if (color.a <1)
            {
                color.a += value;
                img.color = color;
            } 
        }
        else
        {
            if (color.a >0)
            {
                color.a -= value;
                img.color = color;
            }
        }
    }
}
