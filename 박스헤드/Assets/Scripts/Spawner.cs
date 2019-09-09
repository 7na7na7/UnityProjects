using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject zombie;
    private float minTime=3;
    private float maxTime=5;
    
    void Start()
    {
        StartCoroutine(spawn());
    }

    IEnumerator spawn()
    {
        while (true)
        {
            Instantiate(zombie,this.transform);
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
        }
    }
}
