using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackNumberText : MonoBehaviour
{
    void Start()
    {
        transform.SetParent(FindObjectOfType<Canvas>().transform);
    }
    
}
