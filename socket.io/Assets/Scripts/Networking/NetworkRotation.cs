using System.Collections;
using System.Collections.Generic;
using Project.Player;
using Project.Utility;
using Project.Utility.Attributes;
using UnityEngine;

namespace Project.Networking
{
    [RequireComponent(typeof(NetworkIdentity))] //NetworkIdentify 자동으로 추가
    public class NetworkRotation : MonoBehaviour
    {
        [Header("Referenced Values")]
        [GreyOut]
        public float oldTankRotation;
        [GreyOut] 
        public float oldBarrelRotation;

        [Header("Class References")] 
        public PlayerManager playerManager;

        private NetworkIdentity networkIdentity;
        private PlayerRotation player;
        private float stillCounter = 0;
        void Start()
        {
            networkIdentity = GetComponent<NetworkIdentity>();
            
            player=new PlayerRotation();
            player.tankRotation = 0;
            player.barrelRotation = 0;

            if (!networkIdentity.isControlling) //내꺼가아니면
            {
                enabled = false; //비활성화
            }
        }
        
        void Update()
        {
            if (networkIdentity.isControlling)
            {
                //old친구들이 최신정보를 동기화받지 못했다면
                if (oldTankRotation != transform.localEulerAngles.z || oldBarrelRotation != playerManager.GetLastRotation())
                {
                    oldTankRotation = transform.localEulerAngles.z;
                    oldBarrelRotation = playerManager.GetLastRotation();
                    stillCounter = 0;
                    SendData(); //동기화
                }
                else
                {
                    stillCounter += Time.deltaTime;
                    if (stillCounter >= 1) //1초이상 로테이션이 안변하면
                    {
                        stillCounter = 0;
                        SendData(); //동기화
                    }
                }
            }
        }
  
        private void SendData()
        {
            //플레이어데이터설정
            player.tankRotation = transform.localEulerAngles.z.TwoDecimals();
            player.barrelRotation = playerManager.GetLastRotation().TwoDecimals();
            
            //Json으로 로테이션 변환해 보내줌
            networkIdentity.GetSocket().Emit("updateRotation",new JSONObject(JsonUtility.ToJson(player)));
        }
    }

}