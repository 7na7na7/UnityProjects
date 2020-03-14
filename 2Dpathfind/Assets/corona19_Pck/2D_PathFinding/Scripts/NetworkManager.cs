using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private bool once = false;
    MoveByClick[] players;
    public GameObject chatkillbtn;
    public GameObject logs;
    public InputField chatInput;
    public Sprite connecting;
    private Sprite go;
    public GameObject text;
    public GameObject btn;
    public GameObject textNull;
    public int coronaIndex = 0;
    public InputField Nickname;
    public GameObject DisconnectPanel;
    public GameObject score;

    void Start()
    {
        Nickname.Select();
        go = btn.GetComponent<Image>().sprite;
        Screen.SetResolution(1600, 900, false);
        //Screen.SetResolution(960, 540, false);
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
        //동기화 빠르게
    }

    public override void OnConnectedToMaster() //서버에 연결되었을 때
    {
        PhotonNetwork.LocalPlayer.NickName = Nickname.text; //닉네임을 입력한 닉네임으로 바꿈
        PhotonNetwork.JoinOrCreateRoom("room", new RoomOptions {MaxPlayers = 20}, null);
        //room이라는 방을 만들거나 있으면 들어감, 최대 플레이어 20명
    }

    public override void OnJoinedRoom() //방에 들어갔을 때
    {
        chatkillbtn.SetActive(true);
        score.SetActive(true);
        DisconnectPanel.SetActive(false); //비활성화
        //StartCoroutine(DestroyBullet());
        PhotonNetwork.Instantiate("Player" + coronaIndex.ToString(), Vector3.zero, Quaternion.identity); //플레이어 생성
    }

    public override void OnDisconnected(DisconnectCause cause) //연결이 끊어졌을 때
    {
        chatkillbtn.SetActive(false);
        btn.SetActive(true);
        score.SetActive(false);
        DisconnectPanel.SetActive(true); //활성화
        btn.GetComponent<Image>().sprite = go;
        Debug.Log("연결 끊어짐!");
    }
    public void chatkill()
    {
        print("ASD");
        if(logs.GetComponent<RectTransform>().localScale==new Vector3(0,0,1)) 
           logs.GetComponent<RectTransform>().localScale=new Vector3(1,1,1);
        else
            logs.GetComponent<RectTransform>().localScale=new Vector3(0,0,1);
    }
    void Update()
    {

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            btn.SetActive(false);
            text.SetActive(true);
        }
        else
        {
            btn.SetActive(true);
            text.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) //연결끊기
        {
            if (PhotonNetwork.IsConnected) //연결된 상태라면
            {
                PhotonNetwork.Disconnect(); //연결 끊기
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    public void connect()
  {
      if (!PhotonNetwork.IsConnected) //연결된 상태가 아니라면
      {
          if (Nickname.text== null || Nickname.text == "")
          {
              Instantiate(textNull, GameObject.Find("Canvas").transform);
          }
          else
          {
              btn.SetActive(false);
              btn.GetComponent<Image>().sprite = connecting;
              PhotonNetwork.ConnectUsingSettings(); //연결하기
              btn.SetActive(false);
              Debug.Log("연결됨1!");
          }
      }
  }
  
  IEnumerator DestroyBullet() //리스폰할 때 모든 총알 제거
  {
      yield return new WaitForSeconds(0.05f);
      foreach (GameObject GO in GameObject.FindGameObjectsWithTag("Bullet")) GO.GetComponent<PhotonView>().RPC("DestroyRPC", RpcTarget.All);
  }
}
