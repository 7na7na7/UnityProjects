using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject roomPanel;
    public GameObject startPanel;
    public GameObject loadingPanel;
    public InputField nameInput;
    string name;
    public void Connect()
    {
       
        if(Application.internetReachability == NetworkReachability.NotReachable)
        {
            // 인터넷 연결이 안되었을 때 
        }
        else
        {
            name = nameInput.text;
            PhotonNetwork.ConnectUsingSettings();
          
            startPanel.SetActive(false);
            loadingPanel.SetActive(true);
        }
    }

    
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }


    
    public override void OnJoinedLobby()
    {
        PhotonNetwork.LocalPlayer.NickName = name;
        RoomOptions roomOpton=new RoomOptions();
        roomOpton.MaxPlayers =2;
        PhotonNetwork.JoinOrCreateRoom("1",roomOpton,null);
    }
    
    public override void OnJoinedRoom()
    {
        loadingPanel.SetActive(false);
        roomPanel.SetActive(true);
        print("방참가 성공!");
    }
}
