using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Rotation : MonoBehaviour
{
    public float rotateSpeed = 5f;
    private float leftSpeed=0, rightSpeed=0, downSpeed=0, upSpeed=0, forwardSpeed=0, backSpeed = 0;
    
    private void Start()
    {
        int r = Random.Range(0, 3);

        if (r == 0) //아래 오른쪽으로 회전
        {
            leftSpeed = rotateSpeed; //앞에서 아래로회전     
            upSpeed = rotateSpeed; //앞에서 왼쪽회전
        }
        else if (r == 1)
        {
            forwardSpeed = rotateSpeed; //위에서 왼쪽회전
            rightSpeed = rotateSpeed; //앞에서 위로회전   
        }
        else
        {
            downSpeed = rotateSpeed; //앞에서 오른쪽회전
            backSpeed = rotateSpeed; //위에서 오른쪽회전   
        }
    }

    void Update()
    {
        transform.Rotate(Vector3.left * Time.deltaTime * leftSpeed);
        transform.Rotate(Vector3.right * Time.deltaTime * rightSpeed);
        transform.Rotate(Vector3.down * Time.deltaTime * downSpeed);
        transform.Rotate(Vector3.up * Time.deltaTime * upSpeed); 
        transform.Rotate(Vector3.forward * Time.deltaTime * forwardSpeed);
        transform.Rotate(Vector3.back * Time.deltaTime * backSpeed);
    }
}
