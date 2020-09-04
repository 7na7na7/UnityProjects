using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleBg : MonoBehaviour
{
    private Material[] mats;

    private void Awake()
    {
        mats = FindObjectOfType<BulletData>().Themes;
    }

    void Update()
    {
        if (GetComponent<MeshRenderer>().material.ToString().Substring(0, 2) != mats[BulletData.instance.currentColorIndex].ToString().Substring(0, 2))
        {
            GetComponent<MeshRenderer>().material = mats[BulletData.instance.currentColorIndex];
        }
    }
}
