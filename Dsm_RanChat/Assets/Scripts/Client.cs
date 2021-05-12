using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.IO;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using  UnityEngine.UI;

public class Client : MonoBehaviour
{
   public InputField NickInput; // 닉네임
   private string clientName; //클라이언트 이름
   private bool socketReady; //현재 소켓이 준비되었는가?
   private TcpClient socket; //서버와 통신할 Tcp소켓
   private NetworkStream stream; //현재 데이터흐름을 관리하는 스트림
   private StreamWriter writer; //데이터 쓰기
   private StreamReader reader; //데이터 읽기
   
   public void ConnectToServer(int min) //클라이언트로 접속 버튼으로 실행
   {
      if (min > 240)
      {
         FindObjectOfType<Server>().ServerCreate();
         return;
      }
      //이미 연결되어있다면 종료
      if (socketReady)
         return;
      
      //텍스트에 있는 IP/포트주소 받아옴, 없으면 기본값 (자기자신 127, 기본 7777)
      string ip = "10.156.146." + min;
      int port = 7777;

      //소켓 생성
      //print(ip + " 연결 시작!");
         
      socket = new TcpClient();
      var result = socket.BeginConnect(ip, port, null, null);
      var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(.025f));
      if (success)
      {
         stream = socket.GetStream();
         writer=new StreamWriter(stream); 
         reader=new StreamReader(stream);
         socketReady = true; //소켓이 준비되었으므로 true    
         //socket.EndConnect(result);
         return;
      }
      else
      { 
         ConnectToServer(min+1);
      }
   }

   private void Update()
   {
      //데이터를 읽을 수 있으면
      if (socketReady && stream.DataAvailable)
      {
         //데이터 받음
         string data = reader.ReadLine();
         if(data!=null)
            OnIncomingData(data);
      }
   }

   //데이터 받는 함수
   void OnIncomingData(string data)
   {
      //닉네임 표시
      if (data == "%NAME")
      {
         //클라이름이 비어있으면 게스트, 아니면 클라이름 넣어주기
         clientName = NickInput.text == "" ? "Guest" + UnityEngine.Random.Range(1000, 10000) : NickInput.text;
         //서버에서 클라이름으로 등록해주기 위해 이렇게 보냄
         Send($"&NAME|{clientName}");
         return;
      }
      //이걸로 채팅 표시
      Chat.instance.ShowMessage(data);
   }

   void Send(string data)
   {
      //소켓이 없으면 좋료
      if (!socketReady)
         return;
      
      //데이터 보냄
      writer.WriteLine(data);
      writer.Flush();
   }

   //종료 시 실행
   private void OnApplicationQuit()
   {
      CloseSocket();
   }

   //소켓 닫기
   void CloseSocket()
   {
      if (!socketReady)
         return;
      writer.Close();
      reader.Close();
      socket.Close();
      socketReady = false;
   }

   public void SendBtn(InputField SendInput)
   {
#if(UNITY_EDITOR||UNITY_STANDALONE)
      if (!Input.GetButtonDown("Submit"))
         return;
      //포커스 재조정
      SendInput.ActivateInputField();
#endif
      //Trim = 공백제거함수, 이거썼는데도 없으면 진짜없는거니까 종료
      if (SendInput.text.Trim() == "")
         return;

      string message = SendInput.text;
      SendInput.text = "";
      Send(message);
   }
}
