using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class boss : MonoBehaviour
{
    public GameObject top;
    public GameObject mid;
    public GameObject bot;
    public float speedupdelay;
    public float speedupforce;
    public float speed;
    public float topshakespeed,midshakespeed,botshakespeed;

    public Transform player;
    public AudioSource closebosssound;

    private bool istop, ismid, isbot = false;

    private bool isboss = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(speedup());
        StartCoroutine(topshake());
        StartCoroutine(midshake());
        StartCoroutine(botshake());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed*Time.deltaTime,0,0);
        if(istop) 
            top.transform.Translate(0,topshakespeed*Time.deltaTime,0);
        if(ismid)
            mid.transform.Translate(0,midshakespeed*Time.deltaTime,0);
        if(isbot)
            bot.transform.Translate(0,botshakespeed*Time.deltaTime,0);
        if(Vector2.Distance(transform.position, player.position) <= 20f && !closebosssound.isPlaying)
        {
            closebosssound.Play();
        }

    }

    IEnumerator speedup()
    {
        while (true)
        {
            yield return new WaitForSeconds(speedupdelay);
            speed += speed * speedupforce;
        }
    }

    IEnumerator topshake()
    {
        while (true)
        {
            float top=Random.Range(0.15f, 0.3f);
            istop = true;
            yield return new WaitForSeconds(top);
            topshakespeed *= -1;
            yield return new WaitForSeconds(top);
            istop = false;
            yield return new WaitForSeconds(Random.Range(1.0f,3.0f));
        }
    }
    IEnumerator midshake()
    {
        while (true)
        {
            float top=Random.Range(0.15f, 0.3f);
            ismid = true;
            yield return new WaitForSeconds(top);
            midshakespeed *= -1;
            yield return new WaitForSeconds(top);
            ismid = false;
            yield return new WaitForSeconds(Random.Range(1.0f,3.0f));
        }
    }
    IEnumerator botshake()
    {
        while (true)
        {
            float top=Random.Range(0.15f, 0.3f);
            isbot = true;
            yield return new WaitForSeconds(top);
            botshakespeed *= -1;
            yield return new WaitForSeconds(top);
            isbot = false;
            yield return new WaitForSeconds(Random.Range(1.0f,3.0f));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Destroy(other.gameObject);
    }
}
