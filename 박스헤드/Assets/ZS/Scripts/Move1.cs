using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Move1 : MonoBehaviour
{
    public BoxCollider2D boxCollider; //충돌감지영역
    public LayerMask layerMask; //통과가 불가능한 레이어 설정
    public AudioSource audiosource; //오디오소스
    public UnityEngine.UI.Slider stamina;
    public int damage;
    public Transform min, max;
    public bool isdead = false;
    public float invisibletime;
    private Color color;
    public bool isdamaged = false;
    public Collider2D col;
    public UnityEngine.UI.Slider slider;
    public Vector3 moveVelocity;
    Animator animator;//애니메이션 선언
    public bool isflip = false;
    public Transform tr; //카메라밖 제한을 위해서 플레이어의 위치를 넣어준다.
    public float offset = 0.4f; //제한할때 오차 조정 코드(플레이어 중간을 인식해서 생기는 오차)
    float screenRation = (float)Screen.width / (float)Screen.height;

    public float moveforce = 1;
    private float minusforce;
    private float plusforce;
    private void Start()
    {
       
        animator = GetComponent<Animator>();
        minusforce=this.transform.localScale.x * -1;
        plusforce=this.transform.localScale.x;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        stamina.value += 0.1f;
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (stamina.value > 30)
            {
                StartCoroutine(dash());
                stamina.value -= 30;
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("play");
        }
        if (slider.value <= 0)
        {
            isdead = true;
            SpriteRenderer sprite= GetComponent<SpriteRenderer>();//스프라이트로 함
            color.r = 255;
            color.g = 255;
            color.b = 255;
            color.a = 0.0f;
            sprite.color = color;
        }
    }
    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        moveVelocity = Vector3.zero;
            if (Input.GetAxisRaw("Horizontal") > 0) //오른쪽으로 갈때
            {
                if (transform.position.x < max.position.x)
                {
                    isflip = false;
                    moveVelocity.x += 1;
                    if (isCol() == true)
                        moveVelocity.x -= 1;
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                    //transform.localScale = new Vector3(plusforce, this.transform.localScale.y, this.transform.localScale.z);//오른쪽으로 뒤집어짐
                    animator.SetBool("iswalk", true);
                }
            }

            if (Input.GetAxisRaw("Horizontal") < 0) //왼쪽으로 갈때
            {
                if (transform.position.x > min.position.x)
                {
                    isflip = true;
                    moveVelocity.x -= 1;
                    if (isCol() == true)
                        moveVelocity.x += 1;
                    transform.rotation = Quaternion.Euler(180f, 0f, 180f);
                    //transform.localScale = new Vector3(minusforce, this.transform.localScale.y, this.transform.localScale.z); //왼쪽으로 뒤집어짐
                    animator.SetBool("iswalk", true);
                }
            }

            if (Input.GetAxisRaw("Vertical") > 0) //위쪽으로 갈때
            {
                if (transform.position.y < max.position.y)
                {
                    moveVelocity.y += 1;
                    if (isCol() == true)
                        moveVelocity.y -= 1;
                    animator.SetBool("iswalk", true);
                }
            }

            if (Input.GetAxisRaw("Vertical") < 0) //아래쪽으로 갈때
            {
                if (transform.position.y > min.position.y)
                {
                    moveVelocity.y -= 1;
                    if (isCol() == true)
                        moveVelocity.y += 1;
                    animator.SetBool("iswalk", true);
                }
            }

            if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0) //가만있을때
            {
                animator.SetBool("iswalk", false);
            }
            transform.position += moveVelocity * moveforce * Time.deltaTime;
    }
    /*
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("edge"))
        {
            Debug.Log("asfsf");
            if (Input.GetAxisRaw("Vertical") > 0)//위쪽으로 갈때
            {
                transform.position += Vector3.down * moveforce * Time.deltaTime;
            }
            else if (Input.GetAxisRaw("Vertical") < 0)//아래쪽으로 갈때
            {
                transform.position += Vector3.up * moveforce * Time.deltaTime;
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)//왼
            {
                transform.position += Vector3.right * moveforce * Time.deltaTime;
            } 
            else if (Input.GetAxisRaw("Horizontal") > 0)//오
            {
                transform.position += Vector3.left * moveforce * Time.deltaTime;
            }
        }
    }
*/


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("zombie"))
        {
            if (isdamaged == false)
            {
                StartCoroutine(damaged());
                StartCoroutine(invisible());
            }
        }
    }

    IEnumerator damaged()
    {
        isdamaged = true;
        slider.value -= damage;
        yield return new WaitForSeconds(1.5f);
        //col.enabled = false;
        //col.enabled = true;
        isdamaged = false;
    }

    IEnumerator invisible()
    {
        SpriteRenderer sprite= GetComponent<SpriteRenderer>();//스프라이트로 함
        color.r = 255;
        color.g = 255;
        color.b = 255;
        color.a = 0.5f;
        sprite.color = color;
        yield return new WaitForSeconds(invisibletime);
        color.a = 1f;
        sprite.color = color;
        yield return new WaitForSeconds(invisibletime);
        color.a = 0.5f;
        sprite.color = color;
        yield return new WaitForSeconds(invisibletime);
        color.a = 1f;
        sprite.color = color;
        yield return new WaitForSeconds(invisibletime);
        color.a = 0.5f;
        sprite.color = color;
        yield return new WaitForSeconds(invisibletime);
        color.a = 1f;
        sprite.color = color;
        yield return new WaitForSeconds(invisibletime);
    }

    IEnumerator dash()
    {
        moveforce *= 5;
        col.enabled = false;
        yield return new WaitForSeconds(0.1f);
        col.enabled = true;
        moveforce /= 5;
    }

    private bool isCol()
    {
        RaycastHit2D hit;
        //RaycastHit2D란?
        //A지점에서 B지점으로 레이저를 발사했을 때, 레이저가 도달하면 hit에 Null이 반환,
        //레이저가 도달하지 못하면 hit에 레이저와 충돌한 방해물이 반환된다!
        Vector3 start = transform.position; //A지점은 현재 위치
        Vector3 end =
            start + moveVelocity * moveforce * Time.deltaTime;

        boxCollider.enabled = false; //플레이어가 자기 자신과의 충돌 판정을 하는 것을 막기 위해 잠시 콜라이더 비활성화
        hit = Physics2D.Linecast(start, end, layerMask);
        boxCollider.enabled = true; //다시 활성화
        if (hit.transform != null) //만약 플레이어의 위치와 플레이어가 이동할 위치 사이에 장애물이 있을 경우
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
