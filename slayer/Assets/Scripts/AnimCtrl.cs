using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCtrl : MonoBehaviour
{
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
        if (rigid.velocity.y < -0.1f && !GetComponent<Player>().isattack) //떨어지고 있고, 공격 중이 아니라면
        {
            Player.instance.flipY(false);
            transform.eulerAngles=new Vector3(0,0,0);
            GetComponent<Player>().trail.GetComponent<TrailRenderer>().emitting = false;
            GetComponent<Player>().trail2.GetComponent<TrailRenderer>().emitting = false;
            anim.Play("fallAnim");
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            if (!GetComponent<Player>().isattack)
            {
                Player.instance.flipY(false);
                transform.eulerAngles=new Vector3(0,0,0);
                anim.Play("IdleAnim");
            }
        }
    }
}
