using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetColor : MonoBehaviour
{
    public bool startSetColor = false;
    public int ColorIndex;
    public Renderer RD;
    //private Color color;

    private void Start()
    {
        if(startSetColor)
            setColor();
    }

    public void setColor()
    {
        //color = BulletData.instance.SetColor(ColorIndex);
        //color.a = 0;
        GetComponent<SpriteRenderer>().color = BulletData.instance.SetColor(ColorIndex);
        //RD.material.SetColor("Color",color);
    }
}
