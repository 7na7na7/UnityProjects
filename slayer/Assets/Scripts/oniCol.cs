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
            if (Player.instance.isattack)
            {
                if(isHead)
                    oni.die(true);
                else
                    oni.die(false);
            }
        }
    }
    
}
