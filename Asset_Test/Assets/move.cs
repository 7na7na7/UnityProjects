using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public float speed;
    private float localScaleX;
    private void Start()
    {
        localScaleX = transform.localScale.x;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 moveV = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        rb.velocity = moveV * speed;
        if (Input.GetAxisRaw("Horizontal") ==1)
        {
            anim.Play("Walk");
            transform.localScale = new Vector3(localScaleX,transform.localScale.y,transform.localScale.z);
        }
        else if (Input.GetAxisRaw("Horizontal") == -1)
        {
            anim.Play("Walk");
            transform.localScale = new Vector3(localScaleX*-1,transform.localScale.y,transform.localScale.z);
        }
        else
        {
            anim.Play("Idle");
        }
    }
}
