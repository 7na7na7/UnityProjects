using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Player"))
    {
      Enemy[] e = FindObjectsOfType<Enemy>();
      if (e.Length <= 0)
      {
        switch (SceneManager.GetActiveScene().name)
        {
          case "stage1":
            SceneManager.LoadScene("stage2");
            break;
          case "stage2":
            SceneManager.LoadScene("StageSelect");
            break;
        }
      }
    }
  }
}
