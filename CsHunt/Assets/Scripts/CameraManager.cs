using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    Transform target;
    private void Start()
    {
        target = Player.instance.transform;
    }
    private void LateUpdate()
    {
        if(target!=null)
        {
            transform.position = Vector3.Lerp(transform.position,new Vector3(target.position.x, target.position.y, transform.position.z),5f*Time.deltaTime);
        }
    }
}
