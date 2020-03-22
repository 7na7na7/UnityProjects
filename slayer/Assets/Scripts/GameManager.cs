using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
  public int bossCount = 0;
  public GameObject boss;
  public int bossTime;
  public int jumpingTIme;
  public int fallingTime;
  public int beforeBossTime;
  public GameObject pausePanel;
  public Sprite pauseSprite, goSprite;
  public Button pauseBtn;
  public bool isGameOver=false;
  public bool bossDead = false;
  private Spawner[] spawners;
  private Fire[] fires;
  public GameObject txt;
  public bool isPause = false;
  public static GameManager instance;
  private void Awake()
  {
    instance = this;
    Time.timeScale = 1;
  }

  private void Start()
  {
    FadePanel.instance.UnFade();
    spawners = FindObjectsOfType<Spawner>();
    fires = FindObjectsOfType<Fire>();
    StartCoroutine(Game());
  }

  IEnumerator Game()
  { yield return new WaitForSeconds(jumpingTIme);
    foreach (Fire f in fires)
    {
      f.canSpawn = true;
    }
    yield return new WaitForSeconds(fallingTime);
    foreach (Spawner s in spawners)
    {
      if (s.isFalling)
        s.canSpawn = true;
    }

    while (true)
    {
      yield return new WaitForSeconds(bossTime);
      foreach (Spawner s in spawners)
      {
        s.canSpawn = false;
      }
      foreach (Fire f in fires)
      { 
        f.canSpawn = false;
      }

      GameObject t=Instantiate(txt, GameObject.Find("bossTextTr").transform);
      yield return new WaitForSeconds(beforeBossTime);
      GameObject bossObj=Instantiate(boss,new Vector3(Random.Range(GameObject.Find("Min").transform.position.x+10,GameObject.Find("Max").transform.position.x-10),transform.position.y,0),Quaternion.identity);
      yield return new WaitUntil(() => bossDead);
      bossDead = false;
      foreach (Spawner s in spawners)
      {
        s.canSpawn = true;
      }
      foreach (Fire f in fires)
      { 
        f.canSpawn = true;
      } 
    }
  }

  public void pause()
  {
    if (!isGameOver)
    {
      if (isPause)
      {
        if (Time.timeScale == 0)
        {
          GameObject.Find("BGM").GetComponent<AudioSource>().UnPause();
          pausePanel.SetActive(false);
          Time.timeScale = 1;
          pauseBtn.GetComponent<Image>().sprite = goSprite;
          isPause = false;
        }
      }
      else //일시정지
      {
        if (Time.timeScale == 1)
        {
          GameObject.Find("BGM").GetComponent<AudioSource>().Pause();
          pausePanel.SetActive(true);
          Time.timeScale = 0;
          pauseBtn.GetComponent<Image>().sprite = pauseSprite;
          isPause = true;
        }
      }
    }
  }

  public void RESTART()
  {
    FadePanel.instance.rightFade();
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }

  public void TITLE()
  {
    FadePanel.instance.rightFade();
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
