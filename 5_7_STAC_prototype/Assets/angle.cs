using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class angle : MonoBehaviour
{
    private bool isRotation = false;
    
    public int duration;
    public float delay;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartCoroutine(right());
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StartCoroutine(left());
        }
    }

    IEnumerator left()
    {
        if (!isRotation)
        {
            isRotation = true;
           int a = (int)transform.eulerAngles.z + 120;
           if (a >= 360)
               a -= 360;
           if (transform.eulerAngles.z < a)
           {
               print("1");
               while (transform.eulerAngles.z < a)
               {
                   transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + duration);
                   yield return new WaitForSeconds(delay);
               }
               isRotation = false;
           }
           else
           {
               print("2");
               while (transform.eulerAngles.z > a)
               {
                   transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + duration);
                   yield return new WaitForSeconds(delay);
               }
               isRotation = false;
           }
        }
        yield break;
    }
    IEnumerator right()
    {
        if (!isRotation)
        {
            isRotation = true;
            int a = (int)transform.eulerAngles.z - 120;
            if (a <=0)
                a +=360;
            print(a);
            int b = 0;
            if (transform.eulerAngles.z <= 0)
                b = 360;
            else
                b = (int)transform.eulerAngles.z;
            if (b > a)
            {
                print("1");
                while (b >a)
                {
                    transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z - duration);
                    b -= duration;
                    yield return new WaitForSeconds(delay);
                }
                isRotation = false;
            }
            else
            {
                print("2");
                print(b+" " +a);
                if (a - b >= 120)
                    a -= 120;
                while (b <a)
                {
                    transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z - duration);
                    b += duration;
                    yield return new WaitForSeconds(delay);
                }
                isRotation = false;
            }
        }
        yield break;
    }
}
