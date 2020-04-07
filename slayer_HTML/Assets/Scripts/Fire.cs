using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public bool canSpawn = true;
    public float delayMinusValue=0;
    public float delayMinusDuration=0;
    public bool isRight;
    public float minDelay, maxDelay;
    public GameObject goPrefab = null; //포물선으로 날아갈 화살의 프리팹
    public Transform ShotTr; //화살이 생성될 위치
    public float minForce;
    public float maxForce;
    void Start()
    {
        StartCoroutine(spawn());
        if (delayMinusDuration + delayMinusValue > 0)
            StartCoroutine(delayCor());
    }

    IEnumerator spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
            if (canSpawn)
            {
                Vector2 direction = Vector2.zero;
                if (isRight)
                    direction = new Vector2(1, Random.Range(0f, 1f));
                else
                    direction = new Vector2(-1, Random.Range(0f, 1f));
                GameObject arrow =
                    Instantiate(goPrefab, ShotTr.position, ShotTr.rotation); //ArrowTr의 위치와 회전값에 화살 프리팹 생성

                arrow.transform.right = direction; //오른쪽 벡터를 direction으로 바꿈
                arrow.GetComponent<Rigidbody2D>().velocity =
                    arrow.transform.right * Random.Range(minForce, maxForce); //가속도 오른쪽으로 줌   
            }
        }
    }

    IEnumerator delayCor()
    {
        while (true)
        {
            yield return new WaitForSeconds(delayMinusDuration);
            if (canSpawn)
            {
                minDelay -= delayMinusValue;
                maxDelay -= delayMinusValue; 
            }
        }
    }
}
