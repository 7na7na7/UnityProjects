﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayDestroy : MonoBehaviour
{
    public float delay;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,delay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
