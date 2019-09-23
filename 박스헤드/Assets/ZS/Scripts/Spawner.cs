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

                switch (level.wave) //2레벨까지는 보통좀비, 그리고 그다음에 순차적으로 좀비소환, 그리고 뚱땡이좀비 추가하기
                {
                    case 1:
                    case 2: 
                        Instantiate(zombie[0],
                            new Vector3(transform.position.x, transform.position.y,
                                transform.position.z), //웨이브 1에서는 보통 좀비만 소환
                            Quaternion.identity);
                        break;
                    case 3:
                    case 4:
                        random = Random.Range(1, 11);
                        if (random == 1||random==2)
                            Instantiate(zombie[2],
                                new Vector3(transform.position.x, transform.position.y,
                                    transform.position.z), //웨이브 3부터 빠른좀비 소환, 5분의 1의 확률
                                Quaternion.identity);
                        else
                            Instantiate(zombie[0],
                                new Vector3(transform.position.x, transform.position.y, transform.position.z),
                                Quaternion.identity);
                        break;
                    case 5:
                    case 6:
                        random = Random.Range(1, 12);
                        if (random == 1)
                            Instantiate(zombie[2],
                                new Vector3(transform.position.x, transform.position.y,
                                    transform.position.z), //빠른좀비 11분의 1의 확률
                                Quaternion.identity);
                        else if (random == 2)
                            Instantiate(zombie[1],
                                new Vector3(transform.position.x, transform.position.y,
                                    transform.position.z), //웨이브 5부터 원거리좀비소환, 11분의 1의 확률
                                Quaternion.identity);
                        else
                            Instantiate(zombie[0],
                                new Vector3(transform.position.x, transform.position.y, transform.position.z),
                                Quaternion.identity);
                        break;
                    case 7:
                        random = Random.Range(1, 16);
                        if (random == 1||random==2)
                            Instantiate(zombie[2],
                                new Vector3(transform.position.x, transform.position.y,
                                    transform.position.z), //빠른좀비 11분의 1의 확률
                                Quaternion.identity);
                        else if (random == 3||random==4)
                            Instantiate(zombie[1],
                                new Vector3(transform.position.x, transform.position.y,
                                    transform.position.z), //웨이브 3부터 원거리좀비조환, 11분의 1의 확률
                                Quaternion.identity);
                        else if (random ==5)
                            Instantiate(zombie[3],
                                new Vector3(transform.position.x, transform.position.y,
                                    transform.position.z), //웨이브 3부터 원거리좀비조환, 11분의 1의 확률
                                Quaternion.identity);
                        else
                            Instantiate(zombie[0],
                                new Vector3(transform.position.x, transform.position.y, transform.position.z),
                                Quaternion.identity);
                        break;
                    default: //그 후로는 이렇게 소환
                        random = Random.Range(1, 16);
                        if (random == 1||random==2)
                            Instantiate(zombie[2],
                                new Vector3(transform.position.x, transform.position.y,
                                    transform.position.z), //빠른좀비 11분의 1의 확률
                                Quaternion.identity);
                        else if (random == 3||random==4)
                            Instantiate(zombie[1],
                                new Vector3(transform.position.x, transform.position.y,
                                    transform.position.z), //웨이브 3부터 원거리좀비조환, 11분의 1의 확률
                                Quaternion.identity);
                        else if (random ==5)
                            Instantiate(zombie[3],
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
