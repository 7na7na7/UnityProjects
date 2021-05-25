using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeColor : MonoBehaviour
{
  public GameObject go;
  public Text ramain;
  public Image y, b, r;
  private Color color = new Color(0.5f,0.5f,0.5f);
  private void Update()
  {
    
    if (Input.anyKeyDown)
    {
      if (FindObjectOfType<Playercontrol>() == null)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    Enemy[] enemies = FindObjectsOfType<Enemy>();
    ramain.text = enemies.Length.ToString();
    if(enemies.Length<=0)
      go.SetActive(true);
    if (Input.GetKeyDown(KeyCode.Alpha1))
    {
      y.color=Color.white;
      b.color = color;
      r.color = color;
    }
    if (Input.GetKeyDown(KeyCode.Alpha2))
    {
      b.color=Color.white;
      y.color = color;
      r.color = color;
    }
    if (Input.GetKeyDown(KeyCode.Alpha3))
    {
      r.color=Color.white;
      b.color = color;
      y.color = color;
    }
  }
}
