using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public bool isSpider = false;
    public bool canSpawn = true;
    public float delayMinusValue=0;
    public float delayMinusDuration=0;
    public bool isFalling;
    public bool isHand = false;
    public float HandSpawnValue = 0;
    public float minDelay,maxDelay;
    public GameObject[] onis;
    void Start()
    {
        if(isSpider)
            StartCoroutine(spawn3());
        else if (isFalling)
            StartCoroutine(spawn2());
        else
            StartCoroutine(spawn());

        if (delayMinusDuration + delayMinusValue > 0)
            StartCoroutine(delayCor());

    }
    IEnumerator spawn3()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
            if (canSpawn)
            {
                float r = Random.Range(GameObject.Find("Min").transform.position.x+3,
                    GameObject.Find("Max").transform.position.x-3);
                Instantiate(onis[Random.Range(0, onis.Length)], new Vector3(r,13.6f,0), Quaternion.identity);
            }
        }
    }
    IEnumerator spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
            if (canSpawn)
            {
                GameObject oni = Instantiate(onis[Random.Range(0, onis.Length)], transform.position,
                    Quaternion.identity);
                if (oni.GetComponent<oniMove>().oniIndex == 2)
                    oni.transform.Translate(0, 1f, 0);

                oni.transform.Translate(0, UnityEngine.Random.Range(-0.2f, 0.2f), 0);
            }
        }
    }
    IEnumerator spawn2()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
            if (canSpawn)
            {
                GameObject oni = null;
               
                if (isHand)
                {
                    float r = Random.Range(Player.instance.transform.position.x-HandSpawnValue*0.1f,
                        Player.instance.transform.position.x + HandSpawnValue);
                    Instantiate(onis[0], new Vector3(r , transform.position.y, 0),
                        Quaternion.identity); 
                }
                else
                {
                    float r = Random.Range(GameObject.Find("Min").transform.position.x+3,
                        GameObject.Find("Max").transform.position.x-3);
                    if (r <= -6.5f)
                        oni = Instantiate(onis[0], new Vector3(r - 6f, transform.position.y, 0),
                            Quaternion.identity); // 인덱스 0이 오른쪽으로 가는오니
                    else if (r > -6.5f)
                        oni = Instantiate(onis[1], new Vector3(r + 6f, transform.position.y, 0),
                            Quaternion.identity); // 인덱스 1이 왼쪽으로 가는오니
                    if (oni.GetComponent<oniMove>().oniIndex == 2)
                        oni.transform.Translate(0, 1f, 0);

                    oni.transform.Translate(0, UnityEngine.Random.Range(-0.2f, 0.2f), 0);
                }
            }
        }
    }
    IEnumerator delayCor()
    {
        while (true)
        {
            yield return new WaitForSeconds(delayMinusDuration);
            if (canSpawn)
            {
                minDelay -= delayMinusValue;
                maxDelay -= delayMinusValue;
            }
        }
    }
}
