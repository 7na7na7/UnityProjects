using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coronaSelect : MonoBehaviour
{
    public GameObject[] selects;

    void Update()
    {
       for(int i=0;i<selects.Length;i++)
       {
           if (selects[i].active)
               FindObjectOfType<NetworkManager>().coronaIndex = i;
       }
    }

    public void selected(int n)
    {
        selects[n].SetActive(true);
        for (int i = 0; i < 4; i++)
        {
            if (i != n)
            {
                selects[i].SetActive(false);
            }
        }
    }
}
