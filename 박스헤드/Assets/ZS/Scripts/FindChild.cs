using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindChild : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Time.timeScale != 0)
            {
                GameObject child = transform.Find("panel").gameObject;
                child.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                GameObject child = transform.Find("panel").gameObject;
                child.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }
}
