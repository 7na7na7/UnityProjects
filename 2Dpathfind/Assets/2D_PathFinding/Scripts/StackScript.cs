using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class StackScript : MonoBehaviourPunCallbacks, IPunObservable
{
    public GameObject[] eachstacks;
    public int count = 0;

    void Update()
    {
        if(gameObject.tag=="Player")
        {
            foreach (GameObject go in eachstacks)
            {
                go.SetActive(false);
            }
        }
        else
        {
            for (int i=0;i<eachstacks.Length;i++)
            {
                if (i < count)
                    eachstacks[i].SetActive(true);
                else
                    eachstacks[i].SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gameObject.tag == "Gay"&&other.CompareTag("Bullet"))
        {
            if (count < 5)
            {
                if (GetComponent<MoveByClick>().pv.IsMine)
                {
                    StopAllCoroutines();
                    StartCoroutine(heal());
                }
                StartCoroutine(speedDown());
                StartCoroutine(GetComponent<MoveByClick>().invisible());
                count++;
               
            }
            else
            {
                GetComponent<MoveByClick>().pv.RPC("manRPC", RpcTarget.AllBuffered);
                count = 0;
                GetComponent<MoveByClick>().score = 0;
                if (GetComponent<MoveByClick>().pv.IsMine)
                { 
                    StopAllCoroutines();
                }
            }
        }
    }

    IEnumerator speedDown()
    {
        if (GetComponent<MoveByClick>().pv.IsMine)
        {
            FindObjectOfType<AStar_PathFinding>().gayspeed *= 0.5f;
            yield return new WaitForSeconds(0.5f);
            FindObjectOfType<AStar_PathFinding>().gayspeed *= (1 / 0.5f);
        }
    }

    IEnumerator heal()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            if(count>=1) 
                count--;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(count);
        }
        else
        {
            count = (int) stream.ReceiveNext();
        }
    }
}
