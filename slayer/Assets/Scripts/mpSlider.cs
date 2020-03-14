using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mpSlider : MonoBehaviour
{
    public static mpSlider instance;
    public Slider mp;
    void Start()
    {
        instance = this;
        mp = GetComponent<Slider>();
        StartCoroutine(mpCor());
    }

    IEnumerator mpCor()
    {
        while (true)
        {
            mp.value += 1f;
            yield return new WaitForSeconds(0.3f);
        }
    }

    public void mpDown(int v)
    {
        mp.value -= v;
    }

    public void mpUp(int v)
    {
        mp.value += v;
    }
}
