using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmManager : MonoBehaviour
{
    public AudioClip boss;
    private AudioClip saved;
    public float bgmValue = 1;
    void Start()
    {
        saved = GetComponent<AudioSource>().clip;
        GetComponent<AudioSource>().volume = SoundManager.instance.savedBgm * bgmValue;
    }

    public void bossFunc()
    {
        GetComponent<AudioSource>().clip = boss;
        GetComponent<AudioSource>().Play();
    }

    public void bossDie()
    {
        GetComponent<AudioSource>().clip = saved;
        GetComponent<AudioSource>().Play(); 
    }
}
