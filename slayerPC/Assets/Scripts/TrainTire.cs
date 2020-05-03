using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainTire : MonoBehaviour
{
    public float speed = 0.1f;
    public float delay = 0.1f;
    private Vector2 min, max;
    void Start()
    {
       min = new Vector2(transform.position.x -0.5f, transform.position.y);
       max = new Vector2(transform.position.x + 0.5f, transform.position.y);
       StartCoroutine(repeat());
    }

    IEnumerator repeat()
    {
        while (true)
        {
            while (true)
            {
                if (transform.position.x < max.x)
                {
                    transform.Translate(speed,0,0);
                    yield return new WaitForSeconds(delay);
                }
                else
                {
                    break;
                }
            }

            while (true)
            { 
                if (transform.position.x > min.x) 
                { 
                    transform.Translate(-1*speed,0,0); 
                    yield return new WaitForSeconds(delay);
                }
                else
                {
                    break;
                }
            }   
        }
    }
}
