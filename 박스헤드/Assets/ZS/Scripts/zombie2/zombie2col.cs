using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombie2col : MonoBehaviour
{
    private GameObject parent;
    private bool isshot = false;
    public GameObject bullet;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (isshot == false)
            {
                parent = transform.parent.gameObject;
                parent.transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("isstop",true);
                gameObject.transform.parent.GetComponent<zombie2move>().canmove = false;
                StartCoroutine(shot());
            }
        }
    }
    IEnumerator shot()
    {
        isshot = true;
        while (true)
        {
            Instantiate(bullet,this.transform);
            yield return new WaitForSeconds(1.5f);
        }
    }
}
