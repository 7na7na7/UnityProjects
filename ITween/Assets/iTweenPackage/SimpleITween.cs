using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleITween : MonoBehaviour
{
    [SerializeField] private Transform targetPos;
    private void Update()
    {
        MoveToExample();
    }

    void MoveToExample()
    {
        //이 오브젝트를 targetPos좌표로 2.5초 안에 이동, easetype형식으로 이동
        if (Input.GetKeyDown(KeyCode.Space))
        {
            iTween.MoveTo(gameObject, iTween.Hash("position",
                targetPos,"time", 2.5f, "easetype", 
                iTween.EaseType.easeInBounce));
        }
    }
}
