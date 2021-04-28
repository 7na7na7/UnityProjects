using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool isMelee = true;
public int damage;

private void OnCollisionEnter(Collision other)
{
    if(other.gameObject.tag=="Floor")
        Destroy(gameObject,3);
    else if(other.gameObject.tag=="Wall")
        Destroy(gameObject);
}

private void OnTriggerEnter(Collider other)
{
   
        if(!isMelee&&other.gameObject.tag=="Wall")
            Destroy(gameObject);
}
}
