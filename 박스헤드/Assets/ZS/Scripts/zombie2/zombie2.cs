using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class zombie2 : zombieclass
{
    private GameObject parent;

    private void Start()
    {
        parent = transform.parent.gameObject;
        if(level.currentzombie>level.zombiecount[level.i])
            Destroy(parent);
        else
            level.currentzombie++;
    }

    private void LateUpdate()
    {
        if (hp.transform.localScale.x <= 0)
        {
            int a = UnityEngine.Random.Range(0,100);
            if (0<=a&&a<=coinpercent) 

    {
                Instantiate(silver,
                    new Vector3(this.transform.position.x, this.transform.position.y, silver.transform.position.z),
                    Quaternion.identity); //동화생성
            }
            int r = UnityEngine.Random.Range(0, player.heartpercent);
            if (r == 0)
                Instantiate(heart,
                    new Vector3(this.transform.position.x, this.transform.position.y, heart.transform.position.z),
                    Quaternion.identity);
            Destroy(parent);
        }
    }
}
