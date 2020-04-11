using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class kagura : MonoBehaviour
{
    public GameObject effect;
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
            if (v == 1)
                slider.value += 1f;
            else
            {
                if (v >= 10)
                    slider.value += v * 3;
                else if (v >= 5)
                    slider.value += v * 2;
                else if (v >= 2)
                    slider.value += v*1.5f;
            }
            if (slider.value >= slider.maxValue)
            {
                isKagura = true;
                StartCoroutine(hinokami());
            }
        }
    }

    IEnumerator hinokami()
    {
        SoundManager.instance.kaguraON();
        effect.SetActive(true);
        fire.SetActive(true);
        while (slider.value>0)
        {
            yield return new WaitForSeconds(0.01f);
            slider.value -= 0.175f;
        }
        fire.SetActive(false);
        effect.SetActive(false);
        isKagura = false;
    }
}
