using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextCtrl : MonoBehaviour
{
    public string kor, eng, jap;

    void Update()
    {
        if (TextManager.instance.isKor == 1) //한국어
            GetComponent<Text>().text = kor;
        else if(TextManager.instance.isKor == 0)//영어
            GetComponent<Text>().text = eng;
        else if(TextManager.instance.isKor == 2) //일본어
            GetComponent<Text>().text = jap;
        
    }
    
}
