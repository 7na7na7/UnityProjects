using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class girlChat : MonoBehaviour
{
    public GameObject chatBalloon;
    public float minDelay, maxDelay;
    public GameObject canvas;
    void Start()
    {
        StartCoroutine(chatCor());
    }
    IEnumerator chatCor()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minDelay,maxDelay));
            Instantiate(chatBalloon, canvas.transform);
            GetComponent<Animator>().Play("girlAnim");
        }
    }
}
