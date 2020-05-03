using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touchEffect : MonoBehaviour
{
    public float delay;
    public float sizeUp;
    private float time = 0;
    Color color;
    void Start()
    {
        color = GetComponent<SpriteRenderer>().color;
        StartCoroutine(cor());
    }

    IEnumerator cor()
    {
        while (time<0.3f)
        {
            transform.localScale+=Vector3.one*Time.deltaTime*sizeUp;
            color.a -= delay*4;
            GetComponent<SpriteRenderer>().color = color;
            yield return new WaitForSeconds(delay);
            time += delay;
        }
        Destroy(gameObject);
    }
}
