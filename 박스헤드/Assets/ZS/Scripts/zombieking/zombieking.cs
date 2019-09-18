using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieking : MonoBehaviour
{
    public GameObject dummy;
    private Vector3 dir;
    private bool cananotdetect = false;
    private Animator anim;
    private bool ismove = true;
    public UnityEngine.UI.Slider hp;
    private GameObject obj;
    private Transform target; // 따라갈 물체의 방향
    public float speed = 1.0f;

    private void Start()
    {
        anim = GetComponent<Animator>();
        Level level = FindObjectOfType<Level>();
        level.currentzombie++;
        obj = this.gameObject;
        anim.SetBool("iswalk",true);
        anim.SetBool("israge",false);
        StartCoroutine(pattern());
    }

    void Update()
    {
        if (cananotdetect == false)
        {
            Move1 player = GameObject.Find("player").GetComponent<Move1>();
            target = player.transform;
            dir = target.position - transform.position; //사이의 거리를 구함
        }

        if (ismove)
        {
            anim.SetBool("iswalk",true);
            transform.position +=
                new Vector3(Mathf.Clamp(dir.x, speed * -1, speed), Mathf.Clamp(dir.y, speed * -1, speed), dir.z) *
                speed *
                Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("banana"))
        {
            hp.value--;
            if (hp.value == hp.maxValue / 2)
            {
                anim.SetBool("israge", true);
                speed *= 2f;
            }

            if (hp.value == 0)
            {
                GameOver gameover = GameObject.Find("Canvas").GetComponent<GameOver>();
                gameover.zombiecount += 1;
                Level level = FindObjectOfType<Level>();
                level.zombiecount[level.i]--; //생성되면 zombiecount--
                Destroy(obj);
            }
        }
    }

    IEnumerator dash()
    {
        cananotdetect = true; //캐릭터 감지 off
        ismove = false;
        //대시준비 소리
        yield return new WaitForSeconds(1f);
        //대시소리
        ismove = true;
        speed *= 5;
        yield return new WaitForSeconds(0.35f);
        speed /= 5;
        cananotdetect = false; //캐릭터 감지 on
    }
    IEnumerator pattern()
    {
        while (true)
        {
            int a;
            yield return new WaitForSeconds(1f);
            if (anim.GetBool("israge") == true)
            {
                a = Random.Range(0,5);
            }
            else
            {
                a = Random.Range(0, 10);
            }
            if (a == 0)
            {
                StartCoroutine(dash());
                yield return new WaitUntil(() => ismove == true);
            }
            else if (a == 1)
            {
                StartCoroutine(dummyspawn());
                yield return new WaitUntil(() => ismove == true);
            }
        }
    }

    IEnumerator dummyspawn()
    {
        ismove = false;
        yield return new WaitForSeconds(1f);
        Instantiate(dummy, new Vector3(transform.position.x+2,transform.position.y,transform.position.z),Quaternion.identity); //오른쪽
        Instantiate(dummy, new Vector3(transform.position.x-2,transform.position.y,transform.position.z),Quaternion.identity);//왼쪽
        Instantiate(dummy, new Vector3(transform.position.x,transform.position.y+2,transform.position.z),Quaternion.identity);//위쪽
        Instantiate(dummy, new Vector3(transform.position.x,transform.position.y-2,transform.position.z),Quaternion.identity);//아래쪽
        if (anim.GetBool("israge") == true)
        {
            Instantiate(dummy, new Vector3(transform.position.x+2,transform.position.y+2,transform.position.z),Quaternion.identity); //오른쪽위
            Instantiate(dummy, new Vector3(transform.position.x-2,transform.position.y-2,transform.position.z),Quaternion.identity);//왼쪽아래
            Instantiate(dummy, new Vector3(transform.position.x+2,transform.position.y-2,transform.position.z),Quaternion.identity);//오른쪽아래
            Instantiate(dummy, new Vector3(transform.position.x-2,transform.position.y-2,transform.position.z),Quaternion.identity);//왼쪽아래
        }
        yield return new WaitForSeconds(1.5f);
        ismove = true;
    }
}
