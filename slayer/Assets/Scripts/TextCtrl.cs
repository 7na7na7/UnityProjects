using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextCtrl : MonoBehaviour
{
    public string kor, eng;

    void Update()
    {
        if (TextManager.instance.isKor == 1) //한국어
            GetComponent<Text>().text = kor;
        else //영어
            GetComponent<Text>().text = eng;
    }
    
}
