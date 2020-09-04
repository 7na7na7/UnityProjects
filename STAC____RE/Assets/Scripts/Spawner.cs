using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public static Spawner instance;
    public Transform player;
    public float radMinX, radMaxX, radMinY, radMaxY;
    public float[] delays;
    public float[] minusPercents;
    public float minusDelay;
    public float[] bulletAppearTimings;
    public float time = 0;
    public float bulletSpeedPercent = 0f;
    public float bulletSpeedUpValue;
    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        time += Time.deltaTime;
    }

    void Start()
    {
        for (int i = 0; i < delays.Length; i++)
        {
            StartCoroutine(spawn(i));
            StartCoroutine(delayMinus(i));
        }
    }

    public void set(int index)
    {
        if (player != null)
            {
                int r = Random.Range(0, 6);
                GameObject enemy = ObjectManager.instance.MakeObj(index);
                if (r == 0||r==1) //위
                {
                    enemy.transform.position = new Vector2(
                        Random.Range(player.position.x - radMaxX*Camera.main.orthographicSize, player.position.x + radMaxX*Camera.main.orthographicSize),
                        Random.Range(player.position.y + radMinY*Camera.main.orthographicSize, player.position.y + radMaxY*Camera.main.orthographicSize));
                }
                else if (r == 2||r==3) //아래
                {
                    enemy.transform.position = new Vector2(
                        Random.Range(player.position.x-radMaxX*Camera.main.orthographicSize,player.position.x+radMaxX*Camera.main.orthographicSize), 
                        Random.Range(player.position.y-radMinY*Camera.main.orthographicSize,player.position.y-radMaxY*Camera.main.orthographicSize));
                }
                else if (r == 4) //오른쪽
                {
                    enemy.transform.position = new Vector2(
                        Random.Range(player.position.x + radMinX*Camera.main.orthographicSize, player.position.x + radMaxX*Camera.main.orthographicSize),
                        Random.Range(player.position.y - radMinY*Camera.main.orthographicSize, player.position.y + radMinY*Camera.main.orthographicSize));
                }
                else if (r == 5) //왼쪽
                {
                    enemy.transform.position = new Vector2(
                        Random.Range(player.position.x - radMinX*Camera.main.orthographicSize, player.position.x - radMaxX*Camera.main.orthographicSize),
                        Random.Range(player.position.y - radMinY*Camera.main.orthographicSize, player.position.y + radMinY*Camera.main.orthographicSize));
                }   
            }
    }
    IEnumerator spawn(int index)
    {
        while (true)
        {
            float r = Random.Range(delays[index] / 1.5f, delays[index]*1.5f);
            yield return new WaitForSeconds(r);
            if (bulletAppearTimings[index] < time)
            {
                int random = Random.Range(0, 3);
                if(random==0) 
                    set(index*2);
                else
                    set(index*2+1);   
            }
        }
    }

    IEnumerator bulletSpeedUP()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            bulletSpeedPercent += bulletSpeedUpValue;
        }
    }
    IEnumerator delayMinus(int index)
    {
        while (true)
        {
            yield return new WaitForSeconds(minusDelay);
            if (bulletAppearTimings[index] < time)
            {
                delays[index] -= (delays[index] * minusPercents[index]/100);   
            }
        }
    }
}
