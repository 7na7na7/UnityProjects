using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class color
{
    public Color color1, color2;
}
public class BulletData : MonoBehaviour
{
    public int isRight;
    private string isRightKey = "isRight";
    public float brightness;
    public Material[] Themes;
    public GameObject[] Colors;
    public Sprite[] tileThemes;
    public AudioClip[] Clips;
    public float[] BPMs;
    public int currentColorIndex;
    public color[] colors;
    public static BulletData instance;
    public float playerAroundValue; //플레이어 방향으로 직선 이동하는 동그라미가 인식하는 부분
    public string currentColorKey = "currentColor";
    public float[] pitches;
    public int[] isLockArray;

    public string[] keys;
    
    public void right()
    {
        isRight = 1;
        PlayerPrefs.SetInt(isRightKey,1);
    }

    public void left()
    {
        isRight = 0;
        PlayerPrefs.SetInt(isRightKey,0);
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            currentColorIndex = PlayerPrefs.GetInt(currentColorKey, 0);
            isRight = PlayerPrefs.GetInt(isRightKey, 1);

            for (int i = 0; i < keys.Length; i++)
            {
                if (i==0)
                {
                    isLockArray[i] = PlayerPrefs.GetInt(keys[i],1);
                }
                else
                {
                    isLockArray[i] = PlayerPrefs.GetInt(keys[i],0);
                }
               
            }

        }
        else
        {
            Destroy(gameObject);
        }
    }
    

    public Color SetColor(int index)
    {
        if (index == 0)
            return colors[currentColorIndex].color1;
        else
            return colors[currentColorIndex].color2;
    }

    public int getCurrentColor()
    {
        return currentColorIndex;
    }
    public void Unlock(int value)
    {
        for (int i = 0; i < isLockArray.Length; i++)
        {
            if(i==value) 
                isLockArray[i] = 1;
            PlayerPrefs.SetInt(keys[i],isLockArray[i]);
        }
    }

    public void Reset()
    {
        for (int i = 0; i < isLockArray.Length; i++)
        {
            isLockArray[i] = 0;
            if(i==0)
                isLockArray[i] = 1;
            PlayerPrefs.SetInt(keys[i],isLockArray[i]);
        }
    }
    public void UnlockAll()
    {
        for (int i = 0; i < isLockArray.Length; i++)
        {
            isLockArray[i] = 1;
            if(i==0)
                isLockArray[i] = 1;
            PlayerPrefs.SetInt(keys[i],isLockArray[i]);
        }
    }
}
