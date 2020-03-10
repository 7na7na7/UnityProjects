using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class oniMove : MonoBehaviour
{
    public float minX, maxX;
    private bool isStop = false;
    public GameObject effect;
    public float speed;

    void Update()
    {
        if(!isStop) 
            transform.Translate(Vector3.right*speed*Time.deltaTime);
        
        if (transform.position.x >= minX && transform.position.x <= maxX)
        {
            if (!isStop)
            {
                isStop = true;
                StartCoroutine(attack());
            }
        }
    }

    IEnumerator attack()
    {
        while (true)
        {
            GetComponent<Animator>().Play("oni1Attack");
            StartCoroutine(girl.instance.hitted(10));
            yield return new WaitForSeconds(0.5f);
            GetComponent<Animator>().Play("oni1Idle");
            yield return new WaitForSeconds(1f);
        }
    }
    public void die()
    {
        ScoreMgr.instance.scoreUp(100,false);
        Instantiate(effect,transform.position,Quaternion.identity);
        Destroy(gameObject);
    }
    private IEnumerator OnMouseDown()
    {
        FindObjectOfType<Player>().StopAllCoroutines();
        StartCoroutine(FindObjectOfType<Player>().go(transform.position,false));
        yield return null;
    }
    private void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.CompareTag("Player"))
        {
            if (Player.instance.isattack)
            {
                die();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D hit)
    {
        if (hit.CompareTag("Player"))
        {
            if (Player.instance.isattack)
            {
               die();
            }
        }
    }
}
