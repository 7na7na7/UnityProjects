using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gyutaroAttack : MonoBehaviour
{
    public bool isRight = true;
    public float speed;

    private void Start()
    {
        if(isRight)
            Destroy(transform.parent.gameObject,5f);
    }

    void Update()
    {
        transform.Translate(speed*Time.deltaTime*(isRight==true?1:-1),0,0);
    }
}
