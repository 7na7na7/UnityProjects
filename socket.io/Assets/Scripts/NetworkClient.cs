using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using SocketIO;
using WebSocketSharp;

namespace Project.Networking
{
    public class NetworkClient : SocketIOComponent
    {
        [Header("Network Client")] 
        [SerializeField]
        private Transform networkContainer;

        private Dictionary<string, GameObject> serverObjects;
        
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
            serverObjects=new Dictionary<string, GameObject>();
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
                string id = e.data["username"].ToString().Trim('"'); //양옆에 "" 없애기
                print("Our Client's ID : "+id);
            });
            On("spawn", (e) =>
            {
                //플레이어 스폰 담당
                string id = e.data["id"].ToString().Trim('"');
                
                GameObject go=new GameObject("Server ID : " + id);
                go.transform.SetParent(networkContainer);
                serverObjects.Add(id,go);
            });
            On("disconnected", (e) =>
            {
                //연결 끊어졌을 때
                string id = e.data["id"].ToString().Trim('"');

                GameObject go = serverObjects[id]; 
                Destroy(go); //자신의 서버오브젝트 삭제
                serverObjects.Remove(id); //딕셔너리에서도 삭제
            });
        }
    }
}
