using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YouDoTanCol : MonoBehaviour
{
    public YouDoTan attack;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            attack.isAttackover = true;   
            attack.DestroyCor();
        }
    }
}
