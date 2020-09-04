using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColor : MonoBehaviour
{
    public Text priceText;
    public GameObject Price;
    public int ColorIndex = 0;
    public GameObject lockImg;
    float speed;
    private RectTransform rect;

    private void Start()
    {
        rect = transform.GetChild(0).GetComponent<RectTransform>();
        speed = FindObjectOfType<TitleTriangleRotate>().speed;
    }
    
    public void ColorChange()
    {
        if (BulletData.instance.isLockArray[ColorIndex] == 1)
        {
            BulletData.instance.currentColorIndex = ColorIndex;
            PlayerPrefs.SetInt(BulletData.instance.currentColorKey,ColorIndex);
        }
    }

    private void Update()
    {
        if (BulletData.instance.isLockArray[ColorIndex] == 0)
        {
            Price.SetActive(true);
            lockImg.SetActive(true);
        }
        else
        {
            Price.SetActive(false);
            lockImg.SetActive(false);
            if(BulletData.instance.currentColorIndex == ColorIndex) 
                rect.eulerAngles=new Vector3(rect.eulerAngles.x,rect.eulerAngles.y,rect.eulerAngles.z+Time.deltaTime*speed*3);
            else
                rect.eulerAngles=new Vector3(rect.eulerAngles.x,rect.eulerAngles.y,rect.eulerAngles.z+Time.deltaTime*speed/2f);
        }
    }

    public void Unlock()
    {
        
        if (GoldManager.instance.isGold(int.Parse(priceText.text))) //골드가 있으면
        {
            GoldManager.instance.LoseGold(int.Parse(priceText.text));   
            BulletData.instance.Unlock(ColorIndex);
            ColorChange();
        }
        else
        {
            print("골드가 부족합니다!");
        }
    }
}
