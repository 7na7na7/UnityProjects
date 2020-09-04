using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : MonoBehaviour
{
    private void Start()
    {
        BulletSetFalse.instance.SetFalse();
        Time.timeScale = 1;
    }

    public void ChangeScene(string SceneName)
    {
        if (SceneName == "this")
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        else
        {
            if(SceneName=="Play")
                FindObjectOfType<Fade>().fade();
            else
                SceneManager.LoadScene(SceneName);   
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name == "Title")
            {
                Application.Quit();
            }
        }
    }
}
