using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oniMove : MonoBehaviour
{
    public GameObject effect;
    public float speed;

    void Update()
    {
        transform.Translate(Vector3.right*speed*Time.deltaTime);
    }

    public void die()
    {
        Instantiate(effect,transform.position,Quaternion.identity);
        Destroy(gameObject);
    }
}
