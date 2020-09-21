using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class player : MonoBehaviour
{
    private bool once2 = false;
    private bool once = false;
    private bool ischeat = false;
    private bool isjump = false;
    private Animator animator;
    private bool cansound = true;
    public AudioSource audio;
    public AudioClip footsound;
    public float foodsoundDelay;
    public float movePower;
    public float jumpforce = 0; //점프하는 힘의 크기
    public int maxjumpcount = 0; //최대 점프카운트
    private int jumpcount = 0; //현재 점프카운트
    public float offset = 1;
    private Rigidbody2D rigid = null;
    private float distance = 0; //raycast의 거리를 지정해 줌
    public LayerMask layermask = 0;
    public float slowMoveSpeed = 1f;
    public lessvision visioncon;
    public GameObject grasspool;
    public RectTransform toptooth;
    public RectTransform bottomtooth;
    public AudioSource endsound;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        distance = GetComponent<BoxCollider2D>().bounds.extents.y + offset; //콜라이더 y축 절반 크기+offset, offset은 플레이어 발판에서 떨어진 범위
        //StartCoroutine(walk());
    }

    void CheckGround()
    {
        if (rigid.velocity.y <= 0) //낙하할 때만 체크
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 
                distance, layermask); 
            //플레이어 방향을 중심으로 아래 방향으로 distance만큼 쏘아 layermask레이어에 충돌하는 것을 저장
            if (hit)
            {
                if (hit.transform.CompareTag("ground")) //닿은 물체의 태그가 ground라면
                {
                    jumpcount = 0; //점프카운트 0으로 초기화
                    isjump = false;
                }
            }
        }
    }
    void jump()
    {
        if (Input.GetKeyDown(KeyCode.Space)) //스페이스 바를 누르면
        {
            if (jumpcount < maxjumpcount) //점프카운트가 최대 점프카운트보다 낮을 떄
            {
                animator.SetBool("isidle",false);
                animator.SetBool("isrun",false);
                animator.SetBool("isjump",true);
                jumpcount++; //점프카운트++
                rigid.velocity = Vector2.up * jumpforce; //점프
                isjump = true;
            }
        }
    }
    private void Move()
    {
        Vector3 moveVelocity = Vector3.zero;

        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            moveVelocity = Vector3.right;
            transform.localScale = new Vector3(1, 1, 1); //오른쪽으로 뒤집어짐
            if (!isjump)
            {
                animator.SetBool("isrun", true);
                animator.SetBool("isidle", false);
                animator.SetBool("isjump", false);
            }
        }

        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            moveVelocity = Vector3.left;
            transform.localScale = new Vector3(-1, 1, 1); //왼쪽으로 뒤집어짐
            if (!isjump)
            {
                animator.SetBool("isrun", true);
                animator.SetBool("isidle", false);
                animator.SetBool("isjump", false);
            }
        }
        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            if (!isjump)
            {
                animator.SetBool("isrun", false);
                animator.SetBool("isidle", true);
                animator.SetBool("isjump", false);
            }
        }
        transform.position += moveVelocity * movePower * Time.deltaTime * slowMoveSpeed;
    }
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.C))
                ischeat = true;
        }
        jump();
        CheckGround();
        Move();
    }

    IEnumerator walk()
    {
        cansound = false;
        for (int i = 0; i < grasspool.transform.childCount; i++)
        {
            if (!grasspool.transform.GetChild(i).gameObject.activeSelf)
            {
                audio.Play();
                grasspool.transform.GetChild(i).transform.position = transform.position - new Vector3(0, 1.5f, 0);
                grasspool.transform.GetChild(i).gameObject.SetActive(true);
                break;
            }
        }
        yield return new WaitUntil(() => Input.GetAxisRaw("Horizontal") != 0);
        yield return new WaitForSeconds(foodsoundDelay);
        cansound = true;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("monster"))
        {
            {
                if (once == false)
                {
                    if (!ischeat)
                    {
                        StartCoroutine(GameOver());
                        once = true;
                    }
                    //SceneManager.LoadScene("gameover");
                }
        }
        }else if (other.CompareTag("Egg"))
        {
            StartCoroutine(slowedbyegg());
        }else if (other.CompareTag("StageB"))
        {
            visioncon.Stageb();
        }else if (other.CompareTag("StageC"))
        {
            visioncon.StageC();
        }else if (other.CompareTag("GameClear"))
        {
            SceneManager.LoadScene("Clear");
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("ground")&&cansound==true&&Input.GetAxisRaw("Horizontal")!=0)
            {
                audio.PlayOneShot(footsound);
                StartCoroutine(walk());
            }
    }

    private IEnumerator slowedbyegg()
    {
        slowMoveSpeed *= 0.5f;
        yield return new WaitForSeconds(2f);
        slowMoveSpeed /= 0.5f;
    }

    private IEnumerator GameOver()
    {
        while (true)
        {
            if (once2 == false)
            {
                Time.timeScale = 0;
                yield return new WaitForSecondsRealtime(0.5f);
                Time.timeScale = 1;
                once2 = true;
            }

            toptooth.position += Vector3.down * 100;
            bottomtooth.position += Vector3.up * 100;
            yield return null;
            if(toptooth.localPosition.y <= 0)
            {
                endsound.Play();
                yield return new WaitForSeconds(endsound.clip.length);
                SceneManager.LoadScene("gameover");
                break;
            }
        }
    }
}
