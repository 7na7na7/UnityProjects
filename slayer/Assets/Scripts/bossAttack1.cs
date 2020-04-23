using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class bossAttack1 : MonoBehaviour
{
    public float speed;

    private void Start()
    {
        transform.position=new Vector3(Random.Range(481-25,481+25),37.6f,0);
    }

    void Update()
    {
       transform.Translate(0,-speed*Time.deltaTime,0);
    }
}
