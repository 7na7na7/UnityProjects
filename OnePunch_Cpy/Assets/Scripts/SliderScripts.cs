using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScripts : MonoBehaviour
{
   public static SliderScripts instance;
   
   public float hpMinusDelay;
   public float speedUpScaleValue;
   public float speedUpTimeValue;
   public Slider slider;
   private float value = 0.2f;
   private void Start()
   {
      instance = this;
      StartCoroutine(sliderCor());
      StartCoroutine(minusAccelerator());
   }

   IEnumerator sliderCor()
   {
      yield return new WaitForSeconds(0.1f);
      while (true)
      {
         if (!PlayerScript.instance.isdead)
         {
            yield return new WaitForSeconds(hpMinusDelay);
            slider.value -= value;
            if (slider.value <= 0)
            {
               if (PlayerScript.instance.anim.GetBool("isleft"))
               {
                  PlayerScript.instance.anim.SetBool("isleftdead", true);
               }
               else
               {
                  PlayerScript.instance.anim.SetBool("isrightdead", true);
               }

               PlayerScript.instance.isdead = true;
               PlayerScript.instance.gameover.SetActive(true);
               PlayerScript.instance.audio.mute = true;
            } 
         }
         else
         {
            yield return new WaitForSeconds(10);
         }
      }
   }

   IEnumerator minusAccelerator()
   {
      while (true)
      {
         yield return new WaitForSeconds(speedUpTimeValue);
         {
            value += speedUpScaleValue;
         }
      }
   }
}
