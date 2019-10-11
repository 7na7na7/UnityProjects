using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NickName : MonoBehaviour
{
    private NetworkManager net;

    private void Start()
    {
        net = FindObjectOfType<NetworkManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            net.Connect();
        }
    }
}
