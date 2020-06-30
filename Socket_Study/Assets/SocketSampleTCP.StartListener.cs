using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class SocketSampleTCP : MonoBehaviour
{
   // 리스닝 소켓.
   private Socket			m_listener = null;

   // 클라이언트와의 접속용 소켓.
   private Socket			m_socket = null;
   
   // 접속 플래그.
   private	bool			m_isConnected = false;
   
   void StartListener(int port)
   {
      //소켓 생성
      m_listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      
      //사용할 포트 번호를 할당
      m_listener.Bind(new IPEndPoint(IPAddress.Any, port));
      
      //대기를 시작한다
      m_listener.Listen(1);
      
      //m_state=State.AcceptClient;
   }

   void AccetptClient()
   {
      if (m_listener != null && m_listener.Poll(0, SelectMode.SelectRead))
      {
         //클라이언트가 접속했다
         m_socket = m_listener.Accept();
         m_isConnected = true;
      }
   }

   void ClientProcess()
   {
      //ㅅ버에 접속
      m_socket=new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
      m_socket.NoDelay = true;
      m_socket.SendBufferSize = 0;
      //m_socket.Connect(m_address,m_port);
   }
}
