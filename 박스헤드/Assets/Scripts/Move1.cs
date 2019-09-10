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
    public UnityEngine.UI.Slider stamina;
    public int damage;
    public Transform min, max;
    public bool isdead = false;
    public float invisibletime;
    private Color color;
    private bool isdamaged = false;
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
        Screen.fullScreen = true;
        animator = GetComponent<Animator>();
        minusforce=this.transform.localScale.x * -1;
        plusforce=this.transform.localScale.x;
    }

    void Update()
    {
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
                animator.SetBool("iswalk", true);
            }
        }

        if (Input.GetAxisRaw("Vertical") < 0) //아래쪽으로 갈때
        {
            if (transform.position.y > min.position.y)
            {
                moveVelocity.y -= 1;
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
        col.enabled = false;
        col.enabled = true;
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
}
