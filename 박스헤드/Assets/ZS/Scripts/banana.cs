using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class banana : MonoBehaviour
{
    private int snipercount = 0;
    private GameObject obj;
    private Transform parent;
    public float speed;

    private void Start()
    {
        obj = this.gameObject;
        Destroy(obj, 1f);
            parent = GameObject.Find("BG").GetComponent<Transform>();
        this.transform.SetParent(parent.transform);//child의 부모를 parent로 설정
    }
    void Update()
    {
        transform.Translate(Vector3.right*Time.deltaTime*speed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")&&!other.CompareTag("object"))
        {
            bananagun gun = FindObjectOfType<bananagun>();
            if (gun.weapon != "sniper")
            {
                if (!other.CompareTag("banana"))
                {
                    if(other.CompareTag("zombie")||other.CompareTag("zombieking")) 
                        Destroy(obj);
                }
            }
            snipercount++;
            if(snipercount>=5)
                Destroy(obj);
                
        }
    }
}
