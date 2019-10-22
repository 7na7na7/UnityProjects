using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objRotate : MonoBehaviour
{
    private float mx;
    private float rx;
    
    void Update()
    {
        mx = Input.GetAxis("Mouse X"); //마우스의 X값 받음

        rx += mx*Time.deltaTime*60; //rx에 마우스의 x값 누적
        
        transform.eulerAngles = new Vector3(0, rx, 0);
    }
}
