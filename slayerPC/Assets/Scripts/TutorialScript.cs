using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.SceneManagement;

public class TutorialScript : MonoBehaviour
{
    private int currentPage = 0;
    public GameObject[] pages;

    private void Update()
    {
        for (int i = 0; i < pages.Length; i++)
        {
            if(i==currentPage)
                pages[i].SetActive(true);
            else
                pages[i].SetActive(false);
        }
        if(Input.GetKeyDown(KeyCode.Escape))
            Title();
    }

    public void right()
    {
        SoundManager.instance.select();
        if (currentPage + 1 != pages.Length)
            currentPage++;
        else
            currentPage = 0;
    }

    public void left()
    {
        SoundManager.instance.select();
        if (currentPage != 0)
            currentPage--;
        else
            currentPage = pages.Length - 1;
    }

    public void Title()
    {
        SoundManager.instance.select();
        SceneManager.LoadScene("Title");
    }
}
