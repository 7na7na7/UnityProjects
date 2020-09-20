using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSource : MonoBehaviour
{
 private bool isOn = false;
 public Animator anim;

 public void Source()
 {
  if (Application.systemLanguage == SystemLanguage.Korean)
  {
   if (!isOn) 
   { 
    isOn = true;
    anim.Play("SettingPanelAnim");
   }
   else
   {
    isOn = false;
    anim.Play("DefaultAnim");
   } 
  }
 }
}
