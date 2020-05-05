using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartManager : MonoBehaviour
{
    public static HeartManager instance;
    public int heartCount = 0;
    public GameObject[] hearts;

    private void Start()
    {
        instance = this;
        heartCount = hearts.Length;
        
        //1280 * 720에 맞추기 위해 처음에 약간 왼쪽으로 땡겨주기
        GameObject[] heartsObjs;
        heartsObjs = GameObject.FindGameObjectsWithTag("heart");
        foreach (GameObject rt in heartsObjs)
        {
            rt.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                rt.GetComponent<RectTransform>().anchoredPosition.x-50,
                rt.GetComponent<RectTransform>().anchoredPosition.y);
        }
    }

    public void tanjiro()
    {
        StartCoroutine(tan());
    }

    IEnumerator tan()
    {
        yield return new WaitForSeconds(0.01f);
        GameObject[] heartsObjs;
        heartsObjs = GameObject.FindGameObjectsWithTag("heart");
        if (Player.instance.playerIndex == 1||Player.instance.playerIndex==3) //기유 또는 탄지로면
        {
            foreach (GameObject rt in heartsObjs)
            {
                rt.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                    rt.GetComponent<RectTransform>().anchoredPosition.x,
                    rt.GetComponent<RectTransform>().anchoredPosition.y - 100);
            }
        }
    }
    public void Damaged1()
    {
        if (Player.instance.playerIndex == 3&&Player.instance.isBarrier)
        {
            Player.instance.OffBarrier();
        }
        else
        {
            heartCount--;
           hearts[heartCount].gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (heartCount <= 0)
        {
            Player.instance.Realdie();
        }  
    }

    public void damaged2()
    {
        if (!Player.instance.isSuper)
        {
            SoundManager.instance.hit();
            StartCoroutine(superTime());   
        }
    }
    
    IEnumerator superTime()
    {
        if (Player.instance.GetComponent<SpriteRenderer>() != null)
        {
            if (heartCount >= 1)
            {
                Player.instance.isSuper = true;
                Color color;
                SpriteRenderer sprite = Player.instance.GetComponent<SpriteRenderer>(); //스프라이트로 함
                color.r = 255;
                color.g = 255;
                color.b = 255;
                color.a = 0.5f;
                sprite.color = color;
                yield return new WaitForSeconds(0.1f);
                color.a = 1f;
                sprite.color = color;
                yield return new WaitForSeconds(0.1f);
                color.a = 0.5f;
                sprite.color = color;
                yield return new WaitForSeconds(0.1f);
                color.a = 1f;
                sprite.color = color;
                yield return new WaitForSeconds(0.1f);
                color.a = 0.5f;
                sprite.color = color;
                yield return new WaitForSeconds(0.1f);
                color.a = 1f;
                sprite.color = color;
                yield return new WaitForSeconds(0.1f);
                color.a = 0.5f;
                sprite.color = color;
                yield return new WaitForSeconds(0.1f);
                color.a = 1f;
                sprite.color = color;
                yield return new WaitForSeconds(0.1f);
                color.a = 0.5f;
                sprite.color = color;
                yield return new WaitForSeconds(0.1f);
                color.a = 1f;
                sprite.color = color;
                yield return new WaitForSeconds(0.1f);
                color.a = 0.5f;
                sprite.color = color;
                yield return new WaitForSeconds(0.1f);
                color.a = 1f;
                sprite.color = color;
                yield return new WaitForSeconds(0.1f);
                color.a = 0.5f;
                sprite.color = color;
                yield return new WaitForSeconds(0.1f);
                color.a = 1f;
                sprite.color = color;
                yield return new WaitForSeconds(0.2f);
                Player.instance.isSuper = false;
            }
        }

        yield break;
    }
}
