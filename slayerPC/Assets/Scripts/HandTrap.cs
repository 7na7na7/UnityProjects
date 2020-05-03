using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTrap : MonoBehaviour
{
    public Sprite spr;
    public GameObject bossObj;
    public bool isBossStage = false;
    public bool isDown = false;
    private bool isGo = false;
    public float speed;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isGo)
            {
                isGo = true;
                if(!isBossStage) 
                    StartCoroutine(go());
            }
        }
    }

    private void Start()
    {
        if (isBossStage)
        {
            GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            GetComponent<Animator>().Play("handTrap2Idle");
            GetComponent<SpriteRenderer>().sprite = spr;
        }
    }

    public void bossAtk()
    {
        StartCoroutine(go2());
    }

    IEnumerator go2()
    {
        transform.localScale=new Vector3(0.7f,0.7f,1);
        bossObj.SetActive(true);
        
        yield return new WaitForSeconds(0.75f);
        GetComponent<Collider2D>().enabled = true;
        bossObj.SetActive(false);
        GetComponent<SpriteRenderer>().sprite = spr;
        GetComponent<Animator>().Play("handTrap2Idle");
        
        float n1, n2, n3;
        n1 = 2.1f;
        n2 = 1.05f;
        n3 = 0;

        while (transform.localScale.y < n1)
        {
            if (transform.localScale.y < n2)
            {
                transform.localScale = new Vector3(transform.localScale.x,
                    transform.localScale.y + speed * Time.deltaTime * 2, 1);
            }
            else
            {
                transform.localScale = new Vector3(transform.localScale.x,
                    transform.localScale.y + speed * Time.deltaTime, 1);
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }

        yield return new WaitForSeconds(0.15f);

        while (transform.localScale.y > n3)
        {
            transform.localScale = new Vector3(transform.localScale.x,
                transform.localScale.y - speed * 0.75f * Time.deltaTime, 1);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        GetComponent<Animator>().Play("None");
        GetComponent<SpriteRenderer>().sprite = null;
        GetComponent<Collider2D>().enabled = false;
    }

    IEnumerator go()
    {
        float n1, n2, n3;
        if (!isDown)
        {
            n1 = 1.5f;
            n2 = 0.75f;
            n3 = 0.573194f;
        }
        else
        {
            n1 = -1f;
            n2 = -0.5f;
            n3 = -0.573194f;
        }

        if (n1 > 0)
        {
            while (transform.localScale.y < n1)
            {
                if (transform.localScale.y < n2)
                {
                    transform.localScale = new Vector3(transform.localScale.x,
                        transform.localScale.y + speed * Time.deltaTime * 2, 1);
                }
                else
                {
                    transform.localScale = new Vector3(transform.localScale.x,
                        transform.localScale.y + speed * Time.deltaTime, 1);
                }

                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        else
        {
            while (transform.localScale.y > n1)
            {
                if (transform.localScale.y > n2)
                {
                    transform.localScale = new Vector3(transform.localScale.x,
                        transform.localScale.y + speed * Time.deltaTime * 2, 1);
                }
                else
                {
                    transform.localScale = new Vector3(transform.localScale.x,
                        transform.localScale.y + speed * Time.deltaTime, 1);
                }

                yield return new WaitForSeconds(Time.deltaTime);
            }
        }

        yield return new WaitForSeconds(0.15f);
        if (n3 > 0)
        {
            while (transform.localScale.y > n3)
            {
                transform.localScale = new Vector3(transform.localScale.x,
                    transform.localScale.y - speed * 0.75f * Time.deltaTime, 1);
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        else
        {
            while (transform.localScale.y < n3)
            {
                transform.localScale = new Vector3(transform.localScale.x,
                    transform.localScale.y - speed * 0.75f * Time.deltaTime, 1);
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }

        yield return new WaitForSeconds(2);
        isGo = false;
    }
}
