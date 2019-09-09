using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detect : MonoBehaviour
{
    public GameObject bullet;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.transform.parent.GetComponent<zombie2>().isDetected = true;
            StartCoroutine(shot());
        }
    }
    IEnumerator shot()
    {
        while (true)
        {
            Instantiate(bullet,this.transform);
            yield return new WaitForSeconds(1.5f);
        }
    }
}
