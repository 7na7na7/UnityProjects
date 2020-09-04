using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GoldSpawner : MonoBehaviour
{
    public float minGoldTime; //골드가 나타나는 최소 빈도
    public float timeMinusValue; 
    public float timeMinusDelay;
    public static GoldSpawner instance;
    public float delay;
    private float radMaxX, radMinY, radMinX,radMaxY;
    public Transform player;

    private void Awake()
    {
        instance = this;
    }
    
    private void Start()
    {
        StartCoroutine(spawn());
        StartCoroutine(goldTimeMinus());
        radMaxX = FindObjectOfType<Spawner>().radMaxX;
        radMinY = FindObjectOfType<Spawner>().radMinY;
        radMinX = FindObjectOfType<Spawner>().radMinX;
        radMaxY = FindObjectOfType<Spawner>().radMaxY;
    }

    IEnumerator spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            if (player != null)
            {
                GameObject gold=ObjectManager.instance.MakeObj(102);
                int r = Random.Range(0, 6);
                if (r == 0||r==1) //위
                {
                    gold.transform.position = new Vector2(
                        Random.Range(player.position.x - radMaxX*Camera.main.orthographicSize, player.position.x + radMaxX*Camera.main.orthographicSize),
                        Random.Range(player.position.y + radMinY*Camera.main.orthographicSize, player.position.y + radMaxY*Camera.main.orthographicSize));
                }
                else if (r == 2||r==3) //아래
                {
                    gold.transform.position = new Vector2(
                        Random.Range(player.position.x-radMaxX*Camera.main.orthographicSize,player.position.x+radMaxX*Camera.main.orthographicSize), 
                        Random.Range(player.position.y-radMinY*Camera.main.orthographicSize,player.position.y-radMaxY*Camera.main.orthographicSize));
                }
                else if (r == 4) //오른쪽
                {
                    gold.transform.position = new Vector2(
                        Random.Range(player.position.x + radMinX*Camera.main.orthographicSize, player.position.x + radMaxX*Camera.main.orthographicSize),
                        Random.Range(player.position.y - radMinY*Camera.main.orthographicSize, player.position.y + radMinY*Camera.main.orthographicSize));
                }
                else if (r == 5) //왼쪽
                {
                    gold.transform.position = new Vector2(
                        Random.Range(player.position.x - radMinX*Camera.main.orthographicSize, player.position.x - radMaxX*Camera.main.orthographicSize),
                        Random.Range(player.position.y - radMinY*Camera.main.orthographicSize, player.position.y + radMinY*Camera.main.orthographicSize));
                }   
            }
        }
    }

    IEnumerator goldTimeMinus()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeMinusDelay);
            if (delay >= minGoldTime)
                delay -= timeMinusValue;
            else
                break;
        }
        yield break;
    }
}
