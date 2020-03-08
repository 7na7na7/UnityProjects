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

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            if (!GetComponent<Player>().isattack)
            {
                GetComponent<SpriteRenderer>().flipY = false;
                transform.eulerAngles=new Vector3(0,0,0);
                anim.Play("IdleAnim");
            }
        }
    }
}
