using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * 15);
        Destroy(gameObject,5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Wall"))
           Destroy(gameObject);
        else
        {
            if ((other.name.Substring(0, 7) == "Player1" && gameObject.name.Substring(0, 7) == "Bullet2"))
            {
                if (!other.GetComponent<PlayerMove>().isSuper)
                {
                    Destroy(gameObject);   
                }
            }
            if(other.name.Substring(0, 7) == "Player2" && gameObject.name.Substring(0, 7) == "Bullet1")
            {
                if (!other.GetComponent<PlayerMove>().isSuper)
                {
                    Destroy(gameObject);   
                }
            }
        }
    }
}
