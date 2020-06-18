using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public float pitch;
    private AudioSource source;
    
    void Start()
    {
        source = GetComponent<AudioSource>();    
    }

    public void Play()
    {
        source.pitch = pitch;
        source.Play();
    }
    public void Record()
    {
        source.clip = Microphone.Start(Microphone.devices[0].ToString(), false, FindObjectOfType<PlayerScript>().hearLength, 44100);
        //3초 녹음
    }
}
