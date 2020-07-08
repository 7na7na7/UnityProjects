using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    void Update()
    {
        transform.Translate(Input.GetAxisRaw("Horizontal")*5*Time.deltaTime,Input.GetAxisRaw("Vertical")*5*Time.deltaTime,0);
    }
}
