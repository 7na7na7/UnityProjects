using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lessvision : MonoBehaviour
{
    public float delayInsmall;
    public float delayInbig;
    public float bounceForce;

    public float stageBForce;
    private bool Bcheck;
    public float stageCForce;
    private bool Ccheck;

    void Start()
    {
        StartCoroutine(bounce());
    }

    IEnumerator bounce()
    {
        while (true)
        {
            transform.localScale *= bounceForce;
            yield return new WaitForSeconds(delayInsmall);
            transform.localScale /= bounceForce;
            yield return new WaitForSeconds(delayInbig);
        }
    }

    public void Stageb()
    {
        if (!Bcheck)
        {
            transform.localScale *= stageBForce;
            Bcheck = true;
        }
    }

    public void StageC()
    {
        if (!Ccheck)
        {
            transform.localScale *= stageCForce;
            Ccheck = true;
        }
    }

}
