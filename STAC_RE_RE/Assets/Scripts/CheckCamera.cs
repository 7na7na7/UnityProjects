using System;
using UnityEngine;
using System.Collections;

public class CheckCamera : MonoBehaviour
{
    public static CheckCamera instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    
    public bool CheckObjectIsInCamera(GameObject _target)
    {
        Camera selectedCamera = Camera.main;
        Vector3 screenPoint = selectedCamera.WorldToViewportPoint(_target.transform.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

        return onScreen;
    }
}