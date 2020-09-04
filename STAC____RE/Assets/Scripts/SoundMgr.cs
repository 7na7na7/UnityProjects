using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundMgr : MonoBehaviour
{
    public static SoundMgr instance;
    public AudioSource source;

    public AudioClip[] clips;
    
    private string bgmKey = "bgmKey";
    private string seKey = "seKey";
    public float savedBgm;
    public float savedSE;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            savedBgm = PlayerPrefs.GetFloat(bgmKey,1);
            savedSE = PlayerPrefs.GetFloat(seKey,1);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Play(int index,float volume,float pitch)
    {
         volume=Mathf.Clamp(savedSE*volume,0f, 1f);
         source.pitch = pitch;
         source.PlayOneShot(clips[index],volume);
    }
    
    public void bgmValue(float v)
    {
        FindObjectOfType<BGM>().GetComponent<AudioSource>().volume = v;
        savedBgm = v;
        PlayerPrefs.SetFloat(bgmKey,savedBgm);
    }

    public void seValue(float v)
    {
        savedSE = v;
        PlayerPrefs.SetFloat(seKey,savedSE);
    }
/*
    #region 발표용
    private void Start()
    {
        GameObject.Find("BGM").GetComponent<AudioSource>().Pause();
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (SceneManager.GetActiveScene().name == "Title")
            {
                if (GameObject.Find("BGM").GetComponent<AudioSource>().isPlaying)
                    GameObject.Find("BGM").GetComponent<AudioSource>().Pause();
                else
                    GameObject.Find("BGM").GetComponent<AudioSource>().UnPause();
            }
        }
    }
    #endregion
   */
}
