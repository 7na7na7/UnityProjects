using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmManager : MonoBehaviour
{
    public float bgmValue = 1;
    void Start()
    {
        GetComponent<AudioSource>().volume = SoundManager.instance.savedBgm * bgmValue;
    }
}
