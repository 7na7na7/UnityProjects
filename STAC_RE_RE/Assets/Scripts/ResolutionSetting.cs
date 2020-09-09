using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResolutionSetting : MonoBehaviour
{
   public GameObject ScrollView, Tab,StartBtn,upUI;
   public bool isOn = true;
   private void Awake()
   { 
      if(isOn) 
         UpDown();
      //SetRes();
   }

   public void UpDown()
   {
      float goodRatio = 9f / 16f;
      float ratio = (float) Screen.width / Screen.height;
      
      if (ratio < goodRatio) //세로가 존나게 길경우
      {
        float difference = goodRatio - ratio;
        ScrollView.GetComponent<RectTransform>().anchoredPosition=new Vector3( ScrollView.GetComponent<RectTransform>().anchoredPosition.x, 
           ScrollView.GetComponent<RectTransform>().anchoredPosition.y+difference*1800,0);
        Tab.GetComponent<RectTransform>().anchoredPosition = new Vector3(
           Tab.GetComponent<RectTransform>().anchoredPosition.x,
           Tab.GetComponent<RectTransform>().anchoredPosition.y + difference * -1800, 0);
        StartBtn.GetComponent<RectTransform>().anchoredPosition = new Vector3(
           StartBtn.GetComponent<RectTransform>().anchoredPosition.x,
           StartBtn.GetComponent<RectTransform>().anchoredPosition.y + difference * -1800, 0);
        upUI.GetComponent<RectTransform>().anchoredPosition = new Vector3(
           upUI.GetComponent<RectTransform>().anchoredPosition.x,
           upUI.GetComponent<RectTransform>().anchoredPosition.y + difference * 1800, 0);
         //print(difference * -1800);
      }
   }
   public void SetRes()
   {
      //남는부분을 Rect를 조정하려 래터박스로 채울 수 있다!
      Camera camera = GetComponent<Camera>();
      Rect rect = camera.rect;

      float scaleHeight, scaleWidth;

      //세로로 하는게임의경우
      scaleHeight = ((float) Screen.width / Screen.height) / ((float) 9 / 16); //가로/세로
      scaleWidth = 1f / scaleHeight;
      if (scaleHeight < 1) //세로가 넓은경우
      {
         rect.height = scaleHeight;
         rect.y = (1f - scaleHeight) / 2f;
      }
      else //가로가 넓은경우
      {
         rect.width = scaleWidth;
         rect.x = (1f - scaleWidth) / 2f;
      }

      camera.rect = rect;
   }
}
