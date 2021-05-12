using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.IO;
using UnityEngine.UI;

public class Server : MonoBehaviour
{
   private List<ServerClient> clients;
   private List<ServerClient> disconnectList;

   private TcpListener server; //리스너, 받는 역할
   private bool isServerStarted; //서버가 시작했는가? 

   
   public void ServerCreate()
   {
      clients=new List<ServerClient>();
      disconnectList=new List<ServerClient>();

      try
      {
         int port = 7777; //포트번호 넣어줌
         server = new TcpListener(IPAddress.Any, port); //IP주소와 포트로 리스너 생성
         server.Start(); //서버 시작

         StartListening();
         isServerStarted = true;
         Chat.instance.ShowMessage($"서버가 {port}에서 시작되었습니다.");
         string ip = Dns.GetHostAddresses(Dns.GetHostName())[1].ToString();
         ip = ip.Split('.')[3];

      FindObjectOfType<Client>().ConnectToServer(int.Parse(ip));
      }
      catch (Exception e)
      {
         print(e);
      }
   }
   
   void StartListening()
   {
      //비동기로 듣기시작
      server.BeginAcceptTcpClient(AcceptTcpClient, server);
   }

   void AcceptTcpClient(IAsyncResult ar)
   {
      TcpListener listener = (TcpListener) ar.AsyncState;
      clients.Add(new ServerClient(listener.EndAcceptTcpClient(ar))); //클라이언트 추가
      StartListening(); 
      
      //메시지를 연결된 모두에게 보냄
      Broadcast("%NAME", new List<ServerClient>() {clients[clients.Count - 1]});
   }
   
   private void Update()
   {
      if (!isServerStarted)
         return; //서버가 시작되지 않았으면 종료

      foreach (ServerClient c in clients)
      {
         //클라이언트가 연결되어 있지 않으면
         if (!IsConnected(c.tcp))
         {
            c.tcp.Close(); //tcp소켓 닫음
            disconnectList.Add(c); //끊긴리스트에 추가
            continue;
         }
         else //연결되어 있으면 클라카라 체크메시지를 받는다
         {
            //데이터의 흐름을 담당하는 스트림 생성
            NetworkStream s = c.tcp.GetStream();
            if (s.DataAvailable) //데이터가 존재하면
            {
               //StreamReader로 데이터 받아옴
               string data=new StreamReader(s,true).ReadLine();
               if (data != null) //데이터가 있다면
                  OnIncomingData(c, data); 
            }
         }  
      }

      for (int i = 0; i < disconnectList.Count - 1; i++)
      {
         //모든 클라에게 연결끊겼다고 보냄
         if(disconnectList[i].clientName.Trim()!="Guest") 
            Broadcast($"{disconnectList[i].clientName} 연결이 끊어졌습니다",clients);
         clients.Remove(disconnectList[i]);
         disconnectList.RemoveAt(i);
      }
   }

   void OnIncomingData(ServerClient c, string data)
   {
      if (data.Contains("&NAME"))
      {
         c.clientName = data.Split('|')[1];
         Broadcast($"{c.clientName}이 연결되었습니다",clients);
         return;
      }
      
      Broadcast($"{c.clientName} : {data}",clients);
   }

   //데이터를 모두에게 보내는 함수
   void Broadcast(string data, List<ServerClient> cl)
   {
      foreach (var c in cl)
      {
         try
         {
            //쓰기 모드 활성화
            StreamWriter writer = new StreamWriter(c.tcp.GetStream());
            //스트링 쓰기
            writer.WriteLine(data);
            //지금까지 쓴 데이터 강제로 내보내기, 에러나면 캐치로 에러메시지
            writer.Flush();
         }
         catch (Exception e)
         {
            print(e);
            //Chat.instance.ShowMessage($"쓰기 에러 : {e.Message}를 클라이언트에게 {c.clientName}");
         }
      }
   }
   
   bool IsConnected(TcpClient c)
   {
      try
      {
         if (c != null && c.Client != null && c.Client.Connected) //Tcp클라가 연결되어있으면
         {
            //Poll : 연결됫는지 테스트로 1바이트 보내고 받으면 true, 즉 연결됐는지 아닌지 테스트
            if (c.Client.Poll(0, SelectMode.SelectRead))
               return c.Client.Receive(new byte[1], SocketFlags.Peek) != 0;

            return true;
         }
         else //연결안되있으면
            return false;
      }
      catch (Exception e)
      {
         //오류나도 false반환
         return false;
      }
   }
}

public class ServerClient
{
   public TcpClient tcp;
   public string clientName;
   
   public ServerClient(TcpClient clientSocket)
   {
      clientName = "Guest";
      tcp = clientSocket;
   }
}
