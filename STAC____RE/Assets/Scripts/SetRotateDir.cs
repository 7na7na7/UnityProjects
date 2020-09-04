using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetRotateDir : MonoBehaviour
{
    public GameObject leftDark, rightDark;

    private void Start()
    {
        if(BulletData.instance.isRight==1)
            rightDark.SetActive(true);
        else
            leftDark.SetActive(true);
    }

    public void left()
    {
        leftDark.SetActive(true);
        rightDark.SetActive(false);
        BulletData.instance.left();
    }

    public void right()
    {
        leftDark.SetActive(false);
        rightDark.SetActive(true);
        BulletData.instance.right();
    }
}
