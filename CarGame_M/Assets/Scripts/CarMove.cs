using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMove : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 10;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = speed;
    }

    void Update()
    {
        
    }
}
