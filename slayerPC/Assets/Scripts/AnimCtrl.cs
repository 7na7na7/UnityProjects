using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCtrl : MonoBehaviour
{
    bool a=false;
    private bool isFalling = false;
    private Vector2 currentV;
    private Animator anim;
    private Rigidbody2D rigid;
    void Start()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (rigid.velocity.y < -3f && !GetComponent<Player>().isattack) //떨어지고 있고, 공격 중이 아니라면
        {
            a = false;
            isFalling = true;
            Player.instance.flipY(false);
            transform.eulerAngles=new Vector3(0,0,0);
            GetComponent<Player>().trail.GetComponent<TrailRenderer>().emitting = false;
            GetComponent<Player>().trail2.GetComponent<TrailRenderer>().emitting = false;
            anim.Play("fallAnim");
            if (Player.instance.isNuckBack)
            {
                if (rigid.velocity.x < 0)
                    Player.instance.flipX(false);
                else
                    Player.instance.flipX(true);
            }
            else
            {
                if (rigid.velocity.x < 0)
                    Player.instance.flipX(true);
                else
                    Player.instance.flipX(false);
            }
            if (GameManager.instance.canChangeTimeScale)
            {
                if (GameManager.instance.isPause)
                    Time.timeScale = 0;
                else
                    Time.timeScale = 1;   
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            if (!GetComponent<Player>().isattack)
            {
                if (!a)
                {
                    if (isFalling)
                    {
                        Player.instance.flipY(false);
                    }
                    else
                    {
                        if (Player.instance.transform.localScale.y < 0) //왼쪽으로 갈때
                        {
                            //print("왼!");
                            Player.instance.flipY(false);
                            Player.instance.flipX(true);
                 
                        }
                        else //오른쪽으로 갈때
                        {
                            //print("오른!");
                            Player.instance.flipX(false);
                        }
                    }
                    isFalling = false;
                    a = true;
                    transform.eulerAngles=new Vector3(0,0,0);
                    anim.Play("IdleAnim");   
                }
            }
            else
            {
                a = false;
                isFalling = false;
            }
        }
    }
}
