using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("설정해줘야하는값")]
    public bool isDesktop;
    public int playerIndex=0;
    //public float comboTimeScale;
    public float nuckBackCantTouchTime; //넉백시에 터치못하게되는 시간
    public float force; //움직이는 속도
    public int nuckbackforce; //밀려나는 힘
    public GameObject slashEffect;
    public GameObject dieEffect;
    [Header("신경쓸필요없음")] 
    public bool isBarrier = false;
    public bool isNuckBack = false;
    public bool isSuper = false;
    private bool isMeetEnmu = false;
    private bool isFirst = false;
    private float playerMoveValue=1.1f;
    private fade panel; //콤보중 화면 어둡게 만들어줌
    public bool isGameOver = false;
    public GameObject comboPop;
    public GameObject headPop;
    private GameObject worldCanvas;
    private Animator anim;
    public static Player instance;
    private float unRotY, RotY;
    private float unRotX, RotX;
    private Vector2 currentV;
    public bool isattack = false; //공격중인지 판단
    private bool canMove = true;
    private CameraManager camera; //카메라스크립트
    private Transform min, max; //터치할수있는 최소, 최대 역역
    private Rigidbody2D rigid; //리지드바디얻어옴
    public GameObject particle; //전기이펙트
    public GameObject trail; //뒤따라오는트레일이펙트
    public GameObject trail2;
    public GameObject jumpEffect; //점프시 이펙트
    public bool canTouch = true;
    private float canTouchTime = 0;

    private void Start()
    {

        panel = FindObjectOfType<fade>();
        worldCanvas=GameObject.Find("World").gameObject;
        min = GameObject.Find("Min").transform;
        max=GameObject.Find("Max").transform;

        for (int i = 0; i < GameManager.instance.bossCount; i++)
        {
            mpSlider.instance.bossCut();
            force *= playerMoveValue;
        }

        if (instance == null)
            instance = this;

        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        camera = Camera.main.GetComponent<CameraManager>();

        unRotY = transform.localScale.y;
        RotY = transform.localScale.y * -1;
        unRotX = transform.localScale.x;
        RotX = transform.localScale.x * -1;
    }

    public void forceUp()
    {
        force *= playerMoveValue;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
            OnBarrier();
        else if(Input.GetKeyDown(KeyCode.S))
            OffBarrier();
        if (SceneManager.GetActiveScene().name == "Main3" || SceneManager.GetActiveScene().name == "Main3_EZ" ||
            SceneManager.GetActiveScene().name == "Main3_H")
        {
            if (transform.position.x > 250)
            {
                if (!isMeetEnmu)
                {
                    CameraManager.instance.meetEnmuFunc();
                    isMeetEnmu = true;
                }
            }
        }

        if ( mpSlider.instance.mp.value >= 1 && !isGameOver && Time.timeScale != 0&&canTouch&&canTouchTime>=nuckBackCantTouchTime) //기력이 1이상이고, 게임오버가 아니고, 멈추지 않았다면
        {
            if (isDesktop)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (Input.mousePosition.x >= 118 && Input.mousePosition.y >= 758&&Input.mousePosition.x<=211&&Input.mousePosition.y<=841)
                    { }
                    else
                    {
                        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                  
                        StopAllCoroutines();
                        if (pos.x < min.position.x)
                        {
                            pos.x = min.position.x;
                        }

                        if (pos.x > max.position.x)
                        {
                            pos.x = max.position.x;
                        }

                        if (pos.y < min.position.y)
                        {
                            pos.y = min.position.y;
                        }

                        if (pos.y > max.position.y)
                        {
                            pos.y = max.position.y;
                        }

                        StartCoroutine(go2(pos));    
                    }
                }
            }
            else
            { 
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began)
                    {
                        if (Input.mousePosition.x >= 170 && Input.mousePosition.y >= 938&&Input.mousePosition.x<=272&&Input.mousePosition.y<=1046)
                        { }
                        else
                        {
                            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                            //if (Vector2.Distance(transform.position, pos) > 1)같은곳 터치안되게하는코드
                            StopAllCoroutines();
                            if (pos.x < min.position.x)
                            {
                                pos.x = min.position.x;
                            }

                            if (pos.x > max.position.x)
                            {
                                pos.x = max.position.x;
                            }

                            if (pos.y < min.position.y)
                            {
                                pos.y = min.position.y;
                            }

                            if (pos.y > max.position.y)
                            {
                                pos.y = max.position.y;
                            }

                            StartCoroutine(go2(pos));
                        }
                    }
                }
            }
        }

        if (canTouchTime < nuckBackCantTouchTime)
            canTouchTime += Time.deltaTime;
    }

    /*예전 go함수
    public IEnumerator go(Vector2 to)
         {
             mpSlider.instance.mpDown(1);
             GetComponent<Player>().trail.GetComponent<TrailRenderer>().emitting = true;
             GetComponent<Player>().trail2.GetComponent<TrailRenderer>().emitting = true;
             touchTime = 0;
             if (to.x > transform.position.x)
             {
                 flipY(false);
                 transform.eulerAngles = new Vector3(0, 0, -getAngle(transform.position.x, transform.position.y, to.x, to.y)+60); //y2값에 180더하거나 말거나 ㅋ
             }
             else
             {
                 flipY(true);
                 transform.eulerAngles = new Vector3(0, 0, -getAngle(transform.position.x, transform.position.y, to.x, to.y)+120); //y2값에 180더하거나 말거나 ㅋ
             }
     
             anim.Play("attack_ready");
             isattack = true;
             trail.SetActive(true);
             rigid.velocity = Vector2.zero;
             rigid.bodyType = RigidbodyType2D.Kinematic;
             canMove = true;
             if (combomgr.canCombo) //콤보중
             {
                 camera.sizeup();
                 StartCoroutine(panel.fadeIn());
                 trail.GetComponent<TrailRenderer>().startColor=Color.yellow;
                 particle.SetActive(true);
                 //int currentcount = comboCount;
                 if(Time.timeScale==1) 
                     Time.timeScale = 0.7f;
                 while (Vector2.Distance(transform.position, to) >= 0.5f)
                 {
                     if (!canMove)
                         break;
                     Vector2 dir = to - (Vector2) transform.position;
                     dir.Normalize();
                     if (Time.timeScale == 0.7f)
                     { 
                         //transform.Translate(dir * force*0.01f);
                         transform.position+=(Vector3)dir * force*0.01f;
                         yield return new WaitForSecondsRealtime(0.005f);
                     }
                     else
                     {
                         //transform.Translate(dir * force*Time.deltaTime);
                         transform.position+=(Vector3)dir * force * Time.deltaTime;
                         yield return new WaitForSeconds(0.005f);
                     }
                 }
                 yield return new WaitForSeconds(0.2f);
             }
             else //첫번째
             {
                 particle.SetActive(false);
                 trail.GetComponent<TrailRenderer>().startColor=Color.white;
                 camera.sizedown();
                 StartCoroutine(panel.fadeout());
                 while (Vector2.Distance(transform.position, to) >= 0.5f) 
                 {
                     if (!canMove)
                         break;
                     Vector2 dir = to - (Vector2) transform.position;
                     dir.Normalize();
                     //transform.Translate(dir * force*0.5f * Time.deltaTime);
                     transform.position += (Vector3) dir * force * 0.5f * Time.deltaTime;
                     yield return new WaitForSeconds(0.005f);
                 }
                 yield return new WaitForSeconds(0.2f);
             }
             isattack = false;
             rigid.velocity = Vector2.zero;
             rigid.bodyType = RigidbodyType2D.Dynamic;
             yield return new WaitForSeconds(combomgr.comboDelay);
             particle.SetActive(false);
             trail.GetComponent<TrailRenderer>().startColor=Color.white;
             camera.sizedown();
             StartCoroutine(panel.fadeout());
         }
         */
    public void rotate(Vector2 to)
    {
        if (to.x > transform.position.x)
        {
            flipY(false);
            transform.eulerAngles =
                new Vector3(0, 0, -getAngle(transform.position.x, transform.position.y, to.x, to.y) + 60);
        }
        else
        {
            flipY(true);
            transform.eulerAngles =
                new Vector3(0, 0, -getAngle(transform.position.x, transform.position.y, to.x, to.y) + 120);
        }
    }
    public IEnumerator go2(Vector2 to)
    {  
        flipX(false);
        isNuckBack = false;
        SoundManager.instance.swing();
        mpSlider.instance.mpDown(1);
        GetComponent<Player>().trail.GetComponent<TrailRenderer>().emitting = true;
        GetComponent<Player>().trail2.GetComponent<TrailRenderer>().emitting = true;
        if (to.x > transform.position.x) //오른쪽
        {
            flipY(false);
            if (playerIndex == 2)
            {
                transform.eulerAngles =
                    new Vector3(0, 0, -getAngle(transform.position.x, transform.position.y, to.x, to.y) + 90);
            }
            else
            {
                transform.eulerAngles =
                    new Vector3(0, 0, -getAngle(transform.position.x, transform.position.y, to.x, to.y) + 60);   
            }
        }
        else //왼쪽
        {
            flipY(true);
            if (playerIndex == 2)
            {
                transform.eulerAngles =
                    new Vector3(0, 0, -getAngle(transform.position.x, transform.position.y, to.x, to.y) + 90);
            }
            else
            {
                transform.eulerAngles =
                    new Vector3(0, 0, -getAngle(transform.position.x, transform.position.y, to.x, to.y) + 120);
            }
        }

        Instantiate(jumpEffect, transform.position,
            Quaternion.Euler(0, 0, -getAngle(transform.position.x, transform.position.y, to.x, to.y) + 90));
        isattack = true;
        trail.SetActive(true);
        rigid.velocity = Vector2.zero;
        rigid.bodyType = RigidbodyType2D.Kinematic;
        canMove = true;
        if (ComboManager.instance.canCombo) //콤보중
        {
            //if (to.x < transform.position.x) 
                anim.Play("attack_ready");

            if (playerIndex == 0)
            {
                camera.sizeup();
                if (!isFirst)
                {
                    isFirst = true;
                    SoundManager.instance.zenichuCombo();
                }

                trail.GetComponent<TrailRenderer>().startColor = Color.yellow;
                particle.SetActive(true);
                StartCoroutine(panel.fadeIn());
            }
            else if (playerIndex == 1)
            {
                camera.sizeup();
                if (kagura.instance.isKagura)
                {
                    trail.GetComponent<TrailRenderer>().startColor = Color.red;
                    particle.SetActive(true);
                }
                else
                {
                    Color color = Color.white;
                    color.r = 0;
                    color.g = 0.72f;
                    color.b =1f;
              
                    trail.GetComponent<TrailRenderer>().startColor = color;
                    particle.SetActive(false);
                }
            }
            else if (playerIndex == 2)
            {
                camera.sizeup();
                if (!isFirst)
                {
                    isFirst = true;
                    SoundManager.instance.inoskaeCombo();
                }

                Color color = Color.white;
                color.r = 0;
                color.g = 0.3f;
                color.b =0.5f;
              
                trail.GetComponent<TrailRenderer>().startColor = color;
                particle.SetActive(true);
            }
            else if (playerIndex == 3)
            {
                camera.sizeup();
                Color color = Color.white;
                color.r = 0;
                color.g = 0.72f;
                color.b =1f;
                trail.GetComponent<TrailRenderer>().startColor = color;
                particle.SetActive(false);
            }
            else if (playerIndex == 4)
            {
                camera.sizeup();
                if (!isFirst)
                {
                    isFirst = true;
                    SoundManager.instance.shinobuCombo();
                }

                Color color = Color.white;
                color.r = 0.66f;
                color.g = 0f;
                color.b =1f;
              
                trail.GetComponent<TrailRenderer>().startColor = color;
                particle.SetActive(true);
            }
            else if (playerIndex == 5)
            {
                camera.sizeup();
                if (!isFirst)
                {
                    isFirst = true;
                    //SoundManager.instance.shinobuCombo();
                }

                Color color = Color.white;
                color.r = 1f;
                color.g = 0.4f;
                color.b =0.4f;
              
                trail.GetComponent<TrailRenderer>().startColor = color;
            }
            //if (Time.timeScale == 1)
              //  Time.timeScale = comboTimeScale;

            Vector2 dir = to - (Vector2) transform.position;
            dir.Normalize();
            if (playerIndex == 0)
                rigid.velocity = dir * force * 2;
            else
                rigid.velocity = dir * force *1.1f;
        }
        else //첫번째
        {
            isFirst = false;
            anim.Play("attack_ready");
            if (playerIndex == 0)
            {
                trail.GetComponent<TrailRenderer>().startColor = Color.white;
                particle.SetActive(false);
                StartCoroutine(panel.fadeout());
            }
            else if (playerIndex == 1)
            {
                if (kagura.instance.isKagura)
                {
                    trail.GetComponent<TrailRenderer>().startColor = Color.red;
                    particle.SetActive(true);
                }
                else
                {
                    Color color = Color.white;
                    color.r = 0;
                    color.g = 0.72f;
                    color.b =1f;
              
                    trail.GetComponent<TrailRenderer>().startColor = color;
                    particle.SetActive(false);
                }
            }
            else if (playerIndex == 2)
            {
                trail.GetComponent<TrailRenderer>().startColor = Color.black;
                particle.SetActive(false);
            }
            else if (playerIndex == 3)
            {
            
                    Color color = Color.white;
                    color.r = 0;
                    color.g = 0.72f;
                    color.b =1f;
              
                    trail.GetComponent<TrailRenderer>().startColor = color;
                    particle.SetActive(false);
            }
            else if (playerIndex == 4)
            {
                Color color = Color.white;
                color.r = 0.66f;
                color.g = 0.5f;
                color.b =1f;
              
                trail.GetComponent<TrailRenderer>().startColor = color;
                particle.SetActive(false);
            }
            else if (playerIndex == 5)
            {
                Color color = Color.white;
                color.r = 1f;
                color.g = 0.73f;
                color.b =1f;
              
                trail.GetComponent<TrailRenderer>().startColor = color;
                particle.SetActive(false);
            }
            camera.sizedown();

            Vector2 dir = to - (Vector2) transform.position;
            dir.Normalize();
            rigid.velocity = dir * force;
        }

        if (ComboManager.instance.canCombo)
        {
            if(playerIndex==0)
                yield return new WaitUntil(() => Vector2.Distance(transform.position, to) <= 1f);
            else 
                yield return new WaitUntil(() => Vector2.Distance(transform.position, to) <= 0.4f);   
        }
        else
        {
            if(playerIndex==0)
                yield return new WaitUntil(() => Vector2.Distance(transform.position, to) <= 0.5f);
            else
                yield return new WaitUntil(() => Vector2.Distance(transform.position, to) <= 0.3f);   
        }
        
        rigid.velocity *= 0.3f;
        rigid.bodyType = RigidbodyType2D.Dynamic;
        yield return new WaitForSeconds(0.2f);
        isattack = false;
        yield return new WaitForSeconds(ComboManager.instance.comboDelay);
        particle.SetActive(false);
        trail.GetComponent<TrailRenderer>().startColor = Color.white;
        camera.sizedown();
        
        if(playerIndex==0) 
            StartCoroutine(panel.fadeout());
    }

    public void Stop()
    {
        rigid.bodyType = RigidbodyType2D.Dynamic;
        isattack = false;
        particle.SetActive(false);
        camera.sizedown();
        if(playerIndex==0) 
            StartCoroutine(panel.fadeout());
    }
    public void flipY(bool flip)
    {
        if (flip)
            transform.localScale = new Vector3(transform.localScale.x, RotY, 1);
        else
            transform.localScale = new Vector3(transform.localScale.x, unRotY, 1);
    }
    public void flipX(bool flip)
    {
        if (flip)
            transform.localScale = new Vector3(RotX, transform.localScale.y, 1);
        else
            transform.localScale = new Vector3(unRotX, transform.localScale.y, 1);
    }
    private void OnTriggerEnter2D(Collider2D hit)
    {
        if (!isGameOver&&!isSuper)
        {
            if (hit.CompareTag("damage"))
            {
                if (playerIndex == 1)
                {
                    if (kagura.instance.isKagura)
                    {
                        
                    }
                    else
                    {
                        HeartManager.instance.Damaged1();
                        HeartManager.instance.damaged2();
                    }
                }
                else
                {
                    HeartManager.instance.Damaged1();
                    HeartManager.instance.damaged2();
                }
            }

            if (hit.CompareTag("die"))
            {
                HeartManager.instance.Die();
            }

            if (hit.CompareTag("dream"))
            {
                GameObject.Find("DreamPanel").gameObject.GetComponent<fade>().Dream();
            }
        }
    }
    
    public void oniBody(GameObject hit)
    {
        if (!isGameOver&&!isSuper)
        {
            canTouchTime = 0;
            Vector2 dir = transform.position - hit.transform.position;
        dir.Normalize();
        if (dir.x <= 0.3f && dir.x >= -0.3f)
        {
            if (dir.x < 0)
                dir = new Vector2(-0.3f, dir.y);
            else
                dir = new Vector2(0.3f, dir.y);
        }
        if (dir.y <= 0.3f)
                dir = new Vector2(dir.x, 0.3f);
        

        rigid.velocity = Vector2.zero;
        rigid.bodyType = RigidbodyType2D.Dynamic;
        rigid.velocity = dir * nuckbackforce;
        Instantiate(slashEffect, transform.position, Quaternion.identity);
        anim.Play("attackAnim");
        canMove = false;
        isNuckBack = true;
        }
}

    public void oniHead(GameObject hit)
    {
        if (!isGameOver&&!isSuper)
        {
            canTouchTime = 0;
            Vector2 dir = transform.position - hit.transform.position;
            dir.Normalize();
            if (dir.x <= 0.3f && dir.x >= -0.3f)
            {
                if (dir.x < 0)
                    dir = new Vector2(-0.3f, dir.y);
                else
                    dir = new Vector2(0.3f, dir.y);
            }
            if (dir.y <= 0.3f)
                dir = new Vector2(dir.x, 0.3f);
         
            rigid.velocity = Vector2.zero;
            rigid.bodyType = RigidbodyType2D.Dynamic;
            rigid.velocity = dir * nuckbackforce;
            
            Instantiate(slashEffect, transform.position, Quaternion.identity);
            anim.Play("attackAnim");
            canMove = false;
            isNuckBack = true;
        }
    }
    
    public void Realdie()
    {
        if (!isGameOver&&!isSuper)
        {
            isGameOver = true;
            Instantiate(dieEffect, transform.position, Quaternion.identity);
            StopAllCoroutines();
            SoundManager.instance.hit();
            FindObjectOfType<GameManager>().isGameOver = true;
          FindObjectOfType<GameOverManager>().GameoverFunc(gameObject);
            Destroy(gameObject);
        }
    }

    public void ComboText(bool isHead)
    {
        if (isHead)
            {
                GameObject cq = Instantiate(headPop, worldCanvas.transform);
                cq.GetComponent<comboText>().initialize();
            }

            //콤보수표시
            GameObject cp = Instantiate(comboPop, worldCanvas.transform);
            cp.GetComponent<comboText>().initialize();
    }
    
    private float getAngle(float x1, float y1, float x2, float y2) //Vector값을 넘겨받고 회전값을 넘겨줌
    {
        float dx = x2 - x1;
        float dy = y2 - y1;

        float rad = Mathf.Atan2(dx, dy);
        float degree = rad * Mathf.Rad2Deg;
        
        return degree;
    }

    public void AttackCor()
    {
        StopAllCoroutines();
        StartCoroutine(atkCor());
    }
    IEnumerator atkCor()
    {
        yield return new WaitForSeconds(0.15f);
        isattack = false;
        yield return new WaitForSeconds(ComboManager.instance.comboDelay);
        particle.SetActive(false);
        trail.GetComponent<TrailRenderer>().startColor = Color.white;
        camera.sizedown();
        if(playerIndex==0) 
            StartCoroutine(panel.fadeout());
    }

    public void OnBarrier()
    {
        if (!isBarrier&&playerIndex==3)
        {
            isBarrier = true;
            GameObject.Find("Barrier").GetComponent<ParticleSystem>().Play();
        }
    }

    public void OffBarrier()
    {
        if (isBarrier&&playerIndex==3)
        {
            isBarrier = false;
            GameObject.Find("Barrier").GetComponent<ParticleSystem>().Stop();
        }
    }
}
