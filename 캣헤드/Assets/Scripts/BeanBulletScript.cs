using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanBulletScript : MonoBehaviour
{
    private AudioSource audio;
    public AudioClip explosionSound;
    public float speedMinusValue;
    public float speed;
    public float force;
    public GameObject explosion;
    void Start()
    {
        audio = Camera.main.GetComponent<AudioSource>();
        StartCoroutine(cor());
    }

    IEnumerator cor()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.down * force;
        yield return new WaitForSeconds(0.5f);
        GetComponent<Rigidbody2D>().velocity = Vector2.up * force*0.6f;
        yield return new WaitForSeconds(0.3f);
        GetComponent<Rigidbody2D>().velocity = Vector2.down * force*0.2f;
        yield return new WaitForSeconds(0.6f);
        speed = 0;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        yield return new WaitForSeconds(0.75f);
        Instantiate(explosion, transform.position, Quaternion.identity);
        audio.PlayOneShot(explosionSound,1f);
        Destroy(gameObject);
    }

    private void Update()
    {
        transform.Translate(Vector3.right*speed*Time.deltaTime);
        if(speed>0)
            speed -= Time.deltaTime*speedMinusValue;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Wall"))
            Destroy(gameObject);
    }
}
