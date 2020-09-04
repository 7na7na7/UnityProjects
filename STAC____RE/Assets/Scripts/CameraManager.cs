using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour
{
    public GameObject Clock;
    public GameObject gameoverPanel;
    public static CameraManager instance;
    public GameObject player_GO; //플레이어 게임오브젝트

    public Transform lastTr;
    private float savedOrthoSize;

    public float CameraSizeUpValue;
    public float CameraSizeUpTime;
    public float MaxSize;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        //if (Screen.orientation == ScreenOrientation.Portrait) //세로면
        //    Camera.main.orthographicSize *= 1.5f;

        if(SceneManager.GetActiveScene().name=="Play") 
            StartCoroutine(sizeUp());
    }

    public void GameOver()
    {
        StartCoroutine(targetChange());
    }
    public IEnumerator targetChange()
    {
        BGM.instance.fadeOut();
        Time.timeScale = 0.3f;
        float size = Camera.main.orthographicSize/2;
        while (Camera.main.orthographicSize > size)
        {
            Camera.main.orthographicSize -= 0.1f;
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSecondsRealtime(0.5f);
        
        Time.timeScale = 1;
        Fade.instance.fade();
        if (GameManager.instance.canRevival)
        {
            GameManager.instance.canRevival = false;
            Clock.SetActive(true);
        }
        else
        {
            gameoverPanel.SetActive(true);
        }
    }

    public void Revival()
    {
        BGM.instance.fadeIn();
        BulletSetFalse.instance.SetFalse();
        StartCoroutine(targetChange2());
        Fade.instance.Unfade();
        GameObject playerGO=Instantiate(player_GO, lastTr);
        FindObjectOfType<Tile>().transform.position = playerGO.transform.position;
        gameoverPanel.SetActive(false);
        Clock.SetActive(false);
        FindObjectOfType<joystick>().go_Player = playerGO;
        transform.position = new Vector3(playerGO.transform.position.x,playerGO.transform.position.y,transform.position.z);
    }
    
    public IEnumerator targetChange2()
    {
        Time.timeScale = 1;
        float size = savedOrthoSize;
        while (Camera.main.orthographicSize < size)
        {
            Camera.main.orthographicSize += 0.05f;
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator sizeUp()
    {
        while (true)
        {
            yield return new WaitForSeconds(CameraSizeUpTime);
            if(Camera.main.orthographicSize<MaxSize) 
                Camera.main.orthographicSize += CameraSizeUpValue;
        }
    }

    public void SetSavedOrthographic()
    {
        savedOrthoSize = Camera.main.orthographicSize;
    }
}
