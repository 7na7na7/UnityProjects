using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerBoom : MonoBehaviour
{
    private AudioSource audio;
    public AudioClip explosionSound;
    public GameObject explosion;

    private void Awake()
    {
        transform.position=new Vector3(Mathf.RoundToInt(transform.position.x),Mathf.RoundToInt(transform.position.y),0);
        
        /*
        //X보정
        if (transform.position.x > Mathf.RoundToInt(transform.position.x))
        {
            if(transform.position.x>Mathf.RoundToInt(transform.position.x)+0.25f) 
                transform.position=new Vector3(Mathf.RoundToInt(transform.position.x)+0.5f,transform.position.y,0);
            else
                transform.position=new Vector3(Mathf.RoundToInt(transform.position.x),transform.position.y,0);
        }
        else
        {
            if(transform.position.x<Mathf.RoundToInt(transform.position.x)-0.25f) 
                transform.position=new Vector3(Mathf.RoundToInt(transform.position.x)-0.5f,transform.position.y,0);
            else
                transform.position=new Vector3(Mathf.RoundToInt(transform.position.x),transform.position.y,0);
        }
        //Y보정
        if (transform.position.y > Mathf.RoundToInt(transform.position.y))
        {
            if(transform.position.y>Mathf.RoundToInt(transform.position.y)+0.25f) 
                transform.position=new Vector3(transform.position.x,Mathf.RoundToInt(transform.position.y)+0.5f,0);
            else
                transform.position=new Vector3(transform.position.x,Mathf.RoundToInt(transform.position.y),0);
        }
        else
        {
            if(transform.position.y<Mathf.RoundToInt(transform.position.y)-0.25f) 
                transform.position=new Vector3(transform.position.x,Mathf.RoundToInt(transform.position.y)-0.5f,0);
            else
                transform.position=new Vector3(transform.position.x,Mathf.RoundToInt(transform.position.y),0);
        }
        */
    }

    void Start()
    {
        audio = Camera.main.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet") || other.CompareTag("Grenade") || other.CompareTag("BossBullet"))
        {
            StartCoroutine(boooom());
        }
    }

    IEnumerator boooom()
    {
        yield return new WaitForSeconds(0.05f);
        Instantiate(explosion, transform.position, Quaternion.identity);
        audio.PlayOneShot(explosionSound,1f);
        Destroy(gameObject);
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
