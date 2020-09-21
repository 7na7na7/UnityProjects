using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossmove : MonoBehaviour
{
    public float shakespeed;
    private bool istop;
    public float delay,delay1,delay2;
    private bool isboss = false;

    void Start()
    {
        StartCoroutine(topshake());
    }
    void Update()
    {
        if(istop) 
            transform.Translate(0,shakespeed*Time.deltaTime,0);
    }
    

    IEnumerator topshake()
    {
        while (true)
        {
            float top=Random.Range(0.0f, delay);
            istop = true;
            yield return new WaitForSeconds(top);
            shakespeed *= -1;
            yield return new WaitForSeconds(top);
            istop = false;
            yield return new WaitForSeconds(Random.Range(delay1,delay2));
        }
    }
}
