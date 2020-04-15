using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTrap : MonoBehaviour
{
    private bool isGo = false;
    public float speed;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isGo)
            {
                isGo = true;
                StartCoroutine(go());
            }
        }
    }

    IEnumerator go()
    {
        while (transform.localScale.y<3.9f)
        {
            if (transform.localScale.y < 3)
            {
                transform.localScale=new Vector3(transform.localScale.x,transform.localScale.y+speed*Time.deltaTime*2,1);
            }
            else
            {
                transform.localScale=new Vector3(transform.localScale.x,transform.localScale.y+speed*Time.deltaTime,1);   
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
        yield return new WaitForSeconds(0.2f);
        while (transform.localScale.y>1)
        {
            transform.localScale=new Vector3(transform.localScale.x,transform.localScale.y-speed*0.5f*Time.deltaTime,1);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        yield return new WaitForSeconds(2);
        isGo = false;
    }
}
