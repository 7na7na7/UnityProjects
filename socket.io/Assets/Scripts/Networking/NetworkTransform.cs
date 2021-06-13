using System;
using System.Collections;
using System.Collections.Generic;
using Project.Networking;
using Project.Utility.Attributes;
using UnityEngine;

namespace Project.Networking
{
    [RequireComponent(typeof(NetworkIdentity))] //NetworkIdentify 자동으로 추가
    public class NetworkTransform : MonoBehaviour
    {
        [GreyOut] 
        public Vector3 oldPosition;

        private NetworkIdentity networkIdenity;
        private Player player;

        private float stillCounter = 0;

        private void Start()
        {
            networkIdenity = GetComponent<NetworkIdentity>();
            oldPosition = transform.position;
            player = new Player();
            player.position=new Position();
            player.position.x = 0;
            player.position.y = 0;

            if (!networkIdenity.isControlling) //내가 아니면
            {
                enabled = false;
            }
        }

        private void Update()
        {
            //자기 자신이면
            if (networkIdenity.isControlling)
            {
                if (oldPosition != transform.position) //포지션이 변경되었다면
                {
                    //포지션 재설정
                    oldPosition = transform.position;
                    stillCounter = 0;
                    SendData();
                }
                else
                {
                    stillCounter += Time.deltaTime;

                    if (stillCounter >= 1) //가만히있던 시간이 1초 이상이면
                    {
                        //포지션 재설정(저 여기 잘 있어요 걱정마요! 라는 뜻)
                        stillCounter = 0;
                        SendData();
                    }
                }
            }
        }

        private void SendData()
        {
            //플레이어 데이터 설정
            player.position.x=Mathf.Round(transform.position.x*1000.0f)/1000.0f;
            player.position.y=Mathf.Round(transform.position.y*1000.0f)/1000.0f;
            //소켓에서 updatePosition동기화, Json으로 변환해 전달
            networkIdenity.GetSocket().Emit("updatePosition",new JSONObject(JsonUtility.ToJson(player))); 
        }
    }
}