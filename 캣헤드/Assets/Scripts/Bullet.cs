using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int dir;
    public bool isCollide = false;
    void Update()
    {
        //transform.Translate(Vector3.right * Time.deltaTime * 15);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall") || other.CompareTag("Slime")|| other.CompareTag("BossSlime"))
        {
            GetComponent<longBullet>().canLong = false;
            Destroy(gameObject,0.05f);
        }
        else
        {
            if ((other.name.Substring(0, 7) == "Player1" && gameObject.name.Substring(0, 7) == "Bullet2"))
            {
                if (!other.GetComponent<PlayerMove>().isSuper)
                {
                    GetComponent<longBullet>().canLong = false;
                    Destroy(gameObject,0.05f);
                }
            }
            if(other.name.Substring(0, 7) == "Player2" && gameObject.name.Substring(0, 7) == "Bullet1")
            {
                if (!other.GetComponent<PlayerMove>().isSuper)
                {
                    GetComponent<longBullet>().canLong = false; 
                    Destroy(gameObject,0.05f);
                }
            }
        }
    }
}
