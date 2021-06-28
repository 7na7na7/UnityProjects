using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    private NemoManager nemoManager;
    public bool isBlack = false;
    private SpriteRenderer spr;
    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
        nemoManager = FindObjectOfType<NemoManager>();
    }
  

    public void SetData(bool _isBlack)
    {
        isBlack = _isBlack;
    }

    void OnMouseUp()
    {
        if (spr.color == Color.white)
        {
            if (isBlack)
                nemoManager.correctCount--;
            else
                nemoManager.unCorrectCount++;
            spr.color=Color.black;
        }
        else
        {
            if (isBlack)
                nemoManager.correctCount++;
            else
                nemoManager.unCorrectCount--;
            spr.color = Color.white;
        }
        if (isBlack)
        {
        }
        else
        {
        }
    }
}
