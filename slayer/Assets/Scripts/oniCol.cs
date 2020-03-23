using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oniCol : MonoBehaviour
{
    public bool isHead = false;
    public oniMove oni;


    private void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.CompareTag("Player"))
        {
            if (oni.dmgDelay >= 0.1f)
            {
                if (Player.instance.isattack)
                {
                    if (isHead)
                    {
                        oni.die(true);
                        Player.instance.oniHead(gameObject);
                    }
                    else
                    {
                        oni.die(false);
                        Player.instance.oniBody(gameObject);
                    }
                }
                else
                {
                    //Player.instance.die();
                }
            }
        }
    }
}
