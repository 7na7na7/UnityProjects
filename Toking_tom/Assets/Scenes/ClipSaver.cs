using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipSaver : MonoBehaviour
{
    public AudioClip savedClip;
    public float savedPitch;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
