using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Slider : MonoBehaviour
{
    private Transform parent;
    private UnityEngine.UI.Slider hp;
    // Start is called before the first frame update
    void Start()
    {
        hp = this.GetComponent<UnityEngine.UI.Slider>();
        parent = GameObject.Find("HPcanvas").GetComponent<Transform>();
        transform.SetParent(parent);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hp.value <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }
}
