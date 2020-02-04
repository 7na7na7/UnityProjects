using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject item;
    public float delay;
    void Start()
    {
        StartCoroutine(ItemSpawn());
    }

    IEnumerator ItemSpawn()
    {
        Instantiate(item,transform.position,Quaternion.identity);
        while (true)
        {
            yield return new WaitForSeconds(delay);
            Instantiate(item,transform.position,Quaternion.identity);
        }
    }
}
