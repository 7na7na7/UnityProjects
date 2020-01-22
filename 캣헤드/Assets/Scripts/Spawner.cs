using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public float SpawnDelayUpValue;
    private bool canBossSpawn = true;
    public GameObject BossMonster;
    private GameManager gm;
    public GameObject monster;
    public float samlldelay,bigdelay;

    void Start()
    {
        if (gameObject.name != "LRSpawner")
            StartCoroutine(spawn());
        
        gm = FindObjectOfType<GameManager>();
            
    }

    private void Update()
    {
        if (gameObject.name == "LRSpawner")
        {
            if (gm.currentzombie == gm.zombiecount[gm.i] - 1)
            {
                if (canBossSpawn)
                {
                    Instantiate(BossMonster, transform.position, Quaternion.identity);
                    canBossSpawn = false;
                    samlldelay -= samlldelay*SpawnDelayUpValue;
                   bigdelay -= bigdelay*SpawnDelayUpValue;
                    StartCoroutine(delaySet());
                }
            }
        }
    }

    IEnumerator delaySet()
    {
        yield return new WaitForSeconds(5f);
        canBossSpawn = true; 
    }
    IEnumerator spawn()
    {
        yield return new WaitForSeconds(5f);
        while (true)
        {
            if (gm.currentzombie < gm.zombiecount[gm.i] && gm.currentzombie - 1 != gm.zombiecount[gm.i])
                {
                    if (gameObject.name == "UpSpawner")
                    {
                        GameObject mob = Instantiate(monster, transform.position, Quaternion.identity);
                        mob.GetComponent<SlimeScript>().StartCoroutine(mob.GetComponent<SlimeScript>().UpSpawner());
                    }
                    else if (gameObject.name == "DownSpawner")
                    {
                        GameObject mob = Instantiate(monster, transform.position, Quaternion.identity);
                        mob.GetComponent<SlimeScript>().StartCoroutine(mob.GetComponent<SlimeScript>().DownSpawner());
                    }
                    else
                    {
                        Instantiate(monster, transform.position, Quaternion.identity);
                    }
                }
            
            yield return new WaitForSeconds(Random.Range(samlldelay, bigdelay));
        }
    }
}
