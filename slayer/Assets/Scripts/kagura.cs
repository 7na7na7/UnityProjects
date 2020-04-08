using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class kagura : MonoBehaviour
{
    public GameObject fire;
    public bool isKagura = false;
    public Slider slider;
    public static kagura instance;

    private void Start()
    {
        instance = this;
    }

    public void valueUp(int v)
    {
        if (!isKagura)
        {
            if (v >= 1)
                slider.value += 100;
            if (v >= 10)
                slider.value += v * 5;
            else if (v >= 5)
                slider.value += v * 4;
            else if (v >= 2)
                slider.value += v * 3;
            if (slider.value >= slider.maxValue)
            {
                isKagura = true;
                StartCoroutine(hinokami());
            }
        }
    }

    IEnumerator hinokami()
    {
        fire.SetActive(true);
        while (slider.value>0)
        {
            yield return new WaitForSeconds(0.01f);
            slider.value -= 0.2f;
        }
        fire.SetActive(false);
        isKagura = false;
    }
}
