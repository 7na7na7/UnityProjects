using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombie3 : MonoBehaviour
{
   
    public GameObject silver;
    public GameObject critical;
    private GameObject parent;
    public UnityEngine.UI.Slider hp;
    
    private void Start()
    {
        Level level = FindObjectOfType<Level>();
        for(int i=0;i<level.wave/5;i++)
        {
            hp.maxValue += 1f;
            hp.value +=1f;
        }
        parent = transform.parent.gameObject;
        if(level.currentzombie>level.zombiecount[level.i])
            Destroy(parent);
    }

    private void Update()
    {
        if (hp.value <= 0)
        {
            Instantiate(silver, new Vector3(this.transform.position.x, this.transform.position.y, silver.transform.position.z), Quaternion.identity); //동화생성
            GameOver gameover = GameObject.Find("Canvas").GetComponent<GameOver>();
            gameover.zombiecount += 1;
            Level level = FindObjectOfType<Level>();
            level.zombiecount[level.i]--; //생성되면 zombiecount--
            Destroy(parent);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        bananagun gun = FindObjectOfType<bananagun>();
        if (other.CompareTag("banana"))
        {
            int r = UnityEngine.Random.Range(0, 30);
            if (r == 0)
            {
                Instantiate(critical, new Vector3(transform.position.x + 0.1f, transform.position.y + 0.5f,transform.position.z),Quaternion.identity);
                if (gun.weapon == "sniper")
                    hp.value -=6;
                else if (gun.weapon == "knife")
                    hp.value -= 10;
                else
                    hp.value -= 2;
            }
            else
            {
                if (gun.weapon == "sniper")
                    hp.value -= 3;
                else if (gun.weapon == "knife")
                    hp.value -= 5;
                else
                    hp.value--;
            }
            Move1 player = GameObject.Find("player").GetComponent<Move1>();
            if (r == 0)
            {
                hp.value -= player.damagecount;
            }
            else
            {
                hp.value -= player.damagecount * 0.5f;
            }
        }
    }
}
