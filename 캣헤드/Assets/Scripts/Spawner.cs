using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    private GameManager gm;
    public GameObject monster;
    public float samlldelay,bigdelay;
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        StartCoroutine(spawn());
    }

    IEnumerator spawn()
    {
        while (true)
        {
            if (gm.currentzombie < gm.zombiecount[gm.i])
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
            yield return new WaitForSeconds(Random.Range(samlldelay,bigdelay));
        }
    }
}
