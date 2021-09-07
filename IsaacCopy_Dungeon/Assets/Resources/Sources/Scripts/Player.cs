using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class Player : MonoBehaviour
{
    private Camera camera;
    //패시브 변수
    private TweenParams parms = new TweenParams();
    //시작시 미니맵표시
    public LayerMask doorCol;
    public float radius;
    private Vector3 spawnPoint;

    //이동, 애니메이션
    private CapsuleCollider2D col;
    public bool canMove = true;
    private Animator anim;
    private Vector2 moveDirection;
    private Rigidbody2D rb;
    private float localScaleX;
    public float footCountCut = 10;
    private float footCount;
    private bool isSwamp = false;

    public int swampMovingSpeed = 40;
    // public Animator headAnim; //다리위쪽 애니메이션
    public float speed;
    public float savedSpeed;

    //구르기
    public bool isSuper = false; //무적인가?
    public Ease easeMode;
    public float rollTime;
    public float rollDistance;
    public float rollDelayMultiply;
    private bool canRoll = true;
    public int rollMp = 20;

    private SoundManager sound;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        localScaleX = transform.localScale.x;
        col = GetComponent<CapsuleCollider2D>();

        speed = savedSpeed;

        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        sound = GetComponent<SoundManager>();

        if (SceneManager.GetActiveScene().name == "Play")
        {
            GetComponent<CapsuleCollider2D>().isTrigger = true;
            Invoke("setCam", 2f);
        }
    }

    private void Update()
    {
        GetMove(); //이동
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            //이동여부에 따른 애니메이션 조정
            if (moveDirection == Vector2.zero)
            {
                anim.Play("Idle");
            }
            else
            {
                anim.Play("Walk");
            }
            rb.velocity = new Vector2(
                (moveDirection.x * speed) * (isSwamp ? swampMovingSpeed / 100f : 1f),
                (moveDirection.y * speed) * (isSwamp ? swampMovingSpeed / 100f : 1f));
            //방향 x 속도 x 무기속도 x 늪속도 x 기동신속도 
        }
        else//그 외는 전부 움직이지 않도록
        {
            //  pv.RPC("SetAnimRPC",RpcTarget.All,false,"Idle");
            rb.velocity = Vector2.zero;
        }
    }

    void setCam()
    {
        GetComponent<CapsuleCollider2D>().isTrigger = false;
        spawnPoint = transform.position;
        canMove = true;

        Destroy(GameObject.Find("LoadingPanel"));
        camera.transform.position = new Vector3(transform.position.x, transform.position.y - 0.25f, -10);
        camera.GetComponent<CameraManager>().target = gameObject;


        //자신 기준으로 이내의 반경의 doorCol검색
        Collider2D col = Physics2D.OverlapCircle(transform.position, radius, doorCol);
        if (col != null) //플레이어가 비지 않았다면
        {
            col.GetComponent<DoorCol>().Minimap();
        }
        else
        {
            print("감지 실패!");
        }

    }
    void roll(Vector2 dir)
    {

        if (canRoll)
        {
            canMove = false;
            canRoll = false;
            StartCoroutine(rollCor(dir, rollDistance));
        }
    }
    IEnumerator rollCor(Vector2 dir, float distance)
    {
        rb.velocity = Vector2.zero;

        anim.Play("Roll");


        isSuper = true; //무적 ON
        Vector2 originalSize = col.size;
        col.size = new Vector2(col.size.x - 0.02f, col.size.y - 0.02f); //크기 아주조금 줄여서 콜라이더 벽에 닿아서 끊기는거 방지
        rb.DOMove(transform.position + new Vector3(dir.x * distance, dir.y * distance), rollTime).SetEase(easeMode).SetAs(parms);
        yield return new WaitForSeconds(rollTime);
        isSuper = false; //무적 OFF
        yield return new WaitForSeconds(rollTime * rollDelayMultiply); //스턴시간은 구르는시간의 10분의 1
        col.size = originalSize; //원래 크기로 돌려줌

        anim.Play("Idle");


        canMove = true;
        canRoll = true;
    }


    void flashWhiteRPC()
    {
        GetComponent<FlashWhite>().Flash();
    }
    void GetMove() //이동입력
    {
        //        if(Input.GetKeyDown(KeyCode.Return))
        //            rb.velocity=Vector2.zero;
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized; //대각선 이동 정규화
        if (canMove)
            footCount += rb.velocity.sqrMagnitude / 100;
        if (footCount > footCountCut)
        {
            sound.Play(Random.Range(3, 10), 0.5f);
            footCount = 0;
        }
    }


    public void Move(Vector3 pos)
    {
        transform.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D other) //충돌함수
    {

        if (other.CompareTag("Teleport")) //순간이동
        {
            StopAllCoroutines();


            DOTween.Kill(parms);
            canRoll = true;
            canMove = true;

            anim.Play("Idle");

            FindObjectOfType<Fade>().Teleport(this, GameObject.Find(other.name + "_T").transform.position);

        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {


        if (other.CompareTag("Swamp")) //늪에 닿으면
            isSwamp = true;

    }

    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.CompareTag("Swamp")) //늪에 닿으면
            isSwamp = false;
    }

    private void OnCollisionStay2D(Collision2D other)  //플레이어 강체 충돌판정
    {

        if (other.gameObject.tag == "Wall") //벽에 닿으면 DOTween취소
        {
            if (!canRoll)
            {
                DOTween.Kill(parms);
            }
        }

    }
}
