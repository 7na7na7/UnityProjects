using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Image = UnityEngine.UI.Image;


public class fade : MonoBehaviour
{
   public bool isDream=false;
   public float speed;
   private Image img;
   private Color fadecolor;

   private void Start()
   {
      img = GetComponent<Image>();
   }

   public void Dream()
   {
      StopAllCoroutines();
      StartCoroutine(dreamCor());
   }

   IEnumerator dreamCor()
   {
      fadeinNow();
      yield return new WaitForSeconds(1.25f);
      fadeoutNow();
   }
   public void  fadeinNow()
   {
      fadecolor = img.color;
      fadecolor.a = 1;
      img.color = fadecolor;
   }

   public void fadeoutNow()
   {
      fadecolor = img.color;
      fadecolor.a = 0;
      img.color = fadecolor;
   }
   public IEnumerator fadeIn()
       {
          if(!isDream)
          {
          StopAllCoroutines();
          if (img.color.a < 0.5f)
          {
             while (true)
             {
                fadecolor = img.color;
                fadecolor.a += 0.1f;
                yield return new WaitForSeconds(speed);
                img.color = fadecolor;
                if (img.color.a >= 0.5f)
                   break;
             }
          }
          }
       }
   public IEnumerator fadeInRealTime()
   {
      if (!isDream)
      {
         StopAllCoroutines();
         if (img.color.a < 0.5f)
         {
            while (true)
            {
               fadecolor = img.color;
               fadecolor.a += 0.1f;
               yield return new WaitForSecondsRealtime(speed);
               img.color = fadecolor;
               if (img.color.a >= 0.5f)
                  break;
            }

            FindObjectOfType<GameOverManager>().panel.SetActive(true);
         }
      }
   }

   public IEnumerator fadeout()
   {
      if (!isDream)
      {
         StopAllCoroutines();
         if (img.color.a > 0)
         {
            while (true)
            {
               fadecolor = img.color;
               fadecolor.a -= 0.1f;
               yield return new WaitForSeconds(speed);
               img.color = fadecolor;
               if (img.color.a <= 0)
                  break;
            }
         }
      }
   }

   public IEnumerator fadeoutRealTime()
   {
      if(!isDream)
      {
      StopAllCoroutines();
      if (img.color.a > 0)
      {
         while (true)
         {
            fadecolor = img.color;
            fadecolor.a -= 0.1f;
            yield return new WaitForSecondsRealtime(speed);
            img.color = fadecolor;
            if (img.color.a <= 0)
               break;
         }
      }
      }
   }
}
