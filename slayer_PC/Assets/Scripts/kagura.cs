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
            slider.value += v * 2;
            if (slider.value >= slider.maxValue)
            {
                isKagura = true;
                StartCoroutine(hinokami());
            }
        }
    }

    IEnumerator hinokami()
    {
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
