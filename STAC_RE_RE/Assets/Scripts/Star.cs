using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Star : MonoBehaviour
{
    public float minSpeed, maxSpeed;
    public float speed;
    public Bullet[] stars;

    private void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);
        foreach (Bullet star in stars)
        { 
            star.speed = speed;
            star.OnEnable();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Edge1")||other.CompareTag("Edge2"))
        {
            foreach (Bullet star in stars)
            { 
                if(star!=null) 
                    star.Star();
            }
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        bool b=false;
        foreach (Bullet star in stars)
        {
            if (star != null)
                b = true;
        }
        if(!b)
            Destroy(gameObject);
    }
}
