using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Cinemachine;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.XR;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerScript : MonoBehaviourPunCallbacks, IPunObservable
{
    private bool candash = true;
    public float dashtime;
    public float speed;
    public Rigidbody2D RB;
    public Animator AN;
    public SpriteRenderer SR;
    public PhotonView PV;
    public Text NickNameText;
    public Slider HP;
    
    private Vector3 curPos;

    private void Awake()
    {
        //닉네임
        NickNameText.text = PV.IsMine ? PhotonNetwork.NickName : PV.Owner.NickName;
        NickNameText.color = PV.IsMine ? Color.green : Color.red;
        if (PV.IsMine)
        {
            //2D카메라
            var CM = GameObject.Find("CMCamera").GetComponent<CinemachineVirtualCamera>();
            CM.Follow = transform;
            CM.LookAt = transform;
        }
    }
    
    private void Update()
    {
        if (PV.IsMine)
        {
            //이동
            float axisx = Input.GetAxisRaw("Horizontal");
            float axisy = Input.GetAxisRaw("Vertical");
            RB.velocity=new Vector2(speed*axisx,speed*axisy);
            if (axisx != 0||axisy!=0)
                AN.SetBool("iswalk",true);
            else
                AN.SetBool("iswalk",false);

            //총알 발사
            /*
            if (Input.GetKeyDown(KeyCode.X)||Input.GetKeyDown(KeyCode.Less))
            {
                PhotonNetwork.Instantiate("Bullet",
                    transform.position + new Vector3(SR.flipX ? -0.4f : 0.4f, -0.11f, 0), Quaternion.identity)
                    .GetComponent<PhotonView>().RPC("DirRPC",RpcTarget.All,SR.flipX ? -1 : 1);
            }
            */
            //대쉬
            {
                if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Greater))
                {
                    StartCoroutine(dash());
                }
            }
        }
        //IsMine이 아닌 것들은 부드럽게 위치 동기화
        else if ((transform.position - curPos).sqrMagnitude >= 100)
            transform.position = curPos;
        else
            transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10);
    }

    public void Hit()
    {
        HP.value--;
        if (HP.value <= 0)
        {
            GameObject.Find("Canvas").transform.Find("RespawnPanel").gameObject.SetActive(true);
            PV.RPC("DestroyRPC",RpcTarget.AllBuffered); //AllBuffered로 해야한다
        }
    }
    

    [PunRPC]
    void DestroyRPC()
    {
        Destroy(gameObject);
    }
    IEnumerator dash()
    {
        if (candash)
        {
            candash = false;
            RB.velocity = new Vector2(0, 0);
            speed *= 3;
            yield return new WaitForSeconds(dashtime);
            speed /= 3;
            yield return new WaitForSeconds(2f);
            candash = true;
        }
        else
        {
            yield return null;
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //여기서 변수 동기화가 진행됨
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(HP.value);
        }
        else
        {
            curPos = (Vector3) stream.ReceiveNext();
            HP.value = (float) stream.ReceiveNext();
        }
    }
    
}
