using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashImg : MonoBehaviour
{
    public Image img1, img2;
    public Text txt;
    public float delay;
    public float value;
    void Start()
    {
        if (BulletData.instance.isSplash == false)
        {
            StartCoroutine(fade());   
        }
        else
        {
            img2.gameObject.SetActive(false);
            img1.gameObject.SetActive(false);
            txt.gameObject.SetActive(false);
        }
    }

    IEnumerator fade()
    { 
        yield return new WaitForSeconds(1.5f);
        Color color=Color.white;
        Color color2=Color.white;
        while (color.a>0f)
        {
            yield return new WaitForSecondsRealtime(delay);
            color = img1.color;
            color.a -= value;
            img1.color = color;

            color2 = img2.color;
            color2.a -= value;
            
            img2.color = color2;
            txt.color = color2;
        }
        img2.gameObject.SetActive(false);
        img1.gameObject.SetActive(false);
        txt.gameObject.SetActive(false);
        BulletData.instance.isSplash = true;
    }
}
