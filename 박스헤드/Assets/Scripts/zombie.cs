using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class zombie : MonoBehaviour
{
    public GameObject heart;
    private GameObject obj;
    private Transform target; // 따라갈 물체의 방향
    public float speed = 1.0f;

    private void Start()
    {
        Level level = FindObjectOfType<Level>();
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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("banana"))
        {
            GameOver gameover = GameObject.Find("Canvas").GetComponent<GameOver>();
            gameover.zombiecount += 1;
            int a = Random.Range(0, 20);
            if (a == 0)
                Instantiate(heart, new Vector3(this.transform.position.x,this.transform.position.y,heart.transform.position.z),Quaternion.identity);
            
            Level level = FindObjectOfType<Level>();
            level.zombiecount[level.i]--; //생성되면 zombiecount--
            Debug.Log("현재 level.i"+level.i);
            Destroy(obj);
        }
    }
}
