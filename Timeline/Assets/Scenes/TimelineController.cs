using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.Playables;
public class TimelineController : MonoBehaviour
{
    public PlayableDirector playableDirector;
    void Update()
    {
        if (Input.anyKeyDown)
        {
            playableDirector.gameObject.SetActive(true);
            playableDirector.Play();
        }
    }
}
