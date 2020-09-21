using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletMonster : MonoBehaviour
{
    private bool isshot = false;
    public GameObject bullet;
    public float delay;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(shot());
    }

    IEnumerator shot()
    {
        while (true)
        {
            if (isshot)
            {
                Instantiate(bullet, transform);
            }
            yield return new WaitForSeconds(delay);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            isshot = true;
    }
}
