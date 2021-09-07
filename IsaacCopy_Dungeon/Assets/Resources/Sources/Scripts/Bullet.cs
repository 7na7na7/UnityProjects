using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int Dmg=10;
    public float nuckBackDistance;
    public Sprite none;
    public float speed = 5;
    public float DestroyTime = 1;
    private int dir;
    private SpriteRenderer spr;

    private Vector3 savedLocalScale;
    void Start()
    {
        Destroy(gameObject,DestroyTime);
        spr = GetComponent<SpriteRenderer>();
        savedLocalScale = transform.localScale;
        transform.localScale=Vector3.zero;
        transform.DOScale(savedLocalScale, 0.1f);
    }
    
    void Update()
    {
        transform.Translate(Vector3.right*speed*Time.deltaTime);
    }
}
