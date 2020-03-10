﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bonusText : MonoBehaviour
{
    private Text text;
    Color color;

    private void Start()
    {
        text = GetComponent<Text>();
        color.r = 255;
        color.g = 255;
        color.b = 0;
        color.a = 1;
    }
    void Update()
    {
        if (color.a > 0)
        {
            color.a -= 0.01f;
            text.color = color;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
