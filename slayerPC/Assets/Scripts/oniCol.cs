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
            if(oni.oniIndex==10) 
                Destroy(oni.gameObject);
                if (oni.dmgDelay >= 0.1f && !GameManager.instance.isGameOver&&!Player.instance.isSuper)
                {
                    if (Player.instance.playerIndex == 1)
                    {
                        if (kagura.instance.isKagura)
                        {
                            Player.instance.rotate(transform.position);

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
                            if (Player.instance.isattack)
                            {
                                kagura.instance.valueUp(1);
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
                            HeartManager.instance.Damaged1();
                            HeartManager.instance.damaged2();
                        }
                    }
                }
        }
    }
}
