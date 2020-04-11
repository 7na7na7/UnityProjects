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
            if (boss.dmgDelay >= 0.05f&&!GameManager.instance.isGameOver)
            {
                if (Player.instance.playerIndex == 1)
                {
                    if (kagura.instance.isKagura)
                    {
                        Player.instance.rotate(transform.position);
                        
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
                        if (Player.instance.isattack)
                        {
                            kagura.instance.valueUp(1);
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
                else
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
}
