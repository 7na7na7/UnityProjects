using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSelect : MonoBehaviour
{
    GameObject[] Colors;

    private void Awake()
    {
        Colors = FindObjectOfType<BulletData>().Colors;
    }

    void Start()
    {
        foreach (var color in Colors)
        {
            Instantiate(color,transform);
        }
    }
}
