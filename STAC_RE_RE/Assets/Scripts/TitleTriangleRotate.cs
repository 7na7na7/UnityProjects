using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleTriangleRotate : MonoBehaviour
{
    public Image color1_1, color1_2, color2_1, color2_2;
    public float speed;
    private RectTransform rect;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        Color color;
        color1_1.color=BulletData.instance.Realcolors[BulletData.instance.currentColorIndex].color1;
        color=BulletData.instance.Realcolors[BulletData.instance.currentColorIndex].color1;
        color.a = 0.5f;
        color1_2.color = color;
        color2_1.color=BulletData.instance.Realcolors[BulletData.instance.currentColorIndex].color2;
        color=BulletData.instance.Realcolors[BulletData.instance.currentColorIndex].color2;
        color.a = 0.5f;
        color2_2.color = color;
        rect.eulerAngles=new Vector3(rect.eulerAngles.x,rect.eulerAngles.y,rect.eulerAngles.z+Time.deltaTime*speed);
    }
}
