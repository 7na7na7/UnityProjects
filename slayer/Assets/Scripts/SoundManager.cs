using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    public AudioClip tsuzumiL, tsuzumiM, tsuzumiR;
    public AudioClip hitSound;
    public AudioClip swingSound;
    public AudioClip bodySound;
    public AudioClip headSound;
    public AudioClip girlSound;
    public AudioClip selectSound;
    public AudioClip comboSound;
    public AudioClip scoreCountSound;
    public AudioClip healSound;
    public AudioClip knifeCoverSound;
    public static SoundManager instance;
    private AudioSource audio;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
       
        audio = GetComponent<AudioSource>();
    }

    public void knifeCover()
    {
        audio.PlayOneShot(knifeCoverSound);
    }
    public void heal()
    {
        audio.PlayOneShot(healSound);
    }
    public void scoreCount()
    {
        audio.PlayOneShot(scoreCountSound);
    }
    public void tsuzumi(int v)
    {
        if(v==0)
            audio.PlayOneShot(tsuzumiL);
        else if(v==1)
            audio.PlayOneShot(tsuzumiM);
        else if(v==2)
            audio.PlayOneShot(tsuzumiR);
    }
    public void swing()
    {
        audio.PlayOneShot(swingSound);
    }

    public void body()
    {
        audio.PlayOneShot(bodySound);
    }

    public void head()
    {
        audio.PlayOneShot(headSound,0.5f);
    }

    public void hit()
    {
        audio.PlayOneShot(hitSound,0.5f);
    }

    public void girl()
    {
        audio.PlayOneShot(girlSound,1f);
    }

    public void combo()
    {
        audio.PlayOneShot(comboSound,1f);
    }

    public void select()
    {
        audio.PlayOneShot(selectSound,1f);
    }
}
