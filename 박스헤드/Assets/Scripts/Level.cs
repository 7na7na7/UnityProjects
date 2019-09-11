using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Level : MonoBehaviour
{
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
        Debug.Log(currentzombie);
        lefttext.text = "Left Zombie : " + zombiecount[i];
        wavetext.text = "Wave " + wave;
    }

    IEnumerator spawncoroutine()
    {
        while (true)
        {
            int a = Random.Range(0, 4);
            if (a == 0)
                Instantiate(spawner, new Vector3(Random.Range(player.min.position.x-10, player.max.position.x+10), player.min.position.y-10, 0),Quaternion.identity); //아래
            else  if (a == 1)
                Instantiate(spawner, new Vector3(Random.Range(player.min.position.x-10, player.max.position.x+10), player.max.position.y+10, 0),Quaternion.identity); //위
            else  if (a == 2)
                Instantiate(spawner, new Vector3( player.max.position.x+10,Random.Range(player.min.position.y-10,player.max.position.y+10), 0),Quaternion.identity); //오른
            else
                Instantiate(spawner, new Vector3( player.min.position.x-10,Random.Range(player.min.position.y-10,player.max.position.y+10), 0),Quaternion.identity); //왼
            //spawner.transform.SetParent(parent.transform);
            yield return new WaitForSeconds(waitTime);
        }
    }
    IEnumerator spawn()
    {
        if (wave-1 > zombiecount.Length) //wave-1이 맞나?
        {
            Debug.Log("끝!");
            StopAllCoroutines();
        }
        for (i = 0; i < zombiecount.Length; i++) //끝나고 i++
        {
            savedcount = zombiecount[i];
            yield return new WaitUntil(()=>zombiecount[i]==0);
            isdelay = true;
            //currentzombie = 0; //현재좀비수 초기화
            yield return new WaitForSeconds(5f); //웨이브 하나 끝나고 딜레이
            player.audiosource.pitch += 0.05f;
            wave++;
            Debug.Log("웨이브 "+ wave);
            isdelay = false;
        }
    }
}
