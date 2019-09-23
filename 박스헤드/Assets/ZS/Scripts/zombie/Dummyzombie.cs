using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummyzombie : MonoBehaviour
{
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
            hp.maxValue += 1.5f;
            hp.value +=1.5f;
        }
        obj = this.gameObject;
        Destroy(obj,30f);
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
            GameOver gameover = GameObject.Find("Canvas").GetComponent<GameOver>();
            gameover.zombiecount += 1;
            int a = Random.Range(0, 20);
            if (a == 0)
                Instantiate(heart,
                    new Vector3(this.transform.position.x, this.transform.position.y, heart.transform.position.z),
                    Quaternion.identity);
            Destroy(obj);
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
