using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class girlText : MonoBehaviour
{
    public string[] texts;
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
        text.text = texts[Random.Range(0, texts.Length)];
        
        Destroy(gameObject,destroyDelay);
    }
}
