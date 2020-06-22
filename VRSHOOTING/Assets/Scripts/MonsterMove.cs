using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterMove : MonoBehaviour
{
    public float minDelay, maxDelay;
    public float speed;
    
    private Vector3 dir = Vector3.zero;
    private void Start()
    {
        StartCoroutine(RandomMove());
    }

    private void Update()
    {
        transform.Translate(dir*speed*Time.deltaTime);
    }

    IEnumerator RandomMove()
    {
        while (true)
        {
            dir=new Vector3(Random.Range(-1f,1f),0,Random.Range(-1f,1f));
            yield return new WaitForSeconds(Random.Range(minDelay,maxDelay));
        }
    }
}
