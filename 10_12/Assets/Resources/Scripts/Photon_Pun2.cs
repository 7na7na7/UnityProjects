using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class Photon_Pun2 :  MonoBehaviourPunCallbacks
{
    public InputField Nickname;
    void Start()
    {
        Screen.SetResolution(1920,1080,true); //해상도 설정
        PhotonNetwork.SendRate = 60; 
        PhotonNetwork.SerializationRate = 30; 
        //동기화 빠르게
    }

    public override void OnConnectedToMaster() //서버에 연결되었을 때 호출됨
    {
        PhotonNetwork.LocalPlayer.NickName = Nickname.text;//닉네임을 입력한 닉네임으로 바꿈
        PhotonNetwork.JoinOrCreateRoom("room", new RoomOptions {MaxPlayers = 20}, null);
        //room이라는 방을 만들거나 있으면 들어감, 최대 플레이어 20명
    }
  
    public override void OnJoinedRoom() //방에 들어갔을 때 호춤됨
    { }
    public override void OnDisconnected(DisconnectCause cause)//연결이 끊어졌을 때 호출됨
    { }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) //엔터 버튼으로 연결하기
        {
            if (PhotonNetwork.IsConnected) //연결된 상태라면
                PhotonNetwork.ConnectUsingSettings();
        }

        if (Input.GetKeyDown(KeyCode.Escape)) //ESC버튼으로 연결끊기
        {
            if (!PhotonNetwork.IsConnected) //연결되어 있지 않다면
                PhotonNetwork.Disconnect();
        }
    }
}
