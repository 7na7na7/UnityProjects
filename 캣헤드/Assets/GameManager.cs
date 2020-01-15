using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject p1, p2;
    public bool p1Dead = false;
    public bool p2Dead = false;
    public float spawnDelay;
    private void Update()
    {
        /*
        if (p1Dead && p2Dead)
        {
            StopAllCoroutines();
            //게임 오버
            SceneManager.LoadScene("GameOver");
        }
        */
    }

    public void Dead1()
    {
        StartCoroutine(spawnP1());
    }

    public void Dead2()
    {
        StartCoroutine(spawnP2());
    }
    IEnumerator spawnP1()
    {
        p1Dead = false;
        yield return new WaitForSeconds(spawnDelay);
        Instantiate(p1, transform.position,Quaternion.identity);
    }
    IEnumerator spawnP2()
    {
        p2Dead = false;
        yield return new WaitForSeconds(spawnDelay);
        Instantiate(p2, transform.position,Quaternion.identity);
    }
}
