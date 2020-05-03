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
            if (boss.dmgDelay >= 0.1f&&!GameManager.instance.isGameOver&&!Player.instance.isSuper)
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
                            {
                                HeartManager.instance.Damaged1();
                                HeartManager.instance.damaged2();
                            }
                        }
                    }
                }
                else
                {
                    if (Player.instance.isattack)
                    {
                        if(Player.instance.playerIndex==3)
                            kagura.instance.ValueUp2(1);
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
                        HeartManager.instance.damaged2();
                        HeartManager.instance.Damaged1();
                    }
                }
            }
        }
    }
}
