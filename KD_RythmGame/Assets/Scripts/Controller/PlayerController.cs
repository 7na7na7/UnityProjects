using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3;
    Vector3 dir=new Vector3();
    Vector3 destPos=new Vector3();
    private TimingManager theTimingManager;

    private void Start()
    {
        theTimingManager = FindObjectOfType<TimingManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)||Input.GetKeyDown(KeyCode.S)||Input.GetKeyDown(KeyCode.D)||Input.GetKeyDown(KeyCode.W))
        {
            if (theTimingManager.CheckTiming())
            {
                StartAction();
            }
        }
    }

    void StartAction()
    {
        //방향 계산
        dir.Set(Input.GetAxisRaw("Vertical"),0,Input.GetAxisRaw("Horizontal"));
        
        //이동 목표값 계산
        destPos = transform.position + new Vector3(-dir.x, 0, dir.z);
        StartCoroutine(MoveCo());
    }

    IEnumerator MoveCo()
    {
        while (Vector3.SqrMagnitude(transform.position - destPos) >0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = destPos;
    }
}
