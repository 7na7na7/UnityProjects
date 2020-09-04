using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleTriangleRotate : MonoBehaviour
{
    public float speed;
    private RectTransform rect;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        rect.eulerAngles=new Vector3(rect.eulerAngles.x,rect.eulerAngles.y,rect.eulerAngles.z+Time.deltaTime*speed);
    }
}
