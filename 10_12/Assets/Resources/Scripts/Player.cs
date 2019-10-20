using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun; //Pun네트워크 사용
using Photon.Pun.Demo.Cockpit;
using UnityEngine;
using UnityEngine.UI; //UI사용
using Random = UnityEngine.Random; //랜덤 사용
using Cinemachine; //2D카메라 Cinemachine 사용

public class Player : MonoBehaviourPunCallbacks, IPunObservable //변수 동기화를 위해 IPunObservable상속받음
{
    public float moveforce;
    public Rigidbody2D rigid; //이동을 위해
    private SpriteRenderer renderer; //색 설정을 위해
    public Animator anim; //에니메이션 설정을 위해
    public PhotonView pv; //포톤 설정을 위해
    public Text nickname; //닉네임 설정을 위해
    public Slider hp; //hp변동을 위해
    public Transform bulletTr;
    
    private Vector3 curPos; //부드러운 위치조정

    //총 회전을 위한 변수들
    public GameObject gun;
    private Camera camera;
    private Vector3 MousePositon; //회전을 위한 벡터값
    private float angle; //최종 회전값 저장
    private float x;

    private void Awake()
    {
        nickname.text = pv.IsMine ? PhotonNetwork.NickName : pv.Owner.NickName; //닉네임 설정, 자기 닉네임이 아니면 상대 닉네임으로
        nickname.color = pv.IsMine ? Color.green : Color.red; //닉네임 색깔 설정, 자기 닉네임이면 초록색, 아니면 빨강색
        if (pv.IsMine)
        {
            camera = GameObject.Find("Main Camera").GetComponent<Camera>();
            renderer = GetComponent<SpriteRenderer>();
            //카메라 Cinemachine을 찾아 자신을 따라오도록 함
            var CM = GameObject.Find("CMCamera").GetComponent<CinemachineVirtualCamera>();
            CM.Follow = transform;
            CM.LookAt = transform;

            Color color = renderer.color;
            int r = Random.Range(0, 2);
            int g = Random.Range(0, 2);
            int b = Random.Range(0, 2);
            color.r = r == 0 ? 0 : 255;
            color.g = g == 0 ? 0 : 255;
            color.b = b == 0 ? 0 : 255;
            Debug.Log("R : " + color.r + "G : " + color.g + "B : " + color.b);
            renderer.color = color;
        }
    }
    private void Update()
    {
        if (pv.IsMine) //자기 자신이라면
        {
            //이동
            float axisx = Input.GetAxisRaw("Horizontal");
            float axisy = Input.GetAxisRaw("Vertical");
            rigid.velocity = new Vector2(moveforce * axisx, moveforce * axisy);
            
            //애니메이션 설정
            if (axisx !=0||axisy!=0)
                anim.SetBool("walk", true);
            else
                anim.SetBool("walk", false);
            
            //총알 발사
            if (Input.GetMouseButtonDown(0))
            {
                PhotonNetwork.Instantiate("Bullet", bulletTr.position,bulletTr.rotation);
            }

            //총 회전
            MousePositon = Input.mousePosition;
            MousePositon =
                camera.ScreenToWorldPoint(MousePositon) - transform.position; //플레이어포지션을 빼줘야한다!!!!!!!!!!!
            //월드포지션은 절대, 카메라와 플레이어 포지션은 변할 수 있다!!!!!!!

            MousePositon.y -= 0.25f; //오차조정을 위한 코드

            angle = Mathf.Atan2(MousePositon.y, MousePositon.x) * Mathf.Rad2Deg;
             x = 0f;

             if (this.hp.value <= 0)
             {
                 if (PhotonNetwork.IsConnected) //연결되어 있다면
                     PhotonNetwork.Disconnect(); //연결 끊기
             }
        }
        //IsMine이 아닌 것들은 부드럽게 위치 동기화
        else if ((transform.position - curPos).sqrMagnitude >= 100)
            transform.position = curPos;
        else
            transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10);
        
        if (Mathf.Abs(angle) > 90)
        {
            gun.transform.rotation = Quaternion.Euler(180, 0f, -1*angle);
        }
        else
        {
            gun.transform.rotation = Quaternion.Euler(x, 0f, angle);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet") && !pv.IsMine) //적이 자신의 총알과 부딪혔을 때
            other.GetComponent<Bullet>().pv.RPC("DestroyRPC", RpcTarget.AllBuffered);
        if (other.CompareTag("Bullet") && !other.GetComponent<Bullet>().pv.IsMine&&pv.IsMine) //총알과 부딪혔고, 그 총알이 적의 총알이고, 자기 자신이라면
        {
            hp.value -= 10; //hp감소
        }
    } 
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) //변수동기화
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(hp.value);
            stream.SendNext(angle);
            stream.SendNext(MousePositon);
            stream.SendNext(x);
        }
        else
        {
            curPos = (Vector3) stream.ReceiveNext();
            hp.value = (float) stream.ReceiveNext();
            angle = (float) stream.ReceiveNext();
            MousePositon = (Vector3) stream.ReceiveNext();
            x = (float) stream.ReceiveNext();
        }
    }
}
