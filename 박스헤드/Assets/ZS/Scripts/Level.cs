using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Level : MonoBehaviour
{
    public Text goldtext;
    public bool isnext = false;
    public bool canpause = true;
    public Canvas shop;
    private int bossNumber = 0;
    public GameObject[] Boss;
    public bool isBossAppear = false;
    public int savedcount;
    public int currentzombie = 0;
    public int i = 0;
    private Move1 player;
    public Text wavetext, lefttext;
    public bool isdelay = false;
    //public Transform parent;
    public int wave = 1;
    public int[] zombiecount;
    public float waitTime = 10;
    public GameObject spawner;
    void Start()
    {
        GoldManager gold = FindObjectOfType<GoldManager>();
        gold.gold = 0;
        player = GameObject.Find("player").GetComponent<Move1>();
        Instantiate(spawner, new Vector3(Random.Range(player.min.position.x-10, player.max.position.x+10), player.min.position.y-10, 0),Quaternion.identity); //아래
        Instantiate(spawner, new Vector3(Random.Range(player.min.position.x-10, player.max.position.x+10), player.max.position.y+10, 0),Quaternion.identity); //위
        Instantiate(spawner, new Vector3( player.max.position.x+10,Random.Range(player.min.position.y-10,player.max.position.y+10), 0),Quaternion.identity); //오른
        Instantiate(spawner, new Vector3( player.min.position.x-10,Random.Range(player.min.position.y-10,player.max.position.y+10), 0),Quaternion.identity); //왼
        StartCoroutine(spawncoroutine());
        StartCoroutine(spawn());
    }

    private void Update()
    {
        GoldManager gold = FindObjectOfType<GoldManager>();
        goldtext.text = gold.gold.ToString() + " " + "Gold";
        lefttext.text = "Left Zombie : " + zombiecount[i];
        wavetext.text = "Wave " + wave;
    }

    IEnumerator spawncoroutine()
    {
        while (true)
        {
            if (isBossAppear == false)
            {
                int a = Random.Range(0, 4);
                if (a == 0)
                    Instantiate(spawner,
                        new Vector3(Random.Range(player.min.position.x - 10, player.max.position.x + 10),
                            player.min.position.y - 10, 0), Quaternion.identity); //아래
                else if (a == 1)
                    Instantiate(spawner,
                        new Vector3(Random.Range(player.min.position.x - 10, player.max.position.x + 10),
                            player.max.position.y + 10, 0), Quaternion.identity); //위
                else if (a == 2)
                    Instantiate(spawner,
                        new Vector3(player.max.position.x + 10,
                            Random.Range(player.min.position.y - 10, player.max.position.y + 10), 0),
                        Quaternion.identity); //오른
                else
                    Instantiate(spawner,
                        new Vector3(player.min.position.x - 10,
                            Random.Range(player.min.position.y - 10, player.max.position.y + 10), 0),
                        Quaternion.identity); //왼
                //spawner.transform.SetParent(parent.transform);
            }

            yield return new WaitForSeconds(waitTime);
        }
    }
    IEnumerator spawn()
    {
        save save = FindObjectOfType<save>();
        GameOver gameover = FindObjectOfType<GameOver>();
        if (wave > zombiecount.Length) //wave-1일수도...
        {
            Debug.Log("끝!");
            StopAllCoroutines();
        }

        for (i = 0; i < zombiecount.Length; i++) //끝나고 i++
        {
            savedcount = zombiecount[i];
            yield return new WaitUntil(() => zombiecount[i] <= 0);
            isdelay = true;
            //currentzombie = 0; //현재좀비수 초기화
            if(wave>=1)
            { 
                canpause = false; 
                yield return new WaitForSeconds(5f); 
                Time.timeScale = 0; //시간 멈추고 상점소환
                shop.gameObject.SetActive(true); //상점소환
                yield return new WaitUntil(() => isnext == true); //isnext가 true가 될때까지 딜레이
                isnext = false;
                canpause = true;
            }
            player.audiosource.pitch += 0.035f;
            wave++;
            save.savedwave = wave;
            save.savedkill = gameover.zombiecount;
            Debug.Log("웨이브 "+ wave);
            if (wave % 5 == 0) //웨이브5번째마다 보스출현
            {
                if(wave%20==0)
                    Instantiate(Boss[bossNumber],new Vector3(transform.position.x*-1,transform.position.y+25,transform.position.z),Quaternion.identity);
                Instantiate(Boss[bossNumber],new Vector3(transform.position.x,transform.position.y+25,transform.position.z),Quaternion.identity);
                bossNumber++;
                isBossAppear = true;
            }
            isdelay = false;
        }
    }
}
