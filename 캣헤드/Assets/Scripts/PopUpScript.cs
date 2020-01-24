using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpScript : MonoBehaviour
{
    public Text text;
    public float textFadeSpeed;
    public float fadeSpeed;
    private void OnEnable()
    {
      StopAllCoroutines();
      StartCoroutine(fade());
    }

    IEnumerator fade()
    {
        Color color;
        color =text.color;
        while (color.a <= 0)
        {
            color.a -= textFadeSpeed;
            text.color = color;
            yield return new WaitForSeconds(fadeSpeed);
        }
    }
}
