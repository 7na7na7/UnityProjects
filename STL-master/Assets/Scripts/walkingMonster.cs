using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walkingMonster : MonoBehaviour
{
    public float speed;
    public float delay;

    private float plus, minus;
    // Start is called before the first frame update
    void Start()
    {
        minus = transform.localScale.x * -1f;
        plus = transform.localScale.x;
        StartCoroutine(walk());
    }

    private void Update()
    {
        transform.Translate(speed*Time.deltaTime,0,0);
    }

    IEnumerator walk()
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            speed *= -1;
            if(speed>0) 
                transform.localScale = new Vector3(minus, transform.localScale.y, transform.localScale.z); //왼쪽으로 뒤집어짐
            else
                transform.localScale = new Vector3(plus, transform.localScale.y, transform.localScale.z); //오른쪽으로 뒤집어짐
        }
    }
}
