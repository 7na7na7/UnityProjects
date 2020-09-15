using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class BGM : MonoBehaviour
{
   // public Slider ClearSlider;
    public static BGM instance;
    
    public float speed;
    public AudioSource source;
    private bool isClear = false;
    private void Start()
    {
        source = GetComponent<AudioSource>();
        if (SceneManager.GetActiveScene().name != "Title")
        {
            source.clip = BulletData.instance.Clips[BulletData.instance.currentColorIndex];
            source.pitch = BulletData.instance.pitches[BulletData.instance.currentColorIndex];   
        }
        instance = this;
        source.volume = SoundMgr.instance.savedBgm;
        if (!BulletData.instance.isSplash)
        {
            StartCoroutine(delayPlay());
        }
        else
            source.Play();
//        if (ClearSlider != null)
//        {
//            ClearSlider.maxValue = source.clip.length;
//            ClearSlider.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color =
//                BulletData.instance.colors[BulletData.instance.currentColorIndex].color1;
//        }
    }

    private void Update()
    {
        if (!isClear)
        {
            if (source.time >= source.clip.length-0.1f)
            {
                Flash.instance.flash();
                ScoreMgr.instance.scoreUp(0,5000,false,false);
                isClear = true;
                GooglePlayManager.instance.Achievement(11);
                BulletData.instance.ClearValueUp();
            }
        }
    }
    IEnumerator delayPlay()
    {
        yield return new WaitForSeconds(1.5f);
        source.Play();
        AdmobVideoScript.instance.SetAD();
        GooglePlayManager.instance.Set();
    }

    public void fadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(fadeOutCor());
    }

    IEnumerator fadeOutCor()
    {
        while (source.volume>0)
        {
            source.volume = Mathf.Lerp(source.volume, 0, speed);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    public void fadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(fadeInCor());
    }

    IEnumerator fadeInCor()
    {
        while (source.volume<1)
        {
            source.volume = Mathf.Lerp(source.volume, SoundMgr.instance.savedBgm, speed);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
