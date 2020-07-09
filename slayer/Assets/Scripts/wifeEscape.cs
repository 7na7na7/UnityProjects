using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wifeEscape : MonoBehaviour
{
    public float Axis;
    public float speed;
    private bool isStop = true;
    private void Update()
    {
        if (!isStop)
        {
            if(transform.position.x<Axis)
                transform.Translate(-speed*Time.deltaTime,0,0);
            else
                transform.Translate(speed*Time.deltaTime,0,0);
        }
        if(transform.position.x<GameObject.Find("Min").transform.position.x-3)
            Destroy(gameObject);
        if(transform.position.x>GameObject.Find("Max").transform.position.x+3)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            isStop = false;
        }
    }
}
