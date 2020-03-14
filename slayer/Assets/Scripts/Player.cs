﻿using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class Player : MonoBehaviour
{
    [Header("설정해줘야하는값")]
    public int force; //움직이는 속도
    public int nuckbackforce; //밀려나는 힘
    public GameObject slashEffect;
    public float touchDelay;
    public GameObject dieEffect;
    [Header("신경쓸필요없음")] 
    public bool isGameOver = false;
    public GameObject comboPop;
    public GameObject headPop;
    public GameObject worldCanvas;
    private Animator anim;
    private float touchTime = 0;
    public static Player instance;
    private float unRotY, RotY;
    private Vector2 currentV;
    public bool isattack = false; //공격중인지 판단
    private bool canMove = true;
    private CameraManager camera; //카메라스크립트
    private fade panel;//콤보중 화면 어둡게 만들어줌
    public Transform min, max; //터치할수있는 최소, 최대 역역
    private Rigidbody2D rigid; //리지드바디얻어옴
    public GameObject particle;//전기이펙트
    public GameObject trail; //뒤따라오는트레일이펙트
    public GameObject trail2;
    public GameObject jumpEffect; //점프시 이펙트
    private float killDelay = 0;
    private void Start()
    {
        if (instance == null)
            instance = this;

        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        panel = FindObjectOfType<fade>();
        camera = Camera.main.GetComponent<CameraManager>();

        unRotY = transform.localScale.y;
        RotY = transform.localScale.y * -1;
    }

    void Update()
    {
        if (touchTime>=touchDelay&&mpSlider.instance.mp.value>=1&&!isGameOver) //터치타임이 터치딜레이 이상이고, 기력이 1이상이고, 게임오버가 아니라면
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    //if (Vector2.Distance(transform.position, pos) > 1)같은곳 터치안되게하는코드
                    StopAllCoroutines();
                        if (pos.x < min.position.x)
                        {
                            pos.x = min.position.x;
                        }

                        if (pos.x > max.position.x)
                        {
                            pos.x = max.position.x;
                        }

                        if (pos.y < min.position.y)
                        {
                            pos.y = min.position.y;
                        }

                        if (pos.y > max.position.y)
                        {
                            pos.y = max.position.y;
                        }

                        StartCoroutine(go2(pos));
                }
            }
            
                if (Input.GetMouseButtonDown(0))
                {
                    Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                  
                        StopAllCoroutines();
                        if (pos.x < min.position.x)
                        {
                            pos.x = min.position.x;
                        }

                        if (pos.x > max.position.x)
                        {
                            pos.x = max.position.x;
                        }

                        if (pos.y < min.position.y)
                        {
                            pos.y = min.position.y;
                        }

                        if (pos.y > max.position.y)
                        {
                            pos.y = max.position.y;
                        }

                        StartCoroutine(go2(pos));
                }

        }
        if(touchTime<touchDelay) 
            touchTime += Time.deltaTime;
        if (killDelay < 0.05f)
            killDelay += Time.deltaTime;
    }

    /*예전 go함수
    public IEnumerator go(Vector2 to)
         {
             mpSlider.instance.mpDown(1);
             GetComponent<Player>().trail.GetComponent<TrailRenderer>().emitting = true;
             GetComponent<Player>().trail2.GetComponent<TrailRenderer>().emitting = true;
             touchTime = 0;
             if (to.x > transform.position.x)
             {
                 flipY(false);
                 transform.eulerAngles = new Vector3(0, 0, -getAngle(transform.position.x, transform.position.y, to.x, to.y)+60); //y2값에 180더하거나 말거나 ㅋ
             }
             else
             {
                 flipY(true);
                 transform.eulerAngles = new Vector3(0, 0, -getAngle(transform.position.x, transform.position.y, to.x, to.y)+120); //y2값에 180더하거나 말거나 ㅋ
             }
     
             anim.Play("attack_ready");
             isattack = true;
             trail.SetActive(true);
             rigid.velocity = Vector2.zero;
             rigid.bodyType = RigidbodyType2D.Kinematic;
             canMove = true;
             if (combomgr.canCombo) //콤보중
             {
                 camera.sizeup();
                 StartCoroutine(panel.fadeIn());
                 trail.GetComponent<TrailRenderer>().startColor=Color.yellow;
                 particle.SetActive(true);
                 //int currentcount = comboCount;
                 if(Time.timeScale==1) 
                     Time.timeScale = 0.7f;
                 while (Vector2.Distance(transform.position, to) >= 0.5f)
                 {
                     if (!canMove)
                         break;
                     Vector2 dir = to - (Vector2) transform.position;
                     dir.Normalize();
                     if (Time.timeScale == 0.7f)
                     { 
                         //transform.Translate(dir * force*0.01f);
                         transform.position+=(Vector3)dir * force*0.01f;
                         yield return new WaitForSecondsRealtime(0.005f);
                     }
                     else
                     {
                         //transform.Translate(dir * force*Time.deltaTime);
                         transform.position+=(Vector3)dir * force * Time.deltaTime;
                         yield return new WaitForSeconds(0.005f);
                     }
                 }
                 yield return new WaitForSeconds(0.2f);
             }
             else //첫번째
             {
                 particle.SetActive(false);
                 trail.GetComponent<TrailRenderer>().startColor=Color.white;
                 camera.sizedown();
                 StartCoroutine(panel.fadeout());
                 while (Vector2.Distance(transform.position, to) >= 0.5f) 
                 {
                     if (!canMove)
                         break;
                     Vector2 dir = to - (Vector2) transform.position;
                     dir.Normalize();
                     //transform.Translate(dir * force*0.5f * Time.deltaTime);
                     transform.position += (Vector3) dir * force * 0.5f * Time.deltaTime;
                     yield return new WaitForSeconds(0.005f);
                 }
                 yield return new WaitForSeconds(0.2f);
             }
             isattack = false;
             rigid.velocity = Vector2.zero;
             rigid.bodyType = RigidbodyType2D.Dynamic;
             yield return new WaitForSeconds(combomgr.comboDelay);
             particle.SetActive(false);
             trail.GetComponent<TrailRenderer>().startColor=Color.white;
             camera.sizedown();
             StartCoroutine(panel.fadeout());
         }
         */
    
    public IEnumerator go2(Vector2 to)
    {
        SoundManager.instance.swing();
        mpSlider.instance.mpDown(1);
        GetComponent<Player>().trail.GetComponent<TrailRenderer>().emitting = true;
        GetComponent<Player>().trail2.GetComponent<TrailRenderer>().emitting = true;
        touchTime = 0;
        if (to.x > transform.position.x)
        {
            flipY(false);
            transform.eulerAngles = new Vector3(0, 0, -getAngle(transform.position.x, transform.position.y, to.x, to.y)+60);
        }
        else
        {
            flipY(true);
            transform.eulerAngles = new Vector3(0, 0, -getAngle(transform.position.x, transform.position.y, to.x, to.y)+120);
        }
        Instantiate(jumpEffect, transform.position,Quaternion.Euler(0,0,-getAngle(transform.position.x, transform.position.y, to.x, to.y)+90));
        anim.Play("attack_ready");
        isattack = true;
        trail.SetActive(true);
        rigid.velocity = Vector2.zero;
        rigid.bodyType = RigidbodyType2D.Kinematic;
        canMove = true;
        if (ComboManager.instance.canCombo) //콤보중
        {
            camera.sizeup();
            StartCoroutine(panel.fadeIn());
            trail.GetComponent<TrailRenderer>().startColor=Color.yellow;
            particle.SetActive(true);
            //int currentcount = comboCount;
            if (Time.timeScale == 1)
                Time.timeScale = 0.7f;

                Vector2 dir = to - (Vector2) transform.position;
                dir.Normalize();
                if (Time.timeScale == 0.7f)
                {
                    rigid.velocity = dir * force*2;
                }
                else
                {
                    rigid.velocity = dir * force;
                }
        }
        else //첫번째
        {
            particle.SetActive(false);
            trail.GetComponent<TrailRenderer>().startColor=Color.white;
            camera.sizedown();
            StartCoroutine(panel.fadeout());
          
                Vector2 dir = to - (Vector2) transform.position;
                dir.Normalize();
                if (Time.timeScale == 0.7f)
                {
                    rigid.velocity = dir * force*50;
                }
                else
                {
                    rigid.velocity = dir * force;
                }
        }
        if(ComboManager.instance.canCombo) 
            yield return new WaitUntil(()=>Vector2.Distance(transform.position, to) <= 0.5f);
        else
            yield return new WaitUntil(()=>Vector2.Distance(transform.position, to) <= 0.3f);
        
       
        rigid.velocity *= 0.3f; 
        rigid.bodyType = RigidbodyType2D.Dynamic;
        yield return new WaitForSeconds(0.2f);
        isattack = false;
        yield return new WaitForSeconds(ComboManager.instance.comboDelay);
        particle.SetActive(false);
        trail.GetComponent<TrailRenderer>().startColor=Color.white;
        camera.sizedown();
        StartCoroutine(panel.fadeout());
    }
    public void flipY(bool flip)
    {
        if(flip)
            transform.localScale=new Vector3(transform.localScale.x,RotY,1);
        else
            transform.localScale=new Vector3(transform.localScale.x,unRotY,1);
    }
    private void OnTriggerEnter2D(Collider2D hit)
    {
        if (killDelay >= 0.05f)
        {
            if (hit.CompareTag("oni"))
            {
                if (isattack)
                {
                    StartCoroutine(atkCor());
                    Instantiate(slashEffect, transform.position, Quaternion.identity);
                    anim.Play("attackAnim");
                    canMove = false;
                    Vector2 dir = transform.position - hit.transform.position;
                    dir.Normalize();
                    if (dir.y <= 0.5f)
                        dir.y = 0;
                    rigid.velocity = Vector2.zero;
                    rigid.bodyType = RigidbodyType2D.Dynamic;
                    rigid.velocity = dir * nuckbackforce;
                }
                else
                {
                    die();
                }

                killDelay = 0;
            }
            else if (hit.CompareTag("oniHead"))
            {
                if (isattack)
                {
                    StartCoroutine(atkCor());
                    Instantiate(slashEffect, transform.position, Quaternion.identity);
                    anim.Play("attackAnim");
                    canMove = false;
                    Vector2 dir = transform.position - hit.transform.position;
                    dir.Normalize();
                    if (dir.y <= 0.5f)
                        dir.y = 0;
                    rigid.velocity = Vector2.zero;
                    rigid.bodyType = RigidbodyType2D.Dynamic;
                    rigid.velocity = dir * nuckbackforce;
                }
                else
                {
                  die();
                }

                killDelay = 0;
            }
        }
    }

    public void die()
    {
        if (!isGameOver)
        {
            print("게임오버!");
            Instantiate(dieEffect, transform.position, Quaternion.identity);
            StopAllCoroutines();
            SoundManager.instance.hit();
            isGameOver = true;
          FindObjectOfType<GameOverManager>().GameoverFunc();
            Destroy(gameObject);
        }
    }
    public void ComboText(bool isHead)
    {
        if (isHead)
            {
                GameObject cq = Instantiate(headPop, worldCanvas.transform);
                cq.GetComponent<comboText>().initialize();
            }

            //콤보수표시
            GameObject cp = Instantiate(comboPop, worldCanvas.transform);
            cp.GetComponent<comboText>().initialize();
    }
    
    private float getAngle(float x1, float y1, float x2, float y2) //Vector값을 넘겨받고 회전값을 넘겨줌
    {
        float dx = x2 - x1;
        float dy = y2 - y1;

        float rad = Mathf.Atan2(dx, dy);
        float degree = rad * Mathf.Rad2Deg;
        
        return degree;
    }

    IEnumerator atkCor()
    {
        yield return new WaitForSeconds(0.2f);
        isattack = false;
    }
}
