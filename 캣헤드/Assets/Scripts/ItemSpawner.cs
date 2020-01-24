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
        while (true)
        {
            yield return new WaitForSeconds(delay);
            Instantiate(item,transform.position+new Vector3(Random.Range(-8f,8f),Random.Range(-3f,3f),0),Quaternion.identity);
        }
    }
}
