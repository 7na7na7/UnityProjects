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
        while (true)
        {
            if ((other.name == "Player1" && gameObject.name.Substring(0, 7) == "Bullet1")||(other.name == "Player2" && gameObject.name.Substring(0, 7) == "Bullet2"))
            {
                break;
            }
            else
            {
                if (other.GetComponent<PlayerMove>().isSuper)
                    break;
                
                Destroy(gameObject);
                break;
            }
        }
    }

}
