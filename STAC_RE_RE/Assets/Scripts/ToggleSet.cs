using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSet : MonoBehaviour
{
    private Image img;

    private void Start()
    {
        img = GetComponent<Image>();
    }

    void Update()
    {
        img.color = BulletData.instance.colors[BulletData.instance.currentColorIndex].color2;

    }
}
