using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public AudioClip[] clips;
    private AudioSource[] ClipSources;
    public int SourceCount;
    private int index = 0;

    private void Awake()
    {
        ClipSources = new AudioSource[SourceCount];
        for (int i = 0; i < SourceCount; i++)
        {
            ClipSources[i] = gameObject.AddComponent<AudioSource>();
            ClipSources[i].Stop();
        }

        for (int j = 0; j < SourceCount; j++)
        {
            ClipSources[j].spatialBlend = 1f;
            ClipSources[j].minDistance = 8;
            ClipSources[j].maxDistance = 20;
            ClipSources[j].rolloffMode = AudioRolloffMode.Linear;
            ClipSources[j].loop = false;
            ClipSources[j].playOnAwake = false;
        }
    }


    public void Play(int clipIndex, float volume = 1f)
    {
        PlaySound(clipIndex, volume);
    }

    void PlaySound(int clipIndex, float volume)
    {
        if (index == SourceCount)
            index = 0;

        ClipSources[index].PlayOneShot(clips[clipIndex], volume);
        index++;
    }
}
