using System.Collections;
using System.Net.Mime;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class bossScript : MonoBehaviour
{
    public float minX, maxX;
    public bool isDaki=true;
    
    private bool isPoison = false;
    public float moveSpeed;
    public Slider slider;
    public GameObject effect;
    public GameObject headEffect;
    public GameObject poisonEffect;
    public GameObject crew;
    private Animator anim;
    public float patternDelay;
    public bool canGo = false;
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
    public GameObject train1;
    public GameObject train2;
    public HandTrap[] spikes;
    public HandTrap[] spikes2;
    public GameObject dakiAttack;
    public GameObject gyutaro;
    public GameObject gyutaroAttack_1;
    public GameObject gyutaroAttack_2;
    public float trainAttack1Delay = 0;
    public float trainAttack2Delay = 0;
    public float spikeDelay = 0;
    private Color color;
    private void Start()
    {
        FindObjectOfType<BgmManager>().bossFunc();
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
        if(SceneManager.GetActiveScene().name=="Main"||SceneManager.GetActiveScene().name=="Main_H"||SceneManager.GetActiveScene().name=="Main_EZ") 
            StartCoroutine(Go());
        else if (SceneManager.GetActiveScene().name == "Main2"||SceneManager.GetActiveScene().name == "Main2_H"||SceneManager.GetActiveScene().name == "Main2_EZ")
            StartCoroutine(Go2());
        else if (SceneManager.GetActiveScene().name == "Main3"||SceneManager.GetActiveScene().name == "Main3_H"||SceneManager.GetActiveScene().name == "Main3_EZ")
            StartCoroutine(Go3());
        else if (SceneManager.GetActiveScene().name == "Main4" || SceneManager.GetActiveScene().name == "Main4_H" ||
                 SceneManager.GetActiveScene().name == "Main4_EZ")
        {
            if (isDaki)
                StartCoroutine(Go4());
            else
            {
                GameManager.instance.bossCount--;
                StartCoroutine(Go5());
            }
        }
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
        CameraManager.instance.theCamera.orthographicSize = 5;
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
        CameraManager.instance.theCamera.orthographicSize = 5;
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

    IEnumerator Go4()
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
        CameraManager.instance.theCamera.orthographicSize = 5;
        canMove = true;
        dakiAttack.GetComponent<PolygonCollider2D>().enabled = false;
        while (true)
        {
            if(slider.value <= Mathf.RoundToInt(slider.maxValue * 0.3f))
                yield return new WaitForSeconds(patternDelay*0.75f);
            else
                yield return new WaitForSeconds(patternDelay);
            int r = Random.Range(0, 5);
            

            if(r==0||r==1) 
                StartCoroutine(dakiAttack1());
            else if (r == 2)
                StartCoroutine(dakiAttack2());
            else if(r==3||r==4)
                StartCoroutine(dakiMove());

            yield return new WaitUntil(()=>canGo);
            canGo = false;
            anim.Play("bossIdle");
        }
    }
    IEnumerator Go5()
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
        CameraManager.instance.theCamera.orthographicSize = 5;
        canMove = true;
        while (true)
        {
            if(slider.value <= Mathf.RoundToInt(slider.maxValue * 0.3f))
                yield return new WaitForSeconds(patternDelay*0.75f);
            else
                yield return new WaitForSeconds(patternDelay);
            int r = Random.Range(0, 5);
            if (r == 0 || r == 1)
                StartCoroutine(gyutaroAttack1());
            else
                StartCoroutine(gyutaroAttack2());

            yield return new WaitUntil(()=>canGo);
            canGo = false;
            anim.Play("bossIdle");
        }
    }

    IEnumerator gyutaroAttack1() //피의 참격
    {
        Instantiate(gyutaroAttack_1,transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        canGo = true;
    }

    IEnumerator gyutaroAttack2() //양방향 참격
    {
        //애니메이션 전환
        yield return new WaitForSeconds(1f);
        Instantiate(gyutaroAttack_2, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        canGo = true;
    }

    IEnumerator gyutaroMove()
    {
        yield return new WaitForSeconds(1f);
        canGo = true;
    }
    IEnumerator gyutaroSecondAttackCor()
    {
        while (true)
        {
            if(slider.value <= Mathf.RoundToInt(slider.maxValue * 0.3f))
                yield return new WaitForSeconds(patternDelay*0.75f);
            else
                yield return new WaitForSeconds(patternDelay);
            int r = Random.Range(0, 5);
            
            StartCoroutine(gyutaroAttack1());

            yield return new WaitUntil(()=>canGo);
            canGo = false;
            anim.Play("bossIdle");
        }
    }
    
    IEnumerator dakiSecondAttackCor()
    {
        while (true)
        {
            if(slider.value <= Mathf.RoundToInt(slider.maxValue * 0.3f))
                yield return new WaitForSeconds(patternDelay*0.75f);
            else
                yield return new WaitForSeconds(patternDelay);
            int r = Random.Range(0, 3);
            

            if(r==0||r==1) 
                StartCoroutine(dakiAttack1());
            else if (r == 2)
                StartCoroutine(dakiAttack2());
            else if(r==3||r==4)
                StartCoroutine(dakiMove());

            yield return new WaitUntil(()=>canGo);
            canGo = false;
            anim.Play("bossIdle");
        }
    }
    IEnumerator dakiAttack1()
    {
        dakiAttack.GetComponent<PolygonCollider2D>().enabled = true;
        dakiAttack.GetComponent<SpriteRenderer>().color=Color.red;
        Vector2 savedScale = dakiAttack.transform.localScale;
        Vector2 ScaleUp;
        if (BossDeadCtrl.instance.isDakiFirstDead)
            ScaleUp= dakiAttack.transform.localScale * 4f;
        else
            ScaleUp= dakiAttack.transform.localScale * 2.5f;
        while (dakiAttack.transform.localScale.sqrMagnitude<ScaleUp.sqrMagnitude)
        {
            dakiAttack.transform.localScale=new Vector2(dakiAttack.transform.localScale.x+0.2f,dakiAttack.transform.localScale.y+0.2f);
            yield return new WaitForSeconds(0.01f);
        }
yield return new WaitForSeconds(0.5f);
dakiAttack.GetComponent<PolygonCollider2D>().enabled = false;
dakiAttack.GetComponent<SpriteRenderer>().color = Color.white;
while (dakiAttack.transform.localScale.sqrMagnitude>savedScale.sqrMagnitude)
{
    dakiAttack.transform.localScale=new Vector2(dakiAttack.transform.localScale.x-0.2f,dakiAttack.transform.localScale.y-0.2f);
    yield return new WaitForSeconds(0.01f);
}
yield return new WaitForSeconds(1f);
        canGo = true;
    }

    IEnumerator dakiAttack2()
    {
        int a = 2;
        if (BossDeadCtrl.instance.isDakiFirstDead)
            a = 3;
        for (int i = 0; i < a; i++)
        {
            int r = 0;
            if (transform.position.x > -6) //오른쪽
                r = 1;
            Spawner[] spawners = FindObjectsOfType<Spawner>();
            foreach (Spawner s in spawners)
            {
                s.DakiMobSpawn(r, new Vector3(transform.position.x,transform.position.y-2));
            }
            yield return new WaitForSeconds(1.25f);   
        }
        canGo = true;
    }
    IEnumerator dakiMove()
    {
        spr = GetComponent<SpriteRenderer>();
        color.r = 255;
        color.g = 0f;
        color.b = 0f;
        color.a = 1;
        spr.color = color;

        float x;
        if (Player.instance.transform.position.x >= minX && Player.instance.transform.position.x < maxX) //해당 범위 안에 있으면
        {
            if (Player.instance.transform.position.x >= (maxX + minX) / 2) //오른쪽이면
                x = maxX;
            else
                x = minX;
        }
        else
            x = Player.instance.transform.position.x;
        
        Vector3 p = new Vector3(x,transform.position.y,0);
        
        anim.Play("walk");
        transform.GetChild(0).gameObject.tag = "damage";
        transform.GetChild(1).gameObject.tag = "damage";
        transform.GetChild(2).gameObject.tag = "damage";
        
       
        Vector2 dir = p - transform.position;
        dir.Normalize();

        float ms = moveSpeed;
        if (BossDeadCtrl.instance.isDakiFirstDead)
            ms *= 1.5f;
        while (Vector3.Distance(p,transform.position)>0.1f)
        {
            transform.Translate(dir * ms * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }
        
        transform.GetChild(0).gameObject.tag = "oni";
        transform.GetChild(1).gameObject.tag = "oniHead";
        transform.GetChild(2).gameObject.tag = "oni";
        
        if (isPoison)
        {
            color.r = 0.66f;
            color.g = 0.32f;
            color.b =1f;
            spr.color = color;
        }
        else
        {
         
            color.r = 255;
            color.g = 255f;
            color.b = 255f;
            color.a = 1;
            spr.color = color;
        }

        canGo = true;
    }
    IEnumerator Go3()
    {
        StartCoroutine(trainAttack1());
        StartCoroutine(trainAttack2());
        StartCoroutine(trainAttack3());
        transform.SetParent(GameObject.Find("enmuStage").transform);
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
        CameraManager.instance.theCamera.orthographicSize = 6;
        canMove = true;
        while (true)
        {
            if(slider.value <= Mathf.RoundToInt(slider.maxValue * 0.3f))
                yield return new WaitForSeconds(patternDelay*0.75f);
            else
                yield return new WaitForSeconds(patternDelay);
            int r = 0;

            //대충 startcorutine
            yield return new WaitUntil(()=>canGo);
            canGo = false;
        }   
    }
    
    IEnumerator trainAttack2()
    {
        bool a = true;
        while (true)
        {
            if (a)
            {
                yield return new WaitForSeconds(Random.Range(0,trainAttack2Delay*0.5f));
                a = false;
            }
            else
            {
                if (slider.value <= Mathf.RoundToInt(slider.maxValue * 0.3f))
                    yield return new WaitForSeconds(0.7f * Random.Range(trainAttack2Delay, trainAttack2Delay + 1));
                else
                    yield return new WaitForSeconds(Random.Range(trainAttack2Delay, trainAttack2Delay + 1));
            }
            Instantiate(train2, new Vector3(
                Random.Range(GameObject.Find("eyeMin").transform.position.x,
                    GameObject.Find("eyeMax").transform.position.x),
                Random.Range(GameObject.Find("eyeMin").transform.position.y,
                    GameObject.Find("eyeMax").transform.position.y), 0), Quaternion.identity);
        }
    }
    IEnumerator trainAttack1()
    {
        bool a = true;
        while (true)
        {
            if (a)
            {
                yield return new WaitForSeconds(Random.Range(0,trainAttack1Delay*0.5f));
                a = false;
            }
            else
            {
                if (slider.value <= Mathf.RoundToInt(slider.maxValue *0.3f))
                    yield return new WaitForSeconds(0.7f*Random.Range(trainAttack1Delay,trainAttack1Delay+1));
                else
                    yield return new WaitForSeconds(Random.Range(trainAttack1Delay,trainAttack1Delay+1));   
            }
            Instantiate(train1);
        }
    }
    IEnumerator trainAttack3()
    {
        bool a = true;
        while (true)
        {
            if (a)
            {
                yield return new WaitForSeconds(Random.Range(0, spikeDelay*0.5f));
                a = false;
            }
            else
            {
                if (slider.value <= Mathf.RoundToInt(slider.maxValue * 0.3f))
                    yield return new WaitForSeconds(0.7f * Random.Range(spikeDelay, spikeDelay + 1));
                else
                    yield return new WaitForSeconds(Random.Range(spikeDelay, spikeDelay + 1));
            }
            int r = Random.Range(0, 2);
            if (r == 0)
            {
                for (int i = 0; i < spikes2.Length; i++)
                    spikes2[i].GetComponent<HandTrap>().bossAtk();
            }
            else
            {
                for (int i = 0; i < spikes.Length; i++)
                    spikes[i].GetComponent<HandTrap>().bossAtk();
            }
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
        spr = GetComponent<SpriteRenderer>();
        color.r = 255;
        color.g = 0f;
        color.b = 0f;
        color.a = 1;
        spr.color = color;
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
        spr = GetComponent<SpriteRenderer>();
        if (isPoison)
        {
            color.r = 0.66f;
            color.g = 0.32f;
            color.b =1f;
            spr.color = color;
        }
        else
        {
            if (slider.value <= Mathf.RoundToInt(slider.maxValue * 0.3f))
            {
                color.r = 255;
                color.g = 0.5f;
                color.b = 0.5f;
                color.a = 1;
                spr.color = color;
            }
            else
            {
                color.r = 255;
                color.g = 255f;
                color.b = 255f;
                color.a = 1;
                spr.color = color;   
            }   
        }
        canGo = true;
    }
    IEnumerator move()
    {
        spr = GetComponent<SpriteRenderer>();
        color.r = 255;
        color.g = 0f;
        color.b = 0f;
        color.a = 1;
        spr.color = color;
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
        if (isPoison)
        {
            color.r = 0.66f;
            color.g = 0.32f;
            color.b =1f;
            spr.color = color;
        }
        else
        {
                if (slider.value <= Mathf.RoundToInt(slider.maxValue * 0.3f))
                    {
                        color.r = 255;
                        color.g = 0.5f;
                        color.b = 0.5f;
                        color.a = 1;
                        spr.color = color;
                    }
                    else
                    {
                        color.r = 255;
                        color.g = 255f;
                        color.b = 255f;
                        color.a = 1;
                        spr.color = color;   
                    }
        }
    
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

    IEnumerator dakiDead()
    {
        BossDeadCtrl.instance.isDakiDead = true;
        transform.GetChild(0).gameObject.GetComponent<Collider2D>().enabled = false;
        transform.GetChild(1).gameObject.GetComponent<Collider2D>().enabled = false;
        transform.GetChild(2).gameObject.GetComponent<Collider2D>().enabled = false;
        Color color = Color.white;
        color.a = 0.3f;
        dakiAttack.GetComponent<SpriteRenderer>().color = color;
        GetComponent<SpriteRenderer>().color = color;
        for (int i = 0; i < 7; i++)
        {
            yield return new WaitForSeconds(1f);
            color.a +=0.1f;
            dakiAttack.GetComponent<SpriteRenderer>().color = color;
            GetComponent<SpriteRenderer>().color = color;
        }
        while (slider.value<slider.maxValue/2.5f)
        {
            slider.value += 0.5f;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        BossDeadCtrl.instance.isDakiDead = false;
        transform.GetChild(0).gameObject.GetComponent<Collider2D>().enabled = true;
        transform.GetChild(1).gameObject.GetComponent<Collider2D>().enabled = true;
        transform.GetChild(2).gameObject.GetComponent<Collider2D>().enabled = true;
        
        StartCoroutine(dakiSecondAttackCor());
    }
    IEnumerator  gyutaroDead()
    {
        BossDeadCtrl.instance.isGyutaroDead = true;
        transform.GetChild(0).gameObject.GetComponent<Collider2D>().enabled = false;
        transform.GetChild(1).gameObject.GetComponent<Collider2D>().enabled = false;
        transform.GetChild(2).gameObject.GetComponent<Collider2D>().enabled = false;
        Color color = Color.white;
        color.a = 0.3f;
        GetComponent<SpriteRenderer>().color = color;
        for (int i = 0; i < 7; i++)
        {
            yield return new WaitForSeconds(1f);
            color.a +=0.1f;
            GetComponent<SpriteRenderer>().color = color;
        }
        while (slider.value<slider.maxValue/2.5f)
        {
            slider.value += 0.5f;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        BossDeadCtrl.instance.isGyutaroDead = false;
        transform.GetChild(0).gameObject.GetComponent<Collider2D>().enabled = true;
        transform.GetChild(1).gameObject.GetComponent<Collider2D>().enabled = true;
        transform.GetChild(2).gameObject.GetComponent<Collider2D>().enabled = true;
        gyutaroSecondAttackCor();
    }
    public void dead(bool isHead)
    {
        if (!isPoison)
        {
            if (SceneManager.GetActiveScene().name == "Main" ||
                SceneManager.GetActiveScene().name == "Main_H" ||
                SceneManager.GetActiveScene().name == "Main_EZ")
            {
                if (slider.value <= Mathf.RoundToInt(slider.maxValue *0.3f))
                {
                    spr = GetComponent<SpriteRenderer>();
                    color.r = 255;
                    color.g = 0.5f;
                    color.b = 0.5f;
                    color.a = 1;
                    spr.color = color;
                }
            }
            else if (SceneManager.GetActiveScene().name == "Main2" ||
                     SceneManager.GetActiveScene().name == "Main2_H" ||
                     SceneManager.GetActiveScene().name == "Main2_EZ")
            {
                if (slider.value <= Mathf.RoundToInt(slider.maxValue *0.3f))
                {
                    spr = GetComponent<SpriteRenderer>();
                    color.r = 255;
                    color.g = 0.5f;
                    color.b = 0.5f;
                    color.a = 1;
                    spr.color = color;
                }
            }
            else if (SceneManager.GetActiveScene().name == "Main3" ||
                     SceneManager.GetActiveScene().name == "Main3_H" ||
                     SceneManager.GetActiveScene().name == "Main3_EZ")
            {
                if (slider.value <= Mathf.RoundToInt(slider.maxValue *0.3f))
                {
                    spr = GetComponent<SpriteRenderer>();
                    color.r = 255;
                    color.g = 0.5f;
                    color.b = 0.5f;
                    color.a = 1;
                    spr.color = color;
                }
            }
            else if (SceneManager.GetActiveScene().name == "Main4" ||
                     SceneManager.GetActiveScene().name == "Main4_H" ||
                     SceneManager.GetActiveScene().name == "Main4_EZ")
            {
                if (slider.value <= Mathf.RoundToInt(slider.maxValue *0.3f))
                {
                    spr = GetComponent<SpriteRenderer>();
                    color.r = 255;
                    color.g = 0.5f;
                    color.b = 0.5f;
                    color.a = 1;
                    spr.color = color;
                }
            }
        } //독묻은상태가 아니면 색바꾸기
        
       
        
        if (slider.value <= 0)
                    {
                        if (SceneManager.GetActiveScene().name == "Main4" || //스테이지4면
                            SceneManager.GetActiveScene().name == "Main4_EZ" ||
                            SceneManager.GetActiveScene().name == "Main4_H")
                        {
                            if (isDaki) //다키면
                            {
                                if (!BossDeadCtrl.instance.isDakiFirstDead)
                                {
                                    StopAllCoroutines();
                                    BossDeadCtrl.instance.isDakiFirstDead = true;
                                    isPoison = false;
                                    StartCoroutine(Heal());
                                }
                                else
                                {
                                    if (BossDeadCtrl.instance.isGyutaroDead) //둘 다 동시에 죽이는 경우
                                    {
                                        GameOver(true);
                                        bossScript[] bosses = FindObjectsOfType<bossScript>();
                                        foreach (bossScript boss in bosses)
                                        {
                                            if(!boss.isDaki) //규타로면
                                            boss.GameOver(false);
                                        }
                                    }
                                    else
                                    {
                                        StopAllCoroutines();
                                        isPoison = false;
                                        StartCoroutine(dakiDead());
                                    } 
                                }
                            }
                            else //규타로면
                            {
                                if (BossDeadCtrl.instance.isDakiDead) //둘 다 동시에 죽이는 경우
                                {
                                    GameOver(true);
                                    bossScript[] bosses = FindObjectsOfType<bossScript>();
                                    foreach (bossScript boss in bosses)
                                    {
                                        if(boss.isDaki) //다키면
                                            boss.GameOver(false);
                                    }
                                }
                                else
                                {
                                    StopAllCoroutines();
                                    isPoison = false;
                                    StartCoroutine(gyutaroDead());
                                }
                            }
                        }
                        else
                        {
                          CameraManager.instance.rotSpeed = CameraManager.instance.fastrotSpeed;
                        CameraManager.instance.rot = 1;
                        if (!Player.instance.isGameOver)
                        {
                            CameraManager.instance.closeUpSlow();   
                        }
                        ScoreMgr.instance.scoreUp(0, 3000, false);
                        FindObjectOfType<GameManager>().bossDead = true;
                        ComboManager.instance.comboIniitailize();
                        ScoreMgr.instance.killedOni++;
                        if(isHead) 
                            Instantiate(headEffect, transform.position, Quaternion.identity);
                        else
                            Instantiate(effect, transform.position, Quaternion.identity);
                        mpSlider.instance.bossCut();
                        if (SceneManager.GetActiveScene().name == "Main2"||SceneManager.GetActiveScene().name == "Main2_EZ"||SceneManager.GetActiveScene().name == "Main2_H") //2스테이지면 거미줄없애고 죽음, 3스테 해금
                        {
                            GameObject[] webs = GameObject.FindGameObjectsWithTag("web");
                            foreach (GameObject web in webs)
                            {
                                Destroy(web);
                            }
                            GooglePlayManager.instance.CanStage2();
                        }
                        else if (SceneManager.GetActiveScene().name == "Main" ||
                                 SceneManager.GetActiveScene().name == "Main_EZ" ||
                                 SceneManager.GetActiveScene().name == "Main_H") //1스테이지면 2스테이지 해금
                        {
                            GooglePlayManager.instance.CanStage1();
                        }
                        else if (SceneManager.GetActiveScene().name == "Main3" || //3스테이지면 4스테이지 해금
                                 SceneManager.GetActiveScene().name == "Main3_EZ" ||
                                 SceneManager.GetActiveScene().name == "Main3_H")
                        {
                            Player.instance.isSuper = true;
                            Player.instance.canTouch = false;
                            GooglePlayManager.instance.CanStage3();
                        } 
                        FindObjectOfType<BgmManager>().bossDie();
                        Player.instance.forceUp();
                        Destroy(gameObject);   
                        }
                    }
    }

    public void GameOver(bool isMine)
    {
        if (isMine) //한번만 실행할것들
        {
            if (!Player.instance.isGameOver)
            {
                CameraManager.instance.closeUpSlow();   
            }
            ScoreMgr.instance.scoreUp(0, 3000, false);
            FindObjectOfType<GameManager>().bossDead = true;      
            ComboManager.instance.comboIniitailize();
            mpSlider.instance.bossCut();
            FindObjectOfType<BgmManager>().bossDie();
            Player.instance.forceUp();
        }
        ScoreMgr.instance.killedOni++;
        Instantiate(effect, transform.position, Quaternion.identity);
        Destroy(gameObject);   
    }
    IEnumerator Heal()
    {
        transform.GetChild(0).gameObject.GetComponent<Collider2D>().enabled = false;
        transform.GetChild(1).gameObject.GetComponent<Collider2D>().enabled = false;
        transform.GetChild(2).gameObject.GetComponent<Collider2D>().enabled = false;
        Color color = Color.white;
        color.a = 0.3f;
        GetComponent<SpriteRenderer>().color = color;
        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(0.5f);
            color.a += 0.175f;
            GetComponent<SpriteRenderer>().color = color;
        }
        
        while (slider.value<slider.maxValue)
        {
         
            slider.value += 0.5f;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        Vector3 bossPos =
            new Vector3(
                Random.Range(GameObject.Find("Min").transform.position.x + 7,
                    GameObject.Find("Max").transform.position.x - 7), transform.position.y, 0);
      
        if (bossPos.x >= -10.73f && bossPos.x < -2.22f) //해당 범위 안에 있으면
        {
            if (bossPos.x >= (-10.73f + -2.22f) / 2) //오른쪽이면
                bossPos.x = -10.73f;
            else
                bossPos.x = -2.22f;
        }
        Instantiate(gyutaro, bossPos, Quaternion.identity);
        transform.GetChild(0).gameObject.GetComponent<Collider2D>().enabled = true;
        transform.GetChild(1).gameObject.GetComponent<Collider2D>().enabled = true;
        transform.GetChild(2).gameObject.GetComponent<Collider2D>().enabled = true;
        GetComponent<SpriteRenderer>().color=Color.white;
        StartCoroutine(dakiSecondAttackCor());
    }
    public void die(bool isHead, bool isPois=false)
    {
        if (canMove)
        {
            float v = 0.1f;
            if (isPois)
                v = 0;
            if (dmgDelay >= v)
            {
                if (Player.instance.playerIndex == 4)
                    poison();
                if(!isPois) 
                    Player.instance.AttackCor();
                
                if (isHead)
                {
                    ScoreMgr.instance.headshot++;
                    if (Player.instance.playerIndex == 4)
                    {
                        SoundManager.instance.body();
                        
                        slider.value--;
                        Player.instance.ComboText(false);
                        Instantiate(effect, transform.position, Quaternion.identity);
                        dead(false);
                    }
                    else
                    {
                        Player.instance.ComboText(true);
                        if(Player.instance.playerIndex==5)
                            SoundManager.instance.Head_N();
                        else
                            SoundManager.instance.head();
                        
                        slider.value -= 2;
                        if (Player.instance.playerIndex == 6)
                        {
                            if (kanao.instance.isRage)
                                slider.value -= 2;
                        }
                        Instantiate(headEffect, transform.position, Quaternion.identity);
                        dead(true);
                    }
                    
                    //플레이어가 이노스케라면 보스가 데미지 더 입음
                    /*
                    if (Player.instance.playerIndex == 2)
                        slider.value--;
                        */
                }
                else
                {
                    if(isPois)
                        SoundManager.instance.poison();
                    else
                    {
                        if(Player.instance.playerIndex==5)
                            SoundManager.instance.Body_N();
                        else
                            SoundManager.instance.body();
                    }
                       
                  
                    slider.value--;
                    if (Player.instance.playerIndex == 6)
                    {
                        if (kanao.instance.isRage)
                            slider.value --;
                    }
                    dead(false);
                }
            } 
            if(!isPois) 
                dmgDelay = 0; 
            Player.instance.ComboText(isHead); 
        } 
    }
    public void poison()
    {
        if (!isPoison)
            StartCoroutine(poisonCor());
    }
    public IEnumerator poisonCor()
    {
        isPoison = true;
        SpriteRenderer spr= GetComponent<SpriteRenderer>();
        Color color = Color.white;
        color.r = 0.66f;
        color.g = 0.32f;
        color.b =1f;
        spr.color = color;
        while (true)
        {
            if(SceneManager.GetActiveScene().name=="Main3"||SceneManager.GetActiveScene().name=="Main3_EZ"||SceneManager.GetActiveScene().name=="Main3_H")
                yield return new WaitForSeconds(1.5f);
            else
                yield return new WaitForSeconds(2f);
            die(false,true);
            Instantiate(poisonEffect, transform.position, Quaternion.identity);
        }
    }
}
