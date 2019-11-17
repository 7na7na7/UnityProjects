using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SandbagScript : MonoBehaviour
{
    private void Start()
    {
        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        Color color = spr.color;
        color.r = Random.Range(0, 2);
        color.g = Random.Range(0, 2);
        color.b = Random.Range(0, 2);
        spr.color = color;

        int r = Random.Range(0, 2);
        transform.position=new Vector3(r==0?transform.position.x:transform.position.x*-1,transform.position.y,transform.position.z);
    }

    IEnumerator delaymove(float x)
    {
        for (int i=0;i<20;i++)
        {
            yield return new WaitForSeconds(0.01f);
            transform.Translate(x*0.05f, 0, 0);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("punch"))
        {
            Destroy(this.gameObject);
        }
    }

    public void onTouch()
    {
        if (!PlayerScript.instance.isdead)
        {
            if (PlayerScript.instance.canpunch)
            {
                if (transform.position.x < -1) //왼쪽에 있으면
                {
                    StartCoroutine(delaymove(2));
                }
                else if (transform.position.x > 1) //오른쪽에 있으면
                {
                    StartCoroutine(delaymove(-2));
                }
            }
        }
    }
}
