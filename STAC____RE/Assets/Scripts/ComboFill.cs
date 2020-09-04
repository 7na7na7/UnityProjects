using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboFill : MonoBehaviour
{
    public static ComboFill instance;
   public Image fill;
   private float currentTime = 1;
   public GameObject fillAmount;
   public Text comboNum;

   private void Awake()
   {
       instance = this;
   }

   private void Update()
   {
       if (currentTime > 0)
           currentTime -= Time.deltaTime;
       if (Player.instance == null)
       {
           if(fill.fillAmount<=0)
               ComboManager.instance.ComboEnd();
           currentTime -= Time.deltaTime;  
           currentTime -= Time.deltaTime;  
       }

       if (ComboManager.instance.comboCount >= 2)
       {
          fillAmount.SetActive(true);
       }
       else
       {
           fillAmount.SetActive(false);
       }
       fill.fillAmount = (currentTime /ComboManager.instance.comboDelay);
   }

   public void Set(int comboCount)
   {
       currentTime = ComboManager.instance.comboDelay;
       comboNum.text = comboCount.ToString();
   }
}
