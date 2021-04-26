using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private Rigidbody rigid;
    private SphereCollider sphereCol;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        sphereCol = GetComponent<SphereCollider>();
    }

    public enum Type
    { Ammo, Coin, Grenade, Heart, Weapon };

    public Type type;
    public int value;

    void Update()
    {
        transform.Rotate(Vector3.up*20*Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Floor")
        {
            rigid.isKinematic = true;
            sphereCol.enabled = false;
        }
    }
}
