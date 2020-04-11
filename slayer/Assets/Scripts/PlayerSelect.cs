using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelect : MonoBehaviour
{
    public GameObject zenichu, tanjiro, inoskae;
    private void Awake()
    {
        Time.timeScale = 0;
    }

    public void Tanjiro()
    {
        Instantiate(tanjiro);
        Time.timeScale = 1;
        CameraManager.instance.GameStart();
        Destroy(gameObject);
    }

    public void Zenichu()
    {
        Instantiate(zenichu);
        Time.timeScale = 1;
        CameraManager.instance.GameStart();
        Destroy(gameObject);
    }

    public void Inoskae()
    {
        Instantiate(inoskae);
        Time.timeScale = 1;
        CameraManager.instance.GameStart();
        Destroy(gameObject);
    }
}
