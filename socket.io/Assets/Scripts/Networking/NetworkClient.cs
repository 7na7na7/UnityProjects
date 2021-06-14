using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Project.Player;
using UnityEngine;
using SocketIO;
using WebSocketSharp;

namespace Project.Networking
{
    public class NetworkClient : SocketIOComponent
    {
        [Header("Network Client")]
        public Transform networkContainer;
        public GameObject playerPrefab;

        public static string ClientID { get; private set; } //얻을순잇는데 변경안댐!

        private Dictionary<string, NetworkIdentity> serverObjects;
        
        public override void Start()
        {
            base.Start();
            Initialize();
            SetupEvents();
        }
        
        public override void Update()
        {
            base.Update();
        }

        private void Initialize()
        {
            serverObjects=new Dictionary<string, NetworkIdentity>();
        }
        private void SetupEvents()
        {
            On("open", (e) =>
            {
                //열렸을 때 실행
                print("Connection Succeed to the Server.");
            });
            On("register", (e) =>
            {
                //회원가입
                ClientID = e.data["id"].ToString().Trim('"'); //양옆에 "" 없애기
                print("Our Client's ID : "+ClientID);
            });
            On("spawn", (e) =>
            {
                //플레이어 스폰 담당
                string id = e.data["id"].ToString().Trim('"');

                //networkContainer에 플레이어 스폰
                GameObject go = Instantiate(playerPrefab, networkContainer);
                go.name = string.Format("Player [{0}]", id);
                NetworkIdentity ni = go.GetComponent<NetworkIdentity>();
                ni.SetControllerID(id);
                ni.SetSocketReference(this);
                serverObjects.Add(id,ni);
            });
            On("disconnected", (e) =>
            {
                //연결 끊어졌을 때
                string id = e.data["id"].ToString().Trim('"');

                GameObject go = serverObjects[id].gameObject; 
                Destroy(go); //자신의 서버오브젝트 삭제
                serverObjects.Remove(id); //딕셔너리에서도 삭제
            });
            On("updatePosition", (e) =>
            {
                string id = e.data["id"].ToString().Trim('"');
                //x, y에 player.position.x, y 저장, f를 붙여 float으로 변환
                float x = e.data["position"]["x"].f;
                float y = e.data["position"]["y"].f;

                NetworkIdentity ni = serverObjects[id];
                ni.transform.position = new Vector3(x, y,0);
            });
            On("updateRotation", (e) =>
            {
                string id = e.data["id"].ToString().Trim('"');
                // tankRotation, barrelRotation에 player.tankRotation, barrelRotation 저장
                float tankRotation = e.data["tankRotation"].f;
                float barrelRotation = e.data["barrelRotation"].f;

                NetworkIdentity ni = serverObjects[id];
                ni.transform.localEulerAngles = new Vector3(0, 0, tankRotation);
                ni.GetComponent<PlayerManager>().SetRotation(barrelRotation);
            });
        }
    } 
    
    [Serializable]
    public class Player
    {
        public string id;
        public Position position;
    }

    [Serializable]
    public class Position
    {
        public float x;
        public float y;
    }

    [Serializable]
    public class PlayerRotation
    {
        public float tankRotation;
        public float barrelRotation;
    }
}
