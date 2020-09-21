using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ashposition : MonoBehaviour
{
    public GameObject camera;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(camera.transform.position.x + 11f, transform.position.y, transform.position.z);
    }
}
