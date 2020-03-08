using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class Player : MonoBehaviour
{
    [Header("설정해줘야하는값")]
    public int force; //움직이는 속도
    public int nuckbackforce; //밀려나는 힘
    public GameObject slashEffect;
    [Header("신경쓸필요없음")] 
    public GameObject comboPop;
    public GameObject worldCanvas;
    private Animator anim;
    private SpriteRenderer spr;
    private Vector2 currentV;
    public bool isattack = false; //공격중인지 판단
    private bool canMove = true;
    public bool isSuper = false;
    private CameraManager camera; //카메라스크립트
    private ComboManager combomgr;
    private fade panel;//콤보중 화면 어둡게 만들어줌
    public Transform min, max; //터치할수있는 최소, 최대 역역
    private Rigidbody2D rigid; //리지드바디얻어옴
    public GameObject particle;//전기이펙트
    public TrailRenderer trail; //뒤따라오는트레일이펙트
   
    private void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        combomgr = GetComponent<ComboManager>();
        rigid = GetComponent<Rigidbody2D>();
        panel = FindObjectOfType<fade>();
        camera = Camera.main.GetComponent<CameraManager>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Vector2 pos=Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
                if (Vector2.Distance(transform.position, pos) > 1)
                {
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
                    StartCoroutine(go(pos));
                }
            }
        }


        
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos=Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Vector2.Distance(transform.position, pos) > 1)
            {
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
                StartCoroutine(go(pos));
            }
        }
    }

    IEnumerator go(Vector2 to)
    {
        if (to.x > transform.position.x)
        {
            spr.flipY = false;
            transform.eulerAngles = new Vector3(0, 0, -getAngle(transform.position.x, transform.position.y, to.x, to.y)+60); //y2값에 180더하거나 말거나 ㅋ
        }
        else
        {
            spr.flipY = true;
            transform.eulerAngles = new Vector3(0, 0, -getAngle(transform.position.x, transform.position.y, to.x, to.y)+120); //y2값에 180더하거나 말거나 ㅋ
        }

        anim.Play("attack_ready");
        isattack = true;
        rigid.velocity = Vector2.zero;
        rigid.bodyType = RigidbodyType2D.Kinematic;
        canMove = true;
        if (combomgr.canCombo) //콤보중
        {
            camera.sizeup();
            StartCoroutine(panel.fadeIn());
            trail.startColor=Color.yellow;
            particle.SetActive(true);
            //int currentcount = comboCount;
            if(Time.timeScale==1) 
                Time.timeScale = 0.7f;
            while (Vector2.Distance(transform.position, to) >= 0.3f)
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
            trail.startColor=Color.white;
            camera.sizedown();
            StartCoroutine(panel.fadeout());
            while (Vector2.Distance(transform.position, to) >= 0.3f) 
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
        trail.startColor=Color.white;
        camera.sizedown();
        StartCoroutine(panel.fadeout());
    }
    
    private void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.CompareTag("oni"))
        {
            if (isattack)
            {
                Instantiate(slashEffect, transform.position, Quaternion.identity);
                anim.Play("attackAnim");
                canMove = false;
                hit.GetComponent<oniMove>().die();
                Vector2 dir = transform.position - hit.transform.position;
                dir.Normalize();
                rigid.velocity = Vector2.zero;
                rigid.bodyType = RigidbodyType2D.Dynamic;
                rigid.velocity = dir * nuckbackforce;
                ComboText();
                combomgr.comboIniitailize();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D hit)
    {
        if (hit.CompareTag("oni"))
        {
            if (isattack)
            {
                hit.GetComponent<oniMove>().die();
            }
        }
    }

    public void ComboText()
    {
        //콤보수표시
        GameObject cp=Instantiate(comboPop, worldCanvas.transform);
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
}
