using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Project.Gameplay;
using Project.Player;
using Project.Scriptable;
using Project.Utility;
using UnityEngine;
using SocketIO;
using WebSocketSharp;

namespace Project.Networking
{
    public class NetworkClient : SocketIOComponent
    {
        public const float SERVER_UPDATE_TIME = 10;
        
        [Header("Network Client")]
        public Transform networkContainer;
        public GameObject playerPrefab;
        public ServerObjects serverSpawnables;
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
                print(e.data["id"].ToString());
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
            On("serverSpawn", (e) =>
            {
                //스폰 
                string name = e.data["name"].str; //이름
                string id = e.data["id"].ToString().RemoveQuotes();
                
                float x = e.data["position"]["x"].f; //위치값 x
                float y = e.data["position"]["y"].f; //위치값 y
                print("server wants us to spawn a "+name+" at "+x+", "+y);
                
                if (!serverObjects.ContainsKey(id)) //서버오브젝트중에 해당 id를 가진오브젝트가 없다면(중복생성방지)
                { 
                    ServerObjectData sod = serverSpawnables.GetObjectByName(name); //이름으로 스폰할 오브젝트 가지고 옴
                    GameObject spawnedObject = Instantiate(sod.Prefab, networkContainer); //스폰
                    spawnedObject.transform.position = new Vector3(x, y, 0); //위치셋
                    var ni = spawnedObject.GetComponent<NetworkIdentity>(); //네트워크오브젝트면 가지고있는 ni가져옴
                    ni.SetControllerID(id); //ni설정 첫번째
                    ni.SetSocketReference(this); //ni설정 두번째

                    if (name == "Bullet") //총알아면
                    {
                        float dirX = e.data["direction"]["x"].f; //direction.x
                        float dirY = e.data["direction"]["y"].f; //direction.y로 방향 갖고옴
                        string activator = e.data["activator"].ToString().RemoveQuotes(); //총알쏜사람갖고오기
                        float speed = e.data["speed"].f;

                        float rot = Mathf.Atan2(dirY, dirX) * Mathf.Rad2Deg;
                        Vector3 currentRotation=new Vector3(0,0,rot-90);
                        spawnedObject.transform.rotation=Quaternion.Euler(currentRotation);

                        //총쏜사람 설정
                        WhoActivateMe whoActivateMe = spawnedObject.GetComponent<WhoActivateMe>();
                        whoActivateMe.SetActivator(activator);

                        Projectile projectile = spawnedObject.GetComponent<Projectile>();
                        projectile.Direction=new Vector2(dirX,dirY);
                        projectile.Speed = speed;
                    }
                    
                    serverObjects.Add(id,ni); //서버오브젝트에 스폰한 오브젝트 추가
                }
            });
            On("serverUnspawn", (e) =>
            {
                //디스폰
                string id = e.data["id"].ToString().RemoveQuotes();
                NetworkIdentity ni = serverObjects[id];
                serverObjects.Remove(id);
                DestroyImmediate(ni.gameObject); //서버상에서 빠르게 지워야하기때문에 쓰는거같다!
            });
            On("playerDead", (e) =>
            {
                //플레이어 죽었을때
                string id = e.data["id"].ToString().RemoveQuotes();
                NetworkIdentity ni = serverObjects[id];
                ni.gameObject.SetActive(false); //안보이게
            });
            On("playerRespawn", (e) =>
            {
                //죽은 플레이어를 리스폰시킴
                string id = e.data["id"].ToString().RemoveQuotes();
                NetworkIdentity ni = serverObjects[id];
                //스폰위치
                float x = e.data["position"]["x"].f;
                float y = e.data["position"]["y"].f;
                ni.gameObject.transform.position=new Vector3(x,y,0);
                ni.gameObject.SetActive(true); //다시보이게
            });
        }

        public void AttemptToJoinLobby()
        {
            Emit("joinGame");
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

    [Serializable]
    public class BulletData
    {
        public string id;
        public string activator;
        public Position position;
        public Position direction;
    }

    [Serializable]
    public class IDdata
    {
        public string id;
    }
}
