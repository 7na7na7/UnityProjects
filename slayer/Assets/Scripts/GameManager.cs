using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
  public GameObject boss;
  public int bossTime;
  public GameObject pausePanel;
  public Sprite pauseSprite, goSprite;
  public Button pauseBtn;
  public bool isGameOver=false;
  private Spawner[] spawners;
  private Fire[] fires;

  private void Awake()
  {
    Time.timeScale = 1;
  }

  private void Start()
  {
    spawners = FindObjectsOfType<Spawner>();
    fires = FindObjectsOfType<Fire>();
    StartCoroutine(Game());
  }

  IEnumerator Game()
  {
    yield return new WaitForSeconds(bossTime);
    /*
    foreach (Spawner s in spawners)
    {
      s.canSpawn = false;
    }
    foreach (Fire f in fires)
    {
      f.canSpawn = false;
    }
    */

    GameObject bossObj=Instantiate(boss,new Vector3(Random.Range(GameObject.Find("Min").transform.position.x,GameObject.Find("Max").transform.position.x),transform.position.y,0),Quaternion.identity);
    //yield return new WaitUntil(()=>bossObj.GetComponent<>())
  }

  public void pause()
  {
    if (!isGameOver)
    {
      if (Time.timeScale == 0) //재개
      {
        GameObject.Find("BGM").GetComponent<AudioSource>().UnPause();
        pausePanel.SetActive(false);
        if (ComboManager.instance.comboCount >= 2)
          Time.timeScale = 0.7f;
        else
          Time.timeScale = 1;
        pauseBtn.GetComponent<Image>().sprite = goSprite;
      }
      else //일시정지
      {
        GameObject.Find("BGM").GetComponent<AudioSource>().Pause();
        pausePanel.SetActive(true);
        Time.timeScale = 0;
        pauseBtn.GetComponent<Image>().sprite = pauseSprite;
      }
    }
  }

  public void RESTART()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }

  public void TITLE()
  {
    SceneManager.LoadScene("Title");
  }
  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      pause();
    }
  }
}
