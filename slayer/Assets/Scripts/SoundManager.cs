using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip hitSound;
    public AudioClip swingSound;
    public AudioClip bodySound;
    public AudioClip headSound;
    public AudioClip girlSound;
    public AudioClip selectSound;
    public AudioClip comboSound;
    public static SoundManager instance;
    private AudioSource audio;
    void Start()
    {
        instance = this;
        audio = GetComponent<AudioSource>();
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
