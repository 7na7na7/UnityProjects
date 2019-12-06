using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float speed;
    public Transform target;
    void Update()
    {
        transform.position=Vector3.Lerp(transform.position, target.transform.position, speed * Time.deltaTime);
        transform.position=new Vector3(transform.position.x,transform.position.y,-10);
    }
}
