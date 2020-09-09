using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fade2 : MonoBehaviour
{
    public static Fade2 instance;
    public float delay;
    public float value;
    private Color color;
    private Image image;
    

    void Start()
    {
        instance = this; 
        image = GetComponent<Image>();
    }

    public void change(int a)
    {
        StartCoroutine(fadeCor(a));
    }
    IEnumerator fadeCor(int a)
    {
        GetComponent<Image>().raycastTarget = true;
        while (color.a< 1)
        {
            yield return new WaitForSecondsRealtime(delay);
            color.a += value;
            image.color = color;
        }
        BulletData.instance.currentColorIndex = a;
        StartCoroutine(UnfadeCor());
    }
    IEnumerator UnfadeCor()
    {
        while (color.a>0)
        {
            yield return new WaitForSecondsRealtime(delay);
            color.a -= value;
            image.color = color;
        }
        GetComponent<Image>().raycastTarget = false;
    }
}