using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class BGM : MonoBehaviour
{
    public static BGM instance;
    
    public float speed;
    private AudioSource source;

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
