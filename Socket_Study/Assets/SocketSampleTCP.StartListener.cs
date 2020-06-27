using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class SocketSampleTCP : MonoBehaviour
{
   private Socket m_listener;
   void StartListener(int port)
   {
      //소켓 생성
      m_listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
   }
}
