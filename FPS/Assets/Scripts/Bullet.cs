using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameObject obj;
    private Rigidbody rb;

    public int speed=30;
    // 중력을 받으면서 날아감
    void Start()
    {
        obj = GameObject.Find("Cube").gameObject;
        Destroy(gameObject,5f);
        rb = GetComponent<Rigidbody>();
        rb.velocity =transform.forward*speed;
        transform.SetParent(obj.transform);
    }

}
