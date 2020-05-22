using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class girlText : MonoBehaviour
{
    public string[] texts;
    public string[] textseng;
    public string[] textsjap;
    public Text text;
    public Sprite spr;
    public float destroyDelay;
    void Start()
    {
        int r = Random.Range(0, 2);
        if (r == 0)
        {
            transform.Translate(-2.3f,0,0);
            GetComponent<Image>().sprite = spr;
        }

        //대사 무작위
        if (TextManager.instance.isKor == 1) //한국어
            text.text = texts[Random.Range(0, texts.Length)];
        else  if (TextManager.instance.isKor == 0)//영어
            text.text = textseng[Random.Range(0, texts.Length)];
        else  if (TextManager.instance.isKor == 2)//일본어
            text.text = textsjap[Random.Range(0, texts.Length)];
      
        
        Destroy(gameObject,destroyDelay);
    }
}
