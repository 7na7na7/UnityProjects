using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public GameObject parent;
    public GameObject bullet;
    public Transform bulletpos;
    private CharacterController cc;
    private float mx;
    private float rx;
    public float speed = 5;
    public float gravity = -9.81f;
    public float jumpPower = 5;
    private float Yvelocity;
    public int maxjump = 2;
    private int jumpcount;
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(h,0,v);
        dir.Normalize();
        
        //카메라 보는 방향을 앞으로 정함
        dir = Camera.main.transform.TransformDirection(dir); 

        //플레이어 회전
        mx = Input.GetAxis("Mouse X"); //마우스의 X값 받음
        rx += mx; //rx에 마우스의 x값 누적
        transform.eulerAngles = new Vector3(0, rx, 0);
        
        
            //점프제한
            if (cc.isGrounded)
            {
                jumpcount = 0;
                Yvelocity = 0;
            }
        //점프
        if (Input.GetButtonDown("Jump")&&jumpcount<maxjump)
        {
            Yvelocity = jumpPower;
            jumpcount++;
        }

        //이동
        Yvelocity += gravity * Time.deltaTime;
        dir.y = Yvelocity;
        cc.Move(Time.deltaTime * speed * dir);
        
        //발사
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(bullet, bulletpos);
        }
    }
}
