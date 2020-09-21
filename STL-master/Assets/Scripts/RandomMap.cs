using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMap : MonoBehaviour
{
    public float randomy, randomx;
    void Start()
    {
        transform.Translate(Random.Range(randomx*-1,randomx),Random.Range(randomy*-1,randomy),0);
    }


}
