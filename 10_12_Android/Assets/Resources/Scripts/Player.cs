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
    public bool ismove = false; //움직이고 있는가?
    
    public float moveforce;
    public Rigidbody2D rigid; //이동을 위해
    private SpriteRenderer renderer; //색 설정을 위해
    public Animator anim; //에니메이션 설정을 위해
    public PhotonView pv; //포톤 설정을 위해
    public Text nickname; //닉네임 설정을 위해
    public Slider hp; //hp변동을 위해
    public Transform bulletTr;
    
    private Vector3 curPos; //부드러운 위치조정
    
    
    //대쉬를 위한 변수들
    public float dashforce;
    public float dashtime;
    public float dashdelay;
    private bool candash = true;
    private void Awake()
    {
        nickname.text = pv.IsMine ? PhotonNetwork.NickName : pv.Owner.NickName; //닉네임 설정, 자기 닉네임이 아니면 상대 닉네임으로
        nickname.color = pv.IsMine ? Color.green : Color.red; //닉네임 색깔 설정, 자기 닉네임이면 초록색, 아니면 빨강색
        if (pv.IsMine)
        {

            //카메라 Cinemachine을 찾아 자신을 따라오도록 함
            var CM = GameObject.Find("CMCamera").GetComponent<CinemachineVirtualCamera>();
            CM.Follow = transform;
            CM.LookAt = transform;
        }
        else
        {
            while (true)
            {
                renderer = GetComponent<SpriteRenderer>();
                Color color = renderer.color;
                int r = Random.Range(0, 2);
                int g = Random.Range(0, 2);
                int b = Random.Range(0, 2);
                color.r = r == 0 ? 0 : 255;
                color.g = g == 0 ? 0 : 255;
                color.b = b == 0 ? 0 : 255;
                Debug.Log("R : " + color.r + "G : " + color.g + "B : " + color.b);
                renderer.color = color;
                if (r + g + b != 3)
                    break;
            }
        }
    }
    private void Update()
    {
        if (pv.IsMine) //자기 자신이라면
        {
            //애니메이션 설정
            if (ismove)
                anim.SetBool("walk", true);
            else
                anim.SetBool("walk", false);

            //대쉬
             if (Input.GetKeyDown(KeyCode.LeftShift))
                 StartCoroutine(dash());

             if (this.hp.value <= 0) //체력이 0보다 낮아지면
             {
                 if (PhotonNetwork.IsConnected) //연결되어 있다면
                 { 
                     PhotonNetwork.Disconnect(); //연결 끊기
                 } 
             }
        }
        //IsMine이 아닌 것들은 부드럽게 위치 동기화
        else if ((transform.position - curPos).sqrMagnitude >= 100)
            transform.position = curPos;
        else
            transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10);
        
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
        }
        else
        {
            curPos = (Vector3) stream.ReceiveNext();
            hp.value = (float) stream.ReceiveNext();
        }
    }
    IEnumerator dash()
    {
        if (candash)
        {
            candash = false;
            moveforce *= dashforce;
            yield return new WaitForSeconds(dashtime);
            moveforce /= dashforce;
            yield return new WaitForSeconds(dashdelay);
            candash = true;
        }
        else
        {
            yield return null;
        }
    }

    public void attack(Vector3 vec)
    {
        GameObject bullet=PhotonNetwork.Instantiate("Bullet", bulletTr.position,bulletTr.rotation);
        bullet.transform.eulerAngles = new Vector3(0,0,vec.z);
    }
    [PunRPC]
    void FlipXRPC()
    {
        GetComponent<SpriteRenderer>().flipX = true;
    }
    [PunRPC]
    void FlipXRPC2()
    {
        GetComponent<SpriteRenderer>().flipX = false;
    }
}
