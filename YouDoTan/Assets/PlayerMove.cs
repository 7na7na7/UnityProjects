using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed;
    void Update()
    {
        transform.Translate(Input.GetAxisRaw("Horizontal")*Time.deltaTime*speed,Input.GetAxisRaw("Vertical")*Time.deltaTime*speed,0);
    }
}
