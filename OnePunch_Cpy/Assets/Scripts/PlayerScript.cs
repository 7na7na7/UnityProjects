using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerScript : MonoBehaviour
{
    public AudioSource audio2;
    public AudioSource audio;
    public AudioClip punchSoound;
    public AudioClip bgm;
    
    private int count = 0;
    public Text scoretext;
    
    public static PlayerScript instance;
    public GameObject gameover;

    private float plusvalue = 0f;
    public float plusvaluetime=0f;
    
    public Animator anim;
    
    public GameObject sandbag;
    public Transform left;
    public Transform right;

    public GameObject punch;
    public Transform leftPunch;
    public Transform rightPunch;


    public bool isdead = false;

    public Text highScore;
    private string keyString = "highScore"; //저장을 위해
    private int savedScore = 0;

    void Awake()
    {
        savedScore = PlayerPrefs.GetInt(keyString, 0); //저장된 하이스코어 불러오기
        highScore.text = "High Score : " + savedScore;
    }
    private void Start()
    {
        instance = this;
        anim.SetBool("isleft",true);
        if (dontdestroy.instance.music)
        {
            StartCoroutine(musicCor());
        }

        StartCoroutine(plusAccelerator());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("sandbag"))
        {
            if (anim.GetBool("isleft"))
            {
                anim.SetBool("isleftdead", true);
            }
            else
            {
                anim.SetBool("isrightdead", true);
            }

            isdead = true;
            gameover.SetActive(true);
            audio.mute = true;
        }
    }

    public void onPunch(string dir)
    {
        if (!isdead)
        {
            int r = Random.Range(0, 2);

            Instantiate(sandbag, r == 0 ? left : right);

            if (dir == "left")
            {
                Instantiate(punch, leftPunch);
                anim.SetTrigger("leftpunch");
                anim.SetBool("isleft", true);
            }

            if (dir == "right")
            {
                Instantiate(punch, rightPunch);
                anim.SetTrigger("rightpunch");
                anim.SetBool("isleft", false);
            }

            SliderScripts.instance.slider.value += 3; //회복
            SliderScripts.instance.slider.value += plusvalue;
            
            StartCoroutine(delaycount());
if(dontdestroy.instance.effect) 
    audio2.PlayOneShot(punchSoound,5f);
        }
    }

    IEnumerator delaycount()
    {
        yield return new WaitForSeconds(0.1f);
        if (!isdead)
        {
            count++;
            scoretext.text = "Current Score : " + count;
            
            if (count > savedScore)
            {
                highScore.text = "High Score : " + count;
                PlayerPrefs.SetInt(keyString, count); //하이스코어 저장
            }
        }
    }

    IEnumerator musicCor()
    {
        while (true)
        {
            audio.PlayOneShot(bgm);
            yield return new WaitForSeconds(18.5f);
        }
    }
    IEnumerator plusAccelerator()
    {
        while (true)
        {
            yield return new WaitForSeconds(plusvaluetime);
            {
                plusvalue += 1f;
            }
        }
    }
}
