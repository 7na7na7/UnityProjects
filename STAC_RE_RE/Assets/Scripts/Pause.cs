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
}
