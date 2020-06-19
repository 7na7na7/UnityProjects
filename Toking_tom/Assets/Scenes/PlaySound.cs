using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaySound : MonoBehaviour
{
    public float pitch;
    private AudioSource source;
    
    void Start()
    {
        source = GetComponent<AudioSource>();    
    }

    public void Play(bool isSec)
    {
        source.pitch = pitch;
        source.Play();
        if (isSec)
        {
            FindObjectOfType<ClipSaver>().savedClip = source.clip;
            FindObjectOfType<ClipSaver>().savedPitch = pitch;
        }
    }
    public void Record()
    {
        source.clip = Microphone.Start(Microphone.devices[0].ToString(), false, FindObjectOfType<PlayerScript>().hearLength, 44100);
        //3초 녹음
    }

    public void Quit()
    {
        Application.Quit(); 
    }
}
