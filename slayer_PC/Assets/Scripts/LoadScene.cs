using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.Escape))
      {
         if(SceneManager.GetActiveScene().name=="Setting"||SceneManager.GetActiveScene().name=="Stage")
            Title();
      }
   }

   public void Title()
   {
      SoundManager.instance.select();
      SceneManager.LoadScene("Title");
   }

   public void Setting()
   {  SoundManager.instance.select();
      SceneManager.LoadScene("Setting");
   }

   public void Tutorial()
   {
      SoundManager.instance.select();
      SceneManager.LoadScene("Tutorial");
   }

   public void Exit()
   {
      Application.Quit();
   }
}
