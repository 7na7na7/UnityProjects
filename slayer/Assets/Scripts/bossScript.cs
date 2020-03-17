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
    private float dmgDelay = 0;
    public float patternDelay;
    private bool canGo = false;
    private int previous = 10;
    private void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(Go());
    }

    IEnumerator Go()
    {
        while (true)
        {
            if(slider.value<=10)
                yield return new WaitForSeconds(patternDelay*0.5f);
            else
                yield return new WaitForSeconds(patternDelay);
            int r = 0;
            while (r==previous)
            {
                r = Random.Range(0, 4);
            }
            if(r!=1) 
                previous = r;
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
            transform.DOMove(p ,0.5f).SetEase(Ease.Linear);
        else
            transform.DOMove(p ,1).SetEase(Ease.Linear);
        yield return new WaitUntil(()=>Vector3.Distance(transform.position,c)<0.1f);
        canGo = true;
    }
    IEnumerator tsuzumi(int r)
    {
        SoundManager.instance.tsuzumi(r);
        if (r == 0)
        {
            anim.Play("tsuzumi_L");
            yield return new WaitForSeconds(0.3f);
        }
        else if (r == 1)
        {
            anim.Play("tsuzumi_M");
            yield return new WaitForSeconds(0.5f);
            if (slider.value <= 10)
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
        if (dmgDelay >= 0.1f)
        {
            if (isHead)
            {
                ScoreMgr.instance.headshot++;
                SoundManager.instance.head();

                slider.value -= 2;
                    if (slider.value <= 0)
                    {
                        CameraManager.instance.rot = 1;
                        ComboManager.instance.comboIniitailize();
                        ScoreMgr.instance.killedOni++;
                        ScoreMgr.instance.scoreUp(2000, false);
                        Instantiate(headEffect, transform.position, Quaternion.identity);
                        StartCoroutine(CameraManager.instance.targetChange(gameObject));
                        FindObjectOfType<GameManager>().bossDead=true;
                        CameraManager.instance.rot = 1;
                        Destroy(gameObject);
                    }
            }
            else
            {
                SoundManager.instance.body();
                slider.value--;
                    if (slider.value <= 0)
                    {
                        CameraManager.instance.rot = 1;
                        ComboManager.instance.comboIniitailize();
                        ScoreMgr.instance.killedOni++;
                        ScoreMgr.instance.scoreUp(2000, false);
                        Instantiate(effect, transform.position, Quaternion.identity);
                        StartCoroutine(CameraManager.instance.targetChange(gameObject));
                        FindObjectOfType<GameManager>().bossDead=true;
                        CameraManager.instance.rot = 1;
                        Destroy(gameObject);
                    }
            }
            Player.instance.ComboText(isHead);
            dmgDelay = 0;
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
