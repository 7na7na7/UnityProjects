using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public bool isLerp = true;
    Transform target;
    private void Start()
    {
        target = Player.instance.transform;
    }
    private void LateUpdate()
    {
        if(target!=null)
        {
            if (isLerp)
                transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), 5f * Time.deltaTime);
            else
                transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
        }
    }
}
