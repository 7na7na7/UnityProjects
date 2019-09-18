using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private int random;
    public GameObject[] zombie;
    private float minTime=3;
    private float maxTime=5;
    
    void Start()
    {
        Level level = FindObjectOfType<Level>();
        if(level.wave<level.zombiecount.Length) 
            StartCoroutine(spawn());
    }

    IEnumerator spawn()
    {
        Level level = FindObjectOfType<Level>();
        while (true)
        {
            if(level.isBossAppear==false) //보스가 나타났으면 스폰멈춤
            {
            if (level.isdelay)
                level.currentzombie = 0;
            yield return new WaitUntil(()=>level.isdelay==false);
            if (level.currentzombie < level.savedcount)
            {
                #region spawn

                switch (level.wave)
                {
                    case 1:
                        Instantiate(zombie[0],
                            new Vector3(transform.position.x, transform.position.y,
                                transform.position.z), //웨이브 1에서는 보통 좀비만 소환
                            Quaternion.identity);
                        break;
                    case 2:
                        random = Random.Range(1, 11);
                        if (random == 1)
                            Instantiate(zombie[2],
                                new Vector3(transform.position.x, transform.position.y,
                                    transform.position.z), //웨이브 2부터 빠른좀비 소환, 10분의 1의 확률
                                Quaternion.identity);
                        else
                            Instantiate(zombie[0],
                                new Vector3(transform.position.x, transform.position.y, transform.position.z),
                                Quaternion.identity);
                        break;
                    case 3:
                        random = Random.Range(1, 12);
                        if (random == 1)
                            Instantiate(zombie[2],
                                new Vector3(transform.position.x, transform.position.y,
                                    transform.position.z), //빠른좀비 11분의 1의 확률
                                Quaternion.identity);
                        else if (random == 2)
                            Instantiate(zombie[1],
                                new Vector3(transform.position.x, transform.position.y,
                                    transform.position.z), //웨이브 3부터 원거리좀비소환, 11분의 1의 확률
                                Quaternion.identity);
                        else
                            Instantiate(zombie[0],
                                new Vector3(transform.position.x, transform.position.y, transform.position.z),
                                Quaternion.identity);
                        break;
                    default: //그 후로는 이렇게 소환
                        random = Random.Range(1, 12);
                        if (random == 1)
                            Instantiate(zombie[2],
                                new Vector3(transform.position.x, transform.position.y,
                                    transform.position.z), //빠른좀비 11분의 1의 확률
                                Quaternion.identity);
                        else if (random == 2)
                            Instantiate(zombie[1],
                                new Vector3(transform.position.x, transform.position.y,
                                    transform.position.z), //웨이브 3부터 원거리좀비조환, 11분의 1의 확률
                                Quaternion.identity);
                        else
                            Instantiate(zombie[0],
                                new Vector3(transform.position.x, transform.position.y, transform.position.z),
                                Quaternion.identity);
                        break;
                }
            }
            #endregion spawn
            }
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
            if (level.wave > level.zombiecount.Length)
            {
                Debug.Log("스폰코루틴종료!");
                StopAllCoroutines();
            }
        }
    }
}
