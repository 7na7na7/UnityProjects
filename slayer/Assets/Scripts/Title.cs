using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
   public Transform tr;
   public GameObject cover;
   public GameObject btn;
   public float speed;
   public float delay;
   public float anotherDelay;
   private void Start()
   {
      Time.timeScale = 1;
      FindObjectOfType<FadePanel>().UnFade();
   }

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.Escape))
      {
         Application.Quit();
      }
   }

   public void main()
   {
      StartCoroutine(mainCor());
   }

   public void title()
   {
      SceneManager.LoadScene("Title");
   }

   IEnumerator mainCor()
   {
      btn.SetActive(false);
      SoundManager.instance.knifeCover();
      FadePanel.instance.Fade();
      while (cover.transform.position.x<=tr.transform.position.x)
      {
         cover.transform.Translate(Vector3.right*speed);
         yield return new WaitForSeconds(delay);
      }
yield return new WaitForSeconds(anotherDelay);
      SceneManager.LoadScene("Stage");
   }
}
