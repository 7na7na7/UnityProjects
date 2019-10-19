using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public InputField Nickname;
    public GameObject DisconnectPanel;
  void Start()
    {
        Screen.SetResolution(960,540,false);
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30; 
        //동기화 빠르게
    }

  public override void OnConnectedToMaster() //서버에 연결되었을 때
  {
      PhotonNetwork.LocalPlayer.NickName = Nickname.text;//닉네임을 입력한 닉네임으로 바꿈
      PhotonNetwork.JoinOrCreateRoom("room", new RoomOptions {MaxPlayers = 20}, null);
      //room이라는 방을 만들거나 있으면 들어감, 최대 플레이어 20명
  }
  
  public override void OnJoinedRoom() //방에 들어갔을 때
  {
      DisconnectPanel.SetActive(false);//비활성화
      PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);//플레이어 생성
  }

  public override void OnDisconnected(DisconnectCause cause)//연결이 끊어졌을 때
  {
      DisconnectPanel.SetActive(true);//활성화
      Debug.Log("연결 끊어짐!");
  }

  void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) //연결
        {
            if (!PhotonNetwork.IsConnected) //연결된 상태가 아니라면
            {
                PhotonNetwork.ConnectUsingSettings(); //연결하기
                Debug.Log("연결됨!");
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape)) //연결끊기
        {
            if (PhotonNetwork.IsConnected)//연결된 상태라면
            {
                PhotonNetwork.Disconnect(); //연결 끊기
            }
        }
    }

  public void spawn()
  {
     
  }
}
