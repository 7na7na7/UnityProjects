using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SlimeScript : MonoBehaviour
{
    public GameObject[] bloods;
    private bool canMove = true;
    private GameManager gm;
    public Slider hp;
    public LayerMask layer; //감지할 레이어
    public float speed = 5f; //속도
    private float[] radius=new float[150];
    private bool isUp = false;
    private bool isDown = false;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        gm.zombieCreate();
        if (gm.currentzombie > gm.zombiecount[gm.i])
        {
            gm.currentzombie--;
            Destroy(gameObject);
        }

        for (int i = 0; i < 150; i++)
        {
            radius[i] = i * 0.1f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            if (!other.GetComponent<Bullet>().isCollide)
            {
                other.GetComponent<Bullet>().isCollide = true;
                hp.value -= 35;
            }
        }
    }

    private void Update()
    {
        if (canMove)
        {
            if (isUp)
            {
                transform.Translate(Vector2.down * Time.deltaTime * speed);
            }
            else if (isDown)
            {
                transform.Translate(Vector2.up * Time.deltaTime * speed);
            }
            else
            {
                int i = 0;
                while (true)
                {
                    Collider2D col = Physics2D.OverlapCircle(transform.position, radius[i], layer);
                    if (col != null) //플레이어가 비지 않았다면
                    {
                        //부드럽게 플레이어를 따라감
                        Vector2 dir = col.transform.position - transform.position;
                        dir.Normalize();
                        transform.Translate(dir * Time.deltaTime * speed);
                        transform.position = new Vector3(transform.position.x, transform.position.y, 0); //Z축 고정
                        break;
                    }
                    else
                    {
                        i++;
                    }
                }
            }

            if (hp.value <= 0)
            {
                StartCoroutine(die());
            }
        }
    }

    public IEnumerator UpSpawner()
    {
        isUp = true;
        Vector3 lastPos = transform.position + new Vector3(0, -1.3f, 0);
        yield return new WaitUntil(()=>transform.position.y-lastPos.y<0.1f);
        isUp = false;
    }
    public IEnumerator DownSpawner()
    {
        isDown = true;
        Vector3 lastPos = transform.position + new Vector3(0, 1.3f, 0);
        yield return new WaitUntil(()=>lastPos.y-transform.position.y<0.1f);
        isDown = false;
    }

    IEnumerator die()
    {
        canMove = false;
        float a = 1f;
        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        Color color = spr.color;
        Destroy(GetComponent<CapsuleCollider2D>());
        while (true)
        {
            color.a -= 0.1f;
            spr.color = color;
            yield return new WaitForSeconds(Time.deltaTime);
            if (color.a < 0.1f)
                break;
        }
        int r = Random.Range(0, 4);
        Instantiate(bloods[r],transform.position,Quaternion.EulerAngles(new Vector3(0,0,Random.Range(0,360))));
        gm.zombieDead();
        Destroy(gameObject);
    }

    public void onHit()
    {
        int r = Random.Range(0, 5);
        if (r == 0)
        {
            int r2 = Random.Range(0, 4);
            Instantiate(bloods[r2],transform.position,Quaternion.EulerAngles(new Vector3(0,0,Random.Range(0,360))));
        }
    }
}
