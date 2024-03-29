﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
            {
                if (SceneManager.GetActiveScene().name == "Main3" || SceneManager.GetActiveScene().name == "Main3_EZ" ||
                    SceneManager.GetActiveScene().name == "Main3_H")
                    slider.value += 2;
                else
                    slider.value += 1;
            }
            else
            {
                if (v >= 10)
                    slider.value += v * 2.5f;
                else if (v >= 5)
                    slider.value += v * 2;
                else if (v >= 2)
                    slider.value += v*1.5f;
                if (SceneManager.GetActiveScene().name == "Main3" || SceneManager.GetActiveScene().name == "Main3_EZ" ||
                    SceneManager.GetActiveScene().name == "Main3_H")
                {
                    if (v >= 10)
                        slider.value += v * 5f;
                    else if (v >= 5)
                        slider.value += v * 4;
                    else if (v >= 2)
                        slider.value += v*3f;
                }
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

    public void ValueUp2(int v)
    {
        if (!isKagura)
        {
            if (v == 1)
            {
                if (SceneManager.GetActiveScene().name == "Main3" || SceneManager.GetActiveScene().name == "Main3_EZ" ||
                    SceneManager.GetActiveScene().name == "Main3_H")
                    slider.value += 2;
                else
                    slider.value += 1;
            }
            else
            {
                if (v >= 10)
                    slider.value += v * 1.5f;
                else if (v >= 5)
                    slider.value += v * 1.25f;
                else if (v >= 2)
                    slider.value += v*1f;
                if (SceneManager.GetActiveScene().name == "Main3" || SceneManager.GetActiveScene().name == "Main3_EZ" ||
                    SceneManager.GetActiveScene().name == "Main3_H")
                {
                    if (v >= 10)
                        slider.value += v * 3f;
                    else if (v >= 5)
                        slider.value += v * 2.4f;
                    else if (v >= 2)
                        slider.value += v*2f;
                }
            }
            if (slider.value >= slider.maxValue)
            {
                isKagura = true;
                StartCoroutine(Barrier());
            }
        }
    }
    IEnumerator Barrier()
    {
        SoundManager.instance.waterDrop();
        fire.SetActive(true);
        Player.instance.OnBarrier();
        yield return new WaitUntil(()=>Player.instance.isBarrier==false);
        print("끝!");
        slider.value = 0;
        fire.SetActive(false);
        isKagura = false;
    }
}
