using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool isMelee;
    public bool isRock;
public int damage;

private void OnCollisionEnter(Collision other)
{
    if(other.gameObject.tag=="Floor" && !isRock)
        Destroy(gameObject,3);
}

private void OnTriggerEnter(Collider other)
{
   
        if(!isMelee&&other.gameObject.tag=="Wall")
            Destroy(gameObject);
}
}
