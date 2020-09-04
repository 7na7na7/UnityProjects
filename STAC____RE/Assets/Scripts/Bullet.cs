using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Bullet : MonoBehaviour
{
    public TrailRenderer trail;
    public bool isColor1;
    public GameObject dieParticle;
    public int BulletIndex = 0;
    public Vector2 dir=Vector2.zero;
    private bool canDetect = true;
    public float minSpeed, maxSpeed;
    public float speed;
    private bool canDestroy = false;

    private float savedTrailWidth;
    private float savedTrailTime;

    private bool isOnce = false;
    
    private void Awake()
    {
        if (!isColor1)
        {
            gameObject.AddComponent<Rigidbody2D>();
            GetComponent<Rigidbody2D>().isKinematic = true;
        }
    }
    
    public void OnEnable()
    {
        if (!isOnce)
        {
            savedTrailWidth = trail.startWidth;
            isOnce = true;
        }
        else
        {
            GetComponent<SetColor>().setColor();
            trail.time = 0;
            canDestroy = false;
            canDetect = true;
            if (isColor1)
                gameObject.tag = "Color1";
            else
                gameObject.tag = "Color2";

            if (BulletIndex != 3)
            {
                if (BulletIndex != 5)
                {
                    speed = Random.Range(minSpeed,maxSpeed);
                    speed += speed * Spawner.instance.bulletSpeedPercent;
                }
                Set();  
            }

            StartCoroutine(switchCor());
        }
    }
IEnumerator switchCor()
{
    yield return new WaitForSeconds(0.05f);
    switch (BulletIndex)
    {
        case 0:
            straight();
            break;
        case 1:
            guide();
            break;
        case 2:
            straight();
            break;
        case 3:
            break;
        case 4:
            randomStraight();
            break;
    }   
}
    public void Star()
    {
        transform.parent = null;
        dir = Vector2.up;
    }
    IEnumerator SetTrailTime()
    {
        yield return new WaitForSeconds(0.1f);
        trail.time= 1+(1/Mathf.Pow(speed,2));
    }

    public void randomStraight()
    {
        dir = new Vector3(Random.Range(-1f,1f),Random.Range(-1f,1f),0);
        dir.Normalize();
    }
    public void delayGuide()
    {
        StartCoroutine(delayGuideCor());
    }

    IEnumerator delayGuideCor()
    {
        yield return new WaitForSeconds(0.1f);
        guide();
    }
    public void Set()
    {
        trail.startWidth =savedTrailWidth* (1/speed);
        StartCoroutine(SetTrailTime());
        
        if(BulletIndex==1)
            transform.localScale = new Vector3(1/speed/3f, 1/speed/3f, transform.localScale.z);
        else if(BulletIndex==2)
            transform.localScale = new Vector3(1/speed/4f, 1/speed/4f, transform.localScale.z);
        else if(BulletIndex==3)
            transform.localScale = new Vector3(1/speed/3f, 1/speed/3f, transform.localScale.z);
        else if(BulletIndex==5)
            transform.localScale = new Vector3(1/speed/1.5f, 1/speed/1.5f, transform.localScale.z);
        else
            transform.localScale = new Vector3(1/speed, 1/speed, transform.localScale.z);
    }

    private void Update()
    {
        if (!CheckCamera.instance.CheckObjectIsInCamera(gameObject))
        {
            if (!canDestroy)
            {
                canDestroy = true;
                StartCoroutine(Destroy());
            }
        } 
        if(BulletIndex==5) 
            transform.Translate(dir*speed*Time.deltaTime);
        else
            transform.position+=new Vector3(dir.x * speed * Time.deltaTime,dir.y * speed * Time.deltaTime);
    }

    public void straight()
    {
        dir = Player.instance.transform.position - transform.position;
        dir.Normalize();
    }

    public void guide()
    {
        StartCoroutine(guided());
    }

    IEnumerator guided()
    {
        while (true)
        {
            if (Player.instance != null)
            {
                dir = Player.instance.transform.position - transform.position;
                dir.Normalize();
                
                float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0f, angle+30);
            }
            yield return new WaitForSeconds(0.05f);
        }
    }
    public void cluster()
    {
        int i = 0;
        if (isColor1)
            i = 100;
        else
            i = 101;
        
        GameObject obj1=ObjectManager.instance.MakeObj(i);
        GameObject obj2=ObjectManager.instance.MakeObj(i);
        obj1.transform.position = transform.position;
        obj2.transform.position = transform.position;
        
        obj1.GetComponent<Bullet>().speed = speed * 2;
        obj1.GetComponent<Bullet>().Set();
        obj1.GetComponent<Bullet>().dir=new Vector2( dir.x+1, dir.y);
        obj1.GetComponent<Bullet>().dir.Normalize();
        obj1.GetComponent<Bullet>().delayGuide();
                
        obj2.GetComponent<Bullet>().speed = speed * 2;
        obj2.GetComponent<Bullet>().Set();
        obj2.GetComponent<Bullet>().dir=new Vector2( dir.x, dir.y+1);
        obj2.GetComponent<Bullet>().dir.Normalize();
        obj2.GetComponent<Bullet>().delayGuide();
        
        gameObject.SetActive(false);
    }

    public void die()
    {
        ShowParticle();
        if (BulletIndex != 5)
            gameObject.SetActive(false);
        else
            Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Color1") && !col.CompareTag("Color2")) //충돌체가 총알이 아니었을 경우
        {
            if (isColor1)
            {
                if (col.CompareTag("Edge1")) //같은 색에 닿았으면
                {
                    if(canDetect) 
                        SameColor();
                }
                else if (col.CompareTag("Edge2"))//다른 색이면
                {
                    if(canDetect) 
                        OtherColor();
                }
            }
            else
            {
                if (col.CompareTag("Edge2")) //같은 색에 닿았으면
                {
                    if(canDetect) 
                        SameColor();
                }
                else if (col.CompareTag("Edge1"))//다른 색이면
                {
                    if(canDetect) 
                        OtherColor(); 
                }
            }
        }
        else
        {
            if(col.GetComponent<Bullet>().isColor1!=isColor1) //두 탄의 색이 다르면
            {
                col.GetComponent<Bullet>().die();
                die();
            }
        }
        if (col.CompareTag("Cluster"))
        {
            if(BulletIndex==2)
                cluster();
        }   
        if(col.CompareTag("Yudotan"))
            StopAllCoroutines();
    }

    public void SameColor()
    {
        Vibrate.instance.Vibe(50);
        canDetect = false;
        
        ScoreMgr.instance.scoreUp(0,GameManager.instance.scoreUpValue,false);
        ComboManager.instance.comboIniitailize(); 
        ShowParticle();
        SoundMgr.instance.Play(0,1,1);
        if (BulletIndex != 5)
            gameObject.SetActive(false);
        else
            Destroy(gameObject);
    }

    void ShowParticle()
    {
        GameObject p=Instantiate(dieParticle, transform.position, Quaternion.identity);
        GetComponent<SpriteRenderer>().color = BulletData.instance.SetColor(GetComponent<SetColor>().ColorIndex);
        p.GetComponent<ParticleSystem>().startColor = GetComponent<SpriteRenderer>().color*3f;
    }
    public void OtherColor()
    {
        canDetect = false;
        Player.instance.Die();
    }

    IEnumerator Destroy() //10초동안 보이지 않으면 파괴
    {
        for(int i=0;i<20;i++)
        {
            yield return new WaitForSeconds(0.5f);
            if (CheckCamera.instance.CheckObjectIsInCamera(gameObject))
            {
                canDestroy = false;
                yield break;
            }
        }

        if (BulletIndex != 5)
            gameObject.SetActive(false);
        else
            Destroy(gameObject);
    }

    public void SetFalse()
    {
        if (gameObject.activeSelf)
        {
            if (BulletIndex != 5)
                gameObject.SetActive(false);
            else
                Destroy(gameObject);
        }
    }
}
