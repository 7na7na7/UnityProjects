using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpEffect : MonoBehaviour
{
    private SpriteRenderer spr;
    Color color;
    public float scaleSpeed, posSpeed;
    private void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        color.r = 255;
        color.g = 255;
        color.b = 255;
        color.a = 0.5f;
    }
    
    void Update()
    {
        if (color.a > 0)
        {
            color.a -= 0.01f;
            spr.color = color;
            transform.localScale += new Vector3(1,1.5f,0) * Time.deltaTime * scaleSpeed;
            transform.Translate(Vector3.left*Time.deltaTime*posSpeed);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
