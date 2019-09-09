using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    //public Transform parent;
    public float waitTime = 10;
    public GameObject spawner;
    void Start()
    {
        Instantiate(spawner, new Vector3(Random.Range(-35, 35), -30, 0),Quaternion.identity); //아래
        Instantiate(spawner, new Vector3(Random.Range(-35, 35), 27, 0),Quaternion.identity); //위
        Instantiate(spawner, new Vector3( 35,Random.Range(-27,30), 0),Quaternion.identity); //오른
        Instantiate(spawner, new Vector3( -35,Random.Range(-27,30), 0),Quaternion.identity); //왼
        StartCoroutine(spawncoroutine());
    }

    IEnumerator spawncoroutine()
    {
        while (true)
        {
            int a = Random.Range(0, 4);
            if (a == 0)
                Instantiate(spawner, new Vector3(Random.Range(-35, 35), -30, 0),Quaternion.identity); //아래
            else  if (a == 1)
                Instantiate(spawner, new Vector3(Random.Range(-35, 35), 27, 0),Quaternion.identity); //위
            else  if (a == 2)
                Instantiate(spawner, new Vector3( 35,Random.Range(-27,30), 0),Quaternion.identity); //오른
            else
                Instantiate(spawner, new Vector3( -35,Random.Range(-27,30), 0),Quaternion.identity); //왼
            //spawner.transform.SetParent(parent.transform);
            yield return new WaitForSeconds(waitTime);
        }
    }

}
