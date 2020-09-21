using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pattern : MonoBehaviour
{
    private int a;
    public GameObject[] patterns;
    // Start is called before the first frame update
    void Start()
    {
        if (transform.position.x < 130)
        {
            a = Random.Range(0, 5);
        }
        else if (transform.position.x < 300)
        {
            a = Random.Range(0, 12);
        }
        else
        {
            a = Random.Range(5, 12);
        }
        Instantiate(patterns[a],transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
