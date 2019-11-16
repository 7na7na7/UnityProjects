using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BtnScript : MonoBehaviour
{
    public Text effectText, musicText;
    private SandbagScript[] sandbags;

    private void Start()
    {
        if (dontdestroy.instance.music)
        {
            musicText.text = "Music\nOn";
        }
        else
        {
            musicText.text = "Music\nOff";
        }
            
        if (dontdestroy.instance.effect)
        {
            effectText.text = "Sound\nOn";
        }
        else
        {
            effectText.text = "Sound\nOff";
        }
    }

    public void punch(string dir)
    {
        if (!PlayerScript.instance.isdead)
        {
            sandbags = FindObjectsOfType<SandbagScript>();
            for (int i = 0; i < sandbags.Length; i++)
            {
                sandbags[i].onTouch();
            }

            PlayerScript.instance.onPunch(dir);
        }
    }

    public void restart()
    {
        SceneManager.LoadScene("Play");
    }

    public void title()
    {
        SceneManager.LoadScene("Title");
    }

    public void musicChange()
    {
        if (dontdestroy.instance.music)
        {
            musicText.text = "Music\nOff";
            dontdestroy.instance.music = false;
        }
        else
        {
            musicText.text = "Music\nOn";
            dontdestroy.instance.music = true;
        }
    }

    public void soundChange()
    {
        if (dontdestroy.instance.effect)
        {
            effectText.text = "Sound\nOff";
            dontdestroy.instance.effect = false;
        }
        else
        {
            effectText.text = "Sound\nOn";
            dontdestroy.instance.effect = true;
        }
    }

    public void Creator()
    {
        SceneManager.LoadScene("Creator");
    }
}
