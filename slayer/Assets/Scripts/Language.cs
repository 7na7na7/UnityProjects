using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Language : MonoBehaviour
{
   public void Kor(bool isTutorial)
   {
      TextManager.instance.Set(1);
      GooglePlayManager.instance.isFirst = 1;
      PlayerPrefs.SetInt(   GooglePlayManager.instance.isFirstKey, 1);
      if(isTutorial) 
         FindObjectOfType<LoadScene>().Tutorial();
   }

   public void Eng(bool isTutorial)
   {
      TextManager.instance.Set(0);
      GooglePlayManager.instance.isFirst = 1;
      PlayerPrefs.SetInt(   GooglePlayManager.instance.isFirstKey, 1);
      if(isTutorial) 
         FindObjectOfType<LoadScene>().Tutorial();
   }

   public void Jap(bool isTutorial)
   {
      TextManager.instance.Set(2);
      GooglePlayManager.instance.isFirst = 1;
      PlayerPrefs.SetInt(   GooglePlayManager.instance.isFirstKey, 1);
      if(isTutorial) 
         FindObjectOfType<LoadScene>().Tutorial();
   }
}
