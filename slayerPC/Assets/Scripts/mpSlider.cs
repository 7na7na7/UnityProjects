using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mpSlider : MonoBehaviour
{
    public bool isMana = true;
    public static mpSlider instance;
    public Slider mp;
    private float duration = 0.4f;
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
            mp.value += 1;
            yield return new WaitForSeconds(duration);
        }
    }

    public void mpDown(int v)
    {
        if (isMana)
        {
            if (Player.instance.playerIndex == 1)
            {
                if (kagura.instance.isKagura)
                {
                }
                else
                {
                    mp.value -=v;   
                }
            }
            else
            {
                mp.value -= v;
            }
        }
    }

    public void mpUp(int v)
    {
        mp.value += v;
    }

    public void bossCut()
    {
        duration *= 0.9f;
    }
}
