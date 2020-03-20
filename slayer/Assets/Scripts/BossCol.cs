using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCol : MonoBehaviour
{
    public bool isHead = false;
    public bossScript boss;
    private void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.CompareTag("Player"))
        {
            if (boss.dmgDelay >= 0.1f)
            {
                if (Player.instance.isattack)
                {
                    if (isHead)
                    {
                        boss.die(true);
                        Player.instance.oniHead(gameObject);
                    }
                    else
                    {
                        boss.die(false);
                        Player.instance.oniBody(gameObject);
                    }
                }
                else
                {
                    Player.instance.die();
                }
            }
        }
    }
}
