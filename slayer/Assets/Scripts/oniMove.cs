using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class oniMove : MonoBehaviour
{
    private bool isPoison = false;
    public bool isJumping;
    public bool canGo;
    public int hp = 2;
    public int oniIndex;
    public float minX, maxX;
    private bool isStop = false;
    public GameObject effect;
    public GameObject headEffect;
    public GameObject poisonEffect;
    public GameObject ExplodingBloodEffect;
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
        if(oniIndex==5)
            transform.Translate(0,Random.Range(1.5f,1.6f),0);
        else if(oniIndex==9)
            transform.Translate(0,1.3f,0);
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

        if (oniIndex != 8)
        {
            if (!isStop)
            {
                if (transform.position.x >= minX && transform.position.x <= maxX)
                {
                    StartCoroutine(attack());
                    isStop = true;
                }
            }
        }

        if (dmgDelay < 0.1f)
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
                else if(oniIndex==5)
                {
                    anim.Play("oni5_Attack");
                    StartCoroutine(girl.instance.hitted(10));
                    yield return new WaitForSeconds(0.75f);
                    anim.Play("oni5_Idle");
                    yield return new WaitForSeconds(1f);
                }
                else if(oniIndex==6)
                {
                    anim.Play("oni6_Attack");
                    StartCoroutine(girl.instance.hitted(20));
                    yield return new WaitForSeconds(1);
                    anim.Play("oni6_Idle");
                    yield return new WaitForSeconds(1f);
                }
                else if(oniIndex==9)
                {
                    anim.Play("oni7_Attack");
                    StartCoroutine(girl.instance.hitted(10));
                    yield return new WaitForSeconds(1);
                    anim.Play("oni7_Idle");
                    yield return new WaitForSeconds(1f);
                }
            }
        }
        else
        {
            yield return null;
        }
    }

    private void justDie()
    {
        if (oniIndex != 9)
        {
            Instantiate(effect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void Explosion()
    {
        print("A");
        if (dmgDelay >= 0.1f)
        {
            if(Player.instance.playerIndex==5)
                SoundManager.instance.Body_N();
            else
                SoundManager.instance.body();
            Player.instance.ComboText(false);
            hp--;
            if (hp <= 0)
            {
                if (oniIndex == 1)
                    ScoreMgr.instance.scoreUp(0,100, false);
                else if (oniIndex == 2)
                    ScoreMgr.instance.scoreUp(0,150, false);
                else if(oniIndex==3) 
                    ScoreMgr.instance.scoreUp(0,100,false);
                else if(oniIndex==4) 
                    ScoreMgr.instance.scoreUp(0,300,false);
                else if(oniIndex==5) 
                    ScoreMgr.instance.scoreUp(0,200,false);
                else if(oniIndex==6) 
                    ScoreMgr.instance.scoreUp(0,250,false);
                else if(oniIndex==9) 
                    ScoreMgr.instance.scoreUp(0,150,false);
                else if(oniIndex==10) 
                    ScoreMgr.instance.scoreUp(0,300,false);
                
                ComboManager.instance.comboIniitailize();
                ScoreMgr.instance.killedOni++;
                CameraManager.instance.closeUp();
                Instantiate(effect, transform.position, Quaternion.identity);
           
                Destroy(gameObject);
            }   
        }
    }
    public void die(bool isHead, bool isPois=false)
    {
        float v = 0.1f;
        if (isPois)
            v = 0;
        if (dmgDelay >= v)
        { 
            if (Player.instance.playerIndex == 4)
                poison();
            if(!isPois) 
                Player.instance.AttackCor();
            if (isHead)
            {
                ScoreMgr.instance.headshot++;

                if (Player.instance.playerIndex == 4)
                {
                    SoundManager.instance.body();
                    hp--;
                    Player.instance.ComboText(false);
                    Instantiate(effect, transform.position, Quaternion.identity);
                }
                else
                {
                    Player.instance.ComboText(true);
                    if(Player.instance.playerIndex==5)
                        SoundManager.instance.Head_N();
                    else
                        SoundManager.instance.head();
                   
                    if (Player.instance.playerIndex == 6)
                    {
                        if (kanao.instance.isRage)
                            hp -= 2;
                    }
                    hp -= 2;
                    Instantiate(headEffect, transform.position, Quaternion.identity);
                }
                if (hp <= 0)
                {
                    if (oniIndex == 1)
                        ScoreMgr.instance.scoreUp(0,100, false);
                    else if (oniIndex == 2)
                        ScoreMgr.instance.scoreUp(0,150, false);
                    else if(oniIndex==3) 
                        ScoreMgr.instance.scoreUp(0,100,false);
                    else if(oniIndex==4) 
                        ScoreMgr.instance.scoreUp(0,300,false);
                    else if(oniIndex==5) 
                        ScoreMgr.instance.scoreUp(0,200,false);
                    else if(oniIndex==6) 
                        ScoreMgr.instance.scoreUp(0,250,false);
                    else if(oniIndex==9) 
                        ScoreMgr.instance.scoreUp(0,150,false);
                    else if(oniIndex==10) 
                        ScoreMgr.instance.scoreUp(0,300,false);
                    
                    ComboManager.instance.comboIniitailize();
                    ScoreMgr.instance.killedOni++;
                    CameraManager.instance.closeUp();
                    if (Player.instance.playerIndex == 5) //네즈코면 폭혈소환
                        Instantiate(ExplodingBloodEffect, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
            }
            else
            {
                Player.instance.ComboText(false);
                
                if(isPois)
                    SoundManager.instance.poison();
                else
                {
                    if(Player.instance.playerIndex==5)
                        SoundManager.instance.Body_N();
                    else
                        SoundManager.instance.body();
                }
                    
                if (Player.instance.playerIndex == 6)
                {
                    if (kanao.instance.isRage)
                        hp --;
                }
                hp--;
                if (hp <= 0)
                {
                    if (oniIndex == 1)
                        ScoreMgr.instance.scoreUp(0,100, false);
                    else if (oniIndex == 2)
                        ScoreMgr.instance.scoreUp(0,150, false);
                    else if(oniIndex==3) 
                        ScoreMgr.instance.scoreUp(0,100,false);
                    else if(oniIndex==4) 
                        ScoreMgr.instance.scoreUp(0,300,false);
                    else if(oniIndex==5) 
                        ScoreMgr.instance.scoreUp(0,200,false);
                    else if(oniIndex==6) 
                        ScoreMgr.instance.scoreUp(0,250,false);
                    else if(oniIndex==9) 
                        ScoreMgr.instance.scoreUp(0,150,false);
                    else if(oniIndex==10) 
                        ScoreMgr.instance.scoreUp(0,300,false);
                    ComboManager.instance.comboIniitailize();
                    ScoreMgr.instance.killedOni++;
                    CameraManager.instance.closeUp();
                    Instantiate(effect, transform.position, Quaternion.identity);
                    if (Player.instance.playerIndex == 5) //네즈코면 폭혈소환
                        Instantiate(ExplodingBloodEffect, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
            }
            if(!isPois) 
                dmgDelay = 0;
        }
    }

    IEnumerator reverse()
    {
        speed *= Random.Range(0, 2) == 0 ? -1 : 1;
        while (true)
        {
            speed *= -1;
            yield return new WaitForSeconds(2);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Player.instance.playerIndex == 1||Player.instance.playerIndex==3)
            {
                if (kagura.instance.isKagura)
                {
                    return;
                }
            }
            if (!Player.instance.isattack)
            {
                if (oniIndex == 1 || oniIndex == 2 || oniIndex == 3 || oniIndex == 5)
                {
                    if (HeartManager.instance.heartCount == 0)
                    {
                        if (oniIndex == 1)
                            anim.Play("oni1Attack");
                        else if (oniIndex == 2)
                            anim.Play("oni2Attack");
                        else if (oniIndex == 3)
                            anim.Play("oni3_Attack");
                        else if (oniIndex == 5)
                            anim.Play("oni5_Attack");
                        else if (oniIndex == 6)
                            anim.Play("oni6_Attack");
                        else if (oniIndex == 9)
                            anim.Play("oni7_Attack");
                        if (canGo)
                        {
                            if (other.transform.position.x < transform.position.x)
                                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x),
                                    transform.localScale.y, transform.localScale.z);
                            else if (other.transform.position.x > transform.position.x)
                                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1,
                                    transform.localScale.y, transform.localScale.z);
                        }
                    }
                }
            }
        }

        if (other.CompareTag("Ground"))
        {
            transform.Translate(0,Random.Range(-0.1f,0.1f),0);
            if (!canGo)
            {
                if (speed < 0)
                {
                    if(oniIndex==1) 
                        transform.localScale = new Vector3(1, 1, 1);
                    else if(oniIndex==2)
                        transform.localScale = new Vector3(5, 5, 1);
                    else if(oniIndex==3)
                        transform.localScale = new Vector3(1, 1, 1);
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
                else if (oniIndex == 6)
                {
                    anim.Play("oni6_Walking");
                }
                else if (oniIndex == 8)
                {
                    anim.Play("HandIdle");
                    StartCoroutine(reverse());
                }
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
               
            }
        }

        if (other.CompareTag("die"))
        {
            justDie();
        }
        if(other.CompareTag("explosion"))
            Explosion();
    }

    public void poison()
    {
        if (!isPoison)
            StartCoroutine(poisonCor());
    }
    public IEnumerator poisonCor()
    {
        isPoison = true;
        SpriteRenderer spr= GetComponent<SpriteRenderer>();
        Color color = Color.white;
        color.r = 0.66f;
        color.g = 0.32f;
        color.b =1f;
        spr.color = color;
        while (true)
        {
            yield return new WaitForSeconds(0.75f);
            die(false,true);
            Instantiate(poisonEffect, transform.position, Quaternion.identity);
        }
    }
}
