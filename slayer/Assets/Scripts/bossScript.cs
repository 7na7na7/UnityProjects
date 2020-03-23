using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class bossScript : MonoBehaviour
{
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
    public float moveTime;
    public float hpPlusValue;
    public float patternMinusValue;
    public float moveFastValue;
    public float dmgDelay = 0;
    private SpriteRenderer spr;
    private Color color;
    private void Start()
    {
        slider.maxValue += GameManager.instance.bossCount * hpPlusValue;
        patternDelay -= GameManager.instance.bossCount * patternMinusValue;
        moveTime -= GameManager.instance.bossCount * moveFastValue;
        if (moveTime == 0)
            moveTime = moveFastValue;
        if (patternDelay == 0)
            patternDelay = patternMinusValue;
        GameManager.instance.bossCount++;
        
        anim = GetComponent<Animator>();
        StartCoroutine(Go());
    }

    IEnumerator Go()
    {
        CameraManager.instance.rotSpeed = CameraManager.instance.savedrotSpeed;
        int t = (int)Time.timeScale;
        Time.timeScale = 0;
        CameraManager.instance.theCamera.orthographicSize = 5;
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
        canMove = true;
        while (true)
        {
            if(slider.value <= Mathf.RoundToInt(slider.maxValue * 0.3f))
                yield return new WaitForSeconds(patternDelay*0.5f);
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

    IEnumerator move()
    {
        Vector3 c = transform.position;
        Vector3 p = new Vector3(Player.instance.transform.position.x,Player.instance.transform.position.y+1.6f,0);
        if(slider.value<=10) 
            transform.DOMove(p ,moveTime*0.5f).SetEase(Ease.Linear);
        else
            transform.DOMove(p ,moveTime).SetEase(Ease.Linear);
        yield return new WaitUntil(()=>Vector3.Distance(transform.position,c)<0.1f);
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
                    if (slider.value <= Mathf.RoundToInt(slider.maxValue * 0.3f))
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
                        ScoreMgr.instance.scoreUp(0, 2000, false);
                        FindObjectOfType<GameManager>().bossDead = true;
                        ComboManager.instance.comboIniitailize();
                        ScoreMgr.instance.killedOni++;
                        Instantiate(headEffect, transform.position, Quaternion.identity);
                        Destroy(gameObject);
                    }
                }
                else
                {
                    SoundManager.instance.body();
                    slider.value--;
                    if (slider.value <= Mathf.RoundToInt(slider.maxValue * 0.3f))
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
                        ScoreMgr.instance.scoreUp(0, 2000, false);
                        Instantiate(effect, transform.position, Quaternion.identity);
                        FindObjectOfType<GameManager>().bossDead = true;
                        Destroy(gameObject);
                    }
                }

                dmgDelay = 0;
                Player.instance.ComboText(isHead);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!Player.instance.isattack)
            {
                //공격애니메이션재생
            }
        }
    }
}
