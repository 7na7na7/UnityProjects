using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public bool canjump = false;

    
    private void OnCollisionEnter(Collision other)
    {
        canjump = true;
    }
}
