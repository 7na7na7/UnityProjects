using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummyzombie : MonoBehaviour
{
    public UnityEngine.UI.Slider hp;
    public GameObject heart;
    private GameObject obj;
    private Transform target; // 따라갈 물체의 방향
    public float speed = 1.0f;

    private void Start()
    {
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
            hp.value--;
            if (hp.value == 0)
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
    }
}
