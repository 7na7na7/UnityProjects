using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebScript : MonoBehaviour
{
    private bool once = false;
    private Color color;
    private SpriteRenderer spr;
    public float beforeAttackDelay;
    public GameObject web;
    public float delay;
    public float speed;
    
    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        color.r = spr.color.r;
        color.g = spr.color.g;
        color.b = spr.color.b;
        color.a = 0;
    }

    void Update()
    {
        if (color.a < 1f)
        {
            if (!once)
            {
                color.a += 0.02f;
                spr.color = color;
            }
        }
        else
        {
            if (!once)
            {
                once = true;
                StartCoroutine(webAttack());
            }
        }
    }

    IEnumerator webAttack()
    {
        yield return new WaitForSeconds(beforeAttackDelay);
        web.transform.eulerAngles =
            new Vector3(0, 0,
                -getAngle(transform.position.x, transform.position.y, Player.instance.transform.position.x,
                    Player.instance.transform.position.y) + 180);
        while (true)
        {
            if (web)
            {
                web.transform.localScale = new Vector3(web.transform.localScale.x, web.transform.localScale.y + speed,
                    web.transform.localScale.z);
                yield return new WaitForSeconds(delay);
            }
            else
            {
                if (color.a > 0f)
                {
                    color.a -= 0.01f;
                    spr.color = color;
                }
                else
                {
                    Destroy(gameObject);
                }
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
    }
    private float getAngle(float x1, float y1, float x2, float y2) //Vector값을 넘겨받고 회전값을 넘겨줌
    {
        float dx = x2 - x1;
        float dy = y2 - y1;

        float rad = Mathf.Atan2(dx, dy);
        float degree = rad * Mathf.Rad2Deg;
        
        return degree;
    }
}
