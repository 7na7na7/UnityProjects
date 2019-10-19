using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Random = System.Random;

public class NetworkManager : MonoBehaviourPunCallbacks
{
  public InputField NickNameInput;
  public GameObject DisconnectPanel;

  void Awake()
  {
    Screen.SetResolution(960, 540, false);
    PhotonNetwork.SendRate = 60;
    PhotonNetwork.SerializationRate = 30;
  }

  public void Connect()
  {
    PhotonNetwork.ConnectUsingSettings();//서버에 연결함
  }

public override void OnConnectedToMaster()//연결이 되었을 때
  {
    PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
    PhotonNetwork.JoinOrCreateRoom("room", new RoomOptions { MaxPlayers = 20 }, null);
  }

  public override void OnJoinedRoom() //방에 들어갔을 때
  {
    DisconnectPanel.SetActive(false); 
    Spawn();
  }
  private void Spawn() //캐릭터 스폰
  {
    PhotonNetwork.Instantiate("Player", new Vector2(UnityEngine.Random.Range(-5,5),UnityEngine.Random.Range(-5,5)), Quaternion.identity); //Resource폴더의 Player를 생성
    DisconnectPanel.SetActive(false);
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape) && PhotonNetwork.IsConnected) PhotonNetwork.Disconnect(); //ESC키를 누르면 
  }
  
  public override void OnDisconnected(DisconnectCause cause) //연결이 끊겼을 때
  {
    DisconnectPanel.SetActive(true);
  }
}
