﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour
{
    private int damage = 0;
    private AudioSource audio;
    public AudioClip breakSound;
    private SpriteRenderer spr;
    public Sprite wall1, wall2, wall3, wall4;

    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
        transform.position=new Vector3(Mathf.RoundToInt(transform.position.x),Mathf.RoundToInt(transform.position.y),0);
    }

    void Start()
    {
        audio = Camera.main.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
            damage += 10;
        if (other.CompareTag("Grenade"))
            damage += 50;
        if (other.CompareTag("BossBullet"))
            damage += 30;
    }

    private void Update()
    {
        if (damage >= 200)
        {
            damage += 100;
            audio.PlayOneShot(breakSound,1f);
            Destroy(gameObject);
        }
        else
        {
            if (0 <= damage && damage <= 50)
                spr.sprite = wall1;
            else if (50 < damage && damage <= 100)
                spr.sprite = wall2;
            else if (100 < damage && damage <= 150)
                spr.sprite = wall3;
            else if (150 < damage && damage <= 200)
                spr.sprite = wall4; 
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Container")||other.gameObject.CompareTag("wallsu"))
        {
            if(other.gameObject.transform.position.x<transform.position.x)
                transform.Translate(1,0,0);
            else
                transform.Translate(-1,0,0);
            if(other.gameObject.transform.position.y<transform.position.y)
                transform.Translate(0,1,0);
            else
                transform.Translate(0,-1,0);
            
        }
    }
}