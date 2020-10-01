using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Btn : MonoBehaviour
{
   private bool isOn = false;
   public Animator anim;
   
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

   public void Ad()
   {
      AdmobVideoScript.instance.Show();
   }

   public void Purchase(int index)
   {
      IAPManager.instance.OnBtnPurchaseClicked(index);
   }

   public void PurchaseAnim()
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
