using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Btn : MonoBehaviour
{
   public void LogInOut()
   {
      GooglePlayManager.instance.LogInOrLogOut();
   }

   public void Leader()
   {
      GooglePlayManager.instance.OnShowLeaderBoard();
   }

   public void Ahieve()
   {
      GooglePlayManager.instance.OnShowAchievement();
   }
}
