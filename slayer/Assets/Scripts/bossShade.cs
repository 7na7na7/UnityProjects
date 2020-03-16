using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossShade : MonoBehaviour
{
    public GameObject boss;
    Color color;
    private SpriteRenderer spr;
    private void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        color.r = 255;
        color.g = 255;
        color.b = 255;
        color.a = 0;
    }
    
    void Update()
    {
        if (color.a < 1)
        {
            color.a += 0.01f;
            spr.color = color;
        }
        else
        {
            Instantiate(boss, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
