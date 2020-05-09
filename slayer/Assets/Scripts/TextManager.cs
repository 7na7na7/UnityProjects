using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    private string lanKey = "langKey";
    public int isKor = 1;
    public static TextManager instance;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            isKor = PlayerPrefs.GetInt(lanKey, 0);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Set(bool isKorB)
    {
        if (isKorB)
        {
            isKor = 1;
            PlayerPrefs.SetInt(lanKey,1);
        }
        else
        {
            isKor = 0;
            PlayerPrefs.SetInt(lanKey,0);
        }
    }
}
