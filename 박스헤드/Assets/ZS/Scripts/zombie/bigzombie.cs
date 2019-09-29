using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bigzombie : zombieclass
{
    private GameObject obj;
    private Transform target;

    private void Start()
    {
        obj = this.gameObject;
        if(level.currentzombie>level.zombiecount[level.i])
            Destroy(obj);
        else
            level.currentzombie++;
    }

    void LateUpdate()
    {
        target = player.transform;
        Vector3 dir = target.position - transform.position; //사이의 거리를 구함
        dir.Normalize();
        transform.position += dir * speed * Time.deltaTime;
        if (hp.transform.localScale.x <= 0)
        {
            if (hp.transform.localScale.x <= 0)
            {
                int a = Random.Range(0, player.heartpercent);
                if (a == 0)
                    Instantiate(heart,
                        new Vector3(this.transform.position.x, this.transform.position.y, heart.transform.position.z),
                        Quaternion.identity);
                int r = UnityEngine.Random.Range(0, 100);
                if (0 <= r && r <= coinpercent)
                {
                    Instantiate(gold,
                        new Vector3(this.transform.position.x, this.transform.position.y, brown.transform.position.z),
                        Quaternion.identity); //동화생성
                }
            }
            Destroy(obj);
        }
    }
}