using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject pausePanel;
    public void pause()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1; 
            BGM.instance.GetComponent<AudioSource>().UnPause();
            pausePanel.SetActive(false);
          
        }
        else
        {
            Time.timeScale = 0; 
            BGM.instance.GetComponent<AudioSource>().Pause();
            pausePanel.SetActive(true);
        }
    }

    public void RealPause()
    {
        Time.timeScale = 0; 
        BGM.instance.GetComponent<AudioSource>().Pause();
        pausePanel.SetActive(true);
    }  
    public void RealUnPause()
    {
        Time.timeScale = 1; 
        BGM.instance.GetComponent<AudioSource>().UnPause();
        pausePanel.SetActive(false);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            pause();
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0; 
            BGM.instance.GetComponent<AudioSource>().Pause();
            pausePanel.SetActive(true);
        }
    }
}
