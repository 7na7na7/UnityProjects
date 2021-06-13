using System.Collections;
using System.Collections.Generic;
using Project.Utility.Attributes;
using SocketIO;
using UnityEngine;

namespace Project.Networking
{
    public class NetworkIdentity : MonoBehaviour
    {
        [Header("Helpful Values")]
        [GreyOut]
        public string id;
        [GreyOut]
        public bool isControlling;

        private SocketIOComponent socket;
        void Awake()
        {
            isControlling = false;
        }
        //ID설정에 따른 컨트롤할자기자신 파악
        public void SetControllerID(string ID)
        {
            id = ID;
            isControlling = (NetworkClient.ClientID == id) ? true : false; //자기면 true아니면 false
        }
        //소켓설정
        public void SetSocketReference(SocketIOComponent Socket)
        {
            socket = Socket;
        }
        //아이디얻기
        public string GetID()
        {
            return id;
        }
        //자신인지얻기
        public bool IsControlling()
        {
            return isControlling;
        }
        //소켓얻기
        public SocketIOComponent GetSocket()
        {
            return socket;
        }
    }
}
