using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class publicScript : MonoBehaviour
{
    public static publicScript instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (SceneManager.GetActiveScene().name == "gameover")
            {
                SceneManager.LoadScene("play");
            }
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (SceneManager.GetActiveScene().name == "gameover")
            {
                SceneManager.LoadScene("title");
            }
        }
    }
}
