using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fishspawn : MonoBehaviour
{
    public GameObject fishobj;

    public float delay;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(fishmove());
    }

    IEnumerator fishmove()
    {
        while (true)
        {
            Instantiate(fishobj,transform);
            yield return new WaitForSeconds(delay);
        }
    }
}
