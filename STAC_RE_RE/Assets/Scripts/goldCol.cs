using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goldCol : MonoBehaviour
{
    public GameObject goldParticle;
    private bool canDestroy = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Edge1")||other.CompareTag("Edge2"))
        {
            GoldManager.instance.GetGold(10);
            SoundMgr.instance.Play(3,0.7f,3);
            ScoreMgr.instance.goldPong();
            Instantiate(goldParticle, transform.position, Quaternion.identity);
            SetFalse();
        }
    }
    public void SetFalse()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!CheckCamera.instance.CheckObjectIsInCamera(gameObject))
        {
            if (!canDestroy)
            {
                canDestroy = true;
                StartCoroutine(Destroy());
            }
        }
    }

    IEnumerator Destroy() //20초동안 보이지 않으면 파괴
    {
        for(int i=0;i<40;i++)
        {
            yield return new WaitForSeconds(0.5f);
            if (CheckCamera.instance.CheckObjectIsInCamera(gameObject))
            {
                canDestroy = false;
                yield break;
            }
        }
        gameObject.SetActive(false);
    }
}
