using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject zombie3;
    public GameObject zombie;
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
            if (level.isdelay)
                level.currentzombie = 0;
            yield return new WaitUntil(()=>level.isdelay==false);
            int random = Random.Range(1, 7);
            if (level.currentzombie < level.savedcount)
            {
                #region spawn
                if (random == 1)
                    Instantiate(zombie3, new Vector3(transform.position.x, transform.position.y, transform.position.z),
                        Quaternion.identity);
                else
                    Instantiate(zombie, new Vector3(transform.position.x, transform.position.y, transform.position.z),
                        Quaternion.identity);
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
