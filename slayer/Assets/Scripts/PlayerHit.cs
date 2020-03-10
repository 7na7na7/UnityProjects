using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.CompareTag("oni"))
        {
            Time.timeScale = 0;
        }
    }
}
