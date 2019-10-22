using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private float mx, my; //마우스 입력값
    private float rx, ry; //카메라 로테이션 값
    
    void Update()
    {
        mx = Input.GetAxis("Mouse X"); //마우스의 X값 받음
        my = Input.GetAxis("Mouse Y"); //마우스의 Y값 받음

        rx += mx*Time.deltaTime*60; //rx에 마우스의 x값 누적
        ry -= my*Time.deltaTime*60; //ry에 마우스의 y값 누적

        ry=Mathf.Clamp(ry, -60, 60); //카메라 각도제한
        transform.eulerAngles = new Vector3(ry, rx, 0);
    }
}
