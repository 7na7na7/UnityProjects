using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event : MonoBehaviour
{
   public event EventHandler OnSpacePressed; //이벤트 정의

   private void Start()
   {
      //OnSpacePressed += Testing_OnspacePressed; 이렇게 추가가능!
   }

//   void Testing_OnspacePressed(object sender, EventArgs e)
//   {
//      print("스페이스바가 눌렸어요!"); //이렇게 선언!
//   }
   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.Space))
      {
          //?는 비어있지 않으면 실행한다는 거다!
          if(OnSpacePressed!=null) 
              OnSpacePressed.Invoke(this,EventArgs.Empty); //이벤트 실행
      }
   }
}
