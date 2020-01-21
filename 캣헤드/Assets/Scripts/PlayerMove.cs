using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerMove : MonoBehaviour
{
    public GameObject[] bloods;
    private bool canMove = true;
    private bool canAttack = true;
    public float nuckBackPower;
    public float hpSpeed;
    public float GunCool;
    public float DashCool;
    public float dashTime;
    public float dashSpeed;
    public Slider hp;
    private AudioSource audio;
    public AudioClip attackSound;
    public GameObject bullet;
    public bool isup, isright, isleft, isdown;
    public float speed;
    public Animator anim;
    private bool isStop = true;
    public KeyCode right, left, up, down;
    public KeyCode attack;
    public KeyCode dash;
    public float invisibleTime;
    
    public Transform tr;
    public float offset = 0.4f;
    private float size, screenRation, wSize;
    public bool isSuper = false;
    private float gunCooltime = 0;
    private float dashCooltime = 0;
    private void Start()
    {
        size = Camera.main.orthographicSize;
        screenRation = (float)Screen.width / (float)Screen.height;
        wSize = Camera.main.orthographicSize * screenRation;
        StartCoroutine(stopCor());
        dashCooltime  = DashCool;
        StartCoroutine(invisible());
        audio = GameObject.Find("AudioSource").GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if(canMove) 
            Move();
    }

    void Move()
    {
         //이동
        if (Input.GetKey(up))
        {
            if (Input.GetKey(right))
            {
                isup = true;
                isdown = false;
                isleft = false;
                isright = true;
                Vector3 dir = Vector3.right + Vector3.up;
                dir.Normalize();
                transform.Translate(dir * speed * Time.deltaTime);
                anim.Play("rightup");
            }
            else if (Input.GetKey(left))
            {
                isup = true;
                isdown = false;
                isleft = true;
                isright = false;
                Vector3 dir = Vector3.left + Vector3.up;
                dir.Normalize();
                transform.Translate(dir * speed * Time.deltaTime);
                anim.Play("leftup");
            }
            else
            {
                isup = true;
                isdown = false;
                isleft = false;
                isright = false;
                transform.Translate(Vector3.up * speed * Time.deltaTime);

                anim.Play("up");
            }
        }
        else if (Input.GetKey(left))
        {
            if (Input.GetKey(up))
            {
                isup = true;
                isdown = false;
                isleft = true;
                isright = false;
                Vector3 dir = Vector3.left + Vector3.up;
                dir.Normalize();
                transform.Translate(dir * speed * Time.deltaTime);
                anim.Play("leftup");
            }
            else if (Input.GetKey(down))
            {
                isup = false;
                isdown = true;
                isleft = true;
                isright = false;
                Vector3 dir = Vector3.left + Vector3.down;
                dir.Normalize();
                transform.Translate(dir * speed * Time.deltaTime);
                anim.Play("leftdown");
            }
            else
            {
                isup = false;
                isdown = false;
                isleft = true;
                isright = false;
                transform.Translate(Vector3.left*speed*Time.deltaTime);
            
                anim.Play("left");   
            }
        }
        else if (Input.GetKey(down))
        {
            if (Input.GetKey(left))
            {
                isup = false;
                isdown = true;
                isleft = true;
                isright = false;
                Vector3 dir = Vector3.left + Vector3.down;
                dir.Normalize();
                transform.Translate(dir * speed * Time.deltaTime);
                anim.Play("leftdown");   
            }
            else if (Input.GetKey(right))
            {
                {
                    isup = false;
                    isdown = true;
                    isleft = false;
                    isright = true;
                    Vector3 dir = Vector3.right + Vector3.down;
                    dir.Normalize();
                    transform.Translate(dir * speed * Time.deltaTime);
                    anim.Play("rightdown");
                }
            }
            else
            {
                isup = false;
                isdown = true;
                isleft = false;
                isright = false;
                transform.Translate(Vector3.down*speed*Time.deltaTime);
            
                anim.Play("down");   
            }
        }
        else if (Input.GetKey(right))
        {
            if(Input.GetKey(up))
            {
                isup = true;
                isdown = false;
                isleft = false;
                isright = false;
                Vector3 dir = Vector3.right + Vector3.up;
                dir.Normalize();
                transform.Translate(dir * speed * Time.deltaTime);
                anim.Play("rightup");
            }
            else if(Input.GetKey(down))
            {
                isup = false;
                isdown = true;
                isleft = false;
                isright = true;
                Vector3 dir = Vector3.right + Vector3.down;
                dir.Normalize();
                transform.Translate(dir * speed * Time.deltaTime);
                anim.Play("rightdown");   
            }
            else
            {
                isup = false;
                isdown = false;
                isleft = false;
                isright = true;
                transform.Translate(Vector3.right*speed*Time.deltaTime);
            
                anim.Play("right");
            }
        }
        if (isStop)
        {
            if(isleft&&isup)
                anim.Play("leftupIdle");
            if(isleft&&isdown)
                anim.Play("leftdownIdle");
            if(isright&&isup)
                anim.Play("rightupIdle");
            if(isright&&isdown)
                anim.Play("rightdownIdle"); 
            if(isright&&!isdown&&!isup) 
                anim.Play("rightIdle");
            if(isleft&&!isdown&&!isup) 
                anim.Play("leftIdle");
            if(isup&&!isright&&!isleft) 
                anim.Play("upIdle");
            if(isdown&&!isright&&!isleft) 
                anim.Play("downIdle");
        }
        
        if (tr.position.y >= size - offset)
        {
            tr.position = new Vector3(tr.position.x, size - offset, 0);//위

        }
        if (tr.position.y <= -size + offset)
        {
            tr.position = new Vector3(tr.position.x, -(size - offset), 0);//아래

        }
        if (tr.position.x >= wSize - offset)
        {
            tr.position = new Vector3(wSize - offset, tr.position.y, 0);//오른쪽
        }
        if (tr.position.x <= -wSize + offset)
        {
            tr.position = new Vector3(-wSize + offset, tr.position.y, 0);//왼쪽
        }
    }

   
    private void Update()
    { 
        if(canAttack)
         attackFunc();

        if(canMove) 
            if (Input.GetKeyDown(dash))
        {
            if (dashCooltime  >= DashCool)
            {
                StartCoroutine(Dash());
                dashCooltime  = 0;
            }
        }

        if (hp.value <= 0)
        {
            for (int i = 0; i < 4; i++)
            {
                int quarter = Random.Range(0, 4);
                Instantiate(bloods[quarter],
                    transform.position + new Vector3(Random.Range(-.5f, .5f), Random.Range(-.5f, .5f), 0),
                    Quaternion.EulerAngles(new Vector3(0, 0, Random.Range(0, 360))));
            }

            if (gameObject.name.Substring(0, 7) == "Player1")
            {
                FindObjectOfType<GameManager>().p1Dead = true;
                FindObjectOfType<GameManager>().Dead1();
                Destroy(gameObject);
            }
            if (gameObject.name.Substring(0, 7) == "Player2")
            {
                FindObjectOfType<GameManager>().p2Dead = true;
                FindObjectOfType<GameManager>().Dead2();
                Destroy(gameObject);
            }
        }
        else
            hp.value += hpSpeed * Time.deltaTime;
        
        if(dashCooltime<DashCool)
            dashCooltime  += Time.deltaTime;
        if(gunCooltime<GunCool)
            gunCooltime += Time.deltaTime;
    }

    private void attackFunc()
    {
           if (Input.GetKey(attack))
                {
                    if (gunCooltime > GunCool)
                    {
                        if (isleft && isup)
                        {
                            GameObject b=Instantiate(bullet, transform.position + new Vector3(-0.1f, 0.1f, 0),
                                Quaternion.Euler(0, 0, 135)) as GameObject;
                            b.GetComponent<Bullet>().dir = 8;
                        }
                        if (isleft && isdown)
                        {
                            GameObject b=Instantiate(bullet, transform.position + new Vector3(-0.1f, -0.1f, 0),
                                Quaternion.Euler(0, 0, 225)) as GameObject;
                            b.GetComponent<Bullet>().dir = 6;
                        }
                        if (isright && isup)
                        {
                            GameObject b= Instantiate(bullet, transform.position + new Vector3(0.1f, 0.1f, 0),
                                Quaternion.Euler(0, 0, 45)) as GameObject;
                            b.GetComponent<Bullet>().dir = 2;
                        }

                        if (isright && isdown)
                        {
                            GameObject b=  Instantiate(bullet, transform.position + new Vector3(0.1f, -0.1f, 0),
                                Quaternion.Euler(0, 0, 315)) as GameObject;
                            b.GetComponent<Bullet>().dir = 4;
                        }
                        if (isright && !isdown && !isup)
                        {
                            GameObject b=  Instantiate(bullet, transform.position + new Vector3(0.1f, 0, 0),
                                Quaternion.Euler(0, 0, 0)) as GameObject;
                            b.GetComponent<Bullet>().dir = 3;
                        }
                        if (isleft && !isdown && !isup)
                        {
                            GameObject b=     Instantiate(bullet, transform.position + new Vector3(-0.1f, 0, 0),
                                Quaternion.Euler(0, 0, 180)) as GameObject;
                            b.GetComponent<Bullet>().dir = 7;
                        }
                        if (isup && !isright && !isleft)
                        {
                            GameObject b=   Instantiate(bullet, transform.position + new Vector3(0, 0.1f, 0),
                                Quaternion.Euler(0, 0, 90)) as GameObject;
                            b.GetComponent<Bullet>().dir = 1;
                        }
                        if (isdown && !isright && !isleft)
                        {
                            GameObject b= Instantiate(bullet, transform.position + new Vector3(0, -0.1f, 0),
                                Quaternion.Euler(0, 0, 270)) as GameObject;
                            b.GetComponent<Bullet>().dir = 5;
                        }
                        
                        audio.PlayOneShot(attackSound, 0.5f);
                        gunCooltime = 0;
                    }
                }
    }
    IEnumerator stopCor()
    {
        while (true)
        {
            Vector2 start = transform.position;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
            Vector2 last = transform.position;
            if (start == last)
            {
                isStop = true;
            }
            else
            {
                isStop = false;
            }
        }
    }
    IEnumerator nuckBack(bool isRandom = false, int dir = 3)
    {
        //피
        int half = Random.Range(0, 2);
        int quarter = Random.Range(0,4);
        if (half == 0)
        {
            Instantiate(bloods[quarter], transform.position, Quaternion.EulerAngles(new Vector3(0,0,Random.Range(0,360))));
        }

        canAttack = false;
        canMove = false;
        float x=0, y=0;
        if (isRandom)
        {
            int r = -1 * Random.Range(0, 2);
            int r2 = -1 * Random.Range(0, 2);
            x = nuckBackPower * r;
            y = nuckBackPower * r2;
        }
        else
        {
            if (dir == 0)
            {
                if (isup)
                    y = nuckBackPower * -1;
                if (isdown)
                    y = nuckBackPower;
                if (isright)
                    x = nuckBackPower * -1;
                if (isleft)
                    x = nuckBackPower;
            }
            else
            {
                switch (dir)
                {
                    //위부터 1, 시계방향
                    case 1: 
                        y = nuckBackPower;
                        break;
                    case 2:
                        y = nuckBackPower;
                        x = nuckBackPower;
                        break;
                    case 3:
                        x = nuckBackPower;
                        break;
                    case 4:
                        y = nuckBackPower * -1;
                        x = nuckBackPower;
                        break;
                    case 5:
                        y = nuckBackPower * -1;
                        break;
                    case 6:
                        y = nuckBackPower * -1;
                        x = nuckBackPower * -1;
                        break;
                    case 7:
                        x = nuckBackPower * -1;
                        break;
                    case 8:
                        y = nuckBackPower;
                        x = nuckBackPower * -1;
                        break;
                }   
            }
        }
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Vector2 V = new Vector2(x,y); 
        GetComponent<Rigidbody2D>().AddForce(V, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.1f);
        GetComponent<Rigidbody2D>().velocity=Vector2.zero;
        canMove = true;
        canAttack = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Slime"))
        {
            if (!isSuper)
            {
                StartCoroutine(invisibleOnce());
                StartCoroutine(nuckBack(false,0));
                hp.value -= 5;
            }
        }

        if (other.CompareTag("BossBullet"))
        {
            if (!isSuper)
            {
                hp.value -= 10f;
                StartCoroutine(nuckBack(false, 0));
            }
        }

        if (other.CompareTag("Item"))
        {
            Destroy(other.gameObject);
        }
        if (other.name.Substring(0, 7) == "Bullet1")
        {
            if (gameObject.name.Substring(0, 7) == "Player2")
            {
                if (!isSuper)
                {
                    StartCoroutine(nuckBack(false,other.GetComponent<Bullet>().dir));
                    StartCoroutine(invisibleOnce());
                    hp.value -= 10;
                }
            }
        }

        if (other.name.Substring(0, 7) == "Bullet2")
        {
            if (gameObject.name.Substring(0, 7) == "Player1")
            {
                if (!isSuper)
                {
                    print(bullet.GetComponent<Bullet>().dir);
                    StartCoroutine(nuckBack(false,other.GetComponent<Bullet>().dir));
                    StartCoroutine(invisibleOnce());
                    hp.value -= 10;
                }
            }
        }
        /*
        if (other.CompareTag("Slime"))
        {
            if (!isSuper)
            {
                StartCoroutine(invisibleOnce());
                hp.value -= 5;
                
                Vector2 dir = Vector2.zero;
                int r = Random.Range(0, 7);
                switch (r)
                {
                    case 0:
                        dir = Vector2.up;
                        break;
                    case 1:
                        dir = Vector2.up+Vector2.right;
                        dir.Normalize();
                        break;
                    case 2:
                        dir = Vector2.right;
                        break;
                    case 3:
                        dir = Vector2.right + Vector2.down;
                        dir.Normalize();
                        break;
                    case 4:
                        dir = Vector2.down;
                        break;
                    case 5:
                        dir = Vector2.down * Vector2.left;
                        dir.Normalize();
                        break;
                    case 6:
                        dir = Vector2.left;
                        break;
                    case 7:
                        dir = Vector2.left+Vector2.up;
                        dir.Normalize();
                        break;
                }
                GetComponent<Rigidbody2D>().AddForce(dir*10,ForceMode2D.Impulse);
                print("A");
            }
        }*/
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Slime"))
        {
            if (!isSuper)
            {
                hp.value -= 1.5f;
                StartCoroutine(nuckBack(true));
                StartCoroutine(invisibleOnce());
            }
        }
    }

    IEnumerator Dash()
    {
        isSuper = true;
        speed *= dashSpeed;
        yield return new WaitForSeconds(dashTime);
        speed /= dashSpeed;
        isSuper = false;
    }
   
    IEnumerator invisible()
    {
        isSuper = true;
        Color color;
        SpriteRenderer sprite= GetComponent<SpriteRenderer>();//스프라이트로 함
        color.r = 255;
        color.g = 255;
        color.b = 255;
        color.a = 0.5f;
        sprite.color = color;
        yield return new WaitForSeconds(invisibleTime);
        color.a = 1f;
        sprite.color = color;
        yield return new WaitForSeconds(invisibleTime);
        color.a = 0.5f;
        sprite.color = color;
        yield return new WaitForSeconds(invisibleTime);
        color.a = 1f;
        sprite.color = color;
        yield return new WaitForSeconds(invisibleTime);
        color.a = 0.5f;
        sprite.color = color;
        yield return new WaitForSeconds(invisibleTime);
        color.a = 1f;
        sprite.color = color;
        yield return new WaitForSeconds(invisibleTime);
        color.a = 1f;
        sprite.color = color;
        yield return new WaitForSeconds(invisibleTime);
        color.a = 0.5f;
        sprite.color = color;
        yield return new WaitForSeconds(invisibleTime);
        color.a = 1f;
        sprite.color = color;
        yield return new WaitForSeconds(invisibleTime);
        color.a = 0.5f;
        sprite.color = color;
        yield return new WaitForSeconds(invisibleTime);
        color.a = 1f;
        sprite.color = color;
        yield return new WaitForSeconds(invisibleTime);
        color.a = 0.5f;
        sprite.color = color;
        yield return new WaitForSeconds(invisibleTime);
        color.a = 1f;
        sprite.color = color;
        yield return new WaitForSeconds(invisibleTime);
        color.a = 0.5f;
        sprite.color = color;
        yield return new WaitForSeconds(invisibleTime);
        color.a = 1f;
        sprite.color = color;
        isSuper = false;
    }
    IEnumerator invisibleOnce()
    {
        isSuper = true;
        Color color;
        SpriteRenderer sprite= GetComponent<SpriteRenderer>();//스프라이트로 함
        color.r = 255;
        color.g = 255;
        color.b = 255;
        color.a = 0.5f;
        sprite.color = color;
        yield return new WaitForSeconds(invisibleTime);
        color.a = 1f;
        sprite.color = color;
        isSuper = false;
    }
}
