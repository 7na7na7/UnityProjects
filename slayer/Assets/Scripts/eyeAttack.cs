using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eyeAttack : MonoBehaviour
{
    public GameObject attackCol;
    private int a = 0;
    private void OnEnable()
    {
        StartCoroutine(attack());
    }

    IEnumerator attack()
    {
        yield return new WaitForSeconds(0.5f);
        attackCol.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        attackCol.SetActive(false);
        gameObject.SetActive(false);
    }
}
