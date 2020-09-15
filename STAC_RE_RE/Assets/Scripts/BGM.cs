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
        source.Play();
//        if (ClearSlider != null)
//        {
//            ClearSlider.maxValue = source.clip.length;
//            ClearSlider.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color =
//                BulletData.instance.colors[BulletData.instance.currentColorIndex].color1;
//        }
    }

//    private void Update()
//    {
//        if (ClearSlider != null&&!isClear)
//        {
//            ClearSlider.value = source.time;
//            if (ClearSlider.value >= ClearSlider.maxValue-0.1f)
//            {
//                ClearSlider.gameObject.SetActive(false);
//                Flash.instance.flash();
//                isClear = true;
//                print("끝!");
//            }
//        }
//    }

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
