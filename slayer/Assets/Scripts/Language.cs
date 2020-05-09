using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Language : MonoBehaviour
{
   public void Kor(bool isTutorial)
   {
      TextManager.instance.Set(true);
      GooglePlayManager.instance.isFirst = 1;
      PlayerPrefs.SetInt(   GooglePlayManager.instance.isFirstKey, 1);
      if(isTutorial) 
         FindObjectOfType<LoadScene>().Tutorial();
   }

   public void Eng(bool isTutorial)
   {
      TextManager.instance.Set(false);
      GooglePlayManager.instance.isFirst = 1;
      PlayerPrefs.SetInt(   GooglePlayManager.instance.isFirstKey, 1);
      if(isTutorial) 
         FindObjectOfType<LoadScene>().Tutorial();
   }
}
