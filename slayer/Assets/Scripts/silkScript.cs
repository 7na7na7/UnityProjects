using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class silkScript : MonoBehaviour
{
    public GameObject bloodEffect;
    public Sprite[] sprites;
    public oniMove oni;
    public SpriteRenderer spr;
    public Sprite stringNULL;

    private void Start()
    {
        spr.sprite = sprites[Random.Range(0,sprites.Length)];
    }

    private void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.CompareTag("Player"))
        {
          if (oni.dmgDelay >= 0.1f && !GameManager.instance.isGameOver&&!Player.instance.isSuper)
                {
                    if (Player.instance.isattack)
                        {
                            if(Player.instance.playerIndex==3)
                                kagura.instance.ValueUp2(1);

                            oni.dmgDelay = 0;
                                oni.die(false);
                                Player.instance.oniBody(gameObject);

                                spr.sprite = stringNULL;
                                Instantiate(bloodEffect, transform.position, Quaternion.identity);
                                Destroy(gameObject);
                                
                                GameManager.instance.OnscoreFunc();
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
