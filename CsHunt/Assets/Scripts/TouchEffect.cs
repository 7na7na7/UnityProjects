using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TouchEffect : MonoBehaviour
{
    public float fadeTime=0.2f;

    void Start()
    {
        GetComponent<SpriteRenderer>().DOColor(Color.clear, fadeTime).OnComplete(()=>Destroy(gameObject));
    }
}
