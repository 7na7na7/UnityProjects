using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawner : MonoBehaviour
{
    public GameObject minion;
    public float delay;
    public int startEntity;
    void Start()
    {
        for(int i=0;i<startEntity;i++)
        {
            Instantiate(minion, transform.position + new Vector3(Random.Range(-14, 14), Random.Range(-10, 10)), Quaternion.identity);
        }
        StartCoroutine(spawnCor());
    }
    
    IEnumerator spawnCor()
    {
        while(true)
        {
            yield return new WaitForSeconds(delay);
            Instantiate(minion, transform.position + new Vector3(Random.Range(-14,14),Random.Range(-10,10)), Quaternion.identity);
        }
    }
}
