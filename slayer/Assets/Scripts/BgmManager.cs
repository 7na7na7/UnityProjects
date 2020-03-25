using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmManager : MonoBehaviour
{
    void Start()
    {
        GetComponent<AudioSource>().volume = SoundManager.instance.savedBgm;
    }
}
