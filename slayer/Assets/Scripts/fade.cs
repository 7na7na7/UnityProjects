using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Image = UnityEngine.UI.Image;


public class fade : MonoBehaviour
{
   public float speed;
   private Image img;
   private Color fadecolor;

   private void Start()
   {
      img = GetComponent<Image>();
   }

   public IEnumerator fadeIn()
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
   public IEnumerator fadeInRealTime()
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
   public IEnumerator fadeout()
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
   public IEnumerator fadeoutRealTime()
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
