using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class zombie : MonoBehaviour
{
    private Transform parent;
    public GameObject brown;
    public GameObject gold;
    public GameObject critical;
    public UnityEngine.UI.Slider hp;
    public GameObject heart;
    private GameObject obj;
    private Transform target; // 따라갈 물체의 방향
    public float speed = 1.0f;

    private void Start()
    {
        Level level = FindObjectOfType<Level>();
        for(int i=0;i<level.wave/5;i++)
        {
            if (speed == 1.2f)
            {
                hp.maxValue += 1.5f;
                hp.value +=1.5f;
            }
            else
            {
                hp.maxValue += 5;
                hp.value +=5;
            }
            
        }
        level.currentzombie++;
        obj = this.gameObject;
    }

    void Update()
    {
        Move1 player = GameObject.Find("player").GetComponent<Move1>();
        target = player.transform;
        Vector3 dir = target.position - transform.position; //사이의 거리를 구함
        transform.position +=
            new Vector3(Mathf.Clamp(dir.x, speed * -1, speed), Mathf.Clamp(dir.y, speed * -1, speed), dir.z) * speed *
            Time.deltaTime;
        if (hp.value <= 0)
        {
            if(speed!=1.2f) 
                Instantiate(gold, new Vector3(this.transform.position.x, this.transform.position.y, gold.transform.position.z), Quaternion.identity); //금화생성
            else
            {
                int a = Random.Range(0, 20);
                if (a == 0)
                    Instantiate(heart,
                        new Vector3(this.transform.position.x, this.transform.position.y, heart.transform.position.z),
                        Quaternion.identity);
                Instantiate(brown,
                    new Vector3(this.transform.position.x, this.transform.position.y, brown.transform.position.z),
                    Quaternion.identity); //동화생성
            }

            GameOver gameover = GameObject.Find("Canvas").GetComponent<GameOver>();
            gameover.zombiecount += 1;
            Level level = FindObjectOfType<Level>();
            level.zombiecount[level.i]--; //생성되면 zombiecount--
            Debug.Log("현재 level.i" + level.i);
            Destroy(hp);
            Destroy(obj);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        bananagun gun = FindObjectOfType<bananagun>();
        if (other.CompareTag("banana"))
        {
            int r = Random.Range(0, 30);
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
