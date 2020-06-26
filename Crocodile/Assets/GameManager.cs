using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clip;
    public AudioClip clip2;
    public Sprite pressedTooth;
    public Sprite pressedCrocodile;
    public Image crocodile;
    public GameObject[] toothImages;
    public bool[] tooths;
    public GameObject CrocodileText;
    public GameObject GameOverPanel;
    public void Set(int correctTooth)
    {
        for (int i = 0; i < tooths.Length; i++)
        {
            tooths[i] = false;
        }

        int a = 0;
        int r = 0;
        while (a<correctTooth)
        {
          
            while (true) 
            {
                r = Random.Range(0, tooths.Length);
                if (tooths[r] != true)
                    break;
            }
          
            tooths[r] = true;
            a++;
        }
    }


   public void Press(int value)
   {
       toothImages[value].GetComponent<Image>().sprite = pressedTooth;
       source.PlayOneShot(clip2);
       if (tooths[value] == true)
       {
           foreach (GameObject tooth in toothImages)
           {
               tooth.SetActive(false);
           }
           crocodile.sprite = pressedCrocodile;
           CrocodileText.SetActive(true);
           source.PlayOneShot(clip);
           StartCoroutine(delay());
       }
   }

   IEnumerator delay()
   {
       yield return new WaitForSeconds(1f);
       GameOverPanel.SetActive(true);
   }
   public void Exit()
   {
       Application.Quit();
   }

   public void Restart()
   {
       SceneManager.LoadScene(SceneManager.GetActiveScene().name);
   }
}
