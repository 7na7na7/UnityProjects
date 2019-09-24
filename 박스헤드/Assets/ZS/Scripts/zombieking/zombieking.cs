using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieking : MonoBehaviour
{
    private Level lv;
    private bool isleft=false, isright=false;
    private bool once2 = false;
    private bool once = false;
    public GameObject brown, silver, gold;
    
    public GameObject critical;
    public GameObject bigheart; 
    private bool isdead = false;
    private bool canevent = true;
    public AudioSource audiosource;
    public AudioClip dashsound;
    public AudioClip readysound;
    public AudioClip spawnsound;
    public AudioClip deathsound;

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
        lv = FindObjectOfType<Level>();
        for(int i=0;i<lv.wave/5;i++)
        {
            hp.maxValue *= 1.5f;
            hp.value *=1.5f;
        }
        anim = GetComponent<Animator>();
        lv.currentzombie++;
        obj = this.gameObject;
        anim.SetBool("iswalk",true);
        anim.SetBool("israge",false);
        StartCoroutine(pattern());
    }

    void Update()
    {
        if(isleft)
            transform.Translate(-0.1f,0,0);
        if(isright)
            transform.Translate(0.1f,0,0);
        if (cananotdetect == false)
        {
            Move1 player = GameObject.Find("player").GetComponent<Move1>();
            target = player.transform;
            dir = target.position - transform.position; //사이의 거리를 구함
        }

        if (ismove)
        {
            if (dir.x <= 0.25f && dir.y <= 0.25f)
            {
                if (once2 == false)
                {
                    once2 = true;
                    StartCoroutine(moving());
                }
            }
            Debug.Log("dirx : "+dir.x+"diry " +dir.y);
            
            
            anim.SetBool("iswalk",true);
            transform.position +=
                new Vector3(Mathf.Clamp(dir.x, speed * -1, speed), Mathf.Clamp(dir.y, speed * -1, speed), dir.z) *
                speed *
                Time.deltaTime;
        }
        if (hp.value <= hp.maxValue / 2)
        {
            if (once == false)
            {
                anim.SetBool("israge", true);
                speed *= 2f;
                once = true;
            }
        }

        if (hp.value <= 0)
        {
            if (isdead == false)
            {
                BoxCollider2D col = GetComponent<BoxCollider2D>();
                col.enabled = false;
                audiosource.PlayOneShot(deathsound,2f);
                GameOver gameover = GameObject.Find("Canvas").GetComponent<GameOver>();
                gameover.zombiecount += 1;
                Level level = FindObjectOfType<Level>();
                level.zombiecount[level.i]--; //생성되면 zombiecount--
                level.isBossAppear = false;
                ismove = false;
                canevent = false;
                isdead = true;
                anim.SetBool("iswalk",false);
                StartCoroutine(delaycoin());
                Destroy(obj, 2f);
            }
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

    IEnumerator delaycoin()
    {
        yield return new WaitForSeconds(1.9f);
        Instantiate(bigheart,
            new Vector3(this.transform.position.x, this.transform.position.y, bigheart.transform.position.z),
            Quaternion.identity);
           Instantiate(brown, new Vector3(this.transform.position.x+Random.Range(-2.5f,2.5f), this.transform.position.y+Random.Range(-2.5f,2.5f), brown.transform.position.z), Quaternion.identity); //동화생성
           Instantiate(brown, new Vector3(this.transform.position.x+Random.Range(-2.5f,2.5f), this.transform.position.y+Random.Range(-2.5f,2.5f), brown.transform.position.z), Quaternion.identity); //동화생성
                    Instantiate(brown, new Vector3(this.transform.position.x+Random.Range(-2.5f,2.5f), this.transform.position.y+Random.Range(-2.5f,2.5f), brown.transform.position.z), Quaternion.identity); //동화
                    Instantiate(brown, new Vector3(this.transform.position.x+Random.Range(-2.5f,2.5f), this.transform.position.y+Random.Range(-2.5f,2.5f), brown.transform.position.z), Quaternion.identity); //동화
                    Instantiate(brown, new Vector3(this.transform.position.x+Random.Range(-2.5f,2.5f), this.transform.position.y+Random.Range(-2.5f,2.5f), brown.transform.position.z), Quaternion.identity); //동화
                    Instantiate(brown, new Vector3(this.transform.position.x+Random.Range(-2.5f,2.5f), this.transform.position.y+Random.Range(-2.5f,2.5f), brown.transform.position.z), Quaternion.identity); //동화
                    Instantiate(silver, new Vector3(this.transform.position.x+Random.Range(-2.5f,2.5f), this.transform.position.y+Random.Range(-2.5f,2.5f), brown.transform.position.z), Quaternion.identity); //은화
                    Instantiate(silver, new Vector3(this.transform.position.x+Random.Range(-2.5f,2.5f), this.transform.position.y+Random.Range(-2.5f,2.5f), brown.transform.position.z), Quaternion.identity); //동화
                    Instantiate(silver, new Vector3(this.transform.position.x+Random.Range(-2.5f,2.5f), this.transform.position.y+Random.Range(-2.5f,2.5f), brown.transform.position.z), Quaternion.identity); //동화
                    Instantiate(silver, new Vector3(this.transform.position.x+Random.Range(-2.5f,2.5f), this.transform.position.y+Random.Range(-2.5f,2.5f), brown.transform.position.z), Quaternion.identity); //동화
                    Instantiate(gold, new Vector3(this.transform.position.x+Random.Range(-2.5f,2.5f), this.transform.position.y+Random.Range(-2.5f,2.5f), brown.transform.position.z), Quaternion.identity); //금화
                    Instantiate(gold, new Vector3(this.transform.position.x+Random.Range(-2.5f,2.5f), this.transform.position.y+Random.Range(-2.5f,2.5f), brown.transform.position.z), Quaternion.identity); //금화
    }
    IEnumerator dash()
    {
        if (canevent)
        {
            cananotdetect = true; //캐릭터 감지 off
            ismove = false;
            audiosource.PlayOneShot(readysound,3f);
            yield return new WaitForSeconds(1f);
            audiosource.PlayOneShot(dashsound,4f);
            if(isdead==false) 
                ismove = true;
            speed *= 5;
            if (anim.GetBool("israge") == true) 
                yield return new WaitForSeconds(0.15f);
            else
                yield return new WaitForSeconds(0.35f);
            speed /= 5;
            cananotdetect = false; //캐릭터 감지 on
        }
    }
    IEnumerator pattern()
    {
        while (true)
        {
            int a;
            yield return new WaitForSeconds(1f);
            if (anim.GetBool("israge") == true)
            {
                if(lv.wave>13) 
                    a = Random.Range(0,4);
                else
                    a = Random.Range(0,5);
            }
            else
            {
                if(lv.wave>13)
                    a = Random.Range(0, 8);
                else
                    a = Random.Range(0,10);
            }
            if (a == 0)
            {
                StartCoroutine(dash());
                yield return new WaitUntil(() =>  cananotdetect==false);
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
        if (canevent)
        {
            ismove = false;
            audiosource.PlayOneShot(spawnsound,5f);
            yield return new WaitForSeconds(1f);
            Instantiate(dummy, new Vector3(transform.position.x + 2, transform.position.y, transform.position.z),
                Quaternion.identity); //오른쪽
            Instantiate(dummy, new Vector3(transform.position.x - 2, transform.position.y, transform.position.z),
                Quaternion.identity); //왼쪽
            Instantiate(dummy, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z),
                Quaternion.identity); //위쪽
            Instantiate(dummy, new Vector3(transform.position.x, transform.position.y - 2, transform.position.z),
                Quaternion.identity); //아래쪽
            if (anim.GetBool("israge") == true)
            {
                Instantiate(dummy,
                    new Vector3(transform.position.x + 2, transform.position.y + 2, transform.position.z),
                    Quaternion.identity); //오른쪽위
                Instantiate(dummy,
                    new Vector3(transform.position.x - 2, transform.position.y - 2, transform.position.z),
                    Quaternion.identity); //왼쪽아래
                Instantiate(dummy,
                    new Vector3(transform.position.x + 2, transform.position.y - 2, transform.position.z),
                    Quaternion.identity); //오른쪽아래
                Instantiate(dummy,
                    new Vector3(transform.position.x - 2, transform.position.y - 2, transform.position.z),
                    Quaternion.identity); //왼쪽아래
            }

            yield return new WaitForSeconds(1.5f);
            if (isdead == false) 
                ismove = true;
        }
    }
    IEnumerator moving()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (dir.x <= 0.25f && dir.y <= 0.25f)
            {
                int a = Random.Range(0, 2);
                if (a == 0)
                {
                    isleft = true;
                    yield return new WaitForSeconds(1);
                    isleft = false;
                    isright = true;
                    yield return new WaitForSeconds(1);
                    isright = false;
                }
                else
                {
                    isright = true;
                    yield return new WaitForSeconds(1);
                    isright = false;
                    isleft = true;
                    yield return new WaitForSeconds(1);
                    isleft = false;
                }
            }
        }
    }
}
