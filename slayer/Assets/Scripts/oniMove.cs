using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class oniMove : MonoBehaviour
{
    public bool isJumping;
    public bool canGo;
    public int hp = 2;
    public int oniIndex;
    public float minX, maxX;
    private bool isStop = false;
    public GameObject effect;
    public GameObject headEffect;
    public float speed;
    
    private Animator anim;
    public float dmgDelay = 0;
    public float speedUpValue = 1.1f;
    public int hpUpValue = 1;
    private void Start()
    {
        anim = GetComponent<Animator>();
        for (int i = 0; i < GameManager.instance.bossCount; i++)
            speed *= speedUpValue;
        hp += hpUpValue * (int)(GameManager.instance.bossCount / 2);
    }

    void Update()
    {
        if (isJumping)
            transform.right = GetComponent<Rigidbody2D>().velocity;
        
        if(canGo)
        {
            if (!isStop)
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
            }
        }

        if (!isStop)
        { 
            if (transform.position.x >= minX && transform.position.x <= maxX) 
            { 
                StartCoroutine(attack());
                isStop = true;
            }
        }

        if (dmgDelay < Time.deltaTime*10)
            dmgDelay += Time.deltaTime;
    }

    IEnumerator attack()
    {
        if (!isStop)
        {
            while (true)
            {
                if (oniIndex == 1)
                {
                    anim.Play("oni1Attack");
                    StartCoroutine(girl.instance.hitted(10));
                    yield return new WaitForSeconds(0.5f);
                    anim.Play("oni1Idle");
                    yield return new WaitForSeconds(1f);
                }
                else if(oniIndex==2)
                {
                    anim.Play("oni2Attack");
                    StartCoroutine(girl.instance.hitted(20));
                    yield return new WaitForSeconds(0.5f);
                    anim.Play("oni2Idle");
                    yield return new WaitForSeconds(2f);
                }
                else if(oniIndex==3)
                {
                    anim.Play("oni3_Attack");
                    StartCoroutine(girl.instance.hitted(5));
                    yield return new WaitForSeconds(0.5f);
                    anim.Play("oni3_Idle");
                    yield return new WaitForSeconds(1f);
                }
            }
        }
        else
        {
            yield return null;
        }
    }

    public void die(bool isHead)
    {
        if (dmgDelay >= 0.1f)
        { 
            Player.instance.AttackCor();
            if (isHead)
            {
                ScoreMgr.instance.headshot++;
                SoundManager.instance.head();

                hp -= 2;
                if (hp <= 0)
                {
                    if (oniIndex == 1)
                        ScoreMgr.instance.scoreUp(0,100, false);
                    else if (oniIndex == 2)
                        ScoreMgr.instance.scoreUp(0,150, false);
                    else if(oniIndex==3) 
                        ScoreMgr.instance.scoreUp(0,100,false);
                    ComboManager.instance.comboIniitailize();
                    ScoreMgr.instance.killedOni++;
                    CameraManager.instance.closeUp();
                    Instantiate(headEffect, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
            }
            else
            {
                SoundManager.instance.body();
                hp--;
                if (hp <= 0)
                {
                    if (oniIndex == 1)
                        ScoreMgr.instance.scoreUp(0,100, false);
                    else if (oniIndex == 2)
                        ScoreMgr.instance.scoreUp(0,150, false);
                    else if(oniIndex==3) 
                        ScoreMgr.instance.scoreUp(0,100,false);
                    ComboManager.instance.comboIniitailize();
                    ScoreMgr.instance.killedOni++;
                    CameraManager.instance.closeUp();
                    Instantiate(effect, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
            }
            Player.instance.ComboText(isHead);
            dmgDelay = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!Player.instance.isattack)
            {
                if (oniIndex == 1||oniIndex==3)
                {
                    if(oniIndex==1) 
                        anim.Play("oni1Attack");
                    else if(oniIndex==3)
                        anim.Play("oni3_Attack");
                    if (canGo)
                    {
                        if (other.transform.position.x < transform.position.x)
                            transform.localScale = new Vector3(1, 1, 1);
                        else if (other.transform.position.x > transform.position.x)
                            transform.localScale = new Vector3(-1, 1, 1);
                    }
                }
                else if(oniIndex==2)
                    anim.Play("oni2Attack");
                
            }
        }

        if (other.CompareTag("Ground"))
        {
            if (!canGo)
            {
                if (speed < 0)
                {
                    if(oniIndex==1) 
                        transform.localScale = new Vector3(1, 1, 1);
                    else if(oniIndex==2)
                        transform.localScale = new Vector3(5, 5, 1);
                }
                canGo = true;
                transform.eulerAngles = new Vector3(0, 0, 0);
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                if (oniIndex == 1)
                {
                    anim.Play("oni1Anim");
                    transform.Translate(0,   Random.Range(-0.1f,-0.05f), 0);
                }
                else if (oniIndex == 2)
                {
                    anim.Play("oni2_walk");
                    //transform.Translate(0,   Random.Range(0.5f,1f), 0);
                }
                else if (oniIndex == 3)
                {
                    anim.Play("oni3_walk");
                }
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
               
            }
        }
    }
}
