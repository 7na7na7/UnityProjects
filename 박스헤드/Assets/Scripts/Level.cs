using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Level : MonoBehaviour
{
    private Move1 player;
    public Text wavetext;
    public bool isdelay = false;
    public int spawncount = 0;
    //public Transform parent;
    public int wave = 1;
    public int[] zombiecount;
    public float waitTime = 10;
    public GameObject spawner;
    void Start()
    {
        player = GameObject.Find("player").GetComponent<Move1>();
        Instantiate(spawner, new Vector3(Random.Range(player.min.position.x, player.max.position.x), player.min.position.y, 0),Quaternion.identity); //아래
        Instantiate(spawner, new Vector3(Random.Range(player.min.position.x, player.max.position.x), player.max.position.y, 0),Quaternion.identity); //위
        Instantiate(spawner, new Vector3( player.max.position.x,Random.Range(player.min.position.y,player.max.position.y), 0),Quaternion.identity); //오른
        Instantiate(spawner, new Vector3( player.min.position.x,Random.Range(player.min.position.y,player.max.position.y), 0),Quaternion.identity); //왼
        StartCoroutine(spawncoroutine());
        StartCoroutine(spawn());
    }

    private void Update()
    {
        wavetext.text = "Wave " + wave;
    }

    IEnumerator spawncoroutine()
    {
        while (true)
        {
            int a = Random.Range(0, 4);
            if (a == 0)
                Instantiate(spawner, new Vector3(Random.Range(player.min.position.x, player.max.position.x), player.min.position.y, 0),Quaternion.identity); //아래
            else  if (a == 1)
                Instantiate(spawner, new Vector3(Random.Range(player.min.position.x, player.max.position.x), player.max.position.y, 0),Quaternion.identity); //위
            else  if (a == 2)
                Instantiate(spawner, new Vector3( player.max.position.x,Random.Range(player.min.position.y,player.max.position.y), 0),Quaternion.identity); //오른
            else
                Instantiate(spawner, new Vector3( player.min.position.x,Random.Range(player.min.position.y,player.max.position.y), 0),Quaternion.identity); //왼
            //spawner.transform.SetParent(parent.transform);
            yield return new WaitForSeconds(waitTime);
        }
    }
    IEnumerator spawn()
    {
        if (wave-1 > zombiecount.Length)
        {
            Debug.Log("끝!");
            StopAllCoroutines();
        }
        for (int i = 0; i < zombiecount.Length; i++)
        {
            yield return new WaitUntil(()=>spawncount>zombiecount[i]);
            isdelay = true;
            wave++;
            yield return new WaitForSeconds(15f); //웨이브 하나 끝나고 딜레이
            Debug.Log("웨이브 "+wave);
            isdelay = false;
            spawncount = 0;
            
        }
    }
}
