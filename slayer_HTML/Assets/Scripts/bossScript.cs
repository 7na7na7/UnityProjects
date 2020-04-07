using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class bossScript : MonoBehaviour
{
    public float moveSpeed;
    public Slider slider;
    public GameObject effect;
    public GameObject headEffect;
    public GameObject crew;
    private Animator anim;
    public float patternDelay;
    private bool canGo = false;
    private int previous = 10;
    private bool canMove = false;
    private int attackCount = 0;
    public float hpPlusValue;
    public float patternMinusValue;
    public float moveFastValue;
    public float dmgDelay = 0;
    private SpriteRenderer spr;
    public GameObject webAttack;
    public GameObject RedwebAttack;
    public GameObject webAttack2;
    public GameObject RedwebAttack2;
    private Color color;
    private void Start()
    {
        Player.instance.Stop();
        slider.maxValue += GameManager.instance.bossCount * hpPlusValue;
        for (int i = 0; i < GameManager.instance.bossCount;i++)
        {
            patternDelay *= patternMinusValue;
        }
        for (int i = 0; i < GameManager.instance.bossCount;i++)
        {
           moveSpeed *= moveFastValue;
        }
        if ( moveSpeed <= 0)
            moveSpeed = moveFastValue;
        if (patternDelay <= 0)
            patternDelay = patternMinusValue;
        GameManager.instance.bossCount++;
        
        anim = GetComponent<Animator>();
        if(SceneManager.GetActiveScene().name=="Main"||SceneManager.GetActiveScene().name=="Main_H") 
            StartCoroutine(Go());
        else if (SceneManager.GetActiveScene().name == "Main2"||SceneManager.GetActiveScene().name == "Main2_H")
            StartCoroutine(Go2());
    }

    IEnumerator Go()
    {
        CameraManager.instance.rotSpeed = CameraManager.instance.savedrotSpeed;
        int t = (int)Time.timeScale;
        Time.timeScale = 0;
        CameraManager.instance.transform.position=new Vector3(transform.position.x,transform.position.y,-10);
        CameraManager.instance.OnBound();
        SoundManager.instance.heal();
        while (slider.value<slider.maxValue)
        {
            slider.value += 0.5f;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        CameraManager.instance.canFollow = true;
        Player.instance.canTouch = true;
        Time.timeScale = t;
        CameraManager.instance.StopAllCoroutines();
        CameraManager.instance.theCamera.orthographicSize = 5.5f;
        canMove = true;
        while (true)
        {
            if(slider.value <= Mathf.RoundToInt(slider.maxValue * 0.3f))
                yield return new WaitForSeconds(patternDelay*0.75f);
            else
                yield return new WaitForSeconds(patternDelay);
            int r = 0;
            if (attackCount >= 3)
            {
                r = 1;
                attackCount = 0;
            }
            else
            {
                while (r==previous)
                {
                    r = Random.Range(0, 4);
                }   
            }

            if (r != 1)
            {
                previous = r;
                attackCount++;
            }

            if (r == 3)
                StartCoroutine(move());
            else
                StartCoroutine(tsuzumi(r));
            
            yield return new WaitUntil(()=>canGo);
            canGo = false;
            anim.Play("bossIdle");
        }   
    }
    IEnumerator Go2()
    {
        int t = (int)Time.timeScale;
        Time.timeScale = 0;
        CameraManager.instance.transform.position=new Vector3(transform.position.x,transform.position.y,-10);
        CameraManager.instance.OnBound();
        SoundManager.instance.heal();
        while (slider.value<slider.maxValue)
        {
            slider.value += 0.5f;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        CameraManager.instance.canFollow = true;
        Player.instance.isattack = false;
        Player.instance.canTouch = true;
        Time.timeScale = t;
        CameraManager.instance.StopAllCoroutines();
        CameraManager.instance.theCamera.orthographicSize = 5.5f;
        canMove = true;
        while (true)
        {
            if(slider.value <= Mathf.RoundToInt(slider.maxValue * 0.3f))
                yield return new WaitForSeconds(patternDelay*0.75f);
            else
                yield return new WaitForSeconds(patternDelay);
            int r = Random.Range(0, 3);
            if(r==0) 
                StartCoroutine(move2());
            else if(r==1)
                StartCoroutine(webAttackCor());
            else if(r==2)
                StartCoroutine(webAttackCor2());
            yield return new WaitUntil(()=>canGo);
            canGo = false;
            anim.Play("bossIdle");
        }   
    }

    IEnumerator webAttackCor()
    {
        SoundManager.instance.Skill1();
        anim.Play(("bossAttack"));
        if(slider.value <= Mathf.RoundToInt(slider.maxValue * 0.4f))
        {
            for (int i = 0; i < 7; i++)
            {
                Instantiate(RedwebAttack,
                    new Vector3(
                        Random.Range(GameObject.Find("Min").transform.position.x + 1,
                            GameObject.Find("Max").transform.position.x - 1),
                        Random.Range(GameObject.Find("Min").transform.position.y,
                            GameObject.Find("Max").transform.position.y - 2), 0), Quaternion.identity);
            }
            yield return new WaitForSeconds(4*0.75f);
            GameObject[] webs = GameObject.FindGameObjectsWithTag("web");
            foreach (GameObject web in webs)
            {
                Destroy(web);
            }
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                Instantiate(webAttack,
                    new Vector3(
                        Random.Range(GameObject.Find("Min").transform.position.x + 1,
                            GameObject.Find("Max").transform.position.x - 1),
                        Random.Range(GameObject.Find("Min").transform.position.y,
                            GameObject.Find("Max").transform.position.y - 2), 0), Quaternion.identity);
            }
            yield return new WaitForSeconds(4f);
        }
        
        canGo = true;
    }
    IEnumerator webAttackCor2()
    {
        SoundManager.instance.Skill2();
        anim.Play(("bossAttack"));
        if(slider.value <= Mathf.RoundToInt(slider.maxValue * 0.4f))
        {
            for (int i = 0; i < 10; i++)
            {
                Instantiate(RedwebAttack2, Vector3.zero, Quaternion.identity);
            }
            yield return new WaitForSeconds(4f);
            GameObject[] webs = GameObject.FindGameObjectsWithTag("web");
            foreach (GameObject web in webs)
            {
                Destroy(web);
            }
        }
        else
        {
            for (int i = 0; i < 8; i++)
            {
                Instantiate(webAttack2, Vector3.zero, Quaternion.identity);
            }
            yield return new WaitForSeconds(4f);
        }
        
        canGo = true;
    }
    IEnumerator move2()
    {
        Vector3 p = new Vector3(Player.instance.transform.position.x,Player.instance.transform.position.y+0.6f,0);
        if(Player.instance.transform.position.x>transform.position.x) 
            anim.Play("RightDash");
        else
            anim.Play("LeftDash");
        transform.GetChild(1).gameObject.tag = "damage";
        transform.GetChild(2).gameObject.tag = "damage";
        SoundManager.instance.Dash();
        if (slider.value <= Mathf.RoundToInt(slider.maxValue * 0.4f))
        {
            Vector2 dir = p - transform.position;
            dir.Normalize();
            while (Vector3.Distance(p,transform.position)>1)
            {
                transform.Translate(dir * moveSpeed*1.75f * Time.deltaTime);
                yield return new WaitForSeconds(0.01f);
            }
        }
        else
        {
            Vector2 dir = p - transform.position;
            dir.Normalize();
            while (Vector3.Distance(p,transform.position)>1)
            {
                transform.Translate(dir * moveSpeed * Time.deltaTime);
                yield return new WaitForSeconds(0.01f);
            }
        }
        transform.GetChild(1).gameObject.tag = "oni";
        transform.GetChild(2).gameObject.tag = "oniHead";
        canGo = true;
    }
    IEnumerator move()
    {
        transform.GetChild(1).gameObject.tag = "damage";
        transform.GetChild(2).gameObject.tag = "damage";
        SoundManager.instance.Dash();
        Vector3 p = new Vector3(Player.instance.transform.position.x,Player.instance.transform.position.y+0.6f,0);
        SoundManager.instance.Dash();
        if (slider.value <= Mathf.RoundToInt(slider.maxValue * 0.3f))
        {
            Vector2 dir = p - transform.position;
            dir.Normalize();
            while (Vector3.Distance(p,transform.position)>1)
            {
                transform.Translate(dir * moveSpeed*1.5f * Time.deltaTime);
                yield return new WaitForSeconds(0.01f);
            }
        }
        else
        {
            Vector2 dir = p - transform.position;
            dir.Normalize();
            while (Vector3.Distance(p,transform.position)>1)
            {
                transform.Translate(dir * moveSpeed * Time.deltaTime);
                yield return new WaitForSeconds(0.01f);
            }
        }
        transform.GetChild(1).gameObject.tag = "oni";
        transform.GetChild(2).gameObject.tag = "oniHead";
        canGo = true;
    }
    IEnumerator tsuzumi(int r)
    {
        SoundManager.instance.tsuzumi(r);
        if (r == 0)
        {
            anim.Play("tsuzumi_L");
            if (slider.value <= Mathf.RoundToInt(slider.maxValue * 0.3f))
                yield return new WaitForSeconds(0.15f);
            else
                yield return new WaitForSeconds(0.3f);
        }
        else if (r == 1)
        {
            anim.Play("tsuzumi_M");
            if (slider.value <= Mathf.RoundToInt(slider.maxValue * 0.3f))
                yield return new WaitForSeconds(0.3f);
            else
                yield return new WaitForSeconds(0.6f);
            if (slider.value <= Mathf.RoundToInt(slider.maxValue*0.3f))
            {
                Instantiate(crew, Player.instance.transform.position + new Vector3(5, 5, 0),
                    Quaternion.identity);
                GameObject b=Instantiate(crew, Player.instance.transform.position + new Vector3(-5, 5, 0),
                    Quaternion.identity);
                b.transform.localScale=new Vector3(b.transform.localScale.x*-1,b.transform.localScale.y,b.transform.localScale.z);
            }
            else
            {
                int m = Random.Range(0, 2);
                if (m == 0)
                {
                    Instantiate(crew, Player.instance.transform.position + new Vector3(5, 5, 0),
                        Quaternion.identity);
                }
                else
                {
                    GameObject b=Instantiate(crew, Player.instance.transform.position + new Vector3(-5, 5, 0),
                        Quaternion.identity);
                    b.transform.localScale=new Vector3(b.transform.localScale.x*-1,b.transform.localScale.y,b.transform.localScale.z);
                }
            }
        }
        else if(r==2)
        {
            anim.Play("tsuzumi_R");
            if (slider.value <= Mathf.RoundToInt(slider.maxValue * 0.3f))
                yield return new WaitForSeconds(0.15f);
            else
                yield return new WaitForSeconds(0.3f);
        }
        CameraManager.instance.rot = r;
        canGo = true;
    }
    void Update()
    {
        if (dmgDelay < 0.1f)
            dmgDelay += Time.deltaTime;
        if(FindObjectOfType<GameManager>().isGameOver)
            transform.GetChild(0).gameObject.SetActive(false);
    }

    public void die(bool isHead)
    {
        if (canMove)
        {
            if (dmgDelay >= 0.1f)
            {
                Player.instance.AttackCor();
                if (isHead)
                {
                    ScoreMgr.instance.headshot++;
                    SoundManager.instance.head();
                    slider.value -= 2;
                    
                    if (slider.value <= Mathf.RoundToInt(slider.maxValue * (SceneManager.GetActiveScene().name=="Main" ? 0.3f : 0.4f)))
                    {
                        spr = GetComponent<SpriteRenderer>();
                        color.r = 255;
                        color.g = 0.5f;
                        color.b = 0.5f;
                        color.a = 1;
                        spr.color = color;
                    }
                    if (slider.value <= 0)
                    {
                        CameraManager.instance.rotSpeed = CameraManager.instance.fastrotSpeed;
                        CameraManager.instance.rot = 1;
                        CameraManager.instance.closeUpSlow();
                        ScoreMgr.instance.scoreUp(0, 3000, false);
                        FindObjectOfType<GameManager>().bossDead = true;
                        ComboManager.instance.comboIniitailize();
                        ScoreMgr.instance.killedOni++;
                        Instantiate(headEffect, transform.position, Quaternion.identity);
                        mpSlider.instance.bossCut();
                        if (SceneManager.GetActiveScene().name == "Main2")
                        {
                            GameObject[] webs = GameObject.FindGameObjectsWithTag("web");
                            foreach (GameObject web in webs)
                            {
                                Destroy(web);
                            }
                        }
                        Destroy(gameObject);
                    }
                }
                else
                {
                    SoundManager.instance.body();
                    slider.value--;
                    if (slider.value <= Mathf.RoundToInt(slider.maxValue * (SceneManager.GetActiveScene().name=="Main" ? 0.3f : 0.4f)))
                    {
                        spr = GetComponent<SpriteRenderer>();
                        color.r = 255;
                        color.g = 0.5f;
                        color.b = 0.5f;
                        color.a = 1;
                        spr.color = color;
                    }
                    if (slider.value <= 0)
                    {
                        CameraManager.instance.rotSpeed = CameraManager.instance.fastrotSpeed;
                        CameraManager.instance.rot = 1;
                        CameraManager.instance.closeUpSlow();
                        ComboManager.instance.comboIniitailize();
                        ScoreMgr.instance.killedOni++;
                        ScoreMgr.instance.scoreUp(0, 3000, false);
                        Instantiate(effect, transform.position, Quaternion.identity);
                        FindObjectOfType<GameManager>().bossDead = true;
                        mpSlider.instance.bossCut();
                        if (SceneManager.GetActiveScene().name == "Main2")
                        {
                            GameObject[] webs = GameObject.FindGameObjectsWithTag("web");
                            foreach (GameObject web in webs)
                            {
                                Destroy(web);
                            }
                        }
                        Destroy(gameObject);
                    }
                }

                dmgDelay = 0;
                Player.instance.ComboText(isHead);
            }
        }
    }

   
}
