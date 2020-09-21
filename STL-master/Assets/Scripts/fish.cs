using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fish : MonoBehaviour
{
    private bool once = false;
    public float force;
    private Rigidbody2D rigid;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.AddForce(new Vector2(0,force));
        Destroy(gameObject,5f);
    }

    private void Update()
    {
        if (rigid.velocity.y < 0)
        {
            if (once == false)
            {
                transform.rotation=new Quaternion(0,0,180,0);
                once = true;
            }
        }
    }
}
