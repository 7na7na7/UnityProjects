using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Paddle : MonoBehaviour
{
    public GameObject Ball;
    public Transform BallTr;
    public Rigidbody2D BallRg;
    bool isStart;
    public float paddleX;
    public float ballSpeed;
    float oldBallSpeed = 300;
    float paddleBorder = 2.262f;
    
    private int count;
    public GameObject endPanel;
    
    private void Start() //시작
    {
        StartCoroutine("BallReset");
    }

    private void Update()
    {
        count = 0;
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("block");
        foreach (var VARIABLE in blocks)
        {
            count++;
        }

        if (count <= 0)
        {
            if (!endPanel.activeSelf)
            {
                endPanel.SetActive(true);   
                Destroy(GameObject.Find("Ball0").gameObject);
                Destroy(GameObject.Find("breakText").gameObject);
                   Destroy(gameObject);
            }
        }
    }


    // 볼 위치 초기화하고 0.7초간 깜빡이는 애니메이션 재생
    IEnumerator BallReset()
    {
        BallRg.velocity=Vector2.zero;
        isStart = false;
        Ball.SetActive(true);
        BallTr.position = new Vector2(paddleX, -3.55f);

        StopCoroutine("InfinityLoop");
        yield return new WaitForSeconds(0.1f);
        StartCoroutine("InfinityLoop");
    }

    // 무한 루프
    IEnumerator InfinityLoop()
    {
        while (true)
        {
            // 마우스 누를 때 공이 붙어있음
            if(Input.GetMouseButton(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved))
            {
                paddleX = Mathf.Clamp(Camera.main.ScreenToWorldPoint(Input.GetMouseButton(0) ? Input.mousePosition : (Vector3)Input.GetTouch(0).position).x, -paddleBorder, paddleBorder);
                transform.position = new Vector2(paddleX, transform.position.y);
                if(!isStart) BallTr.position = new Vector2(paddleX, BallTr.position.y);
            }

            // 마우스 떼면 공이 떨어져나감
            if(!isStart && (Input.GetMouseButtonUp(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)))
            {
                isStart = true;
                ballSpeed = oldBallSpeed;
                BallRg.AddForce(new Vector2(0.1f, 0.9f).normalized * ballSpeed);
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    // 볼이 충돌할 때
    public IEnumerator BallCollisionEnter2D(Transform ThisBallTr, Rigidbody2D ThisBallRg, Ball ThisBallCs, GameObject Col, Transform ColTr)
    {
        // 같은 볼끼리 충돌 무시
        Physics2D.IgnoreLayerCollision(2, 2);
        if (!isStart) yield break;

        switch (Col.name)
        {
            // 패들에 부딪히면 차이값만큼 힘 줌
            case "Paddle":
                ThisBallRg.velocity = Vector2.zero;
                ThisBallRg.AddForce((ThisBallTr.position - transform.position).normalized * ballSpeed);
                break;

            // 데스존에 부딪히면 비활성화, 볼 체크
            case "DeathZone":
                StartCoroutine("BallReset");
                break;

            case "Block":
                BlockBreak(Col);
                break;
        }
    }
    

    // 블럭이 부숴질 때
    public void BlockBreak(GameObject Col)
    { 
        Col.gameObject.SetActive(false);
        StopCoroutine("BlockCheck");
        StartCoroutine("BlockCheck");
    }
    
    // 볼에 힘을 줌
    public void BallAddForce(Rigidbody2D ThisBallRg)
    {
        Vector2 dir = ThisBallRg.velocity.normalized;
        ThisBallRg.velocity = Vector2.zero;
        ThisBallRg.AddForce(dir * ballSpeed);
    }

    // 블럭 체크
    IEnumerator BlockCheck()
    {
        yield return new WaitForSeconds(0.5f);
        int blockCount = 0;
    }
}
