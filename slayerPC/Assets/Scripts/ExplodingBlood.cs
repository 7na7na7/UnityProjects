using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingBlood : MonoBehaviour
{
    public float maxScale = 0;
    public float speed = 0;
    public float delay = 0;
    public float colorSpeed;
    void Start()
    {
        StartCoroutine(size());
        transform.eulerAngles=new Vector3(0,0,Random.Range(0,360));
    }
    
    IEnumerator size()
    {
        Color color = Color.white;
        int r = Random.Range(0, 2);
        int s = Random.Range(3, 11);
        while (transform.localScale.x<maxScale)
        {
            transform.localScale=new Vector3(transform.localScale.x+speed,transform.localScale.y+speed,1);
            if(r==0)
                transform.eulerAngles=new Vector3(0,0,transform.eulerAngles.z+speed*s);
            else
                transform.eulerAngles=new Vector3(0,0,transform.eulerAngles.z-speed*s);


            color = GetComponent<SpriteRenderer>().color;
            color.a -= colorSpeed * Time.deltaTime;
            GetComponent<SpriteRenderer>().color = color;
            yield return new WaitForSeconds(delay);
        }
        Destroy(gameObject);
    }
}
