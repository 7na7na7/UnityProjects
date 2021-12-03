using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isDeskTop;
    void Update()


    {
        if (isDeskTop) //PC
        {
if(Input.GetMouseButtonDown(0))
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.position = pos;
            }
        }
        else //¸ð¹ÙÀÏ
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    Vector2 pos = Camera.main.ScreenToWorldPoint(touch.position);
                    transform.position = pos;
                }
            }
        } 
    }
}
