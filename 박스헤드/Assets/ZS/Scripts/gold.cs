using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gold : MonoBehaviour
{
    private GameObject obj;
    private void Start()
    {
        obj = this.gameObject;
        Destroy(this.gameObject,20f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GoldManager gold = FindObjectOfType<GoldManager>();
            if (this.CompareTag("brown"))
            {
                gold.gold++;
                gold.realgold++;
            }
            else if (this.CompareTag("silver"))
            {
                gold.gold += 2;
                gold.realgold += 2;
            }
            else if (this.CompareTag("gold"))
            {
                gold.gold += 3;
                gold.realgold += 3;
            }
            Destroy(obj);
        }
    }
}
