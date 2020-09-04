using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSetFalse : MonoBehaviour
{
    public static BulletSetFalse instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetFalse()
    {
        Bullet[] bullets = FindObjectsOfType<Bullet>();
        foreach (var bullet in bullets)
        {
            bullet.SetFalse();
        }
        goldCol[] golds = FindObjectsOfType<goldCol>();
        foreach (var bullet in golds)
        {
            bullet.SetFalse();
        }
    }
}
