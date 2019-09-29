using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class zombieclass : MonoBehaviour
{
    public float fullattack = 0, lessattack = 0;
    public int coinpercent = 75;
    protected bananagun gun;
    protected Level level;
    protected GameOver gameover;
    protected Move1 player;
    public float levelpower;
    public GameObject heart;
    public GameObject hp, hpframe;
    public float speed = 1.0f;
    public GameObject critical;
    public GameObject brown;
    public GameObject silver;
    public GameObject gold;
    private void Awake()
    {
        gun = FindObjectOfType<bananagun>();
        level = FindObjectOfType<Level>();
        gameover = FindObjectOfType<GameOver>();
        player = FindObjectOfType<Move1>();
        for(int i=0;i<(level.wave-1)/5;i++)
        { 
            hp.transform.localScale += new Vector3(levelpower, 0, 0);
            hpframe.transform.localScale += new Vector3(levelpower, 0, 0);
            hp.transform.Translate(-1*levelpower*0.25f,0,0);
            hpframe.transform.Translate(-1*levelpower*0.25f,0,0);
        }
    }

    private void Update()
    {
        if (hp.transform.localScale.x <= 0)
        {
            gameover.zombiecount += 1;
            level.zombiecount[level.i]--;
            level.currentzombie--;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("banana")||other.CompareTag("knife"))
        {
            if (gun.weapon == "sniper")
                hp.transform.localScale += new Vector3(-1*player.sniperdamage, 0, 0);
            else if (gun.weapon == "knife")
                hp.transform.localScale += new Vector3(-1*player.knifedamage, 0, 0);
            else
                hp.transform.localScale += new Vector3(-1*player.pistoldamage, 0, 0);
                int r = Random.Range(0, player.criticalpercent); //치명타확률계산
            if (r == 0)
            {
                Instantiate(critical, new Vector3(transform.position.x + 0.1f, transform.position.y + 0.5f,transform.position.z),Quaternion.identity);
                if(gun.weapon=="pistol")
                    hp.transform.localScale += new Vector3(-1*player.pistoldamage, 0, 0);
                else if (gun.weapon == "sniper")
                    hp.transform.localScale += new Vector3(-1*player.sniperdamage, 0, 0);
                else if (gun.weapon == "knife")
                    hp.transform.localScale += new Vector3(-1*player.knifedamage, 0, 0);
                hp.transform.localScale += new Vector3(-1*player.criticaldamage, 0, 0);
            }
            if (r == 0) //총기강화
            {
                hp.transform.localScale += new Vector3(-1*player.damagecount, 0, 0);
            }
            else
            {
                hp.transform.localScale += new Vector3(-1*player.damagecount*0.5f, 0, 0);
            }
            if (player.slider.maxValue - player.slider.value == 0) //풀피일떄
            {
                hp.transform.localScale += new Vector3(-1*fullattack, 0, 0);
            }

            if (player.slider.value <= player.slider.maxValue * 0.3f) //현재hp의30%이하일때
            {
                hp.transform.localScale += new Vector3(-1*lessattack, 0, 0);
            }
        }
    }
}
