using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class color
{
    public Color color1, color2;
}
[System.Serializable]
public class realcolor
{
    public Color color1, color2;
}
public class BulletData : MonoBehaviour
{
    public float speedValue = 1;
    public bool isSplash = false;
    public float brightness;
    public Material[] Themes;
    public GameObject[] Colors;
    public Sprite[] tileThemes;
    public AudioClip[] Clips;
    public float[] BPMs;
    public int currentColorIndex;
    public color[] colors;
    public realcolor[] Realcolors;
    public static BulletData instance;
    public float playerAroundValue; //플레이어 방향으로 직선 이동하는 동그라미가 인식하는 부분
    public string currentColorKey = "currentColor";
    private string clearValueKey = "clearValue";
    public int clearValue = 0;
    public int[] isLockArray;
    public bool isDeleteAD = false;
    public string DeleteAdKey = "DeleteAD";
    public string[] keys;

    public void ClearValueUp()
    {
        clearValue++;
        PlayerPrefs.SetInt(clearValueKey,clearValue);
        if(clearValue>=5)
            GooglePlayManager.instance.Achievement(12);
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            currentColorIndex = PlayerPrefs.GetInt(currentColorKey, 0);
            clearValue = PlayerPrefs.GetInt(clearValueKey, 0);
            isDeleteAD = PlayerPrefs.GetInt(DeleteAdKey, 0) == 0 ? false : true;
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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
            Reset();
        if(Input.GetKeyDown(KeyCode.U))
            UnlockAll();
        if (Input.GetKeyDown(KeyCode.O))
        {
            PlayerPrefs.SetInt(DeleteAdKey,1);
            isDeleteAD = PlayerPrefs.GetInt(DeleteAdKey, 0) == 0 ? false : true;
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
