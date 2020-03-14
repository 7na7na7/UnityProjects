using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Bullet : MonoBehaviourPunCallbacks
{
    public float speed = 5;
    private int dir;
    public PhotonView pv;
    void Start()
    {
        Destroy(gameObject,1f);
    }
    
    void Update()
    {
        transform.Translate(new Vector3(-0.5f, -0.5f, 0) * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Gay"))
            Destroy(gameObject);
    }
}