using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public bool isRevival = false;
    public static Fade instance;
    public float delay;
    public float value;
    private Color color;
    private Image image;
    

    void Start()
    {
        if (isRevival)
        {
            if (instance == null)
                instance = this;
            image = GetComponent<Image>();
            color = image.color;
        }
        else
        {
            image = GetComponent<Image>();
            color = image.color;
            if(SceneManager.GetActiveScene().name=="Play")
                Unfade();   
        }
    }

    public void fade()
    {
        StopAllCoroutines();
        StartCoroutine(fadeCor());
    }
    IEnumerator fadeCor()
    {
        GetComponent<Image>().raycastTarget = true;
        while (color.a< (isRevival==true ? 0.5f:1f))
        {
            yield return new WaitForSecondsRealtime(delay);
            color.a += value;
            image.color = color;
        }

        if (SceneManager.GetActiveScene().name == "Title")
            SceneManager.LoadScene("Play");
    }
    
    public void Unfade()
    {
        StopAllCoroutines();
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
