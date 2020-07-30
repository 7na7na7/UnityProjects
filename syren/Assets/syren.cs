using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class syren : MonoBehaviour
{
    public float maxvolume; //최대 볼륨
    public float minvolume; //최소 볼륨
    public float maxpitch; //최대 사이렌 높이
    public float minpitch; //최소 사이렌 높이
    public float pitchChangeSpeed; //높이가 변하는 빠르기
    public float volumeChangeSpeed; //볼륨이 변하는 빠르기
    AudioSource source;
    public bool isTouch = false; //누르는 중인지 판단하는 변수
    void Start()
    {
        source = GetComponent<AudioSource>();
        source.pitch = minpitch;
        source.volume = minvolume;
        StartCoroutine(cor());
    }
    
    IEnumerator cor()
    {
        while (true)
        {
            source.Play();
            yield return new WaitForSeconds(source.clip.length);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (isTouch)
                isTouch = false;
            else
                isTouch = true;
        }
        if (isTouch)
        {
            if (source.pitch < maxpitch)
                source.pitch += pitchChangeSpeed * Time.deltaTime;
            if (source.volume <maxvolume)
                source.volume += volumeChangeSpeed * Time.deltaTime;
        }
        else
        {
            if (source.pitch > minpitch)
                source.pitch -= pitchChangeSpeed * Time.deltaTime;
            if (source.volume > minvolume)
                source.volume -= volumeChangeSpeed * Time.deltaTime;
        }
    }

    public void BtnOn() //사이렌을 울릴 때 호출!
    {
        isTouch = true;
    }

    public void BtnOff() //사이렌을 낮출 때 호출!
    {
        isTouch = false;   
    }
}
