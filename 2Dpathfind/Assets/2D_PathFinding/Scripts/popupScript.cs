using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class popupScript : MonoBehaviour
{
    public Text text;
    public float textFadeSpeed;
    public float fadeSpeed;
    public float canGoDelay;
    private bool canGo = false;
    private void Start()
    {
        StartCoroutine(fade());
        StartCoroutine(delayGo());
    }

    IEnumerator fade()
    {
        Color color;
        color = text.color;

        color.a = 1;
        text.color = color;

        while (true)
        {
            if (canGo)
            {
                if (color.a > 0)
                {
                    color.a -= textFadeSpeed;
                    text.color = color;
                }
                else
                    break;
            }
            yield return new WaitForSeconds(fadeSpeed);
        }
        Destroy(gameObject);
        yield return null;
    }

    IEnumerator delayGo()
    {
        yield return new WaitForSeconds(canGoDelay);
        canGo = true;
    }
}