using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heart : MonoBehaviour
{
    private void Start()
    {
        Destroy(this.gameObject,10f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Move1 player = GameObject.Find("player").GetComponent<Move1>();
            player.slider.value += 5;
            Destroy(this.gameObject);
        }
    }
}
